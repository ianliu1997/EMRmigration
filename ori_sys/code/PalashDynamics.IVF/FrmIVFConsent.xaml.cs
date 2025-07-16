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
using System.IO;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Data;
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.IVF.DashBoard;
using OPDModule.Forms;


namespace PalashDynamics.IVF
{
    public partial class FrmIVFConsent : ChildWindow
    {
        public long PatientID;
        public long PatientUnitID;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;
        public bool IsClosed;
        WaitIndicator win = new WaitIndicator();
        public event EventHandler Onsaved;


        public FrmIVFConsent()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckConsent())
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }

        }

        public bool ConsentCheck = false;
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                win.Show();
                clsAddUpdateIVFPackegeConsentBizActionVO BizAction = new clsAddUpdateIVFPackegeConsentBizActionVO();
                try
                {
                    BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                    //BizAction.PagingEnabled = true;
                    //BizAction.MaximumRows = DataList.PageSize;
                    //BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                    BizAction.PatientID = PatientID;
                    BizAction.PatientUnitID = PatientUnitID;
                    BizAction.PlanTherapyId = PlanTherapyID;
                    BizAction.PlanTherapyUnitId = PlanTherapyUnitID;
                    BizAction.ConsentMatserDetails = DataList.ToList();

                    var item = DataList.Where(x => x.IsConsentCheck = false).ToList();

                    if (item.ToList().Count == 0)
                        BizAction.UpdateConsentCheckInPlanTherapy = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsAddUpdateIVFPackegeConsentBizActionVO)arg.Result).SuccessStatus == 1)
                            {
                                ConsentCheck = ((clsAddUpdateIVFPackegeConsentBizActionVO)arg.Result).ConsentCheck;
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("", "Details Saved.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                GetSavedConsent();
                            }
                            win.Close();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            win.Close();
                        }
                        win.Close();
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private bool CheckConsent()
        {
            bool result = true;

            if (DataList == null || DataList.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Consent is not present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                result = false;
            }
            else
            {
                var item = DataList.Where(x => x.IsConsentCheck == false).ToList();
                if (item.ToList().Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please check consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                    result = false;
                }
            }
            return result;
        }

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

        public int MasterListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        private void GetSavedConsentForClosedCycle()
        {
            clsGetIVFSavedPackegeConsentBizActionVO BizAction = new clsGetIVFSavedPackegeConsentBizActionVO();
            try
            {
                win.Show();
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                BizAction.PatientID = PatientID;
                BizAction.PatientUnitID = PatientUnitID;
                BizAction.PlanTherapyId = PlanTherapyID;
                BizAction.PlanTherapyUnitId = PlanTherapyUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetIVFSavedPackegeConsentBizActionVO result = arg.Result as clsGetIVFSavedPackegeConsentBizActionVO;
                            //DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null && result.ConsentMatserDetails.Count > 0)
                            {
                                SavedList = new List<clsPatientConsentVO>();
                                SavedList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    SavedList.Add(item);
                                }

                                dgPatientConsent.ItemsSource = null;
                                dgPatientConsent.ItemsSource = SavedList;

                                dgDataPager.Source = null;
                                dgDataPager.PageSize = BizAction.MaximumRows;
                                dgDataPager.Source = SavedList;
                            }

                        }
                        win.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        win.Close();
                    }
                    
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        List<clsPatientConsentVO> SavedList = null;



        private void GetSavedConsent()
        {
            clsGetIVFSavedPackegeConsentBizActionVO BizAction = new clsGetIVFSavedPackegeConsentBizActionVO();
            try
            {
                win.Show();
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                BizAction.PatientID = PatientID;
                BizAction.PatientUnitID = PatientUnitID;
                BizAction.PlanTherapyId = PlanTherapyID;
                BizAction.PlanTherapyUnitId = PlanTherapyUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetIVFSavedPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetIVFSavedPackegeConsentBizActionVO result = arg.Result as clsGetIVFSavedPackegeConsentBizActionVO;
                            //DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null && result.ConsentMatserDetails.Count > 0)
                            {
                                SavedList = new List<clsPatientConsentVO>();
                                SavedList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    SavedList.Add(item);
                                }

                            }

                        }
                        GetConsent();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        win.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public PagedSortableCollectionView<clsPatientSignConsentVO> MasterList { get; private set; }



        private void GetConsent()
        {
            clsGetIVFPackegeConsentBizActionVO BizAction = new clsGetIVFPackegeConsentBizActionVO();
            try
            {
                BizAction.ConsentMatserDetails = new List<clsPatientConsentVO>();
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = DataList.PageSize;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;

                BizAction.PatientID = PatientID;
                BizAction.PatientUnitID = PatientUnitID;
                BizAction.PlanTherapyId = PlanTherapyID;
                BizAction.PlanTherapyUnitId = PlanTherapyUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetIVFPackegeConsentBizActionVO)arg.Result).ConsentMatserDetails != null)
                        {
                            clsGetIVFPackegeConsentBizActionVO result = arg.Result as clsGetIVFPackegeConsentBizActionVO;
                            //DataList.TotalItemCount = result.TotalRows;

                            if (result.ConsentMatserDetails != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.ConsentMatserDetails)
                                {
                                    //if (item.PatientID == CPatientID && item.PatientUnitID == CPatientUnitID && item.ConsentID == ConsentID && IsConsentTrue == true)
                                    //    item.IsEnabledConsent = true;
                                    DataList.Add(item);
                                }

                                if (SavedList != null && SavedList.Count > 0)
                                {
                                    (from a1 in DataList
                                     join a2 in SavedList on a1.ConsentID equals a2.ConsentID
                                     select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => x.A1.IsConsentCheck = x.A2.IsConsentCheck);
                                }

                                dgPatientConsent.ItemsSource = null;
                                dgPatientConsent.ItemsSource = DataList;

                                dgDataPager.Source = null;
                                dgDataPager.PageSize = BizAction.MaximumRows;
                                dgDataPager.Source = DataList;
                            }
                        }
                        //GetData();
                        win.Close();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                        win.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void hlbViewnPrintConsent_Click(object sender, RoutedEventArgs e)
        {
            if (dgPatientConsent.SelectedItem != null)
            {
                PatientConsentPrint Win = new PatientConsentPrint(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                Win.DataContext = ((clsPatientConsentVO)dgPatientConsent.SelectedItem);
                Win.IsSaved = true;
                Win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please select consent.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        long CPatientID = 0;
        long CPatientUnitID = 0;
        long ConsentID = 0;
        long ConsentUnitID = 0;
        FrmPatientSignConsent_IVFDashboard obj = null;
        //bool IsConsentTrue = false;
        private void hlbattachedsignconsent_Click(object sender, RoutedEventArgs e)
        {
            CPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            CPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            ConsentID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).ID;
            ConsentUnitID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).UnitID;


            obj = new FrmPatientSignConsent_IVFDashboard(CPatientID, CPatientUnitID, ConsentID, ConsentUnitID, PlanTherapyID, PlanTherapyUnitID);
            obj.Onsaved += new EventHandler(obj_Onsaved);
            obj.Show();
        }

        void obj_Onsaved(object sender, EventArgs e)
        {
            //if (obj.blRecordSaved == true)
            //    IsConsentTrue = true;
            foreach (var item in DataList)
            {
                if (obj.SConsentID == item.ID)
                {
                    if (obj.itemCount > 0)
                        item.IsConsentCheck = true;
                }

            }

            //GetConsent();

            //throw new NotImplementedException();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsPatientConsentVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 7;

            MasterList = new PagedSortableCollectionView<clsPatientSignConsentVO>();
            //MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            MasterListPageSize = 7;

            if (IsClosed == true)
                GetSavedConsentForClosedCycle();
            else
                GetSavedConsent();

        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetConsent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            Onsaved(this, new EventArgs());
            this.DialogResult = false;
        }



        private void chkConsentCheck_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //for (int i = 0; i < DataList.Count; i++)
            //{
            //    if (i == dgPatientConsent.SelectedIndex)
            //    {
            //        if (chk.IsChecked == true)
            //        {
            //            DataList[i].IsConsentCheck = true;
            //        }
            //    }
            //}
        }

        private void chkConsentCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            //CheckBox chk = sender as CheckBox;
            //for (int i = 0; i < DataList.Count; i++)
            //{
            //    if (i == dgPatientConsent.SelectedIndex)
            //    {
            //        if (chk.IsChecked == false)
            //        {
            //            DataList[i].IsConsentCheck = false;
            //        }
            //    }
            //}
        }

        private void dgPatientConsent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgPatientConsent.ItemsSource != null)
            //{
            //    try
            //    {
            //        win.Show();
            //        clsGetPatientSignConsentBizActionVO bizActionVO = new clsGetPatientSignConsentBizActionVO();
            //        bizActionVO.sortExpression = ""; //txtSearch.Text;
            //        bizActionVO.IsPagingEnabled = true;
            //        bizActionVO.MaximumRows = MasterList.PageSize;
            //        bizActionVO.StartIndex = MasterList.PageIndex * MasterList.PageSize;

            //        bizActionVO.ConsentID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).ID;
            //        bizActionVO.ConsentUnitID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).UnitID;
            //        bizActionVO.PlanTherapyID = PlanTherapyID;
            //        bizActionVO.PlanTherapyUnitID = PlanTherapyUnitID;
            //        bizActionVO.Status = true;

            //        bizActionVO.SignPatientList = new List<clsPatientSignConsentVO>();

            //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //        client.ProcessCompleted += (s, args) =>
            //        {
            //            if (args.Error == null && args.Result != null)
            //            {
            //                bizActionVO.SignPatientList = (((clsGetPatientSignConsentBizActionVO)args.Result).SignPatientList);
            //                MasterList.Clear();

            //                if (bizActionVO.SignPatientList.Count > 0)
            //                {
            //                    ((clsPatientConsentVO)dgPatientConsent.SelectedItem).IsEnabledConsent = true;
            //                }
            //            }
            //            win.Close();
            //        };
            //        client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            //        client.CloseAsync();
            //    }
            //    catch (Exception)
            //    {
            //        throw;
            //    }
            //}
        }

        private void chkConsentCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                win.Show();
                clsGetPatientSignConsentBizActionVO bizActionVO = new clsGetPatientSignConsentBizActionVO();
                //bizActionVO.sortExpression = ""; 
                //bizActionVO.IsPagingEnabled = true;
                //bizActionVO.MaximumRows = MasterList.PageSize;
                //bizActionVO.StartIndex = MasterList.PageIndex * MasterList.PageSize;

                bizActionVO.ConsentID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).ID;
                bizActionVO.ConsentUnitID = ((clsPatientConsentVO)dgPatientConsent.SelectedItem).UnitID;
                bizActionVO.PlanTherapyID = PlanTherapyID;
                bizActionVO.PlanTherapyUnitID = PlanTherapyUnitID;
                bizActionVO.Status = true;

                bizActionVO.SignPatientList = new List<clsPatientSignConsentVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        bizActionVO.SignPatientList = (((clsGetPatientSignConsentBizActionVO)args.Result).SignPatientList);
                        MasterList.Clear();

                        if (bizActionVO.SignPatientList.Count > 0)
                        {
                            ((clsPatientConsentVO)dgPatientConsent.SelectedItem).IsConsentCheck = true;
                            //((clsPatientConsentVO)dgPatientConsent.SelectedItem).IsEnabledConsent = true;
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please Attach File", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                            ((clsPatientConsentVO)dgPatientConsent.SelectedItem).IsConsentCheck = false;
                        }
                    }
                    win.Close();
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

