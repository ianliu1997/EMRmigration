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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Text;
using PalashDynamics.Animations;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace PalashDynamics.Administration
{
    //***Added by Ashish Z.
    public partial class frmBulkRateChange : UserControl
    {
        #region Variable Declarations
        WaitIndicator objWIndicator = null;
        private SwivelAnimation objAnimation;
        bool IsCancel = true;
        long SubSPId { get; set; }
        bool IsPercentageRate { get; set; }
        bool IsAmountRate { get; set; }
        bool IsSetRateForAll { get; set; }
        long lID { get; set; }
        string SpecializationName { get; set; } // for Grouping in SubSpecialization DataGrid.
        int selectedOpTypeID { get; set; }
        bool IsFromViewClick { get; set; }
        string msgText = string.Empty;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        public PagedSortableCollectionView<clsTariffMasterBizActionVO> FrontPannelList { get; private set; }
        public PagedSortableCollectionView<clsTariffMasterBizActionVO> MasterList { get; private set; }
        public PagedSortableCollectionView<clsSubSpecializationVO> SpecializationList { get; private set; }
        private List<clsTariffMasterBizActionVO> _OtherTariffSelected;
        private List<clsTariffMasterBizActionVO> SelectedTariffList;

        public List<clsSubSpecializationVO> SubSpecializationList = new List<clsSubSpecializationVO>();
        public List<clsSubSpecializationVO> SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
        public List<clsSubSpecializationVO> DeepCopySubSpecializationList { get; set; }

        List<MasterListItem> OPTypeList = new List<MasterListItem>();
        List<MasterListItem> AmountOrPercentOperationList = new List<MasterListItem>();
        public int FrontPannelListPageSize
        {
            get
            {
                return FrontPannelList.PageSize;
            }
            set
            {
                if (value == FrontPannelList.PageSize) return;
                FrontPannelList.PageSize = value;
            }
        }

        public int TariffListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }

        public int SpecializationListPageSize
        {
            get
            {
                return SpecializationList.PageSize;
            }
            set
            {
                if (value == SpecializationList.PageSize) return;
                SpecializationList.PageSize = value;
            }
        }
        #endregion

        #region Constructor and Loaded
        public frmBulkRateChange()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            objWIndicator = new WaitIndicator();
            _OtherTariffSelected = new List<clsTariffMasterBizActionVO>();
            SelectedTariffList = new List<clsTariffMasterBizActionVO>();
            //dgSubSpecialization.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgSubSpecialization_CellEditEnded);

            //===========================Tariff Paging=======================================//
            MasterList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            TariffListPageSize = 15;
            DataPagerTariff.PageSize = TariffListPageSize;
            DataPagerTariff.Source = MasterList;

            //===========================Specialization Paging==============================//
            SpecializationList = new PagedSortableCollectionView<clsSubSpecializationVO>();
            SpecializationList.OnRefresh += new EventHandler<RefreshEventArgs>(SpecializationList_OnRefresh);
            SpecializationListPageSize = 15;
            DataPagerSpecialization.PageSize = SpecializationListPageSize;
            DataPagerSpecialization.Source = SpecializationList;

            //===========================FrontPannel Paging================================//
            FrontPannelList = new PagedSortableCollectionView<clsTariffMasterBizActionVO>();
            FrontPannelList.OnRefresh += new EventHandler<RefreshEventArgs>(FrontPannelList_OnRefresh);
            FrontPannelListPageSize = 15;
            DataPagerFrontPannelList.PageSize = FrontPannelListPageSize;
            DataPagerFrontPannelList.Source = FrontPannelList;
            //============================================================================//

            //dgSubSpecialization.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgSubSpecialization_CellEditEnded);
            //dgSubSpecialization.PreparingCellForEdit += new EventHandler<DataGridPreparingCellForEditEventArgs>(dgSubSpecialization_PreparingCellForEdit);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
            if (DeepCopySubSpecializationList != null)
            {
                if (DeepCopySubSpecializationList.Count > 0)
                {
                    SelectedSubSpecializationList = DeepCopySubSpecializationList;
                }
            }
            //FillTariffList();
            FillOperationType(false);
            //FillAmtOrPct();
            //FillSpecialization();
            GetBulkRateChangeDetailsList();
            dpEffectiveDate.SelectedDate = DateTime.Today.AddDays(1);
            dpEffectiveDate.DisplayDateStart = DateTime.Today.AddDays(1);
            lblAllRate.Text = "**Selected Rates are Applicable for all Classes.";
        }

        void dgSubSpecialization_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgSubSpecialization.SelectedItem != null)
            {
                if (e.Column.Header.ToString().Equals("Rate") || e.Column.Header.ToString().Equals("Amount/Percent"))
                {
                    if ((dgSubSpecialization.SelectedItem as clsSubSpecializationVO).IsPercentageRate == true)
                    {
                        if ((dgSubSpecialization.SelectedItem as clsSubSpecializationVO).SharePercentage > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please enter Rate upto 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            (dgSubSpecialization.SelectedItem as clsSubSpecializationVO).SharePercentage = 0;
                        }
                    }
                }
            }

        }

        void dgSubSpecialization_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //if (e.Column.DisplayIndex == 4 && (((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe == null || ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe.Count == 0))
            //{
            //    if (OPTypeList != null)
            //        ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe = OPTypeList.DeepCopy();
            //    else
            //        FillOperationType(true);
            //}
            //else
            //{
            //    if (((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe != null && ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe.Count > 0 && (((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SelectedOpType == null || ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SelectedOpType.ID == 0))
            //    {
            //        if (OPTypeList != null)
            //            ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SelectedOpType = OPTypeList[0];
            //    }
            //}
        }
        #endregion

        #region Button Click Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTariffList != null)
            {
                SelectedTariffList = new List<clsTariffMasterBizActionVO>();
                SelectedTariffList.Clear();
                dgTariff.ItemsSource = SelectedTariffList.ToList();
            }
            if (_OtherTariffSelected != null)
            {
                _OtherTariffSelected = new List<clsTariffMasterBizActionVO>();
                _OtherTariffSelected.Clear();
                dgTariff.ItemsSource = _OtherTariffSelected.ToList();
            }
            ControlsIsEnabledOrNot(true);
            CheckValidation(true);
            tabTariff.IsSelected = true;
            ClearUI();
            chkIsApplicable.IsChecked = true;
            FillOperationType(false);
            //FillAmtOrPct();
            FillTariffList();
            //DataPagerTariff.PageIndex = 0;
            FillSpecialization();
            DataPagerSpecialization.PageIndex = 0;
            objAnimation.Invoke(RotationType.Forward);
            SetCommandButtonState("New");
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRateAndOpTypeValidate())
            {
                if (CheckValidation(false))
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveDetails(false);
                        }
                    };
                    msgWindow.Show();
                }
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRateAndOpTypeValidate())
            {
                if (CheckValidation(false))
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Update the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveDetails(true);
                        }
                    };
                    msgWindow.Show();
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            ClearUI();

            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFrontPannelSearch())
            {
                GetBulkRateChangeDetailsList();
                DataPagerFrontPannelList.PageIndex = 0;
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            if (dgFrontPannelList.SelectedItem != null)
            {
                tabTariff.IsSelected = true;
                ClearUI();
                lID = (dgFrontPannelList.SelectedItem as clsTariffMasterBizActionVO).ID;
                IsFromViewClick = true;
                GetBulkRateChangeDetailsListByID(dgFrontPannelList.SelectedItem as clsTariffMasterBizActionVO, IsFromViewClick);
            }
        }

        private void cmdSearchTariff_Click(object sender, RoutedEventArgs e)
        {
            FillTariffList();
            DataPagerTariff.PageIndex = 0;
        }

        private void rdbRateInPercent_Click(object sender, RoutedEventArgs e)
        {
            if (rdbRateInPercent.IsChecked == true)
            {
                this.IsPercentageRate = true;
                this.IsAmountRate = false;
                //if (SubSpecializationList != null) SubSpecializationList.Select(d => d.IsPercentageRate = this.IsPercentageRate); //set the IsPercentageRate flag as per the Radio button Check.
                txtAmountToAllSubSp.IsEnabled = true;
                txtAmountToAllSubSp.Focus();
                //dgSubSpecialization.Columns[2].Header = "Rate in %";
            }
        }

        private void rdbRateInAmount_Click(object sender, RoutedEventArgs e)
        {
            if (rdbRateInAmount.IsChecked == true)
            {
                this.IsAmountRate = true;
                this.IsPercentageRate = false;
                //if (SubSpecializationList != null) SubSpecializationList.Select(d => d.IsPercentageRate = this.IsPercentageRate); //set the IsPercentageRate flag as per the Radio button Check.
                txtAmountToAllSubSp.IsEnabled = true;
                txtAmountToAllSubSp.Focus();
                //dgSubSpecialization.Columns[2].Header = "Rate";
            }
        }

        private void chkSpecSubSpecialization_Click(object sender, RoutedEventArgs e)
        {
            if (dgSpecialization.SelectedItem != null)
            {
                clsSubSpecializationVO SpecializationVO = (clsSubSpecializationVO)dgSpecialization.SelectedItem;
                if (SubSpecializationList == null)
                    SubSpecializationList = new List<clsSubSpecializationVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (SubSpecializationList.Count > 0)
                    {
                        var item = from r in SubSpecializationList
                                   where r.SubSpecializationId == ((clsSubSpecializationVO)dgSpecialization.SelectedItem).SubSpecializationId
                                   select new clsSubSpecializationVO
                                   {
                                       Status = r.Status,
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsSubSpecializationVO)dgSpecialization.SelectedItem).SubSpecializationName);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "'" + strError.ToString() + "'" + " SubSpecialization already Selected";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            //SpecializationVO.OpTpe = OPTypeList.ToList();
                            //SpecializationVO.SelectedOpType = OPTypeList.FirstOrDefault(p => p.ID == 1);
                            //SpecializationVO.AmtOrPctList = AmountOrPercentOperationList.ToList();
                            //SpecializationVO.SelectedAmtOrPct = AmountOrPercentOperationList.FirstOrDefault(p => p.ID == 1);
                            SpecializationVO = (clsSubSpecializationVO)dgSpecialization.SelectedItem;
                            SpecializationVO.IsAddition = true;
                            SpecializationVO.IsSubtaction = false;
                            SpecializationVO.IsAmountRate = true;
                            SpecializationVO.IsPercentageRate = false;
                            SubSpecializationList.Add(SpecializationVO);//SubSpecializationList.Add((clsSubSpecializationVO)dgSpecialization.SelectedItem);
                        }
                    }
                    else
                    {
                        //SpecializationVO.OpTpe = OPTypeList.ToList();
                        //SpecializationVO.SelectedOpType = OPTypeList.FirstOrDefault(p => p.ID == 1);
                        //SpecializationVO.AmtOrPctList = AmountOrPercentOperationList.ToList();
                        //SpecializationVO.SelectedAmtOrPct = AmountOrPercentOperationList.FirstOrDefault(p => p.ID == 1);
                        SpecializationVO = (clsSubSpecializationVO)dgSpecialization.SelectedItem;
                        SpecializationVO.IsAddition = true;
                        SpecializationVO.IsSubtaction = false;
                        SpecializationVO.IsAmountRate = true;
                        SpecializationVO.IsPercentageRate = false;
                        SubSpecializationList.Add(SpecializationVO);//SubSpecializationList.Add((clsSubSpecializationVO)dgSpecialization.SelectedItem);
                    }
                    foreach (var item in SubSpecializationList)
                    {
                        if (item.SpecializationId == SpecializationVO.SpecializationId && item.SubSpecializationId == SpecializationVO.SubSpecializationId)
                        {
                            item.IsAmountRate = true;
                            item.IsAddition = true;
                            item.SelectSubSpecialization = true;
                        }
                    }
                    dgSubSpecialization.ItemsSource = null;
                    PagedCollectionView pcv = new PagedCollectionView(SubSpecializationList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                    dgSubSpecialization.ItemsSource = pcv;
                    dgSubSpecialization.UpdateLayout();
                    dgSubSpecialization.Focus();
                }
                else
                {
                    if (SpecializationVO != null)
                    {
                        clsSubSpecializationVO obj;
                        obj = SubSpecializationList.Where(z => z.SpecializationId == SpecializationVO.SpecializationId && z.SubSpecializationId == SpecializationVO.SubSpecializationId).FirstOrDefault();
                        if (obj != null)
                        {
                            foreach (var item in SpecializationList.ToList())
                            {
                                if (item.SpecializationId == obj.SpecializationId && item.SubSpecializationId == obj.SubSpecializationId) item.SelectSubSpecialization = true;
                            }

                        }
                        dgSpecialization.ItemsSource = null;
                        PagedCollectionView pcv = new PagedCollectionView(SpecializationList);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                        dgSpecialization.ItemsSource = pcv;
                        DataPagerSpecialization.Source = null;
                        DataPagerSpecialization.Source = SpecializationList;
                        dgSpecialization.UpdateLayout();
                    }
                }

            }
        }

        FrameworkElement element;
        DataGridRow row;
        TextBox TxtDoctorShare;
        private void chkSubSpecialization_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubSpecialization.SelectedItems != null)
            {
                element = dgSubSpecialization.Columns.Last().GetCellContent(dgSubSpecialization.SelectedItem);
                row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                TxtDoctorShare = FindVisualChild<TextBox>(row, "txtRate");

                //List<clsSubSpecializationVO> CancelSubSp = new List<clsSubSpecializationVO>();
                //CancelSubSp = ((List<clsSubSpecializationVO>)dgSubSpecialization.ItemsSource).ToList();

                //var item = from r in CancelSubSp
                //           where r.Status == true
                //           select r;

                if (((CheckBox)sender).IsChecked == true)
                {
                    TxtDoctorShare.IsReadOnly = false;
                    clsSubSpecializationVO Obj = new clsSubSpecializationVO();
                    Obj.SubSpecializationId = ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SubSpecializationId;
                    Obj.IsPercentageRate = this.IsPercentageRate;
                    Obj.IsAmountRate = this.IsAmountRate;
                    Obj.SharePercentage = 0;
                    Obj.ShareAmount = 0;
                    Obj.AmtOrPctList = AmountOrPercentOperationList.ToList();
                    Obj.SelectedAmtOrPct = AmountOrPercentOperationList.FirstOrDefault(p => p.ID == 0);

                    Obj.OpTpe = OPTypeList.ToList();
                    Obj.SelectedOpType = OPTypeList.FirstOrDefault(p => p.ID == 0);
                    SelectedSubSpecializationList.Add(Obj);
                }
                else if (chkApplyToSubSp.IsChecked == true && txtAmountToAllSubSp.Text != null)
                {
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsReadOnly = true;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSubSpecialization.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
                else
                {
                    TxtDoctorShare.Text = "0.0";
                    TxtDoctorShare.IsReadOnly = true;
                    clsSubSpecializationVO obj = (clsSubSpecializationVO)dgSubSpecialization.SelectedItem;
                    obj = SelectedSubSpecializationList.Where(z => z.SpecializationId == obj.SpecializationId && z.SubSpecializationId == obj.SubSpecializationId).FirstOrDefault();
                    SelectedSubSpecializationList.Remove(obj);
                }
                //DeepCopySubSpecializationList = SelectedSubSpecializationList.DeepCopy();
            }
        }

        private void chkApplyToSubSp_Click(object sender, RoutedEventArgs e)
        {
            //if (chkApplyToSubSp.IsChecked == true)
            //{
            //foreach (var item in SelectedSubSpecializationList.ToList())
            //{
            //    //if (item.SpecializationId == SubSPId)
            //    //{
            //        SelectedSubSpecializationList.Remove(item);
            //    //}
            //}
            if (CheckValidationApplyToAllSubSpe())
            {
                foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList.ToList())
                {
                    ObjSubSpVO.Status = true;
                    ObjSubSpVO.IsPercentageRate = this.IsPercentageRate;
                    ObjSubSpVO.IsAmountRate = this.IsAmountRate; //set the IsPercentageRate flag as per the Radio button Check.
                    if (ObjSubSpVO.IsPercentageRate) ObjSubSpVO.SharePercentage = Convert.ToDouble(txtAmountToAllSubSp.Text);
                    if (ObjSubSpVO.IsAmountRate) ObjSubSpVO.SharePercentage = Convert.ToDouble(txtAmountToAllSubSp.Text); //ObjSubSpVO.ShareAmount = Convert.ToDouble(txtAmountToAllSubSp.Text);
                    ObjSubSpVO.intOperationType = selectedOpTypeID;
                    if (ObjSubSpVO.intOperationType == 1) ObjSubSpVO.IsAddition = true; else ObjSubSpVO.IsAddition = false;

                    if (ObjSubSpVO.intOperationType == 2) ObjSubSpVO.IsSubtaction = true; else ObjSubSpVO.IsSubtaction = false;
                    //SelectedSubSpecializationList.Add(ObjSubSpVO);
                }
                dgSubSpecialization.ItemsSource = null;
                PagedCollectionView pcv = new PagedCollectionView(SubSpecializationList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                dgSubSpecialization.ItemsSource = pcv;
                dgSubSpecialization.UpdateLayout();
                dgSubSpecialization.Focus();
                txtAmountToAllSubSp.Text = "0";
            }
            //chkApplyToSubSp.IsChecked = false;
            //}
            //else
            //{
            //foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList.ToList())
            //{
            //    ObjSubSpVO.Status = false;
            //    ObjSubSpVO.SharePercentage = 0;
            //    ObjSubSpVO.ShareAmount = 0;
            //}
            //SelectedSubSpecializationList = SubSpecializationList;
            //dgSubSpecialization.ItemsSource = null;
            //dgSubSpecialization.ItemsSource = SubSpecializationList;
            //txtAmountToAllSubSp.Text = "0.0";
            //}
        }

        private void chkIsApplicable_Click(object sender, RoutedEventArgs e)
        {
            //if (chkIsApplicable.IsChecked == true) dpEffectiveDate.IsEnabled = true;
            //else dpEffectiveDate.IsEnabled = false;
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedTariff.SelectedItem != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to delete " + "'" + (dgSelectedTariff.SelectedItem as clsTariffMasterBizActionVO).Description + "'" + " Tariff?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsTariffMasterBizActionVO objVO = (clsTariffMasterBizActionVO)dgSelectedTariff.SelectedItem;
                        //clsTariffMasterBizActionVO objServiceVO = dgTariff.SelectedItem as clsTariffMasterBizActionVO;
                        clsTariffMasterBizActionVO obj;
                        if (objVO != null)
                        {
                            obj = _OtherTariffSelected.Where(z => z.ID == objVO.ID).FirstOrDefault();
                            _OtherTariffSelected.Remove(obj);
                            //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                            foreach (var item in MasterList)
                            {

                                if (item.ID == obj.TariffID)
                                {
                                    item.SelectTariff = false;
                                }
                            }
                            dgTariff.ItemsSource = null;
                            dgTariff.ItemsSource = MasterList;
                            DataPagerTariff.Source = null;
                            DataPagerTariff.Source = MasterList;
                            dgTariff.UpdateLayout();
                        }
                        //else if (objServiceVO != null)
                        //{
                        //    obj = _OtherTariffSelected.Where(z => z.ID == objServiceVO.ID).FirstOrDefault();
                        //    _OtherTariffSelected.Remove(obj);
                        //    //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                        //    foreach (var item in MasterList)
                        //    {
                        //        if (item.ID == obj.ID)
                        //        {
                        //            item.SelectTariff = false;
                        //        }
                        //    }
                        //    dgTariff.ItemsSource = null;
                        //    dgTariff.ItemsSource = MasterList;
                        //    DataPagerTariff.Source = null;
                        //    DataPagerTariff.Source = MasterList;
                        //    dgTariff.UpdateLayout();
                        //}
                        foreach (var item in _OtherTariffSelected)
                        {
                            item.SelectTariff = true;
                        }
                        dgSelectedTariff.ItemsSource = null;
                        dgSelectedTariff.ItemsSource = _OtherTariffSelected;
                        dgSelectedTariff.UpdateLayout();
                        dgSelectedTariff.Focus();
                    }
                };

                msgWD.Show();
            }
        }

        private void cmdDeleteSubSpecialization_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubSpecialization.SelectedItem != null)
            {

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to delete " + "'" + ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SubSpecializationName + "'" + " SubSpecialization?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsSubSpecializationVO objVO = (clsSubSpecializationVO)dgSubSpecialization.SelectedItem;
                        clsSubSpecializationVO objServiceVO = dgSpecialization.SelectedItem as clsSubSpecializationVO;
                        clsSubSpecializationVO obj;
                        if (objVO != null)
                        {
                            obj = SubSpecializationList.Where(z => z.SubSpecializationId == objVO.SubSpecializationId).FirstOrDefault();
                            SubSpecializationList.Remove(obj);
                            //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                            foreach (var item in SpecializationList)
                            {
                                if (item.SubSpecializationId == obj.SubSpecializationId)
                                {
                                    item.ShareAmount = 0.00;
                                    item.SharePercentage = 0.00;
                                    item.IsAddition = false;
                                    item.IsSubtaction = false;
                                    item.IsAmountRate = false;
                                    item.IsPercentageRate = false;
                                    item.SelectSubSpecialization = false;
                                }
                            }
                            dgSpecialization.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(SpecializationList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                            dgSpecialization.ItemsSource = pcv;
                            DataPagerSpecialization.Source = null;
                            DataPagerSpecialization.Source = SpecializationList;
                            dgSpecialization.UpdateLayout();
                        }
                        else if (objServiceVO != null)
                        {
                            obj = SubSpecializationList.Where(z => z.SubSpecializationId == objServiceVO.SubSpecializationId).FirstOrDefault();
                            SubSpecializationList.Remove(obj);
                            //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                            foreach (var item in SpecializationList)
                            {
                                if (item.SubSpecializationId == obj.SubSpecializationId)
                                {
                                    item.SelectSubSpecialization = false;
                                }
                            }
                            dgSpecialization.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(SpecializationList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                            dgSpecialization.ItemsSource = pcv;
                            DataPagerSpecialization.Source = null;
                            DataPagerSpecialization.Source = SpecializationList;
                            dgSpecialization.UpdateLayout();
                        }
                        foreach (var item in SubSpecializationList)
                        {
                            item.SelectSubSpecialization = true;
                        }
                        dgSubSpecialization.ItemsSource = null;
                        PagedCollectionView pcv1 = new PagedCollectionView(SubSpecializationList);
                        pcv1.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                        dgSubSpecialization.ItemsSource = pcv1;
                        dgSubSpecialization.UpdateLayout();
                        dgSubSpecialization.Focus();
                    }
                };

                msgWD.Show();
            }
        }
        #endregion

        #region Private Methods
        private void SaveDetails(bool IsModify)
        {
            clsAddUpdateBulkRateChangeDetailsBizActionVO BizAction = new clsAddUpdateBulkRateChangeDetailsBizActionVO();
            BizAction.BulkRateChangeDetailsVO = new clsTariffMasterBizActionVO();
            BizAction.TariffDetailsList = new List<clsTariffMasterBizActionVO>();
            BizAction.SubSpecializationList = new List<clsSubSpecializationVO>();
            if (IsModify) BizAction.BulkRateChangeDetailsVO.ID = lID;
            else BizAction.BulkRateChangeDetailsVO.ID = 0;
            BizAction.IsModify = IsModify;
            BizAction.BulkRateChangeDetailsVO.IsApplicable = true;
            BizAction.BulkRateChangeDetailsVO.EffectiveDate = dpEffectiveDate.SelectedDate;
            BizAction.BulkRateChangeDetailsVO.IsFreeze = Convert.ToBoolean(chkFreeze.IsChecked);
            //BizAction.BulkRateChangeDetailsVO.IsSetRateForAll = Convert.ToBoolean(chkApplyToSubSp.IsChecked);
            BizAction.TariffDetailsList = _OtherTariffSelected;
            BizAction.SubSpecializationList = SubSpecializationList; //SelectedSubSpecializationList;
            BizAction.BulkRateChangeDetailsVO.Remark = txtRemark.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    ClearUI();
                    GetBulkRateChangeDetailsList();
                    objAnimation.Invoke(RotationType.Backward);
                    SetCommandButtonState("Load");
                    if (!IsModify) msgText = "Added"; else msgText = "Updated";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Record " + msgText + " Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    break;

                case "New":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    lblAllRate.Visibility = Visibility.Visible;
                    lblAllRate.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    break;

                case "Modify":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    break;

                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    lblAllRate.Visibility = Visibility.Collapsed;
                    break;

                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    lblAllRate.Visibility = Visibility.Visible;
                    lblAllRate.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void ControlsIsEnabledOrNot(bool Status)
        {
            cmdModify.IsEnabled = Status;
            rdbRateInAmount.IsEnabled = Status;
            rdbRateInPercent.IsEnabled = Status;
            txtAmountToAllSubSp.IsEnabled = Status;
            dpEffectiveDate.IsEnabled = Status;
            chkFreeze.IsEnabled = Status;
            chkApplyToSubSp.IsEnabled = Status;
            txtRemark.IsEnabled = Status;
            accTariff.IsSelected = Status;
        }

        private void ClearUI()
        {
            txtTariffCode.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtAmountToAllSubSp.Text = "0";
            //chkIsApplicable.IsChecked = false;
            dpEffectiveDate.SelectedDate = DateTime.Today.AddDays(1);
            chkFreeze.IsChecked = false;
            this.IsFromViewClick = false;
            rdbRateInAmount.IsChecked = false;
            rdbRateInPercent.IsChecked = false;
            dgTariff.UpdateLayout();
            dgSpecialization.UpdateLayout();

            if (SelectedSubSpecializationList != null)
            {
                SelectedSubSpecializationList.Clear();
                SelectedSubSpecializationList = new List<clsSubSpecializationVO>();
            }
            if (SubSpecializationList != null)
            {
                SubSpecializationList.Clear();
                SubSpecializationList = new List<clsSubSpecializationVO>();
            }
            dgSubSpecialization.ItemsSource = null;
            dgSubSpecialization.UpdateLayout();

            if (_OtherTariffSelected != null)
            {
                _OtherTariffSelected.Clear();
                _OtherTariffSelected = new List<clsTariffMasterBizActionVO>();
            }
            if (SelectedTariffList != null)
            {
                SelectedTariffList.Clear();
                SelectedTariffList = new List<clsTariffMasterBizActionVO>();
            }
            dgSelectedTariff.ItemsSource = null;
            dgSelectedTariff.UpdateLayout();

        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillTariffList();
        }

        void SpecializationList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillSpecialization();
        }

        void FrontPannelList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetBulkRateChangeDetailsList();
            DataPagerFrontPannelList.PageIndex = 0;

        }

        private void FillTariffList()
        {
            if (objWIndicator != null)
                objWIndicator.Show();
            clsGetTariffListBizActionVO BizAction = new clsGetTariffListBizActionVO();
            BizAction.TariffList = new List<clsTariffMasterBizActionVO>();
            BizAction.SearchExpression = txtTariffCode.Text;

            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;

            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objWIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        MasterList.Clear();
                        var item = from r in (((clsGetTariffListBizActionVO)args.Result).TariffList) where (r.Status == true) select r;
                        BizAction.TariffList = item.ToList();
                        BizAction.TariffList.Select(d => d.TariffID = d.ID).ToList();
                        MasterList.TotalItemCount = (int)(((clsGetTariffListBizActionVO)args.Result).TotalRows);
                        if (_OtherTariffSelected != null && _OtherTariffSelected.Count > 0)
                        {
                            foreach (clsTariffMasterBizActionVO item1 in _OtherTariffSelected.ToList())
                            {
                                BizAction.TariffList.ToList().ForEach(x => { if (item1.TariffID == x.ID) x.SelectTariff = true; });
                            }
                        }
                        foreach (clsTariffMasterBizActionVO item2 in BizAction.TariffList)
                        {
                            MasterList.Add(item2);
                        }
                        dgTariff.ItemsSource = null;
                        dgTariff.ItemsSource = MasterList;
                        dgTariff.SelectedIndex = -1;

                        DataPagerTariff.Source = null;
                        DataPagerTariff.PageSize = MasterList.PageSize;
                        DataPagerTariff.Source = MasterList;
                        dgTariff.UpdateLayout();

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void FillSpecialization()
        {
            try
            {
                if (objWIndicator != null)
                    objWIndicator.Show();
                clsGetSpecializationsByTariffIdBizActionVO BizAction = new clsGetSpecializationsByTariffIdBizActionVO();
                BizAction.TariffID = 0;
                BizAction.IsFromTariffCopyUtility = true;

                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = SpecializationList.PageSize;
                BizAction.StartRowIndex = SpecializationList.PageIndex * SpecializationList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    objWIndicator.Close();
                    if (e.Error == null && e.Result != null)
                    {
                        SpecializationList.Clear();
                        SpecializationList.TotalItemCount = (int)((clsGetSpecializationsByTariffIdBizActionVO)e.Result).TotalRows;
                        clsGetSpecializationsByTariffIdBizActionVO result = e.Result as clsGetSpecializationsByTariffIdBizActionVO;

                        if (SubSpecializationList != null && SubSpecializationList.Count > 0)
                        {
                            foreach (var item1 in SubSpecializationList.ToList())
                            {
                                result.SpecializationList.ToList().ForEach(x => { if (item1.SpecializationId == x.SpecializationId && item1.SubSpecializationId == x.SubSpecializationId) x.SelectSubSpecialization = true; });
                            }
                        }

                        if (result.SpecializationList != null && result.SpecializationList.Count > 0)
                        {
                            foreach (var item in result.SpecializationList)
                            {
                                SpecializationList.Add(item);
                            }
                            dgSpecialization.ItemsSource = null;
                            PagedCollectionView pcv = new PagedCollectionView(SpecializationList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                            dgSpecialization.ItemsSource = pcv;
                            dgSpecialization.SelectedIndex = -1;
                            DataPagerSpecialization.Source = null;
                            DataPagerSpecialization.PageSize = SpecializationList.PageSize;
                            DataPagerSpecialization.Source = SpecializationList;
                            dgSpecialization.UpdateLayout();
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void FillSubSpecialization(string fkSpecializationID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            if (fkSpecializationID != null)
            {
                BizAction.Parent = new KeyValue { Key = fkSpecializationID, Value = "fkSpecializationID" };
            }
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    dgSubSpecialization.ItemsSource = null;
                    foreach (var item in objList)
                    {
                        item.Status = false;
                    }

                    SubSpecializationList = new List<clsSubSpecializationVO>();
                    foreach (var item in objList)
                    {
                        clsSubSpecializationVO ObjSubspe = new clsSubSpecializationVO();
                        ObjSubspe.SubSpecializationId = item.ID;
                        ObjSubspe.Status = false;
                        ObjSubspe.SubSpecializationName = item.Description;
                        ObjSubspe.SpecializationId = Convert.ToInt64(fkSpecializationID);
                        ObjSubspe.IsPercentageRate = this.IsPercentageRate;
                        ObjSubspe.IsAmountRate = this.IsAmountRate;
                        ObjSubspe.SpecializationName = this.SpecializationName;
                        ObjSubspe.intOperationType = selectedOpTypeID;
                        ObjSubspe.stOperationType = GetOpTypeNamebyID(selectedOpTypeID);
                        SubSpecializationList.Add(ObjSubspe);
                    }
                    if (SelectedSubSpecializationList.Count > 0)
                    {
                        foreach (clsSubSpecializationVO item in SelectedSubSpecializationList)
                        {
                            foreach (clsSubSpecializationVO item1 in SubSpecializationList)//dgSubSpecialization.ItemsSource)
                            {
                                if (item.SubSpecializationId == item1.SubSpecializationId)
                                {
                                    item1.Status = true;
                                    item1.SharePercentage = item.SharePercentage;
                                    item1.ShareAmount = item.SharePercentage;
                                    item1.intOperationType = item.intOperationType;
                                    item1.stOperationType = GetOpTypeNamebyID(item.intOperationType);
                                    item1.IsReadOnly = true;
                                    //if (item.IsPercentageRate) { dgSubSpecialization.Columns[2].Header = "Rate in %"; rdbRateInPercent.IsChecked = item.IsPercentageRate; }
                                    //else { dgSubSpecialization.Columns[2].Header = "Rate"; rdbRateInAmount.IsChecked = item.IsPercentageRate; }
                                }
                            }
                        }
                    }
                    if (chkApplyToSubSp.IsChecked == true)
                    {
                        foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList)
                        {
                            ObjSubSpVO.Status = true;
                            ObjSubSpVO.SharePercentage = Convert.ToDouble(txtAmountToAllSubSp.Text);
                            ObjSubSpVO.ShareAmount = Convert.ToDouble(txtAmountToAllSubSp.Text);
                        }
                    }
                    PagedCollectionView pcv = new PagedCollectionView(SubSpecializationList);
                    pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                    //dgSubSpecialization.ItemsSource = null;
                    dgSubSpecialization.ItemsSource = pcv;
                    dgSubSpecialization.UpdateLayout();
                    dgSubSpecialization.Focus();
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void GetBulkRateChangeDetailsList()
        {
            try
            {
                objWIndicator.Show();
                clsGetBulkRateChangeDetailsListBizActionVO BizActionVO = new clsGetBulkRateChangeDetailsListBizActionVO();
                BizActionVO.BulkRateChangeDetailsVO = new clsTariffMasterBizActionVO();
                if (dtpFromDate.SelectedDate != null)
                    BizActionVO.BulkRateChangeDetailsVO.FromEffectiveDate = dtpFromDate.SelectedDate.Value.Date;
                if (dtpToDate.SelectedDate != null)
                    BizActionVO.BulkRateChangeDetailsVO.ToEffectiveDate = dtpToDate.SelectedDate.Value.Date;
                if (!string.IsNullOrEmpty(txtSearchTariff.Text))
                    BizActionVO.BulkRateChangeDetailsVO.TariffName = txtSearchTariff.Text;

                BizActionVO.PagingEnabled = true;
                BizActionVO.MaximumRows = FrontPannelList.PageSize;
                BizActionVO.StartRowIndex = FrontPannelList.PageIndex * FrontPannelList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetBulkRateChangeDetailsListBizActionVO objVO = arg.Result as clsGetBulkRateChangeDetailsListBizActionVO;
                        if (objVO != null)
                        {
                            StringBuilder sbTariff = new StringBuilder();
                            FrontPannelList.Clear();
                            FrontPannelList.TotalItemCount = (int)((clsGetBulkRateChangeDetailsListBizActionVO)arg.Result).TotalRows;

                            //if (objVO.TariffDetailsList.Count > 0)
                            //{
                            //    var item2 = from r in objVO.TariffDetailsList
                            //                group r by r.ID;

                            //    foreach (var item1 in item2)
                            //    {
                            //        var item3 = from r in objVO.TariffDetailsList where (r.ID == item1.Key) select r;
                            //        clsTariffMasterBizActionVO test = new clsTariffMasterBizActionVO();
                            //        foreach (var item4 in item3)
                            //        {
                            //            test.ID = item4.ID;
                            //            test.UnitID = item4.UnitID;
                            //            test.IsApplicable = item4.IsApplicable;
                            //            test.Remark = item4.Remark;
                            //            test.EffectiveDate = item4.EffectiveDate;
                            //            test.IsFreeze = item4.IsFreeze;
                            //            test.TariffName += item4.TariffName + " ,";
                            //        }
                            //        string removecomma = test.TariffName.Remove(test.TariffName.Length - 1);
                            //        test.TariffName = removecomma;
                            //        FrontPannelList.Add(test);
                            //    }
                            //    dgFrontPannelList.ItemsSource = null;
                            //    dgFrontPannelList.ItemsSource = FrontPannelList;
                            //    dgFrontPannelList.SelectedIndex = -1;
                            //    DataPagerFrontPannelList.Source = null;
                            //    DataPagerFrontPannelList.PageSize = FrontPannelList.PageSize;
                            //    DataPagerFrontPannelList.Source = FrontPannelList;
                            //    dgFrontPannelList.UpdateLayout();

                            //}


                            if (objVO.TariffDetailsList.Count > 0)
                            {
                                foreach (var item1 in objVO.TariffDetailsList)
                                {
                                    FrontPannelList.Add(item1);
                                }
                                dgFrontPannelList.ItemsSource = null;
                                dgFrontPannelList.ItemsSource = FrontPannelList;
                                dgFrontPannelList.SelectedIndex = -1;
                                DataPagerFrontPannelList.Source = null;
                                DataPagerFrontPannelList.PageSize = FrontPannelList.PageSize;
                                DataPagerFrontPannelList.Source = FrontPannelList;
                                dgFrontPannelList.UpdateLayout();
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                    objWIndicator.Close();
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void GetBulkRateChangeDetailsListByID(clsTariffMasterBizActionVO _objVO, bool IsFromViewClick)
        {
            try
            {
                objWIndicator.Show();
                clsGetBulkRateChangeDetailsListBizActionVO BizActionVO = new clsGetBulkRateChangeDetailsListBizActionVO();
                BizActionVO.BulkRateChangeDetailsVO = new clsTariffMasterBizActionVO();
                BizActionVO.IsFromViewClick = IsFromViewClick;
                BizActionVO.BulkRateChangeDetailsVO.BulkRateChangeID = _objVO.ID;
                BizActionVO.BulkRateChangeDetailsVO.BulkRateChangeUnitID = _objVO.UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetBulkRateChangeDetailsListBizActionVO objVO = arg.Result as clsGetBulkRateChangeDetailsListBizActionVO;
                        if (SelectedTariffList != null)
                            SelectedTariffList.Clear();
                        if (_OtherTariffSelected != null)
                            _OtherTariffSelected.Clear();
                        if (SelectedSubSpecializationList != null)
                            SelectedSubSpecializationList.Clear();
                        if (objVO != null)
                        {
                            if (_objVO != null)
                            {
                                chkIsApplicable.IsChecked = _objVO.IsApplicable;
                                dpEffectiveDate.SelectedDate = _objVO.EffectiveDate;
                                chkFreeze.IsChecked = _objVO.IsFreeze;
                                txtRemark.Text = _objVO.Remark;

                            }
                            //==================Tariff list======================//
                            if (objVO.TariffDetailsList.Count > 0)
                            {
                                foreach (var item in objVO.TariffDetailsList)
                                {
                                    SelectedTariffList.Add(item);
                                }
                                _OtherTariffSelected = SelectedTariffList;
                                foreach (var item in _OtherTariffSelected)
                                {
                                    item.SelectTariff = true;
                                }
                                dgSelectedTariff.ItemsSource = null;
                                dgSelectedTariff.ItemsSource = _OtherTariffSelected;
                                dgSelectedTariff.UpdateLayout();
                            }
                            //==================SubSpecialization list====================================//
                            if (objVO.SubSpecializationList.Count > 0)
                            {
                                foreach (var item in objVO.SubSpecializationList)
                                {
                                    item.stOperationType = GetOpTypeNamebyID(item.intOperationType);
                                    SubSpecializationList.Add(item);
                                }
                                foreach (var item in SubSpecializationList)
                                {
                                    if (!item.IsPercentageRate) item.IsAmountRate = true;
                                    if (item.intOperationType == 1) item.IsAddition = true;
                                    else if (item.intOperationType == 2) item.IsSubtaction = true;
                                    item.SelectSubSpecialization = true;
                                    //item.ShareAmount = 0.00;
                                    //item.SharePercentage = 0.00;
                                }
                                //SubSpecializationList = SelectedSubSpecializationList;
                            }

                            PagedCollectionView pcv = new PagedCollectionView(SubSpecializationList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                            //dgSubSpecialization.ItemsSource = null;
                            dgSubSpecialization.ItemsSource = pcv;
                            dgSubSpecialization.UpdateLayout();
                            dgSubSpecialization.Focus();
                            //==================================================================================//
                        }
                        FillTariffList();
                        FillSpecialization();
                        objAnimation.Invoke(RotationType.Forward);
                        SetCommandButtonState("View");
                        //**If Record is Freezed
                        if (_objVO != null && _objVO.IsFreeze)
                        {
                            ControlsIsEnabledOrNot(false);
                        }
                        else
                        {
                            ControlsIsEnabledOrNot(true);
                        }
                        //**
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    objWIndicator.Close();
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj, String TextBoxName)
        where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    if (Child is TextBox)
                    {
                        if (((TextBox)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else if (Child is DataGrid)
                    {
                        if (((DataGrid)Child).Name.Equals(TextBoxName))
                        {
                            return (ChildControl)Child;
                        }
                    }
                    else
                    {
                        ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);
                        if (ChildOfChild != null)
                        {
                            return ChildOfChild;
                        }
                    }
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child, TextBoxName);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }

        private void FillOperationType(bool IsFromGrid)
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(InventoryStockOperationType), mlPaymode);
            OPTypeList = mlPaymode;
            cmbOperationType.ItemsSource = mlPaymode;
            cmbOperationType.SelectedItem = mlPaymode[1];

            if (IsFromGrid)
            {
                //((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe = mlPaymode.DeepCopy();
                //((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SelectedOpType = mlPaymode[0];
            }
        }

        private void FillAmtOrPct()
        {
            List<MasterListItem> mlAmtOrPct = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlAmtOrPct.Insert(0, Default);
            EnumToList(typeof(AmountOrPercentOperation), mlAmtOrPct);
            AmountOrPercentOperationList = mlAmtOrPct;
            cmbAmtOrPct.ItemsSource = mlAmtOrPct;
            cmbAmtOrPct.SelectedItem = mlAmtOrPct[1];

            //((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).OpTpe = mlAmtOrPct.DeepCopy();
            //((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SelectedOpType = mlAmtOrPct[0];
        }

        private static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        private static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        private string GetOpTypeNamebyID(int OpTypeID)
        {
            string OperationTypeName = string.Empty;
            if (OpTypeID == 1)
            {
                OperationTypeName = InventoryStockOperationType.Addition.ToString();
            }
            else if (OpTypeID == 2)
            {
                OperationTypeName = InventoryStockOperationType.Subtraction.ToString();
            }
            else
            {
                OperationTypeName = InventoryStockOperationType.None.ToString();
            }
            return OperationTypeName;
        }
        #endregion

        #region Validation Methods
        private bool CheckValidation(bool IsFromNewClick)
        {
            bool result = true;
            if (dpEffectiveDate.SelectedDate == null || dpEffectiveDate.SelectedDate < DateTime.Today.Date.AddDays(1))
            {
                dpEffectiveDate.SetValidation("Please select Future(Today Onwards) Date");
                dpEffectiveDate.RaiseValidationError();
                dpEffectiveDate.Focus();
                result = false;
            }
            else
                dpEffectiveDate.ClearValidationError();
            if (_OtherTariffSelected.Count == 0 && !IsFromNewClick)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Tariff", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
                tabTariff.IsSelected = true;
            }
            else if (SubSpecializationList.Count == 0 && !IsFromNewClick)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Subspecialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
                tabSpecialization.IsSelected = true;
            }

            if (string.IsNullOrEmpty(txtRemark.Text))
            {
                txtRemark.SetValidation("Please Enter Remark");
                txtRemark.RaiseValidationError();
                txtRemark.Focus();
                result = false;
            }
            else
                txtRemark.ClearValidationError();
            return result;
        }

        private bool CheckValidationApplyToAllSubSpe()
        {
            bool result = true;

            if (SubSpecializationList.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Subspecialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
                return result;
            }

            if (rdbRateInAmount.IsChecked == false && rdbRateInPercent.IsChecked == false)
            {
                rdbRateInAmount.SetValidation("Please Select Rate In Amount/Percent");
                rdbRateInPercent.SetValidation("Please Select Rate In Amount/Percent");
                rdbRateInAmount.RaiseValidationError();
                rdbRateInPercent.RaiseValidationError();
                rdbRateInAmount.Focus();
                result = false;
                return result;
            }
            else
            {
                rdbRateInAmount.ClearValidationError();
                rdbRateInPercent.ClearValidationError();
            }

            if (string.IsNullOrEmpty(txtAmountToAllSubSp.Text))
            {
                txtAmountToAllSubSp.SetValidation("Please Enter Rate greater than Zero");
                txtAmountToAllSubSp.RaiseValidationError();
                txtAmountToAllSubSp.Focus();
                result = false;
            }
            else if (double.Parse(txtAmountToAllSubSp.Text) == 0)
            {
                txtAmountToAllSubSp.SetValidation("Please Enter Rate greater than Zero");
                txtAmountToAllSubSp.RaiseValidationError();
                txtAmountToAllSubSp.Focus();
                result = false;
            }
            else if (double.Parse(txtAmountToAllSubSp.Text) > 100 && rdbRateInPercent.IsChecked == true)
            {
                txtAmountToAllSubSp.SetValidation("Please Enter Rate upto 100 for Percentage");
                txtAmountToAllSubSp.RaiseValidationError();
                txtAmountToAllSubSp.Focus();
                result = false;
            }
            else
                txtAmountToAllSubSp.ClearValidationError();

            if (cmbOperationType.SelectedItem == null)
            {
                cmbOperationType.TextBox.SetValidation("Please Select Operation Type");
                cmbOperationType.TextBox.RaiseValidationError();
                cmbOperationType.Focus();
                result = false;
            }
            else if (((MasterListItem)cmbOperationType.SelectedItem).ID == 0)
            {
                cmbOperationType.TextBox.SetValidation("Please Select OpMOeration Type");
                cmbOperationType.TextBox.RaiseValidationError();
                cmbOperationType.Focus();
                result = false;
            }
            else
                cmbOperationType.TextBox.ClearValidationError();

            return result;
        }

        private bool CheckRateAndOpTypeValidate()
        {
            bool result = true;
            if (SubSpecializationList.Count > 0)
            {
                foreach (var item in SubSpecializationList.ToList())
                {
                    if (item.SharePercentage> 100 && item.IsPercentageRate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Rate upto 100 for '" + item.SubSpecializationName + "' SubSpecialization of '" + item.SpecializationName + "' Specialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                    if (item.SharePercentage == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please, Enter Rate greater than Zero for '" + item.SubSpecializationName + "' SubSpecialization of '" + item.SpecializationName + "' Specialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }

                    if (!item.IsAddition && !item.IsSubtaction)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please, Select Operation Type from List of SubSpecialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                    else if (!item.IsAmountRate && !item.IsPercentageRate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please, Select Rate In Amount/Percent from List of SubSpecialization", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                }

            }
            return result;
        }

        private bool CheckFrontPannelSearch()
        {
            bool result = true;

            DateTime? _FromDate = dtpFromDate.SelectedDate;
            DateTime? _ToDate = dtpToDate.SelectedDate;

            if (_FromDate == null)
            {
                //dtpFromDate.SetValidation("Please Enter FromEffective Date");
                //dtpFromDate.RaiseValidationError();
                //dtpFromDate.Focus();
                //result = false;
            }
            else if (_ToDate == null)
            {
                //dtpToDate.SetValidation("Please Enter ToEffective Date");
                //dtpToDate.RaiseValidationError();
                //dtpToDate.Focus();
                //result = false;
            }
            else if (_FromDate > _ToDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter FromEffective Date less than ToEffective Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
                dtpToDate.ClearValidationError();
                result = true;
            }
            return result;
        }
        #endregion

        #region Other Events(KeyDown, Checked, TextChanged, SelectionChanged)
        private void txtTariffCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillTariffList();
                DataPagerTariff.PageIndex = 0;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dgTariff.SelectedItem != null)
            {

                clsTariffMasterBizActionVO objVO = (clsTariffMasterBizActionVO)dgSelectedTariff.SelectedItem;
                clsTariffMasterBizActionVO objServiceVO = dgTariff.SelectedItem as clsTariffMasterBizActionVO;
                if (_OtherTariffSelected == null)
                    _OtherTariffSelected = new List<clsTariffMasterBizActionVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    tabSpecialization.IsEnabled = true;
                    if (_OtherTariffSelected.Count > 0)
                    {
                        var item = from r in _OtherTariffSelected
                                   where r.TariffID == ((clsTariffMasterBizActionVO)dgTariff.SelectedItem).ID
                                   select new clsTariffMasterBizActionVO
                                   {
                                       Status = r.Status,
                                       ID = r.TariffID,
                                       Description = r.Description
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsTariffMasterBizActionVO)dgTariff.SelectedItem).Description);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "'" + strError.ToString() + "'" + " Tariff already Selected";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            _OtherTariffSelected.Add((clsTariffMasterBizActionVO)dgTariff.SelectedItem);
                        }
                    }
                    else
                    {
                        _OtherTariffSelected.Add((clsTariffMasterBizActionVO)dgTariff.SelectedItem);
                    }
                    foreach (var item in _OtherTariffSelected)
                    {
                        item.SelectTariff = true;
                    }
                    dgSelectedTariff.ItemsSource = null;
                    dgSelectedTariff.ItemsSource = _OtherTariffSelected;
                    dgSelectedTariff.UpdateLayout();
                    dgSelectedTariff.Focus();
                }
                else
                {
                    clsTariffMasterBizActionVO obj;
                    if (objServiceVO != null)
                    {
                        obj = _OtherTariffSelected.Where(z => z.TariffID == objServiceVO.ID).FirstOrDefault();
                        if (obj != null)
                        {
                            foreach (var item in MasterList)
                            {
                                if (item.ID == obj.TariffID) item.SelectTariff = true;
                            }
                        }
                        dgTariff.ItemsSource = null;
                        dgTariff.ItemsSource = MasterList;
                        DataPagerTariff.Source = null;
                        DataPagerTariff.Source = MasterList;
                        dgTariff.UpdateLayout();
                    }

                    //clsTariffMasterBizActionVO obj;
                    //if (objVO != null)
                    //{
                    //    obj = _OtherTariffSelected.Where(z => z.ID == objVO.ID).FirstOrDefault();
                    //    _OtherTariffSelected.Remove(obj);
                    //    //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //    foreach (var item in MasterList)
                    //    {
                    //        if (item.ID == obj.ID)
                    //        {
                    //            item.SelectTariff = false;
                    //        }
                    //    }
                    //    dgTariff.ItemsSource = null;
                    //    dgTariff.ItemsSource = MasterList;
                    //    DataPagerTariff.Source = null;
                    //    DataPagerTariff.Source = MasterList;
                    //    dgTariff.UpdateLayout();
                    //}
                    //else if (objServiceVO != null)
                    //{
                    //    obj = _OtherTariffSelected.Where(z => z.ID == objServiceVO.ID).FirstOrDefault();
                    //    _OtherTariffSelected.Remove(obj);
                    //    //DataList.ToList().Where(z => z.ServiceID == obj.ServiceID).FirstOrDefault().SelectService = false;
                    //    foreach (var item in MasterList)
                    //    {
                    //        if (item.ID == obj.ID)
                    //        {
                    //            item.SelectTariff = false;
                    //        }
                    //    }
                    //    dgTariff.ItemsSource = null;
                    //    dgTariff.ItemsSource = MasterList;
                    //    DataPagerTariff.Source = null;
                    //    DataPagerTariff.Source = MasterList;
                    //    dgTariff.UpdateLayout();
                    //}
                }

            }

        }

        private void txtAmountToAllSubSp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAmountToAllSubSp.Text) && !txtAmountToAllSubSp.Text.IsPositiveDoubleValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dgSubSpecialization.SelectedItem != null)
            {
                //element = dgSubSpecialization.Columns.Last().GetCellContent(dgSubSpecialization.SelectedItem);
                //row = DataGridRow.GetRowContainingElement(element.Parent as FrameworkElement);
                //TxtDoctorShare = FindVisualChild<TextBox>(row, "txtRate");

                foreach (var item in SubSpecializationList)//SelectedSubSpecializationList)
                {
                    if ((!String.IsNullOrEmpty(((TextBox)sender).Text)) && !((TextBox)sender).Text.IsPositiveDoubleValid())    //IsValueDouble
                    {
                        ((TextBox)sender).Text = textBefore;
                        ((TextBox)sender).SelectionStart = selectionStart;
                        ((TextBox)sender).SelectionLength = selectionLength;
                        textBefore = String.Empty;
                        selectionStart = 0;
                        selectionLength = 0;
                        //TxtDoctorShare.SetValidation("Input Format is Incorrect ");
                        //TxtDoctorShare.RaiseValidationError();
                        //TxtDoctorShare.Focus();
                    }
                    else if (!String.IsNullOrEmpty(((TextBox)sender).Text) && item.SpecializationId == ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SpecializationId && item.SubSpecializationId == ((clsSubSpecializationVO)dgSubSpecialization.SelectedItem).SubSpecializationId)
                    {
                        if (item.IsPercentageRate) item.SharePercentage = Convert.ToDouble(((TextBox)sender).Text);
                        if (item.IsAmountRate) item.ShareAmount = Convert.ToDouble(((TextBox)sender).Text);
                    }
                }
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }
        private void dgSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgSpecialization.SelectedItem != null)
            //{
            //    rdbRateInAmount.IsEnabled = true; rdbRateInPercent.IsEnabled = true;
            //    txtAmountToAllSubSp.Text = "0.0";
            //    chkApplyToSubSp.IsChecked = false;
            //    var dgsubSpItemSource = dgSubSpecialization.ItemsSource;
            //    #region Validation For Rate Percentage
            //    if (SelectedSubSpecializationList.Count > 0)
            //    {
            //        if (dgsubSpItemSource != null)
            //        {
            //            foreach (var item3 in dgsubSpItemSource)
            //            {
            //                foreach (clsSubSpecializationVO objVO in SelectedSubSpecializationList)
            //                {
            //                    clsSubSpecializationVO item1 = SubSpecializationList.Where(z => z.SubSpecializationId == objVO.SubSpecializationId).FirstOrDefault();
            //                    if (item1 != null)
            //                    {
            //                        if (objVO.SharePercentage == 0 && objVO.Status == true)
            //                        {
            //                            FrameworkElement fe = dgSubSpecialization.Columns[2].GetCellContent(item1);
            //                            FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
            //                            var thisCell = (DataGridCell)parent;
            //                            TextBox txt = thisCell.Content as TextBox;
            //                            txt.SetValidation("Please Enter Rate more than ZERO");
            //                            txt.RaiseValidationError();
            //                            txt.Focus();
            //                            //IsValidate = false;
            //                            return;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    #endregion
            //    SubSPId = (dgSpecialization.SelectedItem as clsTariffMasterBizActionVO).ID;
            //    SpecializationName = (dgSpecialization.SelectedItem as clsTariffMasterBizActionVO).StrGroup;
            //    FillSubSpecialization(Convert.ToString((dgSpecialization.SelectedItem as clsTariffMasterBizActionVO).ID));
            //}
        }

        private void dpEffectiveDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dpEffectiveDate.SelectedDate < DateTime.Today.Date.AddDays(1))
            //{
            //    dpEffectiveDate.SetValidation("Please select Future(Today Onwards) Date");
            //    dpEffectiveDate.RaiseValidationError();
            //    dpEffectiveDate.Focus();
            //}
            //else
            //    dpEffectiveDate.ClearValidationError();
        }

        private void txtAmountToAllSubSp_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void cmbOperationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbOperationType.SelectedItem != null)
            {
                //foreach (clsSubSpecializationVO item in SelectedSubSpecializationList.ToList())
                //{
                //    if (item.SpecializationId == SubSPId) SelectedSubSpecializationList.Remove(item);
                //}

                selectedOpTypeID = Convert.ToInt16((cmbOperationType.SelectedItem as MasterListItem).ID);
                //foreach (clsSubSpecializationVO ObjSubSpVO in SubSpecializationList.ToList())
                //{
                //    ObjSubSpVO.intOperationType = selectedOpTypeID;
                //    ObjSubSpVO.stOperationType = GetOpTypeNamebyID(selectedOpTypeID);
                //    //SelectedSubSpecializationList.Add(ObjSubSpVO);
                //}
                //dgSubSpecialization.ItemsSource = null;
                //PagedCollectionView pcv = new PagedCollectionView(SubSpecializationList);
                //pcv.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                //dgSubSpecialization.ItemsSource = pcv;
                //dgSubSpecialization.UpdateLayout();
                //dgSubSpecialization.Focus();

            }
        }

        private void tabConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;
            if (tabItem == "Specialization")
            {
                lblAllRate.Visibility = Visibility.Visible;
            }
        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void EnterKeySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                GetBulkRateChangeDetailsList();
                DataPagerFrontPannelList.PageIndex = 0;
            }
        }

        private void txtRate_LostFocus(object sender, RoutedEventArgs e)
        {
            //if ((dgSubSpecialization.SelectedItem as clsSubSpecializationVO).IsPercentageRate)
            //{
            //    if ((dgSubSpecialization.SelectedItem as clsSubSpecializationVO).SharePercentage > 100)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //           new MessageBoxControl.MessageBoxChildWindow("", "Please enter Rate upto 100", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW1.Show();
            //        (dgSubSpecialization.SelectedItem as clsSubSpecializationVO).SharePercentage = 0;
            //    }
            //} 
        }

        private void cmbAmtOrPct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgSubSpecialization_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgSubSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion
    }
    //***
}
