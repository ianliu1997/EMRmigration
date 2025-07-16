using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using MessageBoxControl;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Data;
using PalashDynamics.Converters;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using System.Linq;
using PalashDynamics.ValueObjects.Master;
namespace EMR
{
    public partial class frmPatientEMRRefferal : UserControl
    {
        public clsPatientGeneralVO PatientDetail = new clsPatientGeneralVO();
        #region Constructor
        public frmPatientEMRRefferal()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsDoctorSuggestedServiceDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.Loaded += new RoutedEventHandler(frmPatientEMRRefferal_Loaded);

            FillDepartment();
        }
        #endregion

        #region DataMember
        DateConverter dateConverter;
        //string strErrorMsg = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ErrorProcessing_Msg");
        string strErrorMsg = "Error ocurred while processing.";
        string msgTitle = "Palash";
        public event RoutedEventHandler OnAddButton_Click;
        public ObservableCollection<clsDoctorSuggestedServiceDetailVO> DoctorList { get; set; }
        List<clsDoctorSuggestedServiceDetailVO> PatientReferralDeletedList = new List<clsDoctorSuggestedServiceDetailVO>();
        public Boolean IsLoaded { get; set; }
        public ObservableCollection<clsPatientPrescriptionDetailVO> PrescriptionList = new ObservableCollection<clsPatientPrescriptionDetailVO>();
        List<MasterListItem> objDoctorList { get; set; }
        List<MasterListItem> objAllDoctorList { get; set; }
        public clsVisitVO CurrentVisit { get; set; }
        public clsPatientPrescriptionDetailVO CurrentPrescription { get; set; }
        public bool IsEnabledControl = false;
        bool view = false;
        #endregion

        #region Paging

        public PagedSortableCollectionView<clsDoctorSuggestedServiceDetailVO> DataList { get; private set; }

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
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetReferralListHistory();
        }

        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        #region Loaded

        void frmPatientEMRRefferal_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
            {
                if (DoctorList == null)
                    DoctorList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();

                if (PatientReferralDeletedList == null)
                    PatientReferralDeletedList = new List<clsDoctorSuggestedServiceDetailVO>();

                dgReferralList.ItemsSource = DoctorList;

                GetReferralList();
                GetReferredList();
                GetReferralListHistory();
                IsLoaded = true;
            }
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
                this.IsEnabledControl = false;
            }
            else if (this.CurrentVisit.VisitTypeID == 1)
            {
                spSpecDoctor.Visibility = Visibility.Collapsed;
            }
            else
            {
                //spSpecDoctor.Visibility = Visibility.Visible;
                FillSpecialization();
                FillDoctor();
            }
             //DateTime d = CurrentVisit.Date;
             //if (d.ToString("d") != DateTime.Now.ToString("d"))
             //{
             //    cmdSave.IsEnabled = false;
             //    lnkAdd.IsEnabled = false;
             //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
                lnkAdd.IsEnabled = false;
            }
            //End

           // cmdSave.IsEnabled = IsEnabledControl;
         //   lnkAdd.IsEnabled = IsEnabledControl;
        }
        #endregion

        #region Fill DOCTER AND SPLIZATION COMBO
        private void FillDoctor()
        {
            //clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null && arg.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();
            //        objList.Add(new MasterListItem("0", "-- Select --"));
            //        objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
            //    }
            //};
            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(this.CurrentVisit.DoctorCode, this.CurrentVisit.Doctor));
            cmbspDoctor.ItemsSource = null;
            cmbspDoctor.ItemsSource = objList;
            cmbspDoctor.SelectedItem = objList[0];
            cmbspDoctor.IsEnabled = false;
        }
        private void FillSpecialization()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem("0", this.CurrentVisit.DoctorSpecialization));
            cmbSpecialization.ItemsSource = null;
            cmbSpecialization.ItemsSource = objList;
            cmbSpecialization.SelectedItem = objList[0];
            cmbSpecialization.IsEnabled = false;
        }
        #endregion

        #region Click Event
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //lnkSave_Click(sender, e);
            SaveReferral();
        }
        private void lnkSave_Click(object sender, RoutedEventArgs e)
        {
            // if (checkValidation())
            {
                string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
                MessageBoxControl.MessageBoxChildWindow msgW =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }
        #endregion

        #region ComboBox
        private void FillDepartment()
        {
            clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.DescriptionColumn = "Description";
            BizAction.CodeColumn = "ID";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("0", "-- Select --"));
                    objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillDepartmentWiseDoctor(string SpecialCode)
        {
            clsGetRSIJDoctorDepartmentDetailsBizActionVO BizAction = new clsGetRSIJDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsForReferral = true;
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
                BizAction.SpecialCode = SpecialCode;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("0", "-- Select --"));
                    objList.AddRange(((clsGetRSIJDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    cmbDoctor.ItemsSource = null;
                    if (((MasterListItem)cmbDepartment.SelectedItem).Description.Trim() == CurrentVisit.DoctorSpecialization.Trim())
                    {
                        if (CurrentVisit.DoctorCode != null)
                        {
                            MasterListItem objDoc = objList.Where(z => z.Code.Trim() == CurrentVisit.DoctorCode.Trim()).FirstOrDefault();
                            objList.Remove(objDoc);
                        }
                    }
                    cmbDoctor.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            cmbDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        }
                        cmbDoctor.SelectedItem = objList[0];
                    }
                    else
                        cmbDoctor.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        bool checkValidation()
        {
            if (cmbDepartment.SelectedItem == null || ((MasterListItem)cmbDepartment.SelectedItem).Code == "0")
            {
                string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Msg_SelectSpecial");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return false;
            }
            //else if (cmbDoctor.SelectedItem == null || ((MasterListItem)cmbDoctor.SelectedItem).Code == "0")
            //{
            //    string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Msg_SelectDoctor");
            //    ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    return false;
            //}
            else if (dgReferralList.ItemsSource == null)
            {
                string msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Msg_SelectReferral");
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveReferral();
            }
        }

        public void SaveReferral()
        {
            WaitIndicator IndicatiorDiag = new WaitIndicator();
            try
            {
                IndicatiorDiag.Show();
                List<clsDoctorSuggestedServiceDetailVO> SavePatientDiagnosiList = new List<clsDoctorSuggestedServiceDetailVO>();
                foreach (clsDoctorSuggestedServiceDetailVO item in DoctorList)
                    SavePatientDiagnosiList.Add(item);
                foreach (clsDoctorSuggestedServiceDetailVO item in PatientReferralDeletedList)
                {
                    item.Status = false;
                    SavePatientDiagnosiList.Add(item);
                }
                if (SavePatientDiagnosiList != null && SavePatientDiagnosiList.Count > 0)
                {
                    clsAddUpdateReferralDetailsBizActionVO BizActionVO = new clsAddUpdateReferralDetailsBizActionVO();
                    BizActionVO.PatientID = CurrentVisit.PatientId;
                    BizActionVO.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizActionVO.VisitID = CurrentVisit.ID;
                    BizActionVO.ReferalDoctID = CurrentVisit.ReferredDoctorID;
                    BizActionVO.DoctCode = CurrentVisit.DoctorCode;
                    BizActionVO.DoctorSuggestedServiceDetail = SavePatientDiagnosiList;
                    BizActionVO.DeptmtCode = CurrentVisit.DepartmentCode;
                    BizActionVO.DeptmtName = CurrentVisit.Department;
                    BizActionVO.IsOPDIPD = false;
                    BizActionVO.SpecializationCode = ((MasterListItem)cmbDepartment.SelectedItem).Code;
                    BizActionVO.SpecializationName = ((MasterListItem)cmbDepartment.SelectedItem).Description;
                    BizActionVO.DoctorID = CurrentVisit.DoctorID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddUpdateReferralDetailsBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                IndicatiorDiag.Close();
                                //string strSaveMsg = DefaultValues.ResourceManager.GetString("RecordSavePrompt");
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //         new MessageBoxControl.MessageBoxChildWindow("Palash", strSaveMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                //{
                                //    this.Content = null;
                                //    NavigateToNextMenu();
                                //};
                                //msgW1.Show();
                                //this.Content = null;
                                //NavigateToNextMenu();
                                DoctorList = new ObservableCollection<clsDoctorSuggestedServiceDetailVO>();
                                GetReferralList();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                IndicatiorDiag.Close();
                                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            IndicatiorDiag.Close();
                            ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                    };
                    client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    IndicatiorDiag.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Doctor And Specailization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            }
            catch (Exception)
            {
                IndicatiorDiag.Close();
                ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }

        }

        private void NavigateToNextMenu()
        {
            UserControl winEMR;
            if (this.CurrentVisit.VisitTypeID == 2)
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmIPDEMR;
            }
            else
            {
                winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            }
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //string msgText = DefaultValues.ResourceManager.GetString("DiscardChanges");
            //MessageBoxControl.MessageBoxChildWindow msgWinCancel =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWinCancel.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinCancel_OnMessageBoxClosed);
            //msgWinCancel.Show();
            this.Content = null;
            NavigateToNextMenu();
        }

        void msgWinCancel_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                this.Content = null;
                NavigateToNextMenu();
            }
        }

        private void lnkAddService_Click(object sender, RoutedEventArgs e)
        {
            if (checkValidation())
            {
                clsDoctorSuggestedServiceDetailVO ObjDoctorSugg = new clsDoctorSuggestedServiceDetailVO();
                ObjDoctorSugg.ReferralLetter = new clsEMRReferralLetterVO();
                ObjDoctorSugg.IsRefferal = true;
                if (cmbDepartment.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem).Code != "0")
                {
                    ObjDoctorSugg.ReferalSpecialization = ((MasterListItem)cmbDepartment.SelectedItem).Description;
                    ObjDoctorSugg.ReferalSpecializationCode = ((MasterListItem)cmbDepartment.SelectedItem).Code;
                    ObjDoctorSugg.ReferralLetter.ReferalSpeciality = ObjDoctorSugg.ReferalSpecialization;
                    ObjDoctorSugg.ReferralLetter.ReferalSpecialityCode = ObjDoctorSugg.ReferalSpecializationCode;
                }
                if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).Code != "0")
                {
                    ObjDoctorSugg.ReferalDoctorCode = ((MasterListItem)cmbDoctor.SelectedItem).Code;
                    ObjDoctorSugg.ReferalDoctor = ((MasterListItem)cmbDoctor.SelectedItem).Description;
                }

                if (DoctorList.Where(z => ObjDoctorSugg.ReferalSpecialization == z.ReferalSpecialization && ObjDoctorSugg.ReferalDoctorCode == z.ReferalDoctorCode).Any())
                {
                    string msgText = "Is Already Added"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("Msg_DoctorAlreadyAdded");
                    ShowMessageBox("Dr." + ObjDoctorSugg.DoctorName + "" + msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
                else
                {
                    ObjDoctorSugg.Specialization = CurrentVisit.DoctorSpecialization;
                    ObjDoctorSugg.ReferralLetter.ReferredSpeciality = CurrentVisit.DoctorSpecialization;
                    ObjDoctorSugg.ReferralLetter.ReferredDoctor = CurrentVisit.Doctor;
                    ObjDoctorSugg.ReferralLetter.ReferredDoctorCode = CurrentVisit.DoctorCode;
                    ObjDoctorSugg.DoctorName = CurrentVisit.Doctor;
                    ObjDoctorSugg.DoctorCode = CurrentVisit.DoctorCode;
                    ObjDoctorSugg.PrintFlag = "Collapsed";
                    DoctorList.Add(ObjDoctorSugg);
                    cmbDepartment.SelectedItem = new MasterListItem("0", "-- Select --");
                    cmbDoctor.SelectedItem = new MasterListItem("0", "-- Select --");
                }
            }
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmServiceSelectionList)sender).DialogResult == true)
            {
                foreach (var item in (((frmServiceSelectionList)sender).ServiceList))
                {
                    clsDoctorSuggestedServiceDetailVO OBj = new clsDoctorSuggestedServiceDetailVO();
                    OBj.ServiceID = item.ServiceID;
                    OBj.ServiceName = item.ServiceName;
                    OBj.IsRefferal = true;
                    DoctorList.Add(OBj);
                }
                dgReferralList.ItemsSource = DoctorList;
                dgReferralList.UpdateLayout();
                dgReferralList.Focus();
            }

        }

        public void FillData()
        {
            view = true;
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgReferralList.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Service ?"; //((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ConfirmDeleteService_Msg");
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsDoctorSuggestedServiceDetailVO objVo = (clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem;
                        objVo.Status = false;
                        DoctorList.RemoveAt(dgReferralList.SelectedIndex);

                        if (objVo.ID != null && objVo.ID > 0)
                            PatientReferralDeletedList.Add(objVo);
                        dgReferralList.ItemsSource = null;
                        dgReferralList.ItemsSource = DoctorList;
                        dgReferralList.UpdateLayout();
                        dgReferralList.Focus();
                    }
                };
                msgWD.Show();
            }
        }

        private void GetReferralList()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            try
            {
                IndicatiorGet.Show();
                clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.IsOPDIPD = false;
                    BizAction.DoctorID = CurrentVisit.DoctorID;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if (arg.Error == null)
                    {
                        if (arg.Result != null && ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
                        {
                            foreach (var item in ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail)
                            {
                                DoctorList.Add(item);
                            }
                        }
                        dgReferralList.ItemsSource = null;
                        dgReferralList.ItemsSource = DoctorList;
                        dgReferralList.UpdateLayout();
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                        ShowMessageBox(strErrorMsg, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                IndicatiorGet.Close();
            }
        }

        private void GetReferredList()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            try
            {
                IndicatiorGet.Show();
                clsGetReferredDetailsBizActionVO BizAction = new clsGetReferredDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                    BizAction.IsOPDIPD = false;
                    BizAction.DoctorID = CurrentVisit.DoctorID;
                }
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if (arg.Error == null)
                    {
                        if (arg.Result != null && ((clsGetReferredDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetailforReferred != null)
                        {
                            //foreach (var item in ((clsGetReferredDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetailforReferred)
                            //{
                            //    DoctorList.Add(item);
                            //}
                            dgReferralList1.ItemsSource = null;
                            dgReferralList1.ItemsSource = ((clsGetReferredDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetailforReferred;
                            dgReferralList1.UpdateLayout();
                        }
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                        ShowMessageBox(strErrorMsg, MessageBoxButtons.Ok, MessageBoxIcon.Error);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                IndicatiorGet.Close();
            }
        }

        private void GetReferralListHistory()
        {
            WaitIndicator IndicatiorGet = new WaitIndicator();
            List<clsDoctorSuggestedServiceDetailVO> PatientServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
            try
            {
                IndicatiorGet.Show();
                clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = CurrentVisit.ID;
                    BizAction.PatientID = CurrentVisit.PatientId;
                    BizAction.DoctorID = CurrentVisit.DoctorID;
                }
                BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                BizAction.Ishistory = true;
                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    IndicatiorGet.Close();

                    if (arg.Result != null)
                    {
                        dateConverter = new DateConverter();
                        if (((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
                        {
                            DataList.TotalItemCount = ((clsGetReferralDetailsBizActionVO)arg.Result).TotalRows;
                            DataList.Clear();
                            foreach (var item in ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail)
                            {
                                PatientServiceList.Add(item);
                                DataList.Add(item);
                                item.Datetime = String.Format(item.Date.ToString() + " - " + CurrentVisit.Doctor.Trim() + " - " + CurrentVisit.DoctorSpecialization);
                            }
                        }
                        PagedCollectionView pcvRefferalListHistory = new PagedCollectionView(PatientServiceList);
                        if (CurrentVisit.VisitTypeID == 2)
                            pcvRefferalListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Datetime"));
                        else
                            pcvRefferalListHistory.GroupDescriptions.Add(new PropertyGroupDescription("Date", dateConverter));

                        dgPreviousServiceList.ItemsSource = null;
                        dgPreviousServiceList.ItemsSource = pcvRefferalListHistory;
                        dgPreviousServiceList.UpdateLayout();
                        pgrPatientRefferal.Source = DataList;
                        IndicatiorGet.Close();
                    }
                    else
                    {
                        IndicatiorGet.Close();
                        ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                IndicatiorGet.Close();
            }
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDepartment.SelectedItem != null && ((MasterListItem)cmbDepartment.SelectedItem).Code != "0")
            {

                FillDepartmentWiseDoctor(((MasterListItem)cmbDepartment.SelectedItem).Code);
            }
        }

        //private void cmdAddReferel_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenReferal(true);
        //}

        void Refeeral_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmReferralLetter)sender).DialogResult == true)
            {
                ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).ReferralLetter = (clsEMRReferralLetterVO)(((frmReferralLetter)sender)).DataContext;
            }

        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllReferralLetterPrint.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).PrescriptionID + "&ID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).ID + "&UnitID="+CurrentVisit.UnitId), "_blank");
        }
        //private void cmdAcknowledgement_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenReferal(false);
        //}
        public void OpenReferal(bool IsFromReferral)
        {
            frmReferralLetter Win = new frmReferralLetter();
            EMR.frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            Win.Height = winEMR.ActualHeight * 0.8;
            clsDoctorSuggestedServiceDetailVO ReferralVO = null;
            clsDoctorSuggestedServiceDetailVO SelectedItem = new clsDoctorSuggestedServiceDetailVO();
            if (dgReferralList.SelectedItem != null)
            {
                ReferralVO = (clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem;
                SelectedItem = ReferralVO;
            }

            if (SelectedItem.ReferralLetter != null)
            {
                SelectedItem.ReferralLetter.ID = SelectedItem.ID;
                Win.DataContext = SelectedItem.ReferralLetter;
            }
            if (ReferralVO.DoctorName != null)
                Win.Drname = SelectedItem.DoctorName;
            ((clsEMRReferralLetterVO)Win.DataContext).PatientName = PatientDetail.PatientName;

            if (SelectedItem.IsRefferal == false)
                ((clsEMRReferralLetterVO)Win.DataContext).ReferalDoctor = CurrentVisit.Doctor;

            if (SelectedItem.ID == 0)
                ((clsEMRReferralLetterVO)Win.DataContext).Date = DateTime.Today;

            Win.CntReferral.IsEnabled = IsFromReferral;
            Win.CntReply.IsEnabled = !IsFromReferral;

            Win.OnAddButton_Click += new RoutedEventHandler(Refeeral_OnAddButton_Click);
            Win.Show();
        }
        private void btnPrintRefrralletter_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null)
                if ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem != null)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllNewReferralLetterPrint.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).PrescriptionID + "&ID=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList.SelectedItem).ID + "&ISOPDIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID), "_blank");
                }
        }
        private void btnPrintthankletter_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null)
                if ((clsDoctorSuggestedServiceDetailVO)dgReferralList1.SelectedItem != null)
                {
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllNewThankingLetterPrint.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&ReferralDr=" + ((clsDoctorSuggestedServiceDetailVO)dgReferralList1.SelectedItem).ReferalDoctorCode + "&CurrentDr=" + CurrentVisit.DoctorID + "&ISOPDIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID), "_blank");
                }
        }
    }
}

