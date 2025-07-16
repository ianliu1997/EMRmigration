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
using System.Windows.Data;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.Collections.Generic;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using CIMS;
using System.Text;

namespace PalashDynamics.Administration
{
    public partial class frmAssignServiceTariff : ChildWindow
    {
        public Boolean IsEditMode { get; set; }
        Boolean IsPageLoded = false;
        clsServiceMasterVO objMasterVO = null;
        Boolean IsRowSelected = false;
        public long Count;
        public string ClassIDList = null;
        public string TariffIDList = null;
        public string ClassRateList = null;

        List<clsServiceTarrifClassRateDetailsNewVO> SelectedTariffList = new List<clsServiceTarrifClassRateDetailsNewVO>();
        List<clsServiceTarrifClassRateDetailsNewVO> DeleteTariffList = new List<clsServiceTarrifClassRateDetailsNewVO>();
        List<clsServiceTarrifClassRateDetailsNewVO> ModifyTariffList = new List<clsServiceTarrifClassRateDetailsNewVO>();

        public long ServiceID { get; set; }
        public long TariffID { get; set; }
        public long ClassID { get; set; }
        public int OperationType;
        public bool New;
        public bool Remove;
        public bool Flag;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        public frmAssignServiceTariff()
        {
            InitializeComponent();
            this.DataContext = new clsGetTariffServiceClassRateNewBizActionVO();

            DataList = new PagedSortableCollectionView<clsServiceTarrifClassRateDetailsNewVO>();
            DataList1 = new List<clsServiceTarrifClassRateDetailsNewVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

        }
        #region Paging
        public ObservableCollection<clsServiceTarrifClassRateDetailsNewVO> ItemList1 { get; set; }
        public ObservableCollection<clsServiceTarrifClassRateDetailsNewVO> TariffClassList { get; set; }
        public List<clsServiceTarrifClassRateDetailsNewVO> ExistingTariffList { get; set; }
        public ObservableCollection<clsServiceTarrifClassRateDetailsNewVO> TariffClassList2 { get; set; }

        public PagedSortableCollectionView<clsServiceTarrifClassRateDetailsNewVO> DataList { get; private set; }
        List<clsServiceTarrifClassRateDetailsNewVO> DataList1 { get; set; }
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

        #endregion

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData(ServiceID, OperationType);
        }

        public void GetSelectedServiceDetails(clsServiceMasterVO objServiceMasterVO)
        {
            objMasterVO = objServiceMasterVO;
            string Title = "Service Name : ";
            this.Title = Title + objMasterVO.ServiceName;
            rdbBoth.Visibility = Visibility.Collapsed;
            dgServiceTariff.Columns[3].Visibility = Visibility.Collapsed;

            if (OperationType == Convert.ToInt32(TariffOperationType.Remove))
            {
                chkApplyAll.Visibility = Visibility.Collapsed;

            }

        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = child.Parent;
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

        private void dgServiceTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "Are you sure you want to assign selected Tariff / Package to service?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    SaveTariff();
                }
            };
            msgWD.Show();
        }

        private void SaveTariff()
        {
            //=======================================================================================================================//
            /* Added By Sudhir Patil To Add Class Rate Details On 31/03/2014 */
            long ClassID = 0;
            long TariffID = 0;
            decimal Rate = 0;
            StringBuilder builder = new StringBuilder();
            StringBuilder builder1 = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (chkApplyAll.IsChecked == true)
            {
                foreach (var item in ClassServiceList)
                {
                    ClassID = item.ClassID;
                    Rate = item.Rate;
                    builder.Append(ClassID).Append(",");
                    builder2.Append(Rate).Append(",");
                }
                ClassIDList = builder.ToString();
                ClassRateList = builder2.ToString();
                if (ClassIDList.Length != 0)
                    ClassIDList = ClassIDList.TrimEnd(',');
                if (ClassRateList.Length != 0)
                    ClassRateList = ClassRateList.TrimEnd(',');
            }
            else if (SelectedTariffList.Count > 0)
            {
                foreach (var item in SelectedTariffList)
                {
                    ClassID = item.ClassID;
                    TariffID = item.TariffID;
                    Rate = item.Rate;
                    builder.Append(ClassID).Append(",");
                    builder1.Append(TariffID).Append(",");
                    builder2.Append(Rate).Append(",");
                }
                ClassIDList = builder.ToString();
                TariffIDList = builder1.ToString();
                ClassRateList = builder2.ToString();
                if (ClassIDList.Length != 0)
                    ClassIDList = ClassIDList.TrimEnd(',');
                //string answer = new String(ClassIDList.Distinct().Except(",").ToArray()); 
                //SignificantIDList = SignificantIDList.Remove(SignificantIDList.Length - 1);
                /* Above is another way to remove Last Character from String */
                if (TariffIDList.Length != 0)
                    TariffIDList = TariffIDList.TrimEnd(',');
                if (ClassRateList.Length != 0)
                    ClassRateList = ClassRateList.TrimEnd(',');
            }
            WaitIndicator PleaseWait = new WaitIndicator();
            PleaseWait.Show();
            clsAddUpdateServiceMasterTariffBizActionVO BizAction = new clsAddUpdateServiceMasterTariffBizActionVO();
            BizAction.ServiceID = ServiceID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (chkApplyAll.IsChecked == true)
            {
                BizAction.IsApplyToAllTariff = true;
            }
            else
            {
                BizAction.IsApplyToAllTariff = false;
                BizAction.TariffIDList = TariffIDList;
            }
            BizAction.ClassRateList = ClassRateList;
            BizAction.ClassIDList = ClassIDList;
            BizAction.IsupdatePreviousRate = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsAddTariffServiceClassRateBizActionVO result = arg.Result as clsAddTariffServiceClassRateBizActionVO;
                    if (((clsAddUpdateServiceMasterTariffBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "";
                        string msgText = " Tariff Service Rate Linked Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    else if (result.SuccessStatus == 2)
                    {
                        string msgTitle = "";
                        string msgText = "Tariff Service Rate Modified Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    else if (result.SuccessStatus == 3)
                    {
                        string msgTitle = "";
                        string msgText = "Tariff Removed Successfully";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    this.DialogResult = false;
                    PleaseWait.Close();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            PleaseWait.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void frmAssignServiceTariff_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                ItemList1 = new ObservableCollection<clsServiceTarrifClassRateDetailsNewVO>();
                if (OperationType == Convert.ToInt32(TariffOperationType.Modify))
                {
                    SetCommandButtonState("Modify");
                }
                else if (OperationType == Convert.ToInt32(TariffOperationType.New))
                {
                    SetCommandButtonState("New");
                }
                FillClassGrid();
                OKButton.IsEnabled = false;
            }
            IsPageLoded = true;
        }

        private void ApplyAll_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chkApplyAll.IsChecked == true)
            {
                ApplyAllClassRate Win = new ApplyAllClassRate();
                Win.ServiceID = ServiceID;
                Win.DeepCopyServiceClassList = ClassList;
                Win.OnSaveButton_Click += new RoutedEventHandler(ApplyAllClassRate_OnSaveButton_Click);
                Win.OnCancel_Click += new RoutedEventHandler(CancelButton_OnCancel_Click);
                Win.Show();
            }
            else
            {
                foreach (var item in TariffClassList)
                {
                    item.Status = false;
                    item.IsChecked = false;
                    item.IsEnable = false;
                    item.IsChkEnable = true;
                    item.Rate = Convert.ToDecimal("0.00");
                    SelectedTariffList.Remove(item);
                }
                PagedCollectionView collection = new PagedCollectionView(TariffClassList);
                collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
                dgServiceTariff.ItemsSource = null;
                dgServiceTariff.ItemsSource = collection;
            }
            if (SelectedTariffList.Count > 0)
                OKButton.IsEnabled = true;
            else
                OKButton.IsEnabled = false;
        }
        void CancelButton_OnCancel_Click(object sender, RoutedEventArgs e)
        {
            chkApplyAll.IsChecked = false;
        }
        public decimal PreviousServiceClassRate;
        List<clsServiceMasterVO> ClassServiceList = new System.Collections.Generic.List<clsServiceMasterVO>();
        void ApplyAllClassRate_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            ClassServiceList = (List<clsServiceMasterVO>)((ApplyAllClassRate)sender).grdClass.ItemsSource;
            foreach (clsServiceTarrifClassRateDetailsNewVO item in TariffClassList)
            {
                foreach (clsServiceMasterVO item1 in ClassServiceList)
                {
                    if (item.ClassID == item1.ClassID && item.ClassName == item1.ClassName && item1.IsSelected==true)
                    {
                        item.Rate = item1.Rate;
                        item.IsChecked = true;
                        item.IsEnable = false;
                        //item.IsEnable = true;
                        SelectedTariffList.Add(item);
                        break;
                    }
                }
                Count = SelectedTariffList.Count;
                if (Count > 0)
                    OKButton.IsEnabled = true;
                else
                    OKButton.IsEnabled = false;
            }
            PagedCollectionView collection = new PagedCollectionView(TariffClassList);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));

            dgServiceTariff.ItemsSource = null;
            dgServiceTariff.ItemsSource = collection;
            dgServiceTariff.UpdateLayout();
            dgServiceTariff.Focus();
        }

        List<clsServiceMasterVO> ServicesList = new List<clsServiceMasterVO>();
        private void FetchData(long ServiceID, int OperationType)
        {
            WaitIndicator Indicator = new WaitIndicator();
            Indicator.Show();
            clsGetTariffServiceClassRateNewBizActionVO BizAction = new clsGetTariffServiceClassRateNewBizActionVO();
            BizAction.ServiceID = ServiceID;
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.OperationType = OperationType;
            if (!String.IsNullOrEmpty(txtTariffName.Text))
                BizAction.TariffName = txtTariffName.Text;
            else
                BizAction.TariffName = String.Empty;
            BizAction.ServiceDetails = objMasterVO;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            if (rdbBoth.IsChecked == true)
            {
                BizAction.SortExpression = "";
            }
            else if (rdbPackage.IsChecked == true)
            {
                BizAction.SortExpression = "Package";
            }
            else if (rdbTariff.IsChecked == true)
            {
                BizAction.SortExpression = "Tariff";
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetTariffServiceClassRateNewBizActionVO)arg.Result).ServiceList != null)
                    {
                        clsGetTariffServiceClassRateNewBizActionVO result = arg.Result as clsGetTariffServiceClassRateNewBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        TariffClassList = new ObservableCollection<clsServiceTarrifClassRateDetailsNewVO>();

                        if (result.ServiceList != null)
                        {
                            ItemList1.Clear();
                            DataList.Clear();
                            foreach (var item in result.ServiceList)
                            {
                                item.IsChecked = false;
                                item.IsEnable = false;
                                if (OperationType == Convert.ToInt32(TariffOperationType.Remove))
                                {
                                    item.IsEnable = false;
                                }
                                ItemList1.Add(item);
                                DataList.Add(item);
                            }
                            List<clsServiceTarrifClassRateDetailsNewVO> DeleteItemList1 = new List<clsServiceTarrifClassRateDetailsNewVO>();
                            foreach (var ItemList in ItemList1)
                            {
                                DeleteItemList1.Add(ItemList);
                            }
                            if (OperationType == Convert.ToInt32(TariffOperationType.Remove) && DeleteTariffList.Count > 0)
                            {
                                foreach (var item1 in DeleteTariffList)
                                {
                                    foreach (var item in DeleteItemList1)
                                    {
                                        if (item.TSMID == item1.TSMID)
                                        {
                                            item.IsChecked = true;
                                        }
                                    }
                                }
                            }
                            if (OperationType == Convert.ToInt32(TariffOperationType.Modify))
                            {
                                TariffClassList2 = ItemList1;
                            }
                            else
                            {
                                foreach (clsServiceTarrifClassRateDetailsNewVO item in ItemList1)
                                {
                                    foreach (clsServiceMasterVO item1 in ClassList)
                                    {
                                        clsServiceTarrifClassRateDetailsNewVO item3 = new clsServiceTarrifClassRateDetailsNewVO();
                                        item3 = item.DeepCopy();
                                        item3.ClassName = item1.ClassName;
                                        item3.ClassID = item1.ClassID;
                                        item3.Rate = item1.Rate;
                                        TariffClassList.Add(item3);
                                    }
                                }
                                foreach (var item in TariffClassList)
                                {
                                    foreach (var item1 in SelectedTariffList)
                                    {
                                        if (item.TariffID == item1.TariffID && item.ClassID == item1.ClassID)
                                        {
                                            item.Rate = item1.Rate;
                                            item.IsEnable = item1.IsEnable;
                                            item.IsChecked = item1.IsChecked;
                                            break;
                                        }
                                    }
                                }
                                TariffClassList2 = TariffClassList.DeepCopy();
                                ExistingTariffList = result.ExistingClassRates;
                                if (((clsGetTariffServiceClassRateNewBizActionVO)arg.Result).ServiceList != null && result.ExistingClassRates != null && result.ExistingClassRates.Count > 0)
                                {
                                    foreach (var i in TariffClassList2.ToList())
                                    {
                                        foreach (var j in result.ExistingClassRates.ToList())
                                        {
                                            if (OperationType == Convert.ToInt32(TariffOperationType.New) && i.ClassID == j.ClassID && i.ClassName == j.ClassName && i.TariffID == j.TariffID)
                                            {
                                                TariffClassList2.Remove(i);
                                            }
                                        }
                                    }
                                }
                            }
                            TariffClassList = TariffClassList2;
                            PagedCollectionView collection = new PagedCollectionView(TariffClassList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
                            if (TariffClassList != null && TariffClassList.Count < 0)
                            {
                                chkApplyAll.IsEnabled = false;
                                chkSearchTariff.IsEnabled = false;
                            }
                            else
                            {
                                chkApplyAll.IsEnabled = true;
                                chkSearchTariff.IsEnabled = true;
                            }
                            dgServiceTariff.ItemsSource = null;
                            dgServiceTariff.ItemsSource = collection;
                            dgServiceTariff.SelectedIndex = -1;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                            Indicator.Close();
                        }
                        Indicator.Close();
                    }
                    else
                    {
                        if (OperationType == Convert.ToInt32(TariffOperationType.New))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("PALASH", "Master Entry Done For This Service", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            Indicator.Close();
                            this.DialogResult = true;
                        }
                        else if (OperationType == Convert.ToInt32(TariffOperationType.Modify))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("PALASH", "No Records Found To Modify Or Remove", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            Indicator.Close();
                            this.DialogResult = true;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    Indicator.Close();
                }
                Indicator.Close();
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        List<clsServiceMasterVO> ClassList = new List<clsServiceMasterVO>();
        private void FillClassGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        foreach (var item in objList)
                        {
                            ClassList.Add(new clsServiceMasterVO() { ClassID = item.ID, ClassName = item.Description, UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId });
                        }

                        FetchData(ServiceID, OperationType);
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetColumn()
        {

            foreach (var item in ItemList1)
            {
                DataGridColumn column = dgServiceTariff.Columns[2];
                FrameworkElement fe = column.GetCellContent(item);
                FrameworkElement RE = GetParent(fe, typeof(DataGridCell));

                if (RE != null)
                {
                    DataGridCell cell = (DataGridCell)RE;
                    cell.IsEnabled = true;
                }
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            SelectedTariffList = new List<clsServiceTarrifClassRateDetailsNewVO>();
            ItemList1 = new ObservableCollection<clsServiceTarrifClassRateDetailsNewVO>();
            chkApplyAll.IsChecked = false;
            FetchData(ServiceID, OperationType);
        }
        private void txtRate_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdApplyRate_Click(object sender, RoutedEventArgs e)
        {
            if (chkApplyAll.IsChecked == true)
            {
                foreach (var item in ItemList1)
                {
                }
            }
            else
            {
                foreach (var item in ItemList1)
                {
                    item.Rate = 0;
                }
            }
            dgServiceTariff.ItemsSource = null;
            dgServiceTariff.ItemsSource = ItemList1;
            dgServiceTariff.UpdateLayout();
            dgServiceTariff.Focus();
        }

        private void chkTariff_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ch = sender as CheckBox;
            clsServiceTarrifClassRateDetailsNewVO objVO = (clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem;
            if (dgServiceTariff.SelectedItem != null)
            {
                foreach (clsServiceTarrifClassRateDetailsNewVO item in dgServiceTariff.ItemsSource)
                {
                    if (ch.IsChecked == true && item.TariffID == objVO.TariffID && item.ClassID == objVO.ClassID)
                    {
                        item.IsChecked = true;
                        item.IsEnable = true;
                        SelectedTariffList.Add(item);

                       
                       
                    }
                    else if (ch.IsChecked == false && item.TariffID == objVO.TariffID && item.ClassID == objVO.ClassID)
                    {
                        item.IsChecked = false;
                        item.Rate = Convert.ToDecimal("0.00");
                        item.IsEnable = false;
                        SelectedTariffList.Remove(item);
                        chkApplyAll.IsChecked = false;
                    }
                }
                if (SelectedTariffList.Count == 0)
                {
                    chkApplyAll.IsChecked = false;
                    OKButton.IsEnabled = false;
                }
                else if (SelectedTariffList.Count > 0)
                {
                    OKButton.IsEnabled = true;
                }
                else if (Count == SelectedTariffList.Count)
                {
                    chkApplyAll.IsChecked = true;
                }
                //PagedCollectionView collection = new PagedCollectionView(TariffClassList);
                //collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
                //dgServiceTariff.ItemsSource = null;
                //dgServiceTariff.ItemsSource = collection;
                dgServiceTariff.Focus();
                dgServiceTariff.UpdateLayout();
            }

        }

        private void txtRate_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null  )
            {
                if ((selectionStart == 16 && ((TextBox)sender).Text.ToString().ElementAt<Char>(16) == 46))
                {

                }
                else
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
            else if ((((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces()))
            {
                if (dgServiceTariff.SelectedItem != null)
                {
                    var ExistingID = from r in SelectedTariffList
                                     where r.ClassID == ((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem).ClassID && r.TariffID == ((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem).TariffID
                                     select r;
                    if (ExistingID.ToList().Count() > 0)
                    {
                        foreach (var item in SelectedTariffList)
                        {
                            if (item.ClassID == ((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem).ClassID && item.TariffID == ((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem).TariffID)
                            {
                                item.Rate = Convert.ToDecimal(((TextBox)sender).Text);
                                TextBox txt = sender as TextBox;
                                txt.Focus();
                            }
                        }
                    }
                }
                else
                {
                    clsServiceTarrifClassRateDetailsNewVO obj = ((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedItem);
                    if (obj != null)
                    {
                        foreach (clsServiceTarrifClassRateDetailsNewVO item1 in TariffClassList)
                        {
                            if (obj.ClassID == item1.ClassID && obj.ClassName == item1.ClassName)
                            {
                                item1.Rate = Convert.ToDecimal(((TextBox)sender).Text);
                                item1.IsChecked = true;
                                item1.IsEnable = true;
                                SelectedTariffList.Add(item1);
                                break;
                            }
                        }
                    }
                }
            }


            //((clsServiceTarrifClassRateDetailsNewVO)dgServiceTariff.SelectedIndex).


        }

        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            //if (((e.Key > Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Decimal || e.PlatformKeyCode == 46))
            //{
            //    e.Handled = false;
            //}
            //else
            //{
            //    e.Handled = true;
            //}
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        
        }

        private void SearchTariff_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                AllStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                AllStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData(ServiceID, OperationType);
            chkApplyAll.IsChecked = false;
        }

        private void txtTariffName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            {
                cmdSearch_Click(sender, e);
            }
        }

        #region Modify Tariff Class Rate For Service
        /* Added By SUDHIR on 22/05/2014 */
        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "Do you want to Modify Service Tariff Class Rates?";
            MessageBoxControl.MessageBoxChildWindow msgW =
            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    Flag = false;
                    ModifyAndRemoveTariffClassRates();
                }
            };
            msgW.Show();
        }

        #region private Methods
        private void ModifyAndRemoveTariffClassRates()
        {
            WaitIndicator PleaseWait = new WaitIndicator();
            PleaseWait.Show();
            clsAddUpdateServiceMasterTariffBizActionVO BizAction = new clsAddUpdateServiceMasterTariffBizActionVO();
            BizAction.SelectedTariffClassList = new List<clsServiceTarrifClassRateDetailsNewVO>();
            BizAction.SelectedTariffClassList = SelectedTariffList;
            BizAction.ServiceID = ServiceID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsModifyTariffClassRates = true;
            BizAction.IsRemoveTariffClassRatesLink = Flag;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsAddTariffServiceClassRateBizActionVO result = arg.Result as clsAddTariffServiceClassRateBizActionVO;
                    if (((clsAddUpdateServiceMasterTariffBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        string msgTitle = "";
                        string msgText = " Tariff Service Rate Modified Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    if (((clsAddUpdateServiceMasterTariffBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                        string msgTitle = "";
                        string msgText = " Tariff Service Class Rate Linking Removed Successfully.";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.Show();
                    }
                    this.DialogResult = false;
                    PleaseWait.Close();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            PleaseWait.Close();
        }
        #endregion
        #endregion

        #region RemoveTariffand Class Linking
        private void CmdRemove_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "Do you want to Remove Service Tariff Class Rate Linking?";
            MessageBoxControl.MessageBoxChildWindow msgW =
            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    Flag = true;
                    ModifyAndRemoveTariffClassRates();
                }
            };
            msgW.Show();
        }

        #endregion

        #region Button State

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    CmdModify.Visibility = Visibility.Collapsed;
                    OKButton.IsEnabled = true;
                    CloseButton.IsEnabled = true;
                    break;

                case "Modify":
                    CmdModify.Visibility = Visibility.Visible;
                    CmdRemove.Visibility = Visibility.Visible;
                    OKButton.Visibility = Visibility.Collapsed;
                    CloseButton.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void txtTariffRate_LostFocus(object sender, RoutedEventArgs e)
        {
            PagedCollectionView collection = new PagedCollectionView(TariffClassList);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
            dgServiceTariff.ItemsSource = null;
            dgServiceTariff.ItemsSource = collection;
            dgServiceTariff.UpdateLayout();
            dgServiceTariff.Focus();
        }

        private void chkTariffStatus_LostFocus(object sender, RoutedEventArgs e)
        {
            PagedCollectionView collection = new PagedCollectionView(TariffClassList);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
            dgServiceTariff.ItemsSource = null;
            dgServiceTariff.ItemsSource = collection;
            dgServiceTariff.UpdateLayout();
            dgServiceTariff.Focus();
        }
    }
}

