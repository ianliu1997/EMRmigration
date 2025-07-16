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
using System.Collections;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using OPDModule;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using OPDModule.Forms;
using PalashDynamics;
using System.Threading;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Collections.ObjectModel;
using System.Text;
namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmSemenPreparation_Details : ChildWindow
    {
        #region Paging

        public PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO> DataList { get; private set; }


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

        public PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO> DataListSaved { get; private set; }


        public int DataListSavedPageSize
        {
            get
            {
                return DataListSaved.PageSize;
            }
            set
            {
                if (value == DataListSaved.PageSize) return;
                DataListSaved.PageSize = value;
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            // FetchData();

        }
        #endregion


        public bool IsSelf = false;
        bool Flagref = false, IsModify = false;
        bool IsPageLoded = false;
        int ClickedFlag = 0;
        public bool IsCancel = true;

        public Int64 UnitID = 0;
        public Int64 PatientID = 0;
        public Int64 PatientUnitID = 0;
        public Int64 VisitID = 0;

        public long DonorID;
        public long DonorUnitID;

        private SwivelAnimation objAnimation;
        public long SelectedRecord;
        public clsCoupleVO CoupleDetails;
        public long PlanTherapyID;
        public long PlanTherapyUnitID;

        public event RoutedEventHandler OKButtonCode_Click;

        private ObservableCollection<cls_IVFDashboard_SemenWashVO> _SemenList;
        public ObservableCollection<cls_IVFDashboard_SemenWashVO> SemenList { get { return _SemenList; } }

        List<cls_IVFDashboard_SemenWashVO> _SemenSelected = new List<cls_IVFDashboard_SemenWashVO>();

        List<cls_IVFDashboard_SemenWashVO> _SemenSelectedNew = new List<cls_IVFDashboard_SemenWashVO>();


        public FrmSemenPreparation_Details()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);


            DataListSaved = new PagedSortableCollectionView<cls_IVFDashboard_SemenWashVO>();
            DataListSaved.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListSavedPageSize = 15;

            this.DataContext = null;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if ((((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != 0 || (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID != null)
                {
                    FetchData();
                    //FetchDataSaved();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Female Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                }
            }
        }

        private void FetchData()
        {

            //cls_GetIVFDashboard_SemenBizActionVO BizAction = new cls_GetIVFDashboard_SemenBizActionVO();

            cls_GetIVFDashboard_SemenDetailsBizActionVO BizAction = new cls_GetIVFDashboard_SemenDetailsBizActionVO();

            BizAction.List = new List<cls_IVFDashboard_SemenWashVO>();
            // BizAction.PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            //BizAction.PatientID = CoupleDetails.MalePatient.PatientID;
            //BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.PatientID = DonorID;
            BizAction.PatientUnitID = DonorUnitID;
            BizAction.PlanTherapyID = PlanTherapyID;
            BizAction.PlanTherapyUnitID = PlanTherapyUnitID;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = 15;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    if (((cls_GetIVFDashboard_SemenDetailsBizActionVO)args.Result).List != null)
                    {
                        cls_GetIVFDashboard_SemenDetailsBizActionVO result = args.Result as cls_GetIVFDashboard_SemenDetailsBizActionVO;

                        DataList.TotalItemCount = result.TotalRows;
                        if (result.List != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.List)
                            {
                                if ((item.UsedPlanTherapyID == PlanTherapyID) && (item.UsedPlanTherapyUnitID == PlanTherapyUnitID))
                                    item.ISChecked = true;

                                if (item.ISChecked == true)
                                {
                                    item.ISReadonly = false;
                                }
                                else
                                {
                                    item.ISReadonly = true;
                                }

                                DataList.Add(item);
                            }



                            dgSemenWash.ItemsSource = null;
                            dgSemenWash.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void dgSemenWash_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgSemenWash_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((cls_IVFDashboard_SemenWashVO)(row.DataContext)).semenused > 0)
            {
                e.Row.IsEnabled = false;
            }
            else
            {
                e.Row.IsEnabled = true;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSemenStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        public StringBuilder SampleSelfID = new StringBuilder();
        public StringBuilder SampleID = new StringBuilder();
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            SampleSelfID = new StringBuilder();
            SampleID = new StringBuilder();

            foreach (var item in DataList)
            {
                if (IsSelf)
                {
                    if (item.ISChecked == true)
                    {
                        if (SampleSelfID.ToString().Length > 0)
                        {
                            SampleSelfID.Append(",");
                            SampleSelfID.Append(item.ID.ToString());
                        }
                        else
                            SampleSelfID.Append(item.ID.ToString());
                    }
                }
                else
                {
                    if (item.ISChecked == true)
                    {
                        if (SampleID.ToString().Length > 0)
                        {
                            SampleID.Append(",");
                            SampleID.Append(item.ID.ToString());
                        }
                        else
                            SampleID.Append(item.ID.ToString());
                    }
                }
            }

            this.DialogResult = true;
            if (OKButtonCode_Click != null)
                OKButtonCode_Click(this, new RoutedEventArgs());

            //if (DataList. == true)
            //    SampleID.Append(((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremThawingDetailsList[i].SpremNo.ToString());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

