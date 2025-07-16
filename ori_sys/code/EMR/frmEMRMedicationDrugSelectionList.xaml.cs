using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Text;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Master;

namespace EMR
{
    public partial class frmEMRMedicationDrugSelectionList : ChildWindow
    {
        #region Global Valirables

        PagedSortableCollectionView<clsItemMoleculeDetails> DataList { get; set; }
        public List<clsItemMoleculeDetails> SelectedDrugList;
        public event RoutedEventHandler OnAddButton_Click;
        bool isOtherDrug = false;
        WaitIndicator Indicatior;

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

        public frmEMRMedicationDrugSelectionList()
        {
            InitializeComponent();
            this.Title = "Drug List";
        }

        #endregion

        #region Load Event

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            SelectedDrugList = new List<clsItemMoleculeDetails>();
            DataList = new PagedSortableCollectionView<clsItemMoleculeDetails>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSizeSer = 50;

            FillMolecule();
            FillDrugList();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDrugList();
        }

        private void FillMolecule()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Molecule;
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
                        cmbMolecule.ItemsSource = objList;
                        cmbMolecule.SelectedItem = objList[0];
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            { }
        }

        private void FillDrugList()
        {
            try
            {
                Indicatior.Show();
                clsGetItemMoleculeNameBizActionVO BizActionObject = new clsGetItemMoleculeNameBizActionVO();
                BizActionObject.ItemMoleculeDetailsList = new List<clsItemMoleculeDetails>();

                BizActionObject.ItemName = txtBrandName.Text.Trim();

                if ((MasterListItem)cmbMolecule.SelectedItem != null)
                    BizActionObject.MoleculeID = ((MasterListItem)cmbMolecule.SelectedItem).ID;

                if (isOtherDrug == true)
                {
                    BizActionObject.isOtherDrug = true;
                    BizActionObject.StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyStoreID;
                    BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                BizActionObject.PagingEnabled = true;
                BizActionObject.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObject.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetItemMoleculeNameBizActionVO result = ea.Result as clsGetItemMoleculeNameBizActionVO;

                        if (result.ItemMoleculeDetailsList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = Convert.ToInt32(((clsGetItemMoleculeNameBizActionVO)ea.Result).TotalRows);

                            foreach (clsItemMoleculeDetails item in result.ItemMoleculeDetailsList)
                            {
                                if (SelectedDrugList != null && SelectedDrugList.Count > 0) //Added by AJ Date 24/11/2016 
                                {
                                    foreach (var Drugitem in SelectedDrugList)
                                    {
                                        if (Drugitem.ItemID == item.ItemID)
                                        {
                                            item.Selected = true;
                                            break;
                                        }
                                    }
                                }
                                //***// ------------------
                                DataList.Add(item);
                            }
                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DataList;

                        DataPagerDoc.Source = null;
                        DataPagerDoc.PageSize = (int)BizActionObject.MaximumRows;
                        DataPagerDoc.Source = DataList;
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                    }

                };
                client.ProcessAsync(BizActionObject, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        #endregion

        #region Click Events

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataPagerDoc.PageIndex != null)
                DataPagerDoc.PageIndex = 0;
            FillDrugList();
        }

        private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                try
                {
                    CheckBox chk = (CheckBox)sender;
                    StringBuilder strError = new StringBuilder();
                    if (chk.IsChecked == true)
                    {
                        if (SelectedDrugList.Count > 0)
                        {
                            var item = from r in SelectedDrugList
                                       where r.ItemID == ((clsItemMoleculeDetails)dgDrugList.SelectedItem).ItemID
                                       select new clsItemMoleculeDetails
                                       {
                                           Selected = r.Selected,
                                           ItemID = r.ItemID,
                                           ItemName = r.ItemName,
                                           MoleculeName = r.MoleculeName
                                       };
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((clsItemMoleculeDetails)dgDrugList.SelectedItem).ItemName);

                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "DRUG ALREADY SELECTED : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            }
                            else
                            {
                                SelectedDrugList.Add((clsItemMoleculeDetails)dgDrugList.SelectedItem);
                            }
                        }
                        else
                        {
                            SelectedDrugList.Add((clsItemMoleculeDetails)dgDrugList.SelectedItem);
                        }
                    }
                    else
                    {
                        SelectedDrugList.Remove((clsItemMoleculeDetails)dgDrugList.SelectedItem);

                        //Added by AJ Date 24/11/2016
                        foreach (var item in SelectedDrugList.ToList())
                        {
                            if (((clsItemMoleculeDetails)dgDrugList.SelectedItem).ItemID == item.ItemID)
                            {
                                SelectedDrugList.Remove(item);
                                break;
                            }
                        }
                        //***//---------------------
                    }
                    foreach (var item in SelectedDrugList)
                    {
                        item.Selected = true;

                    }
                    dgDrugSelectedList.ItemsSource = null;
                    dgDrugSelectedList.ItemsSource = SelectedDrugList;
                    dgDrugSelectedList.UpdateLayout();
                    dgDrugSelectedList.Focus();
                }
                catch (Exception)
                {
                }
            }
        }

        private void chkSelMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugSelectedList.SelectedItem != null)
            {
                try
                {
                    if (((CheckBox)sender).IsChecked == false)
                    {
                        long ItemID = ((clsItemMoleculeDetails)dgDrugSelectedList.SelectedItem).ItemID;
                        this.SelectedDrugList.Remove((clsItemMoleculeDetails)dgDrugSelectedList.SelectedItem);
                        foreach (var Drug in DataList.Where(x => x.ItemID == ItemID))
                        {
                            Drug.Selected = false;
                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DataList;
                        dgDrugList.UpdateLayout();
                        dgDrugList.Focus();

                        dgDrugSelectedList.ItemsSource = null;
                        dgDrugSelectedList.ItemsSource = SelectedDrugList;
                        dgDrugSelectedList.UpdateLayout();
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            bool isValid = true;
            if (DataList == null || SelectedDrugList == null || SelectedDrugList.Count <= 0)  //Added by AJ Date 25/11/2016
            {
                isValid = false;
            }
            else if (DataList.Count <= 0)
            {
                isValid = false;
            }
            if (isValid)
            {
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "No Drug/s Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        private void txtBrandName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)  //Added by AJ Date 25/11/2016
            {

                FillDrugList();
                DataPagerDoc.PageIndex = 0;
            }
        }

        private void cmbMolecule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}