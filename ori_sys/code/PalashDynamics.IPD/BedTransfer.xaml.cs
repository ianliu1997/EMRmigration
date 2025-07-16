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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;

namespace PalashDynamics.IPD
{
    public partial class BedTransfer : UserControl,IInitiateCIMS
    {
        #region Variable Declaration
        SwivelAnimation objAnimation;
        long ID { get; set; }
        clsIPDAdmissionVO AdmissionDetails { get; set; }

        #endregion

        public BedTransfer()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            //Paging
            DataList = new PagedSortableCollectionView<clsIPDBedTransferVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }
        
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":           
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;


            }
        }

        #endregion


        #region Paging

        public PagedSortableCollectionView<clsIPDBedTransferVO> DataList { get; private set; }


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

        private void BedTransfer_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            this.DataContext = new clsIPDBedTransferVO();
            FillGender();
            FillClinic();
            FillBedCategory();
            FillWard();
            FillBed();
           
            FetchData();
            
        }

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
                    cmbGender.ItemsSource = objList;
                    
                    if (this.DataContext != null)
                    {
                        cmbGender.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).GenderID;
                    }

                   
                }

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

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];

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


                    //if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
                    //{

                    //    var results = from a in objList
                    //                  group a by a.ID into grouped
                    //                  select grouped.First();
                    //    objList = results.ToList();
                    //}

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

                    cmbBedCategory1.ItemsSource = null;
                    cmbBedCategory1.ItemsSource = objList;
                    cmbBedCategory1.SelectedItem = objList[0];

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

                    cmbWard1.ItemsSource = null;
                    cmbWard1.ItemsSource = objList;
                    cmbWard1.SelectedItem = objList[0];

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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            this.SetCommandButtonState("Save");
            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                clsPatientGeneralVO ObjPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;

                GetPatientIPDDetails(ObjPatient.PatientID, ObjPatient.UnitId, ObjPatient.MRNo);

                var results = from r in DataList
                              where (r.PatientID == ObjPatient.PatientID)
                              && (r.PatientUnitID == ObjPatient.UnitId)
                              && (r.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                              select r;
                if (results.ToList().Count > 0)
                {
                    dgTransfer.ItemsSource = null;
                    dgTransfer.ItemsSource = results.ToList(); ;
                }  
            }

            objAnimation.Invoke(RotationType.Forward);
            
        }

        #region Save Data

        /// <summary>
        /// Purpose:To save bed transfer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Bed transfer details?";

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

        private void Save()
        {
            clsAddIPDBedTransferBizActionVO BizAction = new clsAddIPDBedTransferBizActionVO();
            try
            {
                BizAction.BedDetails = (clsIPDBedTransferVO)this.DataContext;
                BizAction.BedDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.BedDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.BedDetails.IPDAdmissionID = AdmissionDetails.ID;
                BizAction.BedDetails.IPDAdmissionNo = AdmissionDetails.AdmissionNO;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                     {
                         SetCommandButtonState("New");
                         if (arg.Error == null && arg.Result != null)
                         {
                             ClearData();
                             objAnimation.Invoke(RotationType.Backward);
                             FetchData();
                             MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("", "Bed transfer details added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        #region Modify Data
        /// <summary>
        /// Purpose:Modify selected patient bed transfer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Update the Bed transfer details?";

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
            clsAddIPDBedTransferBizActionVO BizAction = new clsAddIPDBedTransferBizActionVO();
            try
            {
                BizAction.BedDetails = (clsIPDBedTransferVO)this.DataContext;
              
                
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                Client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        SetCommandButtonState("New");
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);
                        FetchData();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Bed transfer details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            mElement.Text = "";
            objAnimation.Invoke(RotationType.Backward);
            SetCommandButtonState("Cancel");
        }

        #region Patient List
        /// <summary>
        /// Purpose:Get patient list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearchByMrNo_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch WinP = new PatientSearch();
            WinP.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            WinP.Show();
        }
        
        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PatientSearch ObjWin = (PatientSearch)sender;
            if (ObjWin.DialogResult == true)
            {
                clsPatientGeneralVO Obj=((IApplicationConfiguration)App.Current).SelectedPatient;
                GetPatientIPDDetails(Obj.PatientID, Obj.UnitId, Obj.MRNo);

                var results = from r in DataList
                              where (r.PatientID == Obj.PatientID)
                              && (r.PatientUnitID == Obj.UnitId)
                              && (r.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                              select r;
                if (results.ToList().Count > 0)
                {
                    dgTransfer.ItemsSource = null;
                    dgTransfer.ItemsSource = results.ToList(); ;
                } 
            }
        }

        #endregion

        #region Get Bed Details
        /// <summary>
        /// Purpose:Get list of vacant bed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdBedSearch_Click(object sender, RoutedEventArgs e)
        {
            frmSearchBed win = new frmSearchBed();
            if (cmbBedCategory1.SelectedItem != null)
                ((clsIPDBedTransferVO)this.DataContext).BedCategoryID = ((MasterListItem)cmbBedCategory1.SelectedItem).ID;

            if (cmbWard1.SelectedItem != null)
                ((clsIPDBedTransferVO)this.DataContext).WardID = ((MasterListItem)cmbWard1.SelectedItem).ID;


            win.WardID = ((clsIPDBedTransferVO)this.DataContext).WardID;
            win.BedCategoryID = ((clsIPDBedTransferVO)this.DataContext).BedCategoryID;
            win.OnOKButton_Click += new RoutedEventHandler(win_OnOKButton_Click);
            win.Show();
          

        }
        void win_OnOKButton_Click(object sender, RoutedEventArgs e)
        {
            frmSearchBed win = (frmSearchBed)sender;

            if (win != null)
            {
                ((clsIPDBedTransferVO)this.DataContext).BedCategoryID = win.BedCategoryID;
                ((clsIPDBedTransferVO)this.DataContext).WardID = win.WardID;
                ((clsIPDBedTransferVO)this.DataContext).BedID = win.BedID;
                ((clsIPDBedTransferVO)this.DataContext).BedNo = win.BedNO;

                txtBedNo.Text = ((clsIPDBedTransferVO)this.DataContext).BedNo;
                cmbWard1.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).WardID;
                cmbBedCategory1.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).BedCategoryID;

            }
        }
        #endregion
        
        #region Get Admission details
        /// <summary>
        /// Purpose:Get patient admission details.
        /// </summary>
        /// <param name="pID"></param>
        /// <param name="pUnitID"></param>
        /// <param name="mrno"></param>
        private void GetPatientIPDDetails(long pID,long pUnitID,string mrno)
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

                                     if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
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
                                     else
                                     {
                                         txtMRNo.Text = ((clsIPDBedTransferVO)this.DataContext).MRNo;
                                         txtFirstName.Text = ((clsIPDBedTransferVO)this.DataContext).FirstName;
                                         txtMiddleName.Text = ((clsIPDBedTransferVO)this.DataContext).MiddleName;
                                         txtLastName.Text = ((clsIPDBedTransferVO)this.DataContext).LastName;
                                         cmbGender.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).GenderID;
                                         dtpDOB.SelectedDate = ((clsIPDBedTransferVO)this.DataContext).DateOfBirth;
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
       
        #region Getdata
        /// <summary>
        /// Purpose:Get list of existing records of bed transfer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();

        }
        private void FetchData()
        {
            clsGetIPDBedTransferBizActionVO BizAction = new clsGetIPDBedTransferBizActionVO();
            try
            {
                BizAction.BedList = new List<clsIPDBedTransferVO>();
                if(txtFirstName1.Text!="")
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
                        if (((clsGetIPDBedTransferBizActionVO)e.Result).BedList != null)
                        {
                            clsGetIPDBedTransferBizActionVO result = e.Result as clsGetIPDBedTransferBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.BedList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.BedList)
                                {
                                    DataList.Add(item);
                                }

                                dgTransferList.ItemsSource = null;
                                dgTransferList.ItemsSource = DataList;

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

        #region Calculate age from birthdate
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

        #region View Details
        /// <summary>
        /// Purpose:Display selected patient bed transfer details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlView_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            SetCommandButtonState("Modify");
            if (dgTransferList.SelectedItem != null)
            {
                cmdSearchByMrNo.IsEnabled = false;
                this.DataContext=(clsIPDBedTransferVO)dgTransferList.SelectedItem;
                GetPatientIPDDetails(((clsIPDBedTransferVO)this.DataContext).PatientID, ((clsIPDBedTransferVO)this.DataContext).PatientUnitID, ((clsIPDBedTransferVO)this.DataContext).MRNo);
                cmbClinic.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).UnitID;
                cmbBedCategory1.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).BedCategoryID;
                cmbWard1.SelectedValue = ((clsIPDBedTransferVO)this.DataContext).WardID;
                txtBedNo.Text = ((clsIPDBedTransferVO)this.DataContext).BedNo;
                var results = from r in DataList
                              where (r.PatientID == ((clsIPDBedTransferVO)dgTransferList.SelectedItem).PatientID)
                              && (r.PatientUnitID == ((clsIPDBedTransferVO)dgTransferList.SelectedItem).PatientUnitID)
                              && (r.UnitID == ((clsIPDBedTransferVO)dgTransferList.SelectedItem).UnitID)
                              select r;
                if (results.ToList().Count > 0)
                {
                    dgTransfer.ItemsSource = null;
                    dgTransfer.ItemsSource = results.ToList(); ;
                }
                objAnimation.Invoke(RotationType.Forward);

            }
        }
        #endregion

        #region ClearUI
        /// <summary>
        /// Purpose:To clear data
        /// </summary>
        private void ClearData()
        {
            
            AdmissionDetails = null;
            this.DataContext = new clsIPDBedTransferVO();
            PatientDetails.DataContext = null;

            txtMRNo.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";

            cmbGender.SelectedValue = (long)0;
            cmbUnit.SelectedValue = (long)0;
            cmbDepartment.SelectedValue = (long)0;
            cmbDocor.SelectedValue = (long)0;

            cmbBedCategory.SelectedValue = (long)0;
            cmbWard.SelectedValue = (long)0;
            cmbBed.SelectedValue = (long)0;

            dgTransfer.ItemsSource = null;
            cmbClinic.SelectedValue = (long)0;
            cmbBedCategory1.SelectedValue = (long)0;
            cmbWard1.SelectedValue = (long)0;
            txtBedNo.Text = "";

        }

        #endregion

        #region Validation
        /// <summary>
        /// Purpose:Assign validation to controls.
        /// </summary>
        /// <returns></returns>
        private bool CheckValidation()
        {
            bool result = true;
            if ((MasterListItem)cmbBedCategory1.SelectedItem == null)
            {

                cmbBedCategory1.TextBox.SetValidation("Please Select Bed Category");
                cmbBedCategory1.TextBox.RaiseValidationError();
                cmbBedCategory1.Focus();
              
                result = false;


            }
            else if (((MasterListItem)cmbBedCategory1.SelectedItem).ID == 0)
            {
                cmbBedCategory1.TextBox.SetValidation("Please Select Bed Category");
                cmbBedCategory1.TextBox.RaiseValidationError();
                cmbBedCategory1.Focus();
             
                result = false;

            }
            else
                cmbBedCategory1.TextBox.ClearValidationError();

            if ((MasterListItem)cmbWard1.SelectedItem == null)
            {

                cmbWard1.TextBox.SetValidation("Please Select Ward");
                cmbWard1.TextBox.RaiseValidationError();
                cmbWard1.Focus();

                result = false;


            }
            else if (((MasterListItem)cmbWard1.SelectedItem).ID == 0)
            {
                cmbWard1.TextBox.SetValidation("Please Select Ward");
                cmbWard1.TextBox.RaiseValidationError();
                cmbWard1.Focus();

                result = false;

            }
            else
                cmbWard1.TextBox.ClearValidationError();

            if (txtBedNo.Text == null)
            {
                txtBedNo.SetValidation("Please Select Bed No");
                txtBedNo.RaiseValidationError();
                txtBedNo.Focus();

                result = false;
            }
            else
                txtBedNo.ClearValidationError();

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
       
    }
}
