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
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;

namespace PalashDynamics.IPD
{
    public partial class Discharge : UserControl,IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                    {
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                     
                    }
                    break;


            }
        }

        #endregion

        #region Variable Declaration

        clsIPDAdmissionVO AdmissionDetails { get; set; }
        SwivelAnimation objAnimation;
       
#endregion
       
        public Discharge()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<clsIPDDischargeVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        #region Paging

        public PagedSortableCollectionView<clsIPDDischargeVO> DataList { get; private set; }


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

        #region FillCombobox
        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
            BizAction.IsActive = true;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.IsActive = true;
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
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;
                    cmbUnit.SelectedItem = objList[0];

                    

                    if (AdmissionDetails != null)
                    {
                        cmbUnit.SelectedValue = AdmissionDetails.UnitId;
                    }


                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDepartmentList(long iUnitId)
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
                    cmbDepartment.SelectedItem = objList[0];
                    if (AdmissionDetails != null)
                    {
                        cmbDepartment.SelectedValue = AdmissionDetails.DepartmentID;
                    }

                }

            };


            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDoctor(long IUnitId1, long iDeptId1)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbUnit.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId1;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
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

                    cmbDocor.ItemsSource = null;
                    cmbDocor.ItemsSource = objList;
                    cmbDocor.SelectedItem = objList[0];
                    if (AdmissionDetails != null)
                    {
                        cmbDocor.SelectedValue = AdmissionDetails.DoctorID;
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillBedCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
            BizAction.IsActive = true;
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

                    cmbBedCategory.ItemsSource = null;
                    cmbBedCategory.ItemsSource = objList;
                    cmbBedCategory.SelectedItem = objList[0];

                    if (AdmissionDetails != null)
                    {
                        cmbBedCategory.SelectedValue = AdmissionDetails.BedCategoryID;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillWard()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_WardMaster;
            BizAction.IsActive = true;
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

                    cmbWard.ItemsSource = null;
                    cmbWard.ItemsSource = objList;
                    cmbWard.SelectedItem = objList[0];

                    
                    if (AdmissionDetails != null)
                    {
                        cmbWard.SelectedValue = AdmissionDetails.WardID;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillBed()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_BedMaster;
            BizAction.IsActive = true;
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

                    cmbBed.ItemsSource = null;
                    cmbBed.ItemsSource = objList;
                    cmbBed.SelectedItem = objList[0];



                    if (AdmissionDetails != null)
                    {
                        cmbBed.SelectedValue = AdmissionDetails.BedID;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDispensingType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DischargeType;
            BizAction.IsActive = true;
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

                    cmbDischargeType.ItemsSource = null;
                    cmbDischargeType.ItemsSource = objList;
                    cmbDischargeType.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDischargeType.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeType;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDischargeDestination()
           {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DischargeDestination;
            BizAction.IsActive = true;
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

                    cmbDischargeDestination.ItemsSource = null;
                    cmbDischargeDestination.ItemsSource = objList;
                    cmbDischargeDestination.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbDischargeDestination.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeDestination;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillDischargingDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDischargingDoctor.ItemsSource = null;
                    cmbDischargingDoctor.ItemsSource = objList;
                    cmbDischargingDoctor.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbDischargingDoctor.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeDoctor;
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbUnit.SelectedItem != null)
                FillDepartmentList(((MasterListItem)cmbUnit.SelectedItem).ID);
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
                FillDoctor(((MasterListItem)cmbUnit.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
        }
        #endregion

        private void Discharge_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            this.DataContext = new clsIPDDischargeVO 
            {
                DischargeDate=DateTime.Now,
                DischargeTime=DateTime.Now
            };
            
            FillGender();
            FillClinic();
            FillBedCategory();
            FillWard();
            FillBed();
            FillDispensingType();
            FillDischargeDestination();
            FillDischargingDoctor();
            FetchData();
        }
        
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            ClearData();
            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                clsPatientGeneralVO ObjPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                GetPatientIPDDetails(ObjPatient.PatientID, ObjPatient.UnitId, ObjPatient.MRNo);
            }
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
        }


        #region PatientList
        /// <summary>
        /// Purpose:Display patient list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearchbyMRNo_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.Show();

        }

     
        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                clsPatientGeneralVO Obj = ((IApplicationConfiguration)App.Current).SelectedPatient;
                GetPatientIPDDetails(Obj.PatientID, Obj.UnitId, Obj.MRNo);               
            }
        }

        #endregion
      
        #region Get Admission details
        /// <summary>
        /// Purpose:To get selected patient admission details
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pUnitID"></param>
        /// <param name="mrno"></param>
        private void GetPatientIPDDetails(long pID, long pUnitID, string mrno)
        {
            clsGetIPDPatientDetailsBizActionVO BizAction = new clsGetIPDPatientDetailsBizActionVO();
            try
            {
                BizAction.PatientDetails = new clsIPDAdmissionVO();
                BizAction.PatientID = pID;
                BizAction.PatientUnitID = pUnitID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.MRNo = mrno;

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if ((clsGetIPDPatientDetailsBizActionVO)arg.Result != null)
                        {
                            AdmissionDetails = ((clsGetIPDPatientDetailsBizActionVO)arg.Result).PatientDetails;
                            if (AdmissionDetails != null)
                            {
                                PatientDetails.DataContext = AdmissionDetails;

                                cmbUnit.SelectedValue = AdmissionDetails.UnitId;
                                cmbDepartment.SelectedValue = AdmissionDetails.DepartmentID;
                                cmbDocor.SelectedValue = AdmissionDetails.DoctorID;
                                cmbBedCategory.SelectedValue = AdmissionDetails.BedCategoryID;
                                cmbWard.SelectedValue = AdmissionDetails.WardID;
                                cmbBed.SelectedValue = AdmissionDetails.BedID;

                                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0) //For new discharge details
                                {
                                    txtMRNo.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                    txtFirstName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                                    txtMiddleName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                                    txtLastName.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                                    dtpDOB.SelectedDate = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                                    cmbGender.SelectedItem = ((IApplicationConfiguration)App.Current).SelectedPatient.Gender;
                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                }
                                else  //for existing discharge details
                                {
                                    txtMRNo.Text = ((clsIPDDischargeVO)this.DataContext).MRNo;
                                    txtFirstName.Text = ((clsIPDDischargeVO)this.DataContext).FirstName;
                                    txtMiddleName.Text = ((clsIPDDischargeVO)this.DataContext).MiddleName;
                                    txtLastName.Text = ((clsIPDDischargeVO)this.DataContext).LastName;
                                    cmbGender.SelectedValue = ((clsIPDDischargeVO)this.DataContext).GenderID;
                                    dtpDOB.SelectedDate = ((clsIPDDischargeVO)this.DataContext).DateOfBirth;
                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                }
                                
                            }

                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client1.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        
        #region Calculate Age from Birthdate
        /// <summary>
        /// Purpose:Get Age from birthdate.
        /// </summary>
        /// <param name="Datevalue"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;

                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        private DateTime? ConvertDateBack(string parameter, int value, DateTime? DateTobeConvert)
        {
            try
            {
                DateTime BirthDate;
                if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                    BirthDate = DateTobeConvert.Value;
                else
                    BirthDate = DateTime.Now;


                int mValue = Int32.Parse(value.ToString());

                switch (parameter.ToString().ToUpper())
                {
                    case "YY":
                        BirthDate = BirthDate.AddYears(-mValue);

                        break;
                    case "MM":
                        BirthDate = BirthDate.AddMonths(-mValue);
                        // result = (age.Month - 1).ToString();
                        break;
                    case "DD":
                        //result = (age.Day - 1).ToString();
                        BirthDate = BirthDate.AddDays(-mValue);
                        break;
                    default:
                        BirthDate = BirthDate.AddYears(-mValue);
                        break;
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }

        }

        #endregion

        #region Save Data

        /// <summary>
        /// Purpose:To save new discharge details
        /// </summary>
        private void Save()
        {
            clsAddIPDDischargeBizActionVO BizAction = new clsAddIPDDischargeBizActionVO();
            try
            {
                BizAction.DischargeDetails = (clsIPDDischargeVO)this.DataContext;
                BizAction.DischargeDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.DischargeDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.DischargeDetails.IPDAdmissionID = AdmissionDetails.ID;
                BizAction.DischargeDetails.IPDAdmissionNo = AdmissionDetails.AdmissionNO;

                if (cmbDischargingDoctor.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeDoctor = ((MasterListItem)cmbDischargingDoctor.SelectedItem).ID;


                if (cmbDischargeType.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeType = ((MasterListItem)cmbDischargeType.SelectedItem).ID;


                if (cmbDischargeDestination.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeDestination = ((MasterListItem)cmbDischargeDestination.SelectedItem).ID;

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("New");
                    if (arg.Error == null && arg.Result != null)
                    {
                        ClearData();
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        objAnimation.Invoke(RotationType.Backward);
                        // FetchData();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Discharge details added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();


                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client1.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Discharge details?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }

        #endregion
       
        #region Modify Data
        /// <summary>
        /// Purpose:To modify existing discharge details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update the Discharge details?";

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
            clsAddIPDDischargeBizActionVO BizAction = new clsAddIPDDischargeBizActionVO();
            try
            {
                BizAction.DischargeDetails = (clsIPDDischargeVO)this.DataContext;

                if (cmbDischargingDoctor.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeDoctor = ((MasterListItem)cmbDischargingDoctor.SelectedItem).ID;


                if (cmbDischargeType.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeType = ((MasterListItem)cmbDischargeType.SelectedItem).ID;


                if (cmbDischargeDestination.SelectedItem != null)
                    BizAction.DischargeDetails.DischargeDestination = ((MasterListItem)cmbDischargeDestination.SelectedItem).ID;

                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("New");
                    if (arg.Error == null && arg.Result != null)
                    {
                        ClearData();
                        ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        objAnimation.Invoke(RotationType.Backward);
                        FetchData();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Discharge details updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();


                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client1.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// Purpose:Assign validations to controls.
        /// </summary>
        /// <returns></returns>

        private bool CheckValidation()
        {
            bool result = true;

            if (txtFirstName.Text == "")
            {
                txtFirstName.SetValidation("Please select First Name");
                txtFirstName.RaiseValidationError();
                txtFirstName.Focus();
                result = false;
            }
            else
                txtFirstName.ClearValidationError();



            if (txtLastName.Text == "")
            {
                txtLastName.SetValidation("Please select Last Name");
                txtLastName.RaiseValidationError();
                txtLastName.Focus();
                result = false;
            }
            else
                txtLastName.ClearValidationError();

            if (txtIPDNo.Text == "")
            {
                txtIPDNo.SetValidation("Please select IPD No.");
                txtIPDNo.RaiseValidationError();
                txtIPDNo.Focus();
                result = false;
            }
            else
                txtIPDNo.ClearValidationError();

            if ((MasterListItem)cmbDischargingDoctor.SelectedItem == null)
            {

                cmbDischargingDoctor.TextBox.SetValidation("Please Select Discharging Doctor");
                cmbDischargingDoctor.TextBox.RaiseValidationError();
                cmbDischargingDoctor.Focus();

                result = false;


            }
            else if (((MasterListItem)cmbDischargingDoctor.SelectedItem).ID == 0)
            {
                cmbDischargingDoctor.TextBox.SetValidation("Please Select Discharging Doctor");
                cmbDischargingDoctor.TextBox.RaiseValidationError();
                cmbDischargingDoctor.Focus();

                result = false;

            }
            else
                cmbDischargingDoctor.TextBox.ClearValidationError();

            if ((MasterListItem)cmbDischargeType.SelectedItem == null)
            {

                cmbDischargeType.TextBox.SetValidation("Please Select Discharge Type");
                cmbDischargeType.TextBox.RaiseValidationError();
                cmbDischargeType.Focus();

                result = false;


            }
            else if (((MasterListItem)cmbDischargeType.SelectedItem).ID == 0)
            {
                cmbDischargeType.TextBox.SetValidation("Please Select Discharge Type");
                cmbDischargeType.TextBox.RaiseValidationError();
                cmbDischargeType.Focus();

                result = false;

            }
            else
                cmbDischargeType.TextBox.ClearValidationError();

            if ((MasterListItem)cmbDischargeDestination.SelectedItem == null)
            {

                cmbDischargeDestination.TextBox.SetValidation("Please Select Discharge Type");
                cmbDischargeDestination.TextBox.RaiseValidationError();
                cmbDischargeDestination.Focus();

                result = false;


            }
            else if (((MasterListItem)cmbDischargeDestination.SelectedItem).ID == 0)
            {
                cmbDischargeDestination.TextBox.SetValidation("Please Select Discharge Destination");
                cmbDischargeDestination.TextBox.RaiseValidationError();
                cmbDischargeDestination.Focus();

                result = false;

            }
            else
                cmbDischargeDestination.TextBox.ClearValidationError();

           

            return result;

        }

        #endregion

        #region Clear Data
        /// <summary>
        /// Purpose:To clear data
        /// </summary>
        private void ClearData()
        {
            this.DataContext = new clsIPDDischargeVO
            {
                DischargeDate = DateTime.Now,
                DischargeTime = DateTime.Now
            };
            AdmissionDetails = new clsIPDAdmissionVO();
            PatientDetails.DataContext = null;

            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            txtYY.Text = "";
            txtDD.Text = "";
            txtMM.Text = "";
            txtMRNo.Text = "";
            dtpDOB.SelectedDate = null;

            cmbGender.SelectedValue = (long)0;
            cmbUnit.SelectedValue = (long)0;
            cmbDepartment.SelectedValue = (long)0;
            cmbDocor.SelectedValue = (long)0;

            cmbBedCategory.SelectedValue = (long)0;
            cmbWard.SelectedValue = (long)0;
            cmbBed.SelectedValue = (long)0;


            cmbDischargingDoctor.SelectedValue = (long)0;
            cmbDischargeType.SelectedValue = (long)0;
            cmbDischargeDestination.SelectedValue = (long)0;



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

        #region Getdata
        /// <summary>
        /// Purpose:Display list of existing records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }
        private void FetchData()
        {
            clsGetIPDDischargeBizActionVO BizAction = new clsGetIPDDischargeBizActionVO();
           
            try
            {
                BizAction.DischargeList = new List<clsIPDDischargeVO>();
                if (txtFirstName1.Text != "")
                    BizAction.FirstName = txtFirstName1.Text;
                if (txtLastName1.Text != "")
                    BizAction.LastName = txtLastName1.Text;

                if (dtpFromDate.SelectedDate != null)
                    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;

                if (dtpToDate.SelectedDate != null)
                    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date.Date;


                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if (((clsGetIPDDischargeBizActionVO)e.Result).DischargeList != null)
                        {
                            clsGetIPDDischargeBizActionVO result = e.Result as clsGetIPDDischargeBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.DischargeList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.DischargeList)
                                {
                                    DataList.Add(item);
                                }

                                dgPatientList.ItemsSource = null;
                                dgPatientList.ItemsSource = DataList;

                                dgDataPager.Source = null;
                                dgDataPager.PageSize = BizAction.MaximumRows;
                                dgDataPager.Source = DataList;

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
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
        #endregion

        #region View Dtails
        /// <summary>
        /// Purpose:View existing discharge details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlView_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            SetCommandButtonState("Modify");
            if (dgPatientList.SelectedItem != null)
            {

                this.DataContext = (clsIPDDischargeVO)dgPatientList.SelectedItem;
                cmbDischargingDoctor.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeDoctor;
                cmbDischargeType.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeType;
                cmbDischargeDestination.SelectedValue = ((clsIPDDischargeVO)this.DataContext).DischargeDestination;

                GetPatientIPDDetails(((clsIPDDischargeVO)this.DataContext).PatientID, ((clsIPDDischargeVO)this.DataContext).PatientUnitID, ((clsIPDDischargeVO)this.DataContext).MRNo);
                cmdSearchbyMrNo.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
            }

        }

        #endregion



    }
}
