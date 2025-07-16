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
using System.Collections.ObjectModel;

using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.IVFPlanTherapy;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmSpremThawing : ChildWindow
    {

        WaitIndicator wait = new WaitIndicator();
        public long PatientID1 = 0;
        public long PatientUnitID1 = 0;
        public string Impression { get; set; }
        public Boolean IsEdit { get; set; }
        private ObservableCollection<cls_NewThawingDetailsVO> _SpremThawDetails = new ObservableCollection<cls_NewThawingDetailsVO>();
        public ObservableCollection<cls_NewThawingDetailsVO> SpremThawDetails
        {
            get { return _SpremThawDetails; }
            set { _SpremThawDetails = value; }
        }

        private ObservableCollection<clsNew_SpremFreezingVO> _VitriDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> VitriDetails
        {
            get { return _VitriDetails; }
            set { _VitriDetails = value; }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanList
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }

        private List<MasterListItem> _Plan = new List<MasterListItem>();
        public List<MasterListItem> SelectedPLan
        {
            get
            {
                return _Plan;
            }
            set
            {
                _Plan = value;
            }
        }

        private List<MasterListItem> _LabIncharge = new List<MasterListItem>();
        public List<MasterListItem> SelectedLabIncharge
        {
            get
            {
                return _LabIncharge;
            }
            set
            {
                _LabIncharge = value;
            }
        }

        public List<cls_NewThawingDetailsVO> ThawList;

        public PagedSortableCollectionView<cls_NewThawingDetailsVO> ThawDetailsList { get; private set; }
        public int ThawDetailsListPageSize
        {
            get
            {
                return ThawDetailsList.PageSize;
            }
            set
            {
                if (value == ThawDetailsList.PageSize) return;
                ThawDetailsList.PageSize = value;
            }
        }

        public frmSpremThawing(long PatientID, long PatientUnitID)
        {
            InitializeComponent();
            (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID = PatientID;
            (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId = PatientUnitID;
            this.Title = "Semen Thawing  :-(Name- " + (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).MalePatient.FirstName +
                   " " + (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).MalePatient.LastName + ")";
            PatientID1 = PatientID;
            PatientUnitID1 = PatientUnitID;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillCanID();
            ThawList = new List<cls_NewThawingDetailsVO>();
            ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
            ThawDetailsList.OnRefresh += new EventHandler<RefreshEventArgs>(ThawDetailsList_OnRefresh);
            ThawDetailsListPageSize = 15;
            this.ThawingDetilsGridPager.DataContext = ThawDetailsList;
            this.dgThawingDetilsGrid.DataContext = ThawDetailsList;
            dgThawingDetilsGrid.UpdateLayout();
           // FillThawingDetails();
        }

        void ThawDetailsList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillThawingDetails();
        }
        #region Fill Master Item
        private void fillCanID()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_IVFCanMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        CanList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillLabPerson();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private void FillLabPerson()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.DepartmentId = 0;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);
                    SelectedLabIncharge = objList;
                    fillPlan();
                    if (this.DataContext != null)
                    {
                        //   cmbLabPerson.SelectedValue = ((cls_NewThawingDetailsVO)this.DataContext).LabInchargeId;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillPlan()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PostThawingPlan;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        SelectedPLan = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        FillFreezing();
                    }
                    
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }
        #endregion

        private ObservableCollection<clsNew_SpremFreezingVO> _SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
        public ObservableCollection<clsNew_SpremFreezingVO> SpremFreezingGrid
        {
            get { return _SpremFreezingDetails; }
            set { _SpremFreezingDetails = value; }
        }


        void FillFreezing()
        {
            wait.Show();
            try
            {
                cls_NewGetSpremThawingBizActionVO BizAction = new cls_NewGetSpremThawingBizActionVO();
                BizAction.MalePatientID = PatientID1;
                BizAction.MalePatientUnitID = PatientUnitID1;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    // Incomplete please check DAL n Bind varible
                    if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremFreezingDetailsVO != null)
                    {
                        for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremFreezingDetails.Count; i++)
                        {
                            SpremFreezingGrid.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremFreezingDetails[i]);
                            dgVitrificationDetilsGrid.ItemsSource = SpremFreezingGrid;
                        }

                        wait.Close();
                    }
                    FillThawingDetails();
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }


        private void FillThawingDetails()
        {
            try
            {
                cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
                bizAction.MalePatientID = PatientID1;
                bizAction.MalePatientUnitID = PatientUnitID1;
                bizAction.IsThawDetails = true;
                bizAction.IsPagingEnabled = true;
                bizAction.StartIndex = ThawDetailsList.PageIndex * ThawDetailsList.PageSize;
                bizAction.MaximumRows = ThawDetailsList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList != null)
                    {
                        ThawDetailsList.Clear();
                        ThawList = ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList;
                        ThawDetailsList.TotalItemCount = (int)((cls_NewGetSpremThawingBizActionVO)arg.Result).TotalRows;
                        for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                        {
                            if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId > 0)
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].LabPersonId);
                            }
                            else
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedIncharge = SelectedLabIncharge.FirstOrDefault(p => p.ID == 0);
                            }
                            ThawDetailsList.Add(((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i]);
                        }
                        dgThawingDetilsGrid.ItemsSource = null;
                        dgThawingDetilsGrid.ItemsSource = ThawDetailsList;
                        dgThawingDetilsGrid.UpdateLayout();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            cls_NewThawingDetailsVO ThawRow = (cls_NewThawingDetailsVO)e.Row.DataContext;
            if (ThawRow.IsFreezed == true)
                e.Row.IsEnabled = false;
            else
                e.Row.IsEnabled = true;
        }

        private void cmbPlanForSperms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ThawDetailsList.Count > 0)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure \n You Want To Save Semen Thawing Details";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += (result) =>
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        SaveThawing();
                    }
                };
                msgWin.Show();
            }
            else 
            {
                string msgTitle = "Palash";
                string msgText = "No Details Available";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
         
                msgWin.Show();
            }
        }

        void winImp_OnSaveClick(object sender, RoutedEventArgs e)
        {
            ImpressionWindow ObjImp = (ImpressionWindow)sender;
            Impression = ObjImp.Impression;
            SaveThawing();
        }

        private void SaveThawing()
        {
            try
            {
                wait.Show();
                clsAddUpdateSpermThawingBizActionVO BizAction = new clsAddUpdateSpermThawingBizActionVO();
                BizAction.ThawingList = new List<cls_NewThawingDetailsVO>();
                //BizAction.Impression = Impression;
                BizAction.IsNewForm = true;
                BizAction.ThawingList = ThawList;
                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";
                        if (IsEdit == true)
                        {
                            txtmsg = "Semen Thawing Details Updated Successfully";
                        }
                        else
                        {
                            txtmsg = "Semen Thawing Details Saved Successfully";
                        }

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            this.DialogResult = false;
                        };
                        msgW1.Show();
                        wait.Close();
                    }
                    else
                    {
                        wait.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion

            }
            catch (Exception ex)
            {
                wait.Close();
            }

        }
        private void cmbGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbLabIncharge1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ThawDetailsList.Count; i++)
            {
                if (i == dgThawingDetilsGrid.SelectedIndex)
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ThawDetailsList[i].LabPersonId = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }
    }
}

