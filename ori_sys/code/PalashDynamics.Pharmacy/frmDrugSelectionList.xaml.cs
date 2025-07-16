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
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Text;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy
{
    public partial class frmDrugSelectionList : ChildWindow
    {
        PagedSortableCollectionView<clsItemMoleculeDetails> DataList { get; set; }
        public ObservableCollection<clsItemMoleculeDetails> SelectedDrugList;
        public event RoutedEventHandler OnAddButton_Click;
        bool isOtherDrug = false;
        WaitIndicator Indicatior;
        public ObservableCollection<MasterListItem> DrugList { get; set; }

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
        public frmDrugSelectionList()
        {
            InitializeComponent();

        }
        public frmDrugSelectionList(bool isOtherDrug)
        {
            InitializeComponent();
            this.isOtherDrug = isOtherDrug;
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDrugList();
        }

        /// <summary>
        /// Child Window Loadeed is get called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedDrugList = new ObservableCollection<clsItemMoleculeDetails>();
            DataList = new PagedSortableCollectionView<clsItemMoleculeDetails>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSizeSer = 50;
            FillMolecule();
            FillDrugList();
            dgDrugSelectedList.ItemsSource = null;
            dgDrugSelectedList.ItemsSource = SelectedDrugList;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDrugList != null || SelectedDrugList.Count > 0)
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

        /// <summary>
        /// Cancel button click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillMolecule()
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

        private void FillDrugList()
        {
            Indicatior = new WaitIndicator();
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

        private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                if (SelectedDrugList == null)
                    SelectedDrugList = new ObservableCollection<clsItemMoleculeDetails>();
               
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();

                if (chk.IsChecked == true)
                {
                    if (SelectedDrugList.Count > 0)
                    {
                        //var item = from r in SelectedDrugList
                        //           where r.ID == ((clsItemMoleculeDetails)dgDrugList.SelectedItem).ID
                        //           select new MasterListItem
                        //           {
                        //               Status = r.Status,
                        //               ID = r.ID,
                        //               Description = r.Description
                        //           };
                        //if (item.ToList().Count > 0)
                        //{
                        //    if (strError.ToString().Length > 0)
                        //        strError.Append(",");
                        //    strError.Append(((MasterListItem)dgDrugList.SelectedItem).Description);

                        //    if (!string.IsNullOrEmpty(strError.ToString()))
                        //    {
                        //        string strMsg = "Drug already Selected : " + strError.ToString();

                        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //        msgW1.Show();
                        //        ((MasterListItem)dgDrugList.SelectedItem).Selected = false;
                        //        IsValid = false;
                        //    }
                        //}
                        long lngItemId= ((clsItemMoleculeDetails)(dgDrugList.SelectedItem)).ItemID;
                        String strMoleculeName=((clsItemMoleculeDetails)(dgDrugList.SelectedItem)).MoleculeName;
                        if (SelectedDrugList.Where(z => z.ItemID == lngItemId && z.MoleculeName == strMoleculeName).Any())
                        { 
                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "Drug already Selected : " + strError.ToString();

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                ((MasterListItem)dgDrugList.SelectedItem).Selected = false;
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
                    SelectedDrugList.Remove((clsItemMoleculeDetails)dgDrugList.SelectedItem);
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataPagerDoc.PageIndex != null)
                DataPagerDoc.PageIndex = 0;
            FillDrugList();
        }

        private void cmbMolecule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtBrandName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void chkSelMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugSelectedList.SelectedItem != null)
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
        }

    }
}

