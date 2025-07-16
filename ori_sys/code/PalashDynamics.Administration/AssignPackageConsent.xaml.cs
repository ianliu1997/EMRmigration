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
using PalashDynamics.ValueObjects;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.PackageNew;

namespace PalashDynamics.Administration
{
    public partial class AssignPackageConsent : ChildWindow
    {
        public long ServiceID;
        clsCoupleVO _CoupleDetails = new clsCoupleVO();
        clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        public long PatientUnitID;
        public long PatientID;

        public AssignPackageConsent()
        {
            InitializeComponent();
            //_flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging BackPnel
            DataList = new PagedSortableCollectionView<clsPatientConsentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 7;

            // CoupleDetails = CoupleDetails_1;

            //Pagging Front Panel
            ConsentList = new PagedSortableCollectionView<clsPatientConsentVO>();
            ConsentList.OnRefresh += new EventHandler<RefreshEventArgs>(ConsentList_OnRefresh);
            ConsentListPageSize = 7;
            this.Title = "Consent's of  :-(Name- " + CoupleDetails.FemalePatient.FirstName +
                    " " + CoupleDetails.FemalePatient.LastName + ")";
        }


        //public frmFemaleConsent(long PatientID,long PatientUnitID , clsCoupleVO CoupleDetails_1)
        //{
        //    InitializeComponent();
        //    _flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

        //    //Paging BackPnel
        //    DataList = new PagedSortableCollectionView<clsPatientConsentVO>();
        //    DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        //    DataListPageSize = 7;

        //     CoupleDetails = CoupleDetails_1;

        //    //Pagging Front Panel
        //    ConsentList = new PagedSortableCollectionView<clsPatientConsentVO>();
        //    ConsentList.OnRefresh += new EventHandler<RefreshEventArgs>(ConsentList_OnRefresh);
        //    ConsentListPageSize = 7;
        //    this.Title = "Consent's of  :-(Name- " + CoupleDetails.FemalePatient.FirstName +
        //            " " + CoupleDetails.FemalePatient.LastName + ")";

        //}

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //FillTemplate();
            SelectedConsentDetails();
           
            FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            //GetConsent();
            //SetCommandButtonState("Load");
        }

        List<clsServiceMasterVO> SavedConsent = null;
        private void SelectedConsentDetails()
        {
            try
            {
                clsGetPackageConsentLinkBizActionVO BizAction = new clsGetPackageConsentLinkBizActionVO();
                BizAction.ServiceID = ServiceID;
            
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPackageConsentLinkBizActionVO)arg.Result).ServiceItemMasterDetails != null && ((clsGetPackageConsentLinkBizActionVO)arg.Result).ServiceItemMasterDetails.Count>0)
                        {
                            SavedConsent = ((clsGetPackageConsentLinkBizActionVO)arg.Result).ServiceItemMasterDetails;
                            TempConsentCheckedList = new List<clsServiceMasterVO>();
                            foreach (var item in SavedConsent)
                            {
                                item.IsConsentCheck = true;
                                TempConsentCheckedList.Add(item);
                            }
                           
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    FetchData();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #region Set Command Button State New/Save/Modify/Print

        //private void SetCommandButtonState(String strFormMode)
        //{
        //    switch (strFormMode)
        //    {
        //        case "Load":
        //            cmdCancel.IsEnabled = true;
        //            cmdNew.IsEnabled = true;
        //            IsCancel = true;
        //            break;
        //        case "New":
        //            cmdNew.IsEnabled = false;
        //            cmdCancel.IsEnabled = true;
        //            IsCancel = false;
        //            break;
        //        case "Save":
        //            cmdNew.IsEnabled = true;
        //            cmdCancel.IsEnabled = true;
        //            IsCancel = true;
        //            break;
        //        case "Modify":
        //            cmdNew.IsEnabled = true;
        //            cmdCancel.IsEnabled = true;
        //            IsCancel = true;
        //            break;
        //        case "Cancel":
        //            cmdNew.IsEnabled = true;
        //            cmdCancel.IsEnabled = true;
        //            break;
        //        case "View":
        //            cmdNew.IsEnabled = false;
        //            cmdCancel.IsEnabled = true;
        //            IsCancel = false;
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion

        #region Paging for BackPanel

        public PagedSortableCollectionView<clsPatientConsentVO> DataList { get; private set; }

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

        #region Paging for FontPanel

        public PagedSortableCollectionView<clsPatientConsentVO> ConsentList { get; private set; }

        public int ConsentListPageSize
        {
            get
            {
                return ConsentList.PageSize;
            }
            set
            {
                if (value == ConsentList.PageSize) return;
                ConsentList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        void ConsentList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //FetchData();
            //Get PatientConsent List
        }
        #endregion

        #region Variable Declaration
        public bool IsPatientExist = false;
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        bool IsCancel = true;
        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }
                    break;
            }
        }

        #endregion

        #region Fillcombobox
        //private void FillTemplate()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_PatientConsentMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

        //            cmbTemplate.ItemsSource = null;

        //            cmbTemplate.ItemsSource = objList;
        //            cmbTemplate.SelectedItem = objList[0];
        //        }

        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        private void FillTemplate(string DepartmentID)
        {
            //wait.Show();
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientConsentMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                if (DepartmentID != null)
                {
                    BizAction.Parent = new KeyValue { Key = DepartmentID, Value = "DepartmentID" };
                }
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbTemplate.ItemsSource = null;
                        cmbTemplate.ItemsSource = objList;
                        cmbTemplate.SelectedValue = objList[0].ID;
                    }

                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                //wait.Close();
            }
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
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        #region GetData FrontPanel
        //private void GetConsent()
        //{
        //    clsGetPatientConsentBizActionVO BizAction = new clsGetPatientConsentBizActionVO();
        //    try
        //    {
        //        BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
        //        BizAction.PagingEnabled = true;
        //        BizAction.MaximumRows = DataList.PageSize;
        //        BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
        //        //  BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //        BizAction.UnitID = PatientUnitID;
        //        BizAction.PatientID = PatientID;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                if (((clsGetPatientConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
        //                {
        //                    clsGetPatientConsentBizActionVO result = arg.Result as clsGetPatientConsentBizActionVO;
        //                    DataList.TotalItemCount = result.TotalRows;

        //                    if (result.ConsentMatserDetails != null)
        //                    {
        //                        DataList.Clear();

        //                        foreach (var item in result.ConsentMatserDetails)
        //                        {
        //                            DataList.Add(item);
        //                        }

        //                        dgPatientConsent.ItemsSource = null;
        //                        dgPatientConsent.ItemsSource = DataList;

        //                        dgDataPager.Source = null;
        //                        dgDataPager.PageSize = BizAction.MaximumRows;
        //                        dgDataPager.Source = DataList;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //                msgW1.Show();
        //            }
        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        #region GetData BackPanel
        private void FetchData()
        {
            clsGetPatientConsentMasterBizActionVO BizAction = new clsGetPatientConsentMasterBizActionVO();
            try
            {
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();

                if (cmbTemplate.SelectedItem != null)
                    BizAction.Template = ((MasterListItem)cmbTemplate.SelectedItem).ID;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPatientConsentMasterBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetPatientConsentMasterBizActionVO result = arg.Result as clsGetPatientConsentMasterBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    DataList.Add(item);
                                }

                                //foreach (var item in SavedConsent)
                                //{
                                //    foreach (var item1 in DataList)
                                //    {
                                //        if (item.TemplateID == item1.ID)
                                //            item1.IsConsentCheck = true;
                                //    }
                                //}

                                if (SavedConsent!=null && SavedConsent.Count > 0)
                                {
                                    (from a1 in DataList
                                     join a2 in SavedConsent on a1.ID equals a2.TemplateID
                                     select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => x.A1.IsConsentCheck = true);
                                }

                               

                                dgConsent.ItemsSource = null;
                                dgConsent.ItemsSource = DataList;

                                dataGrid2Pager.Source = null;
                                dataGrid2Pager.PageSize = BizAction.MaximumRows;
                                dataGrid2Pager.Source = DataList;
                            }
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
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }
        #endregion

        #region View Consent details
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            //if (dgConsent.SelectedItem != null)
            //{
            //    PatientConsentPrint Win = new PatientConsentPrint(PatientID, PatientUnitID);
            //    Win.DataContext = ((clsPatientConsentVO)dgConsent.SelectedItem);
            //    Win.IsSaved = false;
            //    Win.Show();
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                    new MessageBoxControl.MessageBoxChildWindow("", "Please select consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            //FillTemplate();
            //FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            //FetchData();
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

            //_flip.Invoke(RotationType.Forward);
            //this.SetCommandButtonState("New");

        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //if (IsCancel == true)
            //{
            this.DialogResult = false;
            //}
            //else
            //{
            //   // _flip.Invoke(RotationType.Backward);
            //    //GetConsent();
            //    //SetCommandButtonState("Cancel");
            //    IsCancel = true;
            //}
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                FillTemplate(((MasterListItem)cmbDepartment.SelectedItem).ID.ToString());
                //cmdSearch_Click(sender, e);
                //FillServiceGrid();
            }
        }

        List<clsServiceMasterVO> ConsentCheckedList = new List<clsServiceMasterVO>();
        List<clsServiceMasterVO> TempConsentCheckedList = new List<clsServiceMasterVO>();
        List<clsPatientConsentVO> TempVo = new List<clsPatientConsentVO>();

        private bool ValidateConsent()
        {
            bool result = true;
            ConsentCheckedList = TempConsentCheckedList.Where(x => x.IsConsentCheck == true).ToList();
            //TempVo = DataList.Where(x => x.IsConsentCheck == true).ToList();
            if (ConsentCheckedList.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
                //ConsentCheckedList = new List<clsServiceMasterVO>();
                //foreach (var item in TempVo)
                //{
                //    clsServiceMasterVO Obj = new clsServiceMasterVO();
                //    Obj.ServiceID = ServiceID;
                //    Obj.TemplateID = item.ID;
                //    Obj.Description = item.Description;
                //    Obj.DepartmentID = item.DepartmentID;
                //    if (item.IsConsentCheck)
                //        ConsentCheckedList.Add(Obj);
                //}                
            }
            //else
            //{
                
            //}
            return result;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateConsent())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {                
                try
                {
                    clsAddPackageConsentLinkBizActionVO BizAction = new clsAddPackageConsentLinkBizActionVO();
                    BizAction.ServiceID = ServiceID;
                    BizAction.ServiceItemMasterDetails = new List<clsServiceMasterVO>();
                    BizAction.ServiceItemMasterDetails = ConsentCheckedList;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (((clsAddPackageConsentLinkBizActionVO)arg.Result).SuccessStatus == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
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
                catch (Exception)
                {
                    throw;
                }


                //var selected = items.Where(item => item.Something > 10).ToList();
                //selected.ForEach(item => items.Remove(item));
                //otherList.AddRange(selected);

            }
        }

        private void chkConsentCheck_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //for (int i = 0; i < DataList.Count; i++)
            //{
            //    if (i == dgConsent.SelectedIndex)
            //    {
            //        if (chk.IsChecked == true)
            //        {
            //            clsServiceMasterVO Obj = new clsServiceMasterVO();
            //            Obj.ServiceID = ServiceID;
            //            Obj.TemplateID = DataList[i].ID;
            //            Obj.Description = DataList[i].Description;
            //            Obj.DepartmentID = DataList[i].DepartmentID;
            //            Obj.IsConsentCheck = true;
            //            TempConsentCheckedList.Add(Obj);
            //            //DataList[i].IsConsentCheck = true;
            //        }
            //    }
            //}

        }

        private void chkConsentCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //for (int i = 0; i < DataList.Count; i++)
            //{
            //    if (i == dgConsent.SelectedIndex)
            //    {
            //        if (chk.IsChecked == false)
            //        {
            //            clsServiceMasterVO Obj = new clsServiceMasterVO();
            //            Obj.ServiceID = ServiceID;
            //            Obj.TemplateID = DataList[i].ID;
            //            Obj.Description = DataList[i].Description;
            //            Obj.DepartmentID = DataList[i].DepartmentID;
            //            Obj.IsConsentCheck = false;
            //            TempConsentCheckedList.Add(Obj);
            //            //DataList[i].IsConsentCheck = false;
            //        }
            //    }
            //}
        }

    

        private void chkConsentCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk=sender as CheckBox;
            if (chk.IsChecked == true)
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    if (i == dgConsent.SelectedIndex)
                    {
                        clsServiceMasterVO Obj = new clsServiceMasterVO();
                        Obj.ServiceID = ServiceID;
                        Obj.TemplateID = DataList[i].ID;
                        Obj.Description = DataList[i].Description;
                        Obj.DepartmentID = DataList[i].DepartmentID;
                        Obj.IsConsentCheck = true;
                        TempConsentCheckedList.Add(Obj);
                    }
                }                   
            }
            else if (chk.IsChecked == false)
            {
                for (int i = 0; i < DataList.Count; i++)
                {
                    if (i == dgConsent.SelectedIndex)
                    {
                        TempConsentCheckedList.Where(x => x.TemplateID == ((clsPatientConsentVO)dgConsent.SelectedItem).ID).ToList().ForEach(x => x.IsConsentCheck = false);

                        //clsServiceMasterVO Obj = new clsServiceMasterVO();
                        //Obj.ServiceID = ServiceID;
                        //Obj.TemplateID = DataList[i].ID;
                        //Obj.Description = DataList[i].Description;
                        //Obj.DepartmentID = DataList[i].DepartmentID;
                        //Obj.IsConsentCheck = false;
                        //TempConsentCheckedList.Add(Obj);
                    }
                }    
            }
        }

        //private void hlbViewnPrintConsent_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgPatientConsent.SelectedItem != null)
        //    {
        //        PatientConsentPrint Win = new PatientConsentPrint(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
        //        Win.DataContext = ((clsPatientConsentVO)dgPatientConsent.SelectedItem);
        //        Win.IsSaved = true;
        //        Win.Show();
        //    }
        //    else
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                        new MessageBoxControl.MessageBoxChildWindow("", "Please select consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();
        //    }
        //}

        //private void hlbattachedsignconsent_Click(object sender, RoutedEventArgs e)
        //{
        //    long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
        //    long PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //    long ConsentID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).ID;
        //    long ConsentUnitID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).UnitID;

        //    FrmPatientSignConsent_IVFDashboard obj = null;
        //    obj = new FrmPatientSignConsent_IVFDashboard(PatientID, PatientUnitID, ConsentID, ConsentUnitID);

        //    obj.Show();
        //}
    }
}

