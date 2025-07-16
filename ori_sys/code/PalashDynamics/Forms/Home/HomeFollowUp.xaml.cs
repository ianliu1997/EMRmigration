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
using PalashDynamics;
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Resources;
using System.IO;
using System.Xml.Linq;
namespace PalashDynamics.Forms.Home
{
    public partial class HomeFollowUp : UserControl
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        TextBlock mElement;       
        public HomeFollowUp()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "FollowUp List";
            DataList = new PagedSortableCollectionView<clsFollowUpVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dtpFollowUpSummaryFrom.SelectedDate = DateTime.Now;
            dtpFollowUpSummaryTo.SelectedDate = DateTime.Now;
            if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
            {
                cmbDoctorFollowUpSummary.IsEnabled = false;


                this.DataContext = new clsFollowUpVO()   //clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID

                };

            }
            else
            {
                this.DataContext = new clsFollowUpVO()   //clsAppointmentVO()
                {
                    UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,
                    DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                    DoctorId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,

                };

            }
        }

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


        // public clsGetAppointmentBizActionVO BizAction { get; set; }

        //Added by Manisha
        public event RoutedEventHandler OnSaveButton_Click;

        public bool ChkUnit { get; set; }


        bool UseApplicationID = true;
        bool UseApplicationDoctorID = true;


        #endregion

        #region Paging

        public PagedSortableCollectionView<clsFollowUpVO> DataList { get; private set; }

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




        #endregion

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {

            if (dtpFollowUpSummaryTo.SelectedDate != null && dtpFollowUpSummaryFrom.SelectedDate != null)
            {
                if (dtpFollowUpSummaryTo.SelectedDate.Value.Date.Date < dtpFollowUpSummaryFrom.SelectedDate.Value.Date.Date)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "To date can not be less than From date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

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
        }

        public void FetchData()
        {
            clsGetPatientFollowUpDetailsBizActionVO BizAction = new clsGetPatientFollowUpDetailsBizActionVO();  // new clsGetAppointmentBizActionVO();

            if (dtpFollowUpSummaryFrom.SelectedDate != null)
            {
                BizAction.FromDate = dtpFollowUpSummaryFrom.SelectedDate.Value.Date.Date;
            }
            if (dtpFollowUpSummaryTo.SelectedDate != null)
            {
                BizAction.ToDate = dtpFollowUpSummaryTo.SelectedDate.Value.Date.Date; ;
            }

            if (UseApplicationID != true || UseApplicationDoctorID != true)
            {

                if (cmbUnitFollowUpSummary.SelectedItem != null)
                {
                    BizAction.UnitId = ((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID;
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitId = 0;
                }
                else
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }


                if (cmbDepartmentFollowUpSummary.SelectedItem != null)
                    BizAction.DepartmentId = ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem).ID;
                
                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    BizAction.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                }
                else
                {
                    if (cmbDoctorFollowUpSummary.SelectedItem != null)
                        BizAction.DoctorID = ((MasterListItem)cmbDoctorFollowUpSummary.SelectedItem).ID;
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
                        BizAction.UnitId = ((clsFollowUpVO)this.DataContext).UnitId;
                    }

                    BizAction.DepartmentId = ((clsFollowUpVO)this.DataContext).DepartmentId;

                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                    {
                        BizAction.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    }
                    else
                    {
                        BizAction.DoctorID = ((clsFollowUpVO)this.DataContext).DoctorId;
                    }

                }
            }

            //if (cmbAppointmentStat.SelectedItem != null)
            //{
            //    BizAction.AppintmentStatusID = Convert.ToInt32(((MasterListItem)cmbAppointmentStat.SelectedItem).ID);
            //}

            //if (cmbSpRegistration.SelectedItem != null)
            //{
            //    BizAction.SpecialRegistrationId = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;
            //}

            //if (cmbAppointmentStat.SelectedItem != null)
            //{
            //    if (((MasterListItem)cmbAppointmentType.SelectedItem).ID != 2)
            //    {
            //        BizAction.VisitMark = Convert.ToBoolean(((MasterListItem)cmbAppointmentType.SelectedItem).ID);
            //    }
            //    //else
            //    //{

            //    //    BizAction.VisitMark = true;
            //    //}

            //}



            BizAction.FirstName = txtFirstName.Text;
            //    BizAction.MiddleName = txtMiddleName.Text;
            BizAction.LastName = txtLastName.Text;
            BizAction.ContactNo = txtContactNo.Text;
            BizAction.MrNo = txtMiddleName.Text;

            //BizAction.UnRegistered = UnRegistered;

            BizAction.InputPagingEnabled = true;
            BizAction.InputStartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.InputMaximumRows = DataList.PageSize;

            BizAction.IsFromDashBoard = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null) 
                {
                    if (((clsGetPatientFollowUpDetailsBizActionVO)arg.Result).FollowUpDetailsList != null)
                    {
                        clsGetPatientFollowUpDetailsBizActionVO result = arg.Result as clsGetPatientFollowUpDetailsBizActionVO;
                        DataList.TotalItemCount = result.OutputTotalRows;

                        if (result.FollowUpDetailsList != null)
                        {
                            DataList.Clear();

                            foreach (clsFollowUpVO item in result.FollowUpDetailsList)
                            {

                                DataList.Add(item);
                            }

                        }

                        dgFollowUpList.ItemsSource = DataList;
                        dgDataPager.Source = DataList;

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

        private void cmbUnitFollowUpSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnitFollowUpSummary.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID);
        }

        private void FillUnitList()
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

                    cmbUnitFollowUpSummary.ItemsSource = null;
                    cmbUnitFollowUpSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    //cmbunitappointmentsummary.itemssource = objlist;
                }
                if (this.DataContext != null)
                {
                    cmbUnitFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).UnitId;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDepartmentList(long iUnitId)
        {
            #region Commented to show/not clinical Depatments
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            //if (iUnitId > 0)
            //    BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
            //BizAction.MasterList = new List<MasterListItem>();
            #endregion

            #region To show/not clinical Depatments

            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionVo = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
            BizActionVo.IsUnitWise = true;
            BizActionVo.IsClinical = true;  // flag use to Show/not Clinical Departments  02032017
            BizActionVo.UnitID = iUnitId;   // fill Unitwise Departments  02032017

            #endregion

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);   //Commented to show/not clinical Depatments

                    if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem != null)      // changes for - to show/not clinical Depatments
                    {
                        objList.AddRange(((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem);
                    }

                    if (((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }

                    cmbDepartmentFollowUpSummary.ItemsSource = null;
                    cmbDepartmentFollowUpSummary.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        if (UseApplicationID == true)
                        {
                            cmbDepartmentFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).DepartmentId;
                            UseApplicationID = false;
                            cmbDepartmentFollowUpSummary.SelectedItem = objList[0];
                        }
                        else
                            cmbDepartmentFollowUpSummary.SelectedValue = objList[0].ID;
                    }

                }

            };


            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);   //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmbDepartmentFollowUpSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem != null) ;
            FillDoctor(((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID, ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem).ID);
        }

        private void FillDoctor(long IUnitId1, long iDeptId1)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbUnitFollowUpSummary.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId1;
            }
            if ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem != null)
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

                    cmbDoctorFollowUpSummary.ItemsSource = null;
                    cmbDoctorFollowUpSummary.ItemsSource = objList;
                    cmbDoctorFollowUpSummary.SelectedItem = objList[0];


                    if (this.DataContext != null)
                    {


                        if (UseApplicationDoctorID == true)
                        {
                            cmbDoctorFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).DoctorId;
                            UseApplicationDoctorID = false;
                        }
                        else
                            cmbDoctorFollowUpSummary.SelectedValue = objList[0].ID;



                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctorFollowUpSummary.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public clsFollowUpVO item = new clsFollowUpVO();
        private void grdAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsFollowUpVO)dgFollowUpList.SelectedItem != null)
            {
                item = ((clsFollowUpVO)dgFollowUpList.SelectedItem);
            }
        }

        private void frmFollowUp_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {

                //Email = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Email;
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                //IsAppointmentList = true;



                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do  nothing
                    }
                    //else
                    // cmdCancelAppointment.IsEnabled = false;


                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbUnitFollowUpSummary.IsEnabled = true;
                    //// cmdRescAppointment.IsEnabled = false;
                }
                else
                {
                    cmbUnitFollowUpSummary.IsEnabled = false;
                    cmbUnitFollowUpSummary.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                ////tabAppointmentChart.Visibility = Visibility.Collapsed;
                FillUnitList();
                //FillSpecialRegistration();
                //cmbAppointmentStat.ItemsSource = FillAppointmentStatus();
                //cmbAppointmentStat.SelectedValue = (long)0;
                //cmbAppointmentType.ItemsSource = FillAppointmentType();
                //cmbAppointmentType.SelectedValue = (long)2;



                SetComboboxValue();
                dtpFollowUpSummaryFrom.Focus();
                FetchData();

                Indicatior.Close();
            }

            dtpFollowUpSummaryFrom.Focus();
            dtpFollowUpSummaryFrom.UpdateLayout();
            IsPageLoded = true;
        }

        private void SetComboboxValue()
        {
            cmbUnitFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).UnitId;
            cmbDepartmentFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).DepartmentId;
            cmbDoctorFollowUpSummary.SelectedValue = ((clsFollowUpVO)this.DataContext).DoctorId;
            ////cmbClinic.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            ////cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;

        }

        //public static List<MasterListItem> FillAppointmentStatus()
        //{
        //    List<MasterListItem> objAppointStatus = new List<MasterListItem>();

        //    objAppointStatus.Add(new MasterListItem(0, "New"));
        //    objAppointStatus.Add(new MasterListItem(1, "Cancelled"));
        //    objAppointStatus.Add(new MasterListItem(2, "ReScheduled"));
        //    return objAppointStatus;
        //}
        //public static List<MasterListItem> FillAppointmentType()
        //{
        //    List<MasterListItem> objAppointType = new List<MasterListItem>();
        //    objAppointType.Add(new MasterListItem(1, "Turned-Up"));
        //    objAppointType.Add(new MasterListItem(0, "Non Turned-Up"));
        //    objAppointType.Add(new MasterListItem(2, "All"));
        //    //objAppointType.Add(new MasterListItem(2, "Doctor"));
        //    //objAppointType.Add(new MasterListItem(3, "Department"));
        //    return objAppointType;
        //}

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }
        private void txtMrno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void dtpFollowUpSummaryFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtpFollowUpSummaryTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDoctorFollowUpSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FollowupReport_Click(object sender, RoutedEventArgs e)
        {
           // long ID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            if (cmbUnitFollowUpSummary.SelectedItem != null)
            {
                UnitID = ((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                UnitID = 0;
            }
            else
            {
                UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
           
            DateTime? FrmDate=null;
            DateTime? ToDate = null;
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            long DrId = 0;
            long DepartmentId = 0;
            bool chkToDate = true;

            string FirstName = string.Empty;
            if (txtFirstName.Text != string.Empty)
            {
                FirstName = txtFirstName.Text;
            }
            string LastName = string.Empty;

            if (txtLastName.Text != string.Empty)
            {
                LastName = txtLastName.Text;
            }

         
            //    BizAction.MiddleName = txtMiddleName.Text;
            
            string ContactNo = txtContactNo.Text;
            string MrNo = txtMiddleName.Text;
            string msgTitle = "";
            //BizAction.InputPagingEnabled = true;
            //BizAction.InputStartRowIndex = DataList.PageIndex * DataList.PageSize;
            //BizAction.InputMaximumRows = DataList.PageSize;

            bool IsFromDashBoard = true;

            if (dtpFollowUpSummaryFrom.SelectedDate != null)
            {
               FrmDate = dtpFollowUpSummaryFrom.SelectedDate.Value.Date.Date;
               dtpF = dtpFollowUpSummaryFrom.SelectedDate.Value.Date.Date;
            }
            ////////////////////////

            if (dtpFollowUpSummaryTo.SelectedDate != null)
            {
                dtpT = dtpFollowUpSummaryTo.SelectedDate.Value.Date.Date;
                dtpF = dtpFollowUpSummaryFrom.SelectedDate.Value.Date.Date;
                if (dtpF.Value > dtpT.Value)
                {
                    dtpFollowUpSummaryTo.SelectedDate = dtpFollowUpSummaryFrom.SelectedDate;
                    dtpT = dtpF;
                    chkToDate = false;
                }
                else
                {
                    dtpP = dtpT;
                    dtpT = dtpT.Value.AddDays(1);
                    dtpFollowUpSummaryTo.Focus();
                }
            }



            ////////////////////
            if (dtpFollowUpSummaryTo.SelectedDate != null)
            {
                ToDate = dtpFollowUpSummaryTo.SelectedDate.Value.Date.Date; ;
            }
            //////////////////////////
            //if (dtpFromDate.SelectedDate != null)
            //{
            //    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            //}
            //if (dtpToDate.SelectedDate != null)
            //{
            //    dtpT = dtpToDate.SelectedDate.Value.Date.Date;
            //    if (dtpF.Value > dtpT.Value)
            //    {
            //        dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
            //        dtpT = dtpF;
            //        chkToDate = false;
            //    }
            //    else
            //    {
            //        dtpP = dtpT;
            //        dtpT = dtpT.Value.AddDays(1);
            //        dtpToDate.Focus();
            //    }
            //}

            //if (dtpT != null)
            //{
            //    if (dtpF != null)
            //    {
            //        dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

            //    }
            //}




            ////////////////////

            if (cmbDepartmentFollowUpSummary.SelectedItem != null)
                DepartmentId = ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem).ID;

            if (UseApplicationID != true || UseApplicationDoctorID != true)
            {

                //if (cmbUnitFollowUpSummary.SelectedItem != null)
                //{
                //    BizAction.UnitId = ((MasterListItem)cmbUnitFollowUpSummary.SelectedItem).ID;
                //}
                //else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                //{
                //    BizAction.UnitId = 0;
                //}
                //else
                //{
                //    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //}


                //if (cmbDepartmentFollowUpSummary.SelectedItem != null)
                //    BizAction.DepartmentId = ((MasterListItem)cmbDepartmentFollowUpSummary.SelectedItem).ID;

                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    DrId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                }
                else
                {
                    if (cmbDoctorFollowUpSummary.SelectedItem != null)
                        DrId = ((MasterListItem)cmbDoctorFollowUpSummary.SelectedItem).ID;
                }


            }

            if (chkToDate)
            {

                if (FrmDate != null && ToDate != null)
                {

                    string URL = "../Reports/Patient/rptPatientFollowup.aspx?DoctorID=" + DrId + "&UnitId=" + UnitID + "&FromDate=" + dtpF.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtpT.Value.ToString("dd-MMM-yyyy") + "&DepartmentId=" + DepartmentId + "&FirstName=" + FirstName + "&MiddleName=" + txtMiddleName.Text + "&LastName=" + LastName + "&ContactNo=" + ContactNo + "&MRNO=" + MrNo + "&Status=" + true + "&PatientUnitID=" + UnitID ;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }
            else

            {
                string msgText = "Incorrect Date Range. From Date Cannot Be Greater Than To Date.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            }
            //if (dataGrid2.SelectedItem != null)
            //{
            //    string msgText = "";
            //    msgText = "Are You Sure \n You Want To Print Patient Registration Report?";
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptPatientCase);
            //    msgW.Show();
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        
        //***//-------------------
        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void hlFollowUp_Click(object sender, RoutedEventArgs e)
        {

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

            clsFollowUpVO objVO = dgFollowUpList.SelectedItem as clsFollowUpVO;

            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objVO.PatientId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = objVO.PatientUnitId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objVO.PatientUnitId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID = objVO.DoctorId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpReasonID = objVO.AppointmentReasonId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpRemark = objVO.Remark;
            ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpDate = objVO.FollowUpDate;
            ((IApplicationConfiguration)App.Current).SelectedPatient.FollowUpID = objVO.FollowUpID;
            ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo = objVO.MrNo;
            ((IApplicationConfiguration)App.Current).SelectedPatient.VisitDate = objVO.VisitDate;
            ((IApplicationConfiguration)App.Current).SelectedPatient.DepartmentID = objVO.DepartmentId;
            ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID = objVO.VisitID;                 
          
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientFollowup";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
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
                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.All.ToString());
                }

                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

               

        private void cmdBookAppoinment_Click(object sender, RoutedEventArgs e)
        {
            
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            clsFollowUpVO objVO = dgFollowUpList.SelectedItem as clsFollowUpVO;
            if (objVO.FollowUpDate > DateTime.Now.Date.Date)
                if (objVO.ISAppointment != true)
                {
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = objVO.PatientId;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objVO.UnitId;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo = objVO.MrNo;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId = objVO.UnitId;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName = objVO.FirstName;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName = objVO.MiddleName;
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName = objVO.LastName;
                        ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID = objVO.DepartmentId;
                        ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID = objVO.DoctorId;
                        ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FollowUpDate = objVO.FollowUpDate;                        

                        UserControl rootPage = Application.Current.RootVisual as UserControl;

                        WebClient c2 = new WebClient();
                        c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        c2.OpenReadAsync(new Uri("OPDModule" + ".xap", UriKind.Relative));
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Already Appointment scheduled.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", " Selected follow up Date is greater than Today's date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }   


        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;

                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "OPDModule.dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("CIMS.Forms.PatientAppointment") as UIElement;

                if (myData != null)
                {
                    ((IInitiateCIMS)myData).Initiate("HomeFollowUp");

                    ((ChildWindow)myData).Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //-----------------------
        
    }
}
