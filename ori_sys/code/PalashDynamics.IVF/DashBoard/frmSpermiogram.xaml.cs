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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Text;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.UserControls;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmSpermiogram : ChildWindow
    {
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public string SourceOfSemenCode;
        public long DonorID;
        public long DonorUnitID;
        public bool IsClosed;
        public frmSpermiogram()
        {
            InitializeComponent();
            ThawDetailsList = new PagedSortableCollectionView<cls_NewThawingDetailsVO>();
            ThawDetailsListPageSize = 15;

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

                    if (this.DataContext != null)
                    {
                        //   cmbLabPerson.SelectedValue = ((cls_NewThawingDetailsVO)this.DataContext).LabInchargeId;
                    }
                }
                fillThawingGrid();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private ObservableCollection<clsBatchAndSpemFreezingVO> BatchList;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdGetSample_Click(object sender, RoutedEventArgs e)
        {
            frmGetSemenBatchList win = new frmGetSemenBatchList();
            win.PatientID = DonorID;
            win.PatientUnitID = DonorUnitID;
            win.OnSaveButton_Click += new RoutedEventHandler(OnSaveButtonWin_Click);
            win.Show();
        }
        private void OnSaveButtonWin_Click(object sender, RoutedEventArgs e)
        {
            if (((frmGetSemenBatchList)sender).SelectedBatches != null && ((frmGetSemenBatchList)sender).SelectedBatches.Count > 0)
            {
                dataGrid2.ItemsSource = null;
                dataGrid2.ItemsSource = ((frmGetSemenBatchList)sender).SelectedBatches;
            }


            frmGetSemenBatchList Itemswin = (frmGetSemenBatchList)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.SelectedBatches != null)
                {
                    StringBuilder strError = new StringBuilder();


                    foreach (var item in Itemswin.SelectedBatches)
                    {
                        bool Additem = true;
                        if (BatchList != null && BatchList.Count > 0)
                        {
                            var item1 = from r in BatchList
                                        where (r.BatchID == item.BatchID & r.BatchUnitID == item.BatchUnitID
                                        & r.SpermFreezingDetailsID == item.SpermFreezingDetailsID & r.SpermFreezingDetailsUnitID == item.SpermFreezingDetailsUnitID
                                        & r.SpermFreezingID == item.SpermFreezingID & r.SpermFreezingUnitID == item.SpermFreezingUnitID)
                                        select r;

                            if (item1.ToList().Count > 0)
                            {
                                Additem = false;
                            }
                        }

                        if (Additem)
                        {
                            BatchList.Add(item);
                        }
                    }

                    dataGrid2.ItemsSource = null;
                    dataGrid2.ItemsSource = BatchList;
                    dataGrid2.UpdateLayout();

                    if (!string.IsNullOrEmpty(strError.ToString()))
                    {
                        string strMsg = "Sample Already Added";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }
        private void cmbLabIncharge1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public event RoutedEventHandler OnCloseButton_Click;

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Vitrification != null)
            {
                if (Vitrification.IsSelected)
                {

                }
            }
            if (Thawing != null)
            {
                if (Thawing.IsSelected)
                {
                    FillLabPerson();
                }
            }
        }
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
        private void fillThawingGrid()
        {
            try
            {
                cls_NewGetSpremThawingBizActionVO bizAction = new cls_NewGetSpremThawingBizActionVO();
                bizAction.PlanTherapyID = PlanTherapyID;
                bizAction.PlanTherapyUnitID = PlanTherapyUnitID;
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
                        ThawDetailsList.TotalItemCount = (int)((cls_NewGetSpremThawingBizActionVO)arg.Result).TotalRows;
                        for (int i = 0; i < ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList.Count; i++)
                        {
                            ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SelectedInchargeList = SelectedLabIncharge;
                            if (((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsFreezed == true)
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = false;
                            }
                            else
                            {
                                ((cls_NewGetSpremThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].IsEnabled = true;
                            }
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
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SourceOfSemenCode != null)
                txtSourceofSemen.Text = SourceOfSemenCode;
            BatchList = new ObservableCollection<clsBatchAndSpemFreezingVO>();
            dataGrid2.ItemsSource = null;
            dataGrid2.ItemsSource = BatchList;
            fillGrid();
        }

        private void chkThaw_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((CheckBox)sender).IsChecked == true)
            {
                string msgTitle = "Palash";
                string msgText = "Are You Sure You Want To Thaw Sample";
                MessageBoxControl.MessageBoxChildWindow msgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgWin.Show();

            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    cls_NewAddUpdateSpremFreezingBizActionVO BizActionObj = new cls_NewAddUpdateSpremFreezingBizActionVO();

                    BizActionObj.SpremFreezingVO = new clsNew_SpremFreezingVO();
                    BizActionObj.SpremFreezingMainVO = new cls_NewSpremFreezingMainVO();

                    // Grid Details
                    BizActionObj.SpremFreezingDetails = new ObservableCollection<clsNew_SpremFreezingVO>();
                    clsNew_SpremFreezingVO obj = new clsNew_SpremFreezingVO();
                    obj.ID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).SpermFreezingDetailsID;
                    obj.GobletSizeId = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).GlobletSizeID;
                    obj.GobletColorID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).ColorCodeID;
                    obj.GobletShapeId = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).GlobletShapeID;
                    obj.CanID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).CaneID;
                    obj.CanisterId = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).CanisterID;
                    obj.StrawId = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).StrawID;
                    obj.TankId = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).TankID;
                    obj.Comments = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).Comments;
                    obj.Status = true;
                    obj.IsThaw = true;
                    obj.IsModify = true;
                    obj.SpremNostr = (((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).SpremNo).ToString();
                    obj.PlanTherapy = PlanTherapyID;
                    obj.PlanTherapyUnitID = PlanTherapyUnitID;
                    BizActionObj.SpremFreezingDetails.Add(obj);
                    BizActionObj.MalePatientID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).PatientID;
                    BizActionObj.MalePatientUnitID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).PatientUnitID;


                    BizActionObj.ID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).SpermFreezingID;
                    BizActionObj.SpremFreezingMainVO.BatchID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).BatchID;
                    BizActionObj.SpremFreezingMainVO.BatchCode = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).BatchCode;
                    BizActionObj.SpremFreezingMainVO.LabID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).LabID;
                    BizActionObj.SpremFreezingMainVO.BatchUnitID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).BatchUnitID;
                    BizActionObj.SpremFreezingMainVO.SpremFreezingDate = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).SpremFreezingDate;
                    BizActionObj.SpremFreezingMainVO.SpremFreezingTime = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).SpremFreezingTime;
                    BizActionObj.SpremFreezingMainVO.DoctorID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).DoctorID;
                    BizActionObj.SpremFreezingMainVO.EmbryologistID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).EmbryologistID;

                    BizActionObj.SpremFreezingMainVO.CollectionMethodID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).CollectionMethodID;
                    BizActionObj.SpremFreezingMainVO.CollectionProblem = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).CollectionProblem;
                    BizActionObj.SpremFreezingMainVO.Abstience = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).Abstience;
                    BizActionObj.SpremFreezingMainVO.Volume = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).Volume;

                    BizActionObj.SpremFreezingMainVO.ViscosityID = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).ViscosityID;
                    BizActionObj.SpremFreezingMainVO.GradeA = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).GradeA;
                    BizActionObj.SpremFreezingMainVO.GradeB = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).GradeB;
                    BizActionObj.SpremFreezingMainVO.GradeC = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).GradeC;

                    BizActionObj.SpremFreezingMainVO.TotalSpremCount = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).TotalSpremCount;
                    BizActionObj.SpremFreezingMainVO.Motility = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).Motility;
                    BizActionObj.SpremFreezingMainVO.Other = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).FreezingOther;
                    BizActionObj.SpremFreezingMainVO.Comments = ((clsBatchAndSpemFreezingVO)dataGrid2.SelectedItem).FreezingComments;
                    BizActionObj.SpremFreezingMainVO.Status = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {

                                    fillGrid();
                                }
                            };
                            msgW1.Show();


                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }
        private void fillGrid()
        {
            try
            {

                cls_GetSemenBatchAndSpermiogramBizActionVO BizAction = new cls_GetSemenBatchAndSpermiogramBizActionVO();
                BizAction.DetailsList = new List<ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO>();
                BizAction.Details = new ValueObjects.IVFPlanTherapy.clsBatchAndSpemFreezingVO();
                BizAction.Details.PlanTherapyID = PlanTherapyID;
                BizAction.Details.PlanTherapyUnitID = PlanTherapyUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList.Count > 0)
                        {
                            BatchList = new ObservableCollection<clsBatchAndSpemFreezingVO>();
                            foreach (var i in ((cls_GetSemenBatchAndSpermiogramBizActionVO)arg.Result).DetailsList)
                                BatchList.Add(i);

                            dataGrid2.ItemsSource = null;
                            dataGrid2.ItemsSource = BatchList;
                            dataGrid2.UpdateLayout();
                        }
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

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsBatchAndSpemFreezingVO)(row.DataContext)).IsThaw == true)
            {
                e.Row.IsEnabled = false;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (dgThawingDetilsGrid.SelectedItem != null && ((CheckBox)sender).IsChecked == true)
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
        }
        WaitIndicator wait = new WaitIndicator();
        private void SaveThawing()
        {
            try
            {
                wait.Show();
                clsAddUpdateSpermThawingBizActionVO BizAction = new clsAddUpdateSpermThawingBizActionVO();
                BizAction.ThawingList = new List<cls_NewThawingDetailsVO>();
                BizAction.IsNewForm = true;
                foreach (var i in ThawDetailsList)
                {
                    BizAction.ThawingList.Add(i);
                }

                #region Service Call (Check Validation)
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string txtmsg = "";

                        txtmsg = "Semen Thawing Details Saved Successfully";

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", txtmsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            fillThawingGrid();
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

        private void dgThawingDetilsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((cls_NewThawingDetailsVO)(row.DataContext)).IsFreezed == true)
            {
                e.Row.IsEnabled = false;
            }
        }

    }
}
