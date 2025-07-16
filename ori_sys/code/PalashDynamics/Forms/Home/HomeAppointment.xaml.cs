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

namespace CIMS.Forms
{
    public partial class HomeAppointment : UserControl
    {
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        TextBlock mElement;
        public HomeAppointment()
        {
            InitializeComponent();
            mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Appointment List";
            DataList = new PagedSortableCollectionView<clsAppointmentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dtpAppointmentSummaryF.SelectedDate = DateTime.Now;
            dtpAppointmentSummaryT.SelectedDate = DateTime.Now;
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

       


        #endregion
        
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FetchData();

        }
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {

            if (dtpAppointmentSummaryT.SelectedDate != null && dtpAppointmentSummaryF.SelectedDate != null)
            {
                if (dtpAppointmentSummaryT.SelectedDate.Value.Date.Date < dtpAppointmentSummaryF.SelectedDate.Value.Date.Date)
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
        public bool UnRegistered;
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
                //else
                //{

                //    BizAction.VisitMark = true;
                //}

            }



            BizAction.FirstName = txtFirstName.Text;
            //    BizAction.MiddleName = txtMiddleName.Text;
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
      
        private void cmbUnitAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnitAppointmentSummary.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID);
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

                    cmbUnitAppointmentSummary.ItemsSource = null;
                    cmbUnitAppointmentSummary.ItemsSource = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                    //cmbunitappointmentsummary.itemssource = objlist;
                }
                if (this.DataContext != null)
                {
                    cmbUnitAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;

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

                    if (((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }

                    cmbDepartmentAppointmentSummary.ItemsSource = null;
                    cmbDepartmentAppointmentSummary.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        if (UseApplicationID == true)
                        {
                            cmbDepartmentAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
                            UseApplicationID = false;
                            cmbDepartmentAppointmentSummary.SelectedItem = objList[0];
                        }
                        else
                            cmbDepartmentAppointmentSummary.SelectedValue = objList[0].ID;
                    }
                    
                }

            };


            client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);   //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        private void cmbDepartmentAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem != null) ;
            FillDoctor(((MasterListItem)cmbUnitAppointmentSummary.SelectedItem).ID, ((MasterListItem)cmbDepartmentAppointmentSummary.SelectedItem).ID);


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

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        public clsAppointmentVO item = new clsAppointmentVO();
        private void grdAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsAppointmentVO)dgAppointmentList.SelectedItem != null)
            {
                item = ((clsAppointmentVO)dgAppointmentList.SelectedItem);
            }
        }
      //  bool IsPageLoded = false;
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
                    //else
                       // cmdCancelAppointment.IsEnabled = false;


                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbUnitAppointmentSummary.IsEnabled = true;
                   // cmdRescAppointment.IsEnabled = false;
                }
                else
                {
                    cmbUnitAppointmentSummary.IsEnabled = false;
                    cmbUnitAppointmentSummary.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                //tabAppointmentChart.Visibility = Visibility.Collapsed;
                FillUnitList();
                FillSpecialRegistration();
                cmbAppointmentStat.ItemsSource = FillAppointmentStatus();
                cmbAppointmentStat.SelectedValue = (long)0;
                cmbAppointmentType.ItemsSource = FillAppointmentType();
                cmbAppointmentType.SelectedValue = (long)2;



                SetComboboxValue();
                dtpAppointmentSummaryF.Focus();
                FetchData();

                Indicatior.Close();
            }

            dtpAppointmentSummaryF.Focus();
            dtpAppointmentSummaryF.UpdateLayout();
            IsPageLoded = true;
        }
        private void SetComboboxValue()
        {
            cmbUnitAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            cmbDepartmentAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;
            cmbDoctorAppointmentSummary.SelectedValue = ((clsAppointmentVO)this.DataContext).DoctorId;       
            //cmbClinic.SelectedValue = ((clsAppointmentVO)this.DataContext).UnitId;
            //cmbDepartment.SelectedValue = ((clsAppointmentVO)this.DataContext).DepartmentId;

        }
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
            objAppointType.Add(new MasterListItem(1, "Turned-Up"));
            objAppointType.Add(new MasterListItem(0, "Non Turned-Up"));
            objAppointType.Add(new MasterListItem(2, "All"));
            //objAppointType.Add(new MasterListItem(2, "Doctor"));
            //objAppointType.Add(new MasterListItem(3, "Department"));
            return objAppointType;
        }
       
        private void chkPatient_Click(object sender, RoutedEventArgs e)
        {
            if (item.AppointmentDate == DateTime.Now.Date.Date)
            {
                if (item.MrNo == "")
                {
                    //string msgTitle = "";
                    //string msgText = "This patient is not registered,Do you want to continue with Registration?";

                    //MessageBoxControl.MessageBoxChildWindow msgW5 =
                    //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    //msgW5.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW5_OnMessageBoxClosed);

                    //msgW5.Show();


                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("","This patient is not Registered", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();

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

                          //  EncounterWindow formEncounter = new EncounterWindow();
                           // formEncounter.VAppointmentID = item.AppointmentID;
                           // formEncounter.Initiate("NEW");

                          //  UIElement MyContent = formEncounter;
                          //  ((IApplicationConfiguration)App.Current).OpenMainContent(MyContent);

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
            }
            else
            {
                string msgTitle = "";
                string msgText = "You can not mark visit for this appointment";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "You can not mark visit for this appointment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
                item.VisitMark = false;
            }
        }

        //void msgW5_OnMessageBoxClosed(MessageBoxResult result)
        //{
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

        //        //PatientRegistration Win = new PatientRegistration();
        //        //Win.mAppointmentID = item.AppointmentID;

        //        //Win.Initiate("NEWApp");
        //        //UIElement MyData = Win;
        //        //((IApplicationConfiguration)App.Current).OpenMainContent(MyData);



        //    }
        //    else
        //    {
        //        item.VisitMark = false;
        //    }



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

        private void dtpAppointmentSummary_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbAppointStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDoctorAppointmentSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
     
    }
}
