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
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.IPD
{
    public partial class AdmissionDetailsWin : UserControl, IInitiateCIMS
    {
        #region list And Veriables
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsIPDBedMasterVO> MasterNonCensusList { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        clsIPDAdmMLCDetailsVO _MedicoLegalCase = null;
        bool IsBreak = false;
        List<clsIPDBedMasterVO> SelectedBed = new List<clsIPDBedMasterVO>();
        long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public bool IsFromLoaded = true; // this flag is for showing the Message box of Bed.
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
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
        MessageBoxResult result = MessageBoxResult.Yes;
        long itemID = 0;
        #endregion
        #region Constructor
        public AdmissionDetailsWin()
        {
            InitializeComponent();
        }
        #endregion 
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCensusBedGrid();
        }
        void MasterNonCensusList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillNonCensusBedGrid();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                SetBorderColors();
                MasterList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
                dataGridCensusPager.PageSize = 5;
                dataGridCensusPager.Source = MasterList;
                dgCensusBedList.ItemsSource = MasterList;

                MasterNonCensusList = new PagedSortableCollectionView<clsIPDBedMasterVO>();
                MasterNonCensusList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterNonCensusList_OnRefresh);
                DataPager.PageSize = 5;
                DataPager.Source = MasterNonCensusList;
                dgNonCensusBed.ItemsSource = MasterNonCensusList;
                dtpEffectiveDate.SelectedDate = System.DateTime.Now;

                SelectedBed = new List<clsIPDBedMasterVO>();
                dtpEffectiveDate.SelectedDate = System.DateTime.Now;
                tpToTime.Value = System.DateTime.Now;
                cmbUnit.IsEnabled = false;
                FillUnit();
                FillClass();
                // FillWard(); Commented By Bhushan And Added New GetWardListByClassID 20042017
                GetWardListByClassID();
                FillPatientSource();
                FillAdmissionType();
                FillRefEntity();
                FillRefEntityType();
                FillCensusBedGrid();
                FillNonCensusBedGrid();
                IsFromLoaded = false;
                CheckValidations();
            }
            IsPageLoded = true;
        }

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {

            switch (Mode)
            {

                case "NEWR":
                    IsPatientExist = true;
                    PatientID = -1;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                    {
                        UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                        TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                        TextBlock mElement_1 = (TextBlock)rootPage1.FindName("SampleHeader");
                        mElement_1.Text = "Sponsor Details";
                        mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                        mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    }
                    this.DataContext = new clsIPDAdmissionVO()
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID,
                        CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID,
                        TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID,
                        IPDNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo
                    };

                    break;

                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");


                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsIPDAdmissionVO()
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID,
                        CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID,
                        TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID,
                        IPDNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo
                    };
                    IsPatientExist = true;
                    break;

                case "EDIT":
                    EditMode = true;
                    IsPatientExist = true;
                    break;
            }
        }

        #endregion
        public long sAppointmentID = 0;
        private bool EditMode = false;
        private bool TariffEditFirst = false;
        private bool AssociateCompanyAtEdit = false;
        long PatientID = 0;
        bool IsPatientExist = false;

        private void SetBorderColors()
        {
            cmbAdmissionType.BorderBrush = new SolidColorBrush(Colors.Red);
        }
        #region FillComboBoxex
        private void FillUnit()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); 
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList.DeepCopy();
                    cmbUnit.SelectedValue = UnitID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDepatment(long iUnitId)
        {
            #region Commented By Bhushanp 20042017
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
            //        if (((MasterListItem)cmbUnit.SelectedItem).ID == 0)
            //        {
            //            var results = from a in objList
            //                          group a by a.ID into grouped
            //                          select grouped.First();
            //            objList = results.ToList();
            //        }
            //        cmbDepartmet.ItemsSource = null;
            //        cmbDepartmet.ItemsSource = objList;
            //        cmbDepartmet.SelectedItem = objList[0];
            //        if ((MasterListItem)cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID != 0)
            //        {
            //            if (((clsGetMasterListBizActionVO)arg.Result).MasterList.Count > 0)
            //                cmbDepartmet.SelectedItem = ((clsGetMasterListBizActionVO)arg.Result).MasterList[0];
            //        }
            //        else
            //        {
            //            cmbDepartmet.SelectedItem = ((MasterListItem)objList[0]);
            //        }
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
            #endregion
            // New Add By Bhushan 20042017 
  
            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionVo = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
            BizActionVo.IsUnitWise = true;
            BizActionVo.IsClinical = true;  // flag use to Show/not Clinical Departments  02032017
            BizActionVo.UnitID = iUnitId;   // fill Unitwise Departments  02032017

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem != null)      // changes for - to show/not clinical Depatments
                    {
                        objList.AddRange(((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem);
                    }
                    cmbDepartmet.ItemsSource = null;
                    cmbDepartmet.ItemsSource = objList;
                    cmbDepartmet.SelectedItem = ((MasterListItem)objList[0]);

                    FillAdmittingDoctor(iUnitId, ((MasterListItem)cmbDepartmet.SelectedItem).ID);
                }
            };
            Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);   //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillAdmittingDoctor(long IUnitId1, long iDeptId1)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbUnit.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId1;
            }
            if ((MasterListItem)cmbDepartmet.SelectedItem != null)
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
                    if (iDeptId1 > 0)
                    {
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    }
                    var myNewList = objList.OrderBy(i => i.Description);
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = myNewList.ToList();
                    cmbDoctor.SelectedItem = myNewList.ToList()[0];
                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                    {
                        cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    }
                    if ((MasterListItem)cmbDepartmet.SelectedItem != null && ((MasterListItem)cmbDepartmet.SelectedItem).ID != 0)
                    {
                        if (((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList.Count > 0)
                            cmbDoctor.SelectedItem = ((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList[0];
                    }
                    else
                    {
                        cmbDoctor.SelectedItem = ((MasterListItem)objList[0]);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillClass()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                    cmbClassName.ItemsSource = null;
                    cmbClassName.ItemsSource = objList.DeepCopy();
                    cmbClassName.SelectedItem = objList[0];

                    cmbBilling.ItemsSource = null;
                    cmbBilling.ItemsSource = objList.DeepCopy();
                    cmbBilling.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillWard()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_WardMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                   // objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList); //Comment By Bhushan 20042017
                    cmbward.ItemsSource = null;
                    cmbward.ItemsSource = objList.DeepCopy();
                    cmbward.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPatientSource()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
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
                    cmbPatSource.ItemsSource = null;
                    cmbPatSource.ItemsSource = objList.DeepCopy();
                    cmbPatSource.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillAdmissionType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_AdmissionType;
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
                    cmbAdmissionType.ItemsSource = null;
                    cmbAdmissionType.ItemsSource = objList.DeepCopy();
                    cmbAdmissionType.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillRefEntity()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RefEntityMaster;
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
                    cmbRefEntity.ItemsSource = null;
                    cmbRefEntity.ItemsSource = objList;
                    cmbRefEntity.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillRefEntityType()
        {

        }

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbUnit.SelectedItem).ID != 0)
            {
                FillDepatment(((MasterListItem)cmbUnit.SelectedItem).ID);
            }
            else
            {
                if (((MasterListItem)cmbUnit.SelectedItem).ID == 0)
                {
                    FillDepatment(((MasterListItem)cmbUnit.SelectedItem).ID);
                }
                if (((MasterListItem)cmbUnit.SelectedItem).ID == 0 && ((MasterListItem)cmbDepartmet.SelectedItem != null))
                {
                    FillAdmittingDoctor(((MasterListItem)cmbUnit.SelectedItem).ID, ((MasterListItem)cmbDepartmet.SelectedItem).ID);
                }
            }
        }

        private void cmbDepartmet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbDepartmet.SelectedItem).ID != 0)
            {
                FillAdmittingDoctor(((MasterListItem)cmbUnit.SelectedItem).ID, ((MasterListItem)cmbDepartmet.SelectedItem).ID);
                cmbDoctor.IsEnabled = true;
            }
        }

        #endregion

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;

        public clsPatientVO myPatient { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to save the Admission Details";
            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            msgW.Show();
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveAdmission();
        }
        private void SaveAdmission()
        {

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        ChildWindow BedStatus = new ChildWindow();
        private void cmdSearchBed_Click(object sender, RoutedEventArgs e)
        {
            //frmBedStatus win = new frmBedStatus(false, false);
            //BedStatus.Content = win;
            //BedStatus.Height = 500;
            //BedStatus.Width = 900;
            //BedStatus.Title = "Bed Status";
            //win.OnSaveButton_Click += new RoutedEventHandler(frmBedStatus_OnSaveButton);
            //BedStatus.Show();
        }
        void frmBedStatus_OnSaveButton(object sender, RoutedEventArgs e)
        {
            //foreach (var item in MasterList)
            //{
            //    if (item.BedCategoryID == ((frmBedStatus)sender).newBedCategoryID && item.ID == ((frmBedStatus)sender).newBedID && item.UnitID == ((frmBedStatus)sender).newBedUnitID)
            //    {
            //        SelectedBed.Add(item);

            //        if (SelectedBed.Count > 1)
            //        {
            //            foreach (var item1 in MasterList)
            //            {
            //                if (item1.ID == item.ID)
            //                {
            //                    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure You want to chang the bed?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);

            //                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosedNew);
            //                    itemID = item.ID;
            //                    msgW1.Show();
            //                }
            //                else
            //                {
            //                    if (item1.ID == item.ID)
            //                        item.Status = true;
            //                    else
            //                        item.Status = false;
            //                }
            //            }
            //        }
            //    }
            //}
            //dgCensusBedList.ItemsSource = null;
            //dgCensusBedList.ItemsSource = MasterList;
            //foreach (var item in MasterNonCensusList)
            //{
            //    if (item.BedCategoryID == ((frmBedStatus)sender).newBedCategoryID && item.ID == ((frmBedStatus)sender).newBedID && item.UnitID == ((frmBedStatus)sender).newBedUnitID)
            //    {
            //        SelectedBed.Add(item);

            //        if (SelectedBed.Count > 1)
            //        {
            //            foreach (var item1 in MasterNonCensusList)
            //            {
            //                if (item1.ID == item.ID)
            //                {
            //                    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure You want to chang the bed?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
            //                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosedNew2);
            //                    itemID = item.ID;
            //                    msgW1.Show();
            //                }
            //                else
            //                {
            //                    if (item1.ID == item.ID)
            //                        item.Status = true;
            //                    else
            //                        item.Status = false;
            //                }
            //            }
            //        }
            //    }
            //}
            //dgNonCensusBed.ItemsSource = null;
            //dgNonCensusBed.ItemsSource = MasterNonCensusList;
            //BedStatus.Close();
        }

        #region FillDataGrid
        public void FillCensusBedGrid()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.IsNonCensus = false;
                if (cmbClassName.SelectedItem != null && !((MasterListItem)cmbClassName.SelectedItem).ID.Equals(0))
                {
                    bizActionVO.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
                if (cmbward.SelectedItem != null && !((MasterListItem)cmbward.SelectedItem).ID.Equals(0))
                {
                    bizActionVO.WardID = ((MasterListItem)cmbward.SelectedItem).ID;
                }
                if (cmbUnit.ItemsSource != null)
                {
                    if (cmbUnit.SelectedItem != null && !((MasterListItem)cmbUnit.SelectedItem).ID.Equals(0))
                    {
                        bizActionVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }
                    else
                    {
                        bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                    }
                }
                else
                {
                    bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterList.PageSize;
                bizActionVO.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionVO.objBedMasterDetails = new List<clsIPDBedMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objBedMasterDetails = (((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).objBedMasterDetails);
                        if (bizActionVO.objBedMasterDetails.Count > 0)
                        {
                            MasterList.TotalItemCount = Convert.ToInt32(((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).TotalRows);
                            MasterList.Clear();
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterList.Add(item);
                            }
                            dgCensusBedList.ItemsSource = null;
                            dgCensusBedList.ItemsSource = MasterList.DeepCopy();
                            dgCensusBedList.SelectedIndex = -1;
                            dataGridCensusPager.Source = null;
                            dataGridCensusPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            dataGridCensusPager.Source = MasterList;
                        }
                        else
                        {
                            dgCensusBedList.ItemsSource = null;
                            dataGridCensusPager.Source = null;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void FillNonCensusBedGrid()
        {
            try
            {
                clsIPDGetBedCensusAndNonCensusListBizActionVO bizActionVO = new clsIPDGetBedCensusAndNonCensusListBizActionVO();
                bizActionVO.IsNonCensus = true;
                if (cmbClassName.SelectedItem != null && !((MasterListItem)cmbClassName.SelectedItem).ID.Equals(0))
                {
                    bizActionVO.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
                if (cmbward.SelectedItem != null && !((MasterListItem)cmbward.SelectedItem).ID.Equals(0))
                {
                    bizActionVO.WardID = ((MasterListItem)cmbward.SelectedItem).ID;
                }
                if (cmbUnit.ItemsSource != null)
                {
                    if (cmbUnit.SelectedItem != null && !((MasterListItem)cmbUnit.SelectedItem).ID.Equals(0))
                    {
                        bizActionVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                    }
                    else
                    {
                        bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                    }
                }
                else
                {
                    bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }
                bizActionVO.PagingEnabled = true;
                bizActionVO.MaximumRows = MasterNonCensusList.PageSize;
                bizActionVO.StartRowIndex = MasterNonCensusList.PageIndex * MasterNonCensusList.PageSize;
                bizActionVO.objBedMasterDetails = new List<clsIPDBedMasterVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.objBedMasterDetails = (((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).objBedMasterDetails);
                        if (bizActionVO.objBedMasterDetails.Count > 0)
                        {
                            MasterNonCensusList.TotalItemCount = Convert.ToInt32(((clsIPDGetBedCensusAndNonCensusListBizActionVO)args.Result).TotalRows);
                            MasterNonCensusList.Clear();
                            foreach (clsIPDBedMasterVO item in bizActionVO.objBedMasterDetails)
                            {
                                MasterNonCensusList.Add(item);
                            }
                            dgNonCensusBed.ItemsSource = null;
                            dgNonCensusBed.ItemsSource = MasterNonCensusList.DeepCopy();
                            dgNonCensusBed.SelectedIndex = -1;
                            DataPager.Source = null;
                            DataPager.PageSize = Convert.ToInt32(bizActionVO.MaximumRows);
                            DataPager.Source = MasterNonCensusList;
                        }
                        else
                        {
                            dgNonCensusBed.ItemsSource = null;
                            DataPager.Source = null;
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        private void BedInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        ChildWindow appoitmentForm = null;
        private void chkMedicoLegal_Click(object sender, RoutedEventArgs e)
        {
            //appoitmentForm = new ChildWindow();
            //frmMedicoLegalCaseWindow win = new frmMedicoLegalCaseWindow();
            //win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
            //win.OnClosebutton_Click += new RoutedEventHandler(win_OnClosebutton_Click);
            //appoitmentForm.Content = win;
            //appoitmentForm.Show();
        }

        void win_OnClosebutton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                chkMedicoLegal.IsChecked = false;
                appoitmentForm.Close();
            }
        }
        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                _MedicoLegalCase = ((clsIPDAdmMLCDetailsVO)sender);
            }
            else
            {
                _MedicoLegalCase = new clsIPDAdmMLCDetailsVO();
                chkMedicoLegal.IsChecked = false;
            }
            appoitmentForm.Close();
        }

        private void cmbRefEntity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbRefEntity.SelectedItem).ID == 0)
            {
                cmbRefEntityType.ItemsSource = null;
                cmbRefEntityType.Text = string.Empty;
            }
            else if (((MasterListItem)cmbRefEntity.SelectedItem).ID == 1)
            {
                cmbRefEntityType.ItemsSource = null;
                cmbRefEntityType.Text = string.Empty;
                fillDoctor(true);
            }
            else if (((MasterListItem)cmbRefEntity.SelectedItem).ID == 3)
            {
                cmbRefEntityType.ItemsSource = null;
                cmbRefEntityType.Text = string.Empty;
            }
            else if (((MasterListItem)cmbRefEntity.SelectedItem).ID == 2)
            {
                cmbRefEntityType.ItemsSource = null;
                cmbRefEntityType.Text = string.Empty;
                fillDoctor(false);
            }
            else if (((MasterListItem)cmbRefEntity.SelectedItem).ID > 0)
            {
                cmbRefEntityType.Text = string.Empty;
            }
        }
        private void fillDoctor(bool IsInternal)
        {
            //Created By Bhushanp
            clsGetDoctorListBizActionVO BizAction = new clsGetDoctorListBizActionVO();
            BizAction.DoctorDetailsList = new List<clsDoctorVO>();
            BizAction.IsComboBoxFill = true;
            if (IsInternal)
            {
                BizAction.IsInternal = true;
                BizAction.IsExternal = false;
            }
            else
            {
                BizAction.IsInternal = false;
                BizAction.IsExternal = true;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    if (((clsGetDoctorListBizActionVO)arg.Result).MasterList != null)
                    {
                        objList.AddRange(((clsGetDoctorListBizActionVO)arg.Result).MasterList);
                    }
                    cmbRefEntityType.ItemsSource = null;
                    cmbRefEntityType.ItemsSource = objList;

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            //Commented By Bhushanp
            //clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //        cmbRefEntityType.ItemsSource = null;
            //        cmbRefEntityType.ItemsSource = objList;
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
        }

        private void fillCompany()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbRefEntityType.ItemsSource = null;
                    cmbRefEntityType.ItemsSource = objList;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "--Select--";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtAutocompleteText_KeyUp(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void lnkSearch_Click(object sender, RoutedEventArgs e)
        {
            PrimarySymptomsSearchWindowForIPD Win = new PrimarySymptomsSearchWindowForIPD();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PrimarySymptomsSearchWindowForIPD Itemswin = (PrimarySymptomsSearchWindowForIPD)sender;
            txtProvisionalDiagnosis.Text = string.Empty;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.SelectedItems != null)
                {
                    string strSymptoms = "";
                    foreach (var item in Itemswin.SelectedItems)
                    {
                        strSymptoms = strSymptoms + item.Description;
                        strSymptoms = strSymptoms + ",";
                    }
                    if (strSymptoms.EndsWith(","))
                        strSymptoms = strSymptoms.Remove(strSymptoms.Length - 1, 1);

                    string prevSymptoms = "";

                    if (!String.IsNullOrEmpty(txtProvisionalDiagnosis.Text))
                        prevSymptoms = txtProvisionalDiagnosis.Text;

                    if (prevSymptoms.Length > 0)
                        txtProvisionalDiagnosis.Text = prevSymptoms + "," + strSymptoms;
                    else
                        txtProvisionalDiagnosis.Text = strSymptoms;
                }
            }
        }

        private void dgCensusBedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dgCensusBedList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgCensusBedList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void chkCensus_Click(object sender, RoutedEventArgs e)
        {
            foreach (clsIPDBedMasterVO item in MasterList)
            {
                item.Status = false;
                if ((clsIPDBedMasterVO)dgCensusBedList.SelectedItem != null)
                {
                    if (item.BedID == ((clsIPDBedMasterVO)dgCensusBedList.SelectedItem).BedID)
                    {
                        item.Status = true;
                    }
                }
            }

            dgCensusBedList.ItemsSource = null;
            dgCensusBedList.ItemsSource = MasterList;
            //if (dgCensusBedList.SelectedItem != null)
            //{
            //    SelectedBed.Add((clsIPDBedMasterVO)dgCensusBedList.SelectedItem);
            //    int count = SelectedBed.Count();
            //    if (SelectedBed.Count > 1)
            //    {
            //        foreach (var item in MasterList)
            //        {
            //            if (item.BedID == ((clsIPDBedMasterVO)dgCensusBedList.SelectedItem).BedID)
            //            {
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure You want to change the bed?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
            //                {
            //                    if (res == MessageBoxResult.Yes)
            //                    {
            //                        foreach (var item1 in MasterList)
            //                        {
            //                            if (item1.BedID == ((clsIPDBedMasterVO)dgCensusBedList.SelectedItem).BedID)
            //                                item1.Status = true;
            //                            else
            //                                item1.Status = false;
            //                        }
            //                        foreach (var item2 in MasterNonCensusList)
            //                        {
            //                            item2.Status = false;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        foreach (var item2 in MasterList)
            //                        {
            //                            if (item2.BedID == SelectedBed[count - 1].BedID)
            //                            {
            //                                IsBreak = true;
            //                                item2.Status = false;
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    dgCensusBedList.ItemsSource = null;
            //                    dgCensusBedList.ItemsSource = MasterList;
            //                    dgNonCensusBed.ItemsSource = null;
            //                    dgNonCensusBed.ItemsSource = MasterNonCensusList;
            //                };
            //                msgW1.Show();
            //                if (IsBreak == true)
            //                {
            //                    break;
            //                }
            //                break;
            //            }
            //            else
            //            {
            //                item.Status = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        foreach (var item in MasterList)
            //        {
            //            if (item.BedID == ((clsIPDBedMasterVO)dgCensusBedList.SelectedItem).BedID)
            //                item.Status = true;
            //            else
            //                item.Status = false;
            //        }
            //    }
            //}
            //dgCensusBedList.ItemsSource = null;
            //dgCensusBedList.ItemsSource = MasterList;
        }


        private void chkNonCensus_Click(object sender, RoutedEventArgs e)
        {
            if (dgNonCensusBed.SelectedItem != null)
            {
                SelectedBed.Add((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem);
                int count = SelectedBed.Count();
                if (SelectedBed.Count > 1)
                {
                    foreach (var item in MasterNonCensusList)
                    {
                        if (item.BedID == ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).BedID)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure You want to change the bed?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.OnMessageBoxClosed += (MessageBoxResult result) =>
                            {
                                if (result == MessageBoxResult.Yes)
                                {
                                    foreach (var item1 in MasterNonCensusList)
                                    {
                                        if (item1.BedID == ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).BedID)
                                            item1.Status = true;
                                        else
                                            item1.Status = false;
                                    }
                                    foreach (var item2 in MasterList)
                                    {
                                        item2.Status = false;
                                    }
                                }
                                else
                                {
                                    foreach (var item2 in MasterNonCensusList)
                                    {
                                        if (item2.BedID == SelectedBed[count - 1].BedID)
                                        {
                                            IsBreak = true;
                                            item2.Status = false;
                                            break;
                                        }
                                    }
                                }
                                dgCensusBedList.ItemsSource = null;
                                dgCensusBedList.ItemsSource = MasterList;
                                dgNonCensusBed.ItemsSource = null;
                                dgNonCensusBed.ItemsSource = MasterNonCensusList;
                            }; msgW1.Show();
                            if (IsBreak == true)
                            {
                                break;
                            }
                            break;
                        }
                        else
                        {
                            item.Status = false;
                        }
                    }
                }
                else
                {
                    foreach (var item in MasterNonCensusList)
                    {
                        if (item.BedID == ((clsIPDBedMasterVO)dgNonCensusBed.SelectedItem).BedID)
                            item.Status = true;
                        else
                            item.Status = false;
                    }
                }
            }
            dgNonCensusBed.ItemsSource = null;
            dgNonCensusBed.ItemsSource = MasterNonCensusList;
        }

        void msgWin_OnMessageBoxClosedNew(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in MasterList)
                {
                    if (item.ID == itemID)
                    {
                        item.Status = true;
                    }
                    else
                    {
                        item.Status = false;
                    }
                }
                foreach (var item1 in MasterNonCensusList)
                {
                    item1.Status = false;
                }
            }
            dgCensusBedList.ItemsSource = null;
            dgCensusBedList.ItemsSource = MasterList;
            dgNonCensusBed.ItemsSource = null;
            dgNonCensusBed.ItemsSource = MasterNonCensusList;
        }
        void msgWin_OnMessageBoxClosedNew2(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item3 in MasterNonCensusList)
                {
                    if (item3.ID == itemID)
                    {
                        item3.Status = true;
                    }
                    else
                    {
                        item3.Status = false;
                    }
                }
                foreach (var itemM in MasterList)
                {
                    itemM.Status = false;
                }
                dgCensusBedList.ItemsSource = null;
                dgCensusBedList.ItemsSource = MasterList;
                dgNonCensusBed.ItemsSource = null;
                dgNonCensusBed.ItemsSource = MasterNonCensusList;
            }
        }

        private void cmbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetWardListByClassID();
            FillCensusBedGrid();
            FillNonCensusBedGrid();
        }

        private void cmbward_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillCensusBedGrid();
            FillNonCensusBedGrid();
        }

        public bool CheckValidations()
        {
            bool result = true;
            try
            {
                //if (txtContactNo1.Text == "")
                //{
                //    txtContactNo1.Textbox.SetValidation("Please Enter Mobile No.");
                //    txtContactNo1.Textbox.RaiseValidationError();
                //    txtContactNo1.Focus();
                //    result = false;
                //}
                //else
                //    txtContactNo1.Textbox.ClearValidationError();
              
                //Added by AJ Date 13/12/2016
                if (cmbUnit.SelectedItem == null || ((MasterListItem)cmbUnit.SelectedItem).ID == 0)
                {
                    cmbUnit.TextBox.SetValidation("Please select Unit");
                    cmbUnit.TextBox.RaiseValidationError();
                    cmbUnit.Focus();
                    result = false;
                }
                else
                    cmbUnit.TextBox.ClearValidationError();

                if (cmbDepartmet.SelectedItem == null || ((MasterListItem)cmbDepartmet.SelectedItem).ID == 0)
                {
                    cmbDepartmet.TextBox.SetValidation("Please select Departmet");
                    cmbDepartmet.TextBox.RaiseValidationError();
                    cmbDepartmet.Focus();
                    result = false;
                }
                else
                    cmbDepartmet.TextBox.ClearValidationError();

                if (cmbDoctor.SelectedItem == null || ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                {
                    cmbDoctor.TextBox.SetValidation("Please select Doctor");
                    cmbDoctor.TextBox.RaiseValidationError();
                    cmbDoctor.Focus();
                    result = false;
                }
                else
                    cmbDoctor.TextBox.ClearValidationError();

                //***//--------------------------------
                if (cmbAdmissionType.SelectedItem == null || ((MasterListItem)cmbAdmissionType.SelectedItem).ID == 0)
                {
                    cmbAdmissionType.TextBox.SetValidation("Please select Admission Type");
                    cmbAdmissionType.TextBox.RaiseValidationError();
                    cmbAdmissionType.Focus();
                    result = false;
                }
                else
                    cmbAdmissionType.TextBox.ClearValidationError();

                //if (cmbBilling.SelectedItem == null || ((MasterListItem)cmbBilling.SelectedItem).ID == 0)
                //{
                //    cmbBilling.TextBox.SetValidation("Please select Billing Class");
                //    cmbBilling.TextBox.RaiseValidationError();
                //    cmbBilling.Focus();
                //    result = false;
                //}
                //else
                //    cmbBilling.TextBox.ClearValidationError();

                bool IsValid = false;
                foreach (var item in MasterList)
                {
                    if (item.Status == true)
                    {
                        IsValid = true;
                        break;
                    }
                }
                if (IsValid == false)
                {
                    foreach (var item2 in MasterNonCensusList)
                    {
                        if (item2.Status == true)
                        {
                            IsValid = true;
                            break;
                        }
                    }
                }

                if (IsValid == false && IsFromLoaded)
                {
                    result = false;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the bed?", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                }

            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }

        public void GetWardListByClassID()
        {
            try
            {
                clsGetIPDWardByClassIDBizActionVO bizActionVO = new clsGetIPDWardByClassIDBizActionVO();
                bizActionVO.BedDetails = new clsIPDBedTransferVO();
                if (cmbClassName.SelectedItem != null && !((MasterListItem)cmbClassName.SelectedItem).ID.Equals(0))
                {
                    bizActionVO.BedDetails.ClassID = ((MasterListItem)cmbClassName.SelectedItem).ID;
                }
                else
                {
                    bizActionVO.BedDetails.ClassID = 0;
                }
                if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
                {
                    bizActionVO.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                } 
                else
                {
                    bizActionVO.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.BedList = ((clsGetIPDWardByClassIDBizActionVO)args.Result).BedList;
                        if (bizActionVO.BedList != null && bizActionVO.BedList.Count > 0)
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            objList.Add(new MasterListItem(0, "- Select -"));
                            foreach (clsIPDBedTransferVO item in bizActionVO.BedList)
                            {
                                objList.Add(new MasterListItem(item.WardID, item.Ward));
                            }
                            cmbward.ItemsSource = null;
                            cmbward.ItemsSource = objList.DeepCopy();
                            cmbward.SelectedItem = objList[0];
                        }
                        else
                        {
                            List<MasterListItem> objList = new List<MasterListItem>();
                            objList.Add(new MasterListItem(0, "- Select -"));                            
                            cmbward.ItemsSource = null;
                            cmbward.ItemsSource = objList.DeepCopy();
                            cmbward.SelectedItem = objList[0];
                        }
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
