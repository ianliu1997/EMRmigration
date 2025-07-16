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
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.Converters;
using System.Windows.Data;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;

namespace EMR
{
    public partial class frmFollowpNotes : UserControl
    {
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsEnableControl { get; set; }
        public Patient SelectedPatient { get; set; }
        public string SelectedUser { get; set; }
        WaitIndicator Indicatior = null;
        ObservableCollection<clsPastFollowUpnoteVO> PastChiefComplaintList { get; set; }

        #region Paging
        public PagedSortableCollectionView<clsPastFollowUpnoteVO> DataList { get; private set; }
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
            FillPreviousChiefComplaints();
        }
        #endregion


        public frmFollowpNotes()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsPastFollowUpnoteVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 10;
        }

        int ClickFlag = 0;
        private void cmdFollowupSave_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator IndicatiorDiag = new WaitIndicator();
            if (Convert.ToInt32(txtFollowUpNotes.Text.Length) > 0 && txtFollowUpNotes.Text != null)
            {
                ClickFlag += 1;
                if (ClickFlag == 1)
                {
                    IndicatiorDiag.Show();
                    clsAddUpdatePatientFollowupNotesBizActionVO BizAction = new clsAddUpdatePatientFollowupNotesBizActionVO();
                    BizAction.VisitID = this.CurrentVisit.ID;
                    BizAction.VisitUnitID = this.CurrentVisit.UnitId;
                    BizAction.PatientID = this.CurrentVisit.PatientId;
                    BizAction.PatientUnitID = this.CurrentVisit.PatientUnitId;
                    BizAction.DoctorID = CurrentVisit.DoctorID;
                    BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizAction.CurrentFollowUpNotes = this.DataContext as clsEMRFollowNoteVO;
                    try
                    {
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {

                            if (arg.Error == null && arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Record Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                            else
                            {
                                ShowMessageBox("Error ocurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                            IndicatiorDiag.Close();
                            ClickFlag = 0;
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                    catch (Exception)
                    {
                        ClickFlag=0;
                        IndicatiorDiag.Close();
                        throw;
                    }
                }
            }
            else
            {
                IndicatiorDiag.Close();
                ShowMessageBox("Please Enter Follow Up Notes !!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PastChiefComplaintList = new ObservableCollection<clsPastFollowUpnoteVO>();
            this.DataContext = new clsEMRFollowNoteVO();
            FetchFollowUpNote();
            FillPreviousChiefComplaints();
            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdFollowupSave.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdFollowupSave.IsEnabled = false;
            }
            //End
        }
        private void FetchFollowUpNote()
        {
            clsGetPatientFollowUpNoteBizActionVO BizAction = new clsGetPatientFollowUpNoteBizActionVO();
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.UnitID = CurrentVisit.UnitId;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            //BizAction.DoctorCode = CurrentVisit.DoctorCode;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetPatientFollowUpNoteBizActionVO Obj = e.Result as clsGetPatientFollowUpNoteBizActionVO;
                    this.DataContext = Obj.CurrentFollowUPNotes;
                    if (this.DataContext == null)
                        this.DataContext = new clsEMRFollowNoteVO();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        Boolean ComboFlag = false;
        private void FillPreviousChiefComplaints()
        {
            clsGetPatientPastFollowUPNotesBizActionVO BizAction = new clsGetPatientPastFollowUPNotesBizActionVO();
            BizAction.VisitID = CurrentVisit.ID;
            BizAction.PatientUnitID = CurrentVisit.PatientUnitId;
            BizAction.PatientID = CurrentVisit.PatientId;
            BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
            BizAction.DoctorID = CurrentVisit.DoctorID;
            BizAction.PagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetPatientPastFollowUPNotesBizActionVO Obj = e.Result as clsGetPatientPastFollowUPNotesBizActionVO;
                    lbxPreviousnotes.ItemsSource = null;
                    if (Obj.PastFollowUPList != null)
                    {
                        DataList.TotalItemCount = ((clsGetPatientPastFollowUPNotesBizActionVO)e.Result).TotalRows;
                        DataList.Clear();

                        ComboFlag = false;
                        List<clsPastFollowUpnoteVO> lstChiefComplaint = new List<clsPastFollowUpnoteVO>();
                        lstChiefComplaint = Obj.PastFollowUPList;
                        foreach (var item in lstChiefComplaint)
                        {
                            DataList.Add(item);
                        }
                        DateConverter dateConverter = new DateConverter();
                        PastChiefComplaintList = new ObservableCollection<clsPastFollowUpnoteVO>(lstChiefComplaint);
                        PagedCollectionView pcv = new PagedCollectionView(lstChiefComplaint);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("VisitDate", dateConverter));
                        lbxPreviousnotes.ItemsSource = null;
                        lbxPreviousnotes.ItemsSource = pcv;
                        txtPreviousnotes.Text = lstChiefComplaint[0].Notes;
                        pgrPatientfollowUpNote.Source = DataList;
                        ComboFlag = true;
                    }
                    lbxPreviousnotes.UpdateLayout();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void lbxPreviousnotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ComboFlag == true)
                {
                    clsPastFollowUpnoteVO item = lbxPreviousnotes.SelectedItem as clsPastFollowUpnoteVO;
                    txtPreviousnotes.Text = Convert.ToString(item.Notes);
                }
            }
            catch (Exception ee)
            {

            }
        }
        private void cmdfollowupCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashBoard();
        }
        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }
    }
}
