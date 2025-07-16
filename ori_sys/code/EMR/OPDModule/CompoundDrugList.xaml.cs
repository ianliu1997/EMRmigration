using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.Collections.ObjectModel;
using System.Text;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Data;
using PalashDynamics.ValueObjects.CompoundDrug;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.UserControls;

namespace EMR
{
    public partial class CompoundDrugList : ChildWindow
    {
        #region Properties

        public PagedSortableCollectionView<clsCompoundDrugDetailVO> ComList { get; private set; }
        public PagedSortableCollectionView<MasterListItem> DataList { get; private set; }
        public ObservableCollection<MasterListItem> DrugList { get; set; }
        public clsVisitVO CurrentVisit { get; set; }

        public int ComDataListPageSizeSer
        {
            get
            {
                return ComList.PageSize;
            }
            set
            {
                if (value == ComList.PageSize) return;
                ComList.PageSize = value;
            }
        }
        public int DataListPageSizeSer
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

        #endregion

        #region Constructor
        public CompoundDrugList()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<MasterListItem>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSizeSer = 5;
            ComList = new PagedSortableCollectionView<clsCompoundDrugDetailVO>();
            ComList.OnRefresh += new EventHandler<RefreshEventArgs>(ComDataList_OnRefresh);
            ComDataListPageSizeSer = 5;
        }
        #endregion

        public event RoutedEventHandler OnAddButton_Click;

        #region Paging

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillDrugList();
        }

        void ComDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillCompund();
        }
        #endregion
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillMolecule();
            FillDrugList();
        }

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void FillMolecule()
        {
            clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.GENERIK;
            BizAction.CodeColumn = "KODEGENERIK";
            BizAction.DescriptionColumn = "NAMAGENERIK";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem("0", "-- Select --"));
                    objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
                    cmbMolecule.ItemsSource = objList;
                    cmbMolecule.SelectedItem = objList[0];
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillDrugList()
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            try
            {
                clsGetRSIJItemListBizActionVO BizActionObject = new clsGetRSIJItemListBizActionVO();
                BizActionObject.ItemList = new List<clsRSIJItemMasterVO>();
                if (txtBrandName.Text.Length > 0)
                {
                    BizActionObject.BrandName = txtBrandName.Text;
                    txtCompoundName.Text = "";
                }
                if (cmbMolecule.SelectedItem != null && ((MasterListItem)cmbMolecule.SelectedItem).Code != "0")
                {
                    BizActionObject.MoleculeCode = ((MasterListItem)cmbMolecule.SelectedItem).Description;
                    txtCompoundName.Text = "";
                }
                if (txtCompoundName.Text.Length > 0)
                {
                    FillCompund();
                }
                BizActionObject.IsQtyShow = true;
                BizActionObject.PagingEnabled = true;
                BizActionObject.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObject.MaximumRows = DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetRSIJItemListBizActionVO result = ea.Result as clsGetRSIJItemListBizActionVO;
                        if (result.ItemList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetRSIJItemListBizActionVO)ea.Result).TotalRowCount;

                            foreach (clsRSIJItemMasterVO item in result.ItemList)
                            {
                                item.Status = false;
                                MasterListItem objItem = new MasterListItem();
                                objItem.Code = item.DrugCode;
                                objItem.Description = item.DrugName;
                                objItem.AvailableStock = item.StockQty;
                                DataList.Add(objItem);
                            }

                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DataList;

                        DataPagerDoc.Source = null;
                        DataPagerDoc.PageSize = BizActionObject.MaximumRows;
                        DataPagerDoc.Source = DataList;
                        indicator.Close();
                    }
                };
                client.ProcessAsync(BizActionObject, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
            }
        }
        private void FillCompund()
        {
            clsGetCompoundDrugDetailsBizActionVO BizAction = new clsGetCompoundDrugDetailsBizActionVO();
            BizAction.CompoundDrug = new clsCompoundDrugMasterVO();
            string ItemCodes = string.Empty;

            if (DrugList != null)
            {
                foreach (var item in DrugList)
                {
                    ItemCodes = ItemCodes + item.Code;
                    ItemCodes = ItemCodes + ",";
                }
                if (ItemCodes.EndsWith(","))
                    ItemCodes = ItemCodes.Remove(ItemCodes.Length - 1, 1);
            }
            BizAction.ItemID = ItemCodes;
            BizAction.CompoundDrugValue = txtCompoundName.Text;
            BizAction.CompoundDrugDetailList = new List<clsCompoundDrugDetailVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetCompoundDrugDetailsBizActionVO result = ea.Result as clsGetCompoundDrugDetailsBizActionVO;
                    ComList.Clear();
                    ComList.TotalItemCount = ((clsGetCompoundDrugDetailsBizActionVO)ea.Result).TotalRowCount;
                    if (result.CompoundDrugDetailList != null && result.CompoundDrugDetailList.Count > 0)
                    {
                        foreach (clsCompoundDrugDetailVO item in result.CompoundDrugDetailList)
                        {
                            ComList.Add(item);
                        }
                    }
                    else
                    {
                        if (DrugList != null)
                        {
                            foreach (var item in DrugList)
                            {
                                item.PrintName = string.Empty;
                                item.SelectedID = 0;
                                item.SelectedID1 = 0;
                            }
                        }
                        dgSelectedList.ItemsSource = null;
                        dgSelectedList.ItemsSource = DrugList;
                    }
                    PagedCollectionView collection = new PagedCollectionView(ComList);
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("CompoundDrug"));
                    dgComList.ItemsSource = null;
                    dgComList.ItemsSource = collection;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (DrugList == null)
            {
                isValid = false;
            }
            else if (DrugList.Count <= 0)
            {
                isValid = false;
            }

            if (isValid)
            {
                if (Validation())
                {
                    if (txtCName.Text != "")
                    {
                        clsCheckCompoundDrugBizActionVO BizAction = new clsCheckCompoundDrugBizActionVO();
                        BizAction.CompoundDrug = new clsCompoundDrugMasterVO();
                        BizAction.CompoundDrug.Description = txtCName.Text;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsCheckCompoundDrugBizActionVO)arg.Result).SuccessStatus == 1)
                                {
                                    string strMsg = LocalizationManager.resourceManager.GetString("CompdDrugExist_Msg");
                                    ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                }
                                else
                                {
                                    this.DialogResult = true;
                                    if (OnAddButton_Click != null)
                                        OnAddButton_Click(this, new RoutedEventArgs());
                                }
                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        this.DialogResult = true;
                        if (OnAddButton_Click != null)
                            OnAddButton_Click(this, new RoutedEventArgs());
                    }
                }
            }
            else
            {
                string strMsg = LocalizationManager.resourceManager.GetString("NoDrugSelected_Msg");
                ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            }
        }

        private bool Validation()
        {
            bool Result = true;
            if (DrugList.Where(Z => Z.PrintName == string.Empty).Any() == true)
            {
                if (String.IsNullOrEmpty(txtCName.Text) || String.IsNullOrEmpty(txtCName.Text.Trim()))
                {
                    string strMsg = LocalizationManager.resourceManager.GetString("CompdNameReqd_Msg");
                    txtCName.SetValidation(strMsg);
                    txtCName.RaiseValidationError();
                    txtCName.Focus();
                    Result = false;
                }
                else
                {
                    txtCName.ClearValidationError();
                }
            }
            return Result;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            bool IsValid = true;
            txtCompoundName.Text = "";
            if (dgDrugList.SelectedItem != null)
            {
                if (DrugList == null)
                    DrugList = new ObservableCollection<MasterListItem>();

                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();

                if (chk.IsChecked == true)
                {
                    if (DrugList.Count > 0)
                    {
                        var item = from r in DrugList
                                   where r.Code == ((MasterListItem)dgDrugList.SelectedItem).Code
                                   select new MasterListItem
                                   {
                                       Status = r.Status,
                                       Code = r.Code,
                                       Description = r.Description
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((MasterListItem)dgDrugList.SelectedItem).Description);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = String.Format(LocalizationManager.resourceManager.GetString("DrugSelected_Msg") + strError.ToString());
                                ShowMessageBox(strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                ((MasterListItem)dgDrugList.SelectedItem).Selected = false;
                                IsValid = false;
                            }
                        }
                        else
                        {
                            DrugList.Add((MasterListItem)dgDrugList.SelectedItem);
                        }
                    }
                    else
                    {
                        DrugList.Add((MasterListItem)dgDrugList.SelectedItem);
                    }
                }
                else
                    DrugList.Remove((MasterListItem)dgDrugList.SelectedItem);
                FillCompund();
                dgSelectedList.ItemsSource = null;
                dgSelectedList.ItemsSource = DrugList;

            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataPagerDoc.Source != null)
                DataPagerDoc.PageIndex = 0;
            FillDrugList();
        }

        private void cmbMolecule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMolecule.SelectedItem != null)
            {
                if (DataPagerDoc.Source != null)
                    DataPagerDoc.PageIndex = 0;
                FillDrugList();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDrugList();
        }

        private void txtServiceName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (((TextBox)sender).Name == "txtBrandName")
                {
                    txtCompoundName.Text = String.Empty;
                    if (DataPagerDoc.Source != null)
                        DataPagerDoc.PageIndex = 0;
                    FillDrugList();
                    
                }
                else
                {
                    txtBrandName.Text = String.Empty;
                    if (cmbMolecule.SelectedItem != null && ((MasterListItem)cmbMolecule.SelectedItem).Code != "0")
                    {
                        cmbMolecule.SelectedValue = "0";
                    }
                    FillCompund();
                }
            }
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBox Ch = (CheckBox)sender;
            if (Ch.IsChecked == true)
            {
                DrugList = new ObservableCollection<MasterListItem>();
                foreach (var item in DataList)
                {
                    item.Selected = false;
                }
                var grp = (((CollectionViewGroup)Ch.DataContext).Items).ToList();
                foreach (clsCompoundDrugDetailVO item2 in grp)
                {
                    if (DrugList.Count > 0)
                    {
                        var item = from r in DrugList
                                   where r.Code == item2.ItemCode && r.SelectedID == item2.CompoundDrugID
                                   select new MasterListItem
                                   {
                                       Status = r.Status,
                                       Code = r.Code,
                                       Description = r.Description
                                   };
                        if (!(item.ToList().Count > 0))
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.PrintName = item2.CompoundDrug;
                            Obj.SelectedID = item2.CompoundDrugID;
                            Obj.SelectedID1 = item2.CompoundDrugUnitID;
                            Obj.Code = item2.ItemCode;
                            Obj.Description = item2.ItemName;
                            Obj.AvailableStock = item2.AvailableStock;

                            DrugList.Add(Obj);
                        }
                    }
                    else
                    {
                        MasterListItem Obj = new MasterListItem();
                        Obj.PrintName = item2.CompoundDrug;
                        Obj.SelectedID = item2.CompoundDrugID;
                        Obj.SelectedID1 = item2.CompoundDrugUnitID;
                        Obj.ID = item2.ItemID;
                        Obj.Code = item2.ItemCode;
                        Obj.Description = item2.ItemName;
                        Obj.AvailableStock = item2.AvailableStock;
                        DrugList.Add(Obj);
                    }

                }
            }
            else
            {
                DrugList = new ObservableCollection<MasterListItem>();

                foreach (var item in DataList)
                {
                    item.Selected = false;
                }
            }

            dgDrugList.ItemsSource = null;
            dgDrugList.ItemsSource = DataList;
            dgSelectedList.ItemsSource = null;
            dgSelectedList.ItemsSource = DrugList;
        }

    }
}