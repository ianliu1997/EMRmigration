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
using PalashDynamic.Localization;
using PalashDynamics.ValueObjects.RSIJ;
using PalashDynamics.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR;
namespace EMR
{
    public partial class frmEMRMedDrugSelectionList : ChildWindow
    {
        #region Data Members
        public event RoutedEventHandler OnAddButton_Click;
        public PagedSortableCollectionView<MasterListItem> DataList { get; private set; }
        public ObservableCollection<MasterListItem> DrugList { get; set; }
        public List<clsGetDrugForAllergies> AllergiesDrugList { get; set; }
        List<MasterListItem> RouteList = new List<MasterListItem>();
        public clsVisitVO CurrentVisit { get; set; }
        public Boolean IsOtherDrug { get; set; }
        public Boolean IsInsuraceDrug { get; set; }
        public Boolean Isprocedure { get; set; }
        #endregion

        #region Paging

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDrugList();
        }

        #endregion

        public frmEMRMedDrugSelectionList()
        {
            InitializeComponent();
            this.Title = "Drug List";
            //this.Title = LocalizationManager.resourceManager.GetString("ttlDrugList");
            DataList = new PagedSortableCollectionView<MasterListItem>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSizeSer = 15;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillMolecule();
            FillDrugList();
            FillAllergiesDrug();
        }

        #region Fill Methods
        //private void FillMolecule()
        //{
        //    try
        //    {
        //        clsGetRSIJMasterListBizActionVO BizAction = new clsGetRSIJMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.GENERIK;
        //        BizAction.CodeColumn = "KODEGENERIK";
        //        BizAction.DescriptionColumn = "NAMAGENERIK";
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem("0", "-- Select --"));
        //                objList.AddRange(((clsGetRSIJMasterListBizActionVO)arg.Result).MasterList);
        //                cmbMolecule.ItemsSource = null;
        //                cmbMolecule.ItemsSource = objList;
        //                cmbMolecule.SelectedItem = objList[0];
        //            }

        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception) { }
        //}

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
            catch (Exception) { }
        }

        private void FillDrugList()
        {
            try
            {
                clsGetItemListBizActionVO BizActionObject = new clsGetItemListBizActionVO();
                BizActionObject.ItemList = new List<clsItemMasterVO>();
                BizActionObject.BrandName = txtBrandName.Text;
                if ((MasterListItem)cmbMolecule.SelectedItem != null)
                    BizActionObject.FilterIMoleculeNameId = ((MasterListItem)cmbMolecule.SelectedItem).ID;
                BizActionObject.FilterClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObject.FromEmr = true;
                BizActionObject.ForReportFilter = true;
                if (Isprocedure)
                {
                    BizActionObject.IsQtyShow = false;
                }
                else
                {
                    BizActionObject.IsQtyShow = false;
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
                        clsGetItemListBizActionVO result = ea.Result as clsGetItemListBizActionVO;
                        if (result.MasterList != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetItemListBizActionVO)ea.Result).TotalRowCount;

                            foreach (MasterListItem item in result.MasterList)
                            {
                                if (DrugList != null) //Added by AJ Date 24/11/2016 
                                {
                                    foreach (var Drugitem in DrugList)
                                    {
                                        if (Drugitem.ID == item.ID)
                                        {                                            
                                            item.Selected = true;                                            
                                            break;
                                        }
                                    }
                                }
                               //***// ------------------
                                item.Status = false;
                                DataList.Add(item);                                
                            }
                        }
                        else
                        {
                            string strMsg = "Medicine is not available";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DataList;                       

                        DataPagerDoc.Source = null;
                        DataPagerDoc.PageSize = BizActionObject.MaximumRows;
                        DataPagerDoc.Source = DataList;
                    }
                };
                client.ProcessAsync(BizActionObject, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception) { }
        }


        private void FillAllergiesDrug()
        {
            try
            {
                ClsGetPatientdrugAllergiesListBizActionVO BizAction = new ClsGetPatientdrugAllergiesListBizActionVO();
                BizAction.PatientID = CurrentVisit.PatientId;
                //BizAction.DrugID = ((MasterListItem)dgDrugList.SelectedItem).ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        AllergiesDrugList = ((ClsGetPatientdrugAllergiesListBizActionVO)ea.Result).DrugAllergiesList;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        //private void FillDrugList()
        //{
        //    WaitIndicator indicator = new WaitIndicator();
        //    indicator.Show();
        //    try
        //    {
        //        clsGetRSIJItemListBizActionVO BizActionObject = new clsGetRSIJItemListBizActionVO();
        //        BizActionObject.ItemList = new List<clsRSIJItemMasterVO>();
        //        BizActionObject.BrandName = txtBrandName.Text;
        //        BizActionObject.IsInsuraceDrug = IsInsuraceDrug;
        //        if (cmbMolecule.SelectedItem != null && ((MasterListItem)cmbMolecule.SelectedItem).Code != "0")
        //            BizActionObject.MoleculeCode = ((MasterListItem)cmbMolecule.SelectedItem).Description;
        //        BizActionObject.IsQtyShow = !this.IsOtherDrug;
        //        BizActionObject.PagingEnabled = true;
        //        BizActionObject.OPDNO = CurrentVisit.OPDNO;
        //        BizActionObject.StartRowIndex = DataList.PageIndex * DataList.PageSize;
        //        BizActionObject.MaximumRows = DataList.PageSize;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, ea) =>
        //        {
        //            if (ea.Result != null && ea.Error == null)
        //            {
        //                clsGetRSIJItemListBizActionVO result = ea.Result as clsGetRSIJItemListBizActionVO;
        //                if (result.ItemList != null)
        //                {
        //                    DataList.Clear();
        //                    DataList.TotalItemCount = ((clsGetRSIJItemListBizActionVO)ea.Result).TotalRowCount;

        //                    foreach (clsRSIJItemMasterVO item in result.ItemList)
        //                    {
        //                        item.Status = false;
        //                        DataList.Add(item);
        //                    }
        //                }
        //                dgDrugList.ItemsSource = null;
        //                dgDrugList.ItemsSource = DataList;

        //                DataPagerDoc.Source = null;
        //                DataPagerDoc.PageSize = BizActionObject.MaximumRows;
        //                DataPagerDoc.Source = DataList;
        //                indicator.Close();
        //            }
        //        };
        //        client.ProcessAsync(BizActionObject, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //        indicator.Close();
        //    }
        //}

        #endregion

        #region Click Events

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

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<MasterListItem>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSizeSer = 15;
            FillDrugList();
        }

        //private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        //{
        //    bool IsValid = true;

        //    if (dgDrugList.SelectedItem != null)
        //    {
        //        try
        //        {
        //            if (DrugList == null)
        //                DrugList = new ObservableCollection<clsRSIJItemMasterVO>();

        //            CheckBox chk = (CheckBox)sender;
        //            StringBuilder strError = new StringBuilder();

        //            if (chk.IsChecked == true)
        //            {
        //                if (DrugList.Count > 0)
        //                {
        //                    var item = from r in DrugList
        //                               where r.DrugCode == ((clsRSIJItemMasterVO)dgDrugList.SelectedItem).DrugCode
        //                               select new clsRSIJItemMasterVO
        //                               {
        //                                   Status = r.Status,
        //                                   DrugCode = r.DrugCode
        //                               };
        //                    if (item.ToList().Count > 0)
        //                    {
        //                        if (strError.ToString().Length > 0)
        //                            strError.Append(",");
        //                        strError.Append(((clsRSIJItemMasterVO)dgDrugList.SelectedItem).DrugCode);

        //                        if (!string.IsNullOrEmpty(strError.ToString()))
        //                        {
        //                            string strMsg = "Drug already Selected : " + strError.ToString();

        //                            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                            msgW1.Show();
        //                            ((clsRSIJItemMasterVO)dgDrugList.SelectedItem).Selected = false;

        //                            IsValid = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DrugList.Add((clsRSIJItemMasterVO)dgDrugList.SelectedItem);
        //                    }
        //                }
        //                else
        //                {
        //                    DrugList.Add((clsRSIJItemMasterVO)dgDrugList.SelectedItem);
        //                }
        //            }
        //            else
        //                DrugList.Remove((clsRSIJItemMasterVO)dgDrugList.SelectedItem);
        //        }
        //        catch (Exception) { }
        //    }
        //}

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            //ClsGetPatientdrugAllergiesListBizActionVO BizAction = new ClsGetPatientdrugAllergiesListBizActionVO();
            //BizAction.PatientID = CurrentVisit.PatientId;
            //BizAction.DrugID = ((MasterListItem)dgDrugList.SelectedItem).ID;
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, ea) =>
            //    {
            //        if (ea.Result != null && ea.Error == null)
            //        {
            //            if (((ClsGetPatientdrugAllergiesListBizActionVO)ea.Result).SuccessStatus == 0)
            //            {
                            bool IsValid = true;
                            if (dgDrugList.SelectedItem != null)
                            {
                                try
                                {
                                    if (DrugList == null)
                                        DrugList = new ObservableCollection<MasterListItem>();

                                    CheckBox chk = (CheckBox)sender;
                                    StringBuilder strError = new StringBuilder();

                                    if (chk.IsChecked == true)
                                    {
                                        if (AllergiesDrugList != null)
                                        {
                                            var itemAllergies = from r in AllergiesDrugList
                                                                where r.DrugId == ((MasterListItem)dgDrugList.SelectedItem).ID
                                                                select r;
                                            if (itemAllergies.ToList().Count > 0)
                                            {
                                                chk.IsChecked = false;
                                                ShowMessageBox("Patient have Allergie for this Drug ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                return;
                                            }
                                        }
                                        if (DrugList.Count > 0)
                                        {
                                            var item = from r in DrugList
                                                       where r.ID == ((MasterListItem)dgDrugList.SelectedItem).ID
                                                       select new MasterListItem
                                                       {
                                                           Status = r.Status,
                                                           // ID = r.ID,
                                                           Description = r.Description
                                                       };
                                            if (item.ToList().Count > 0)
                                            {
                                                if (strError.ToString().Length > 0)
                                                    strError.Append(",");
                                                strError.Append(((MasterListItem)dgDrugList.SelectedItem).Description);

                                                if (!string.IsNullOrEmpty(strError.ToString()))
                                                {
                                                    string strMsg = "Drug already Selected : " + strError.ToString();

                                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    msgW1.Show();
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
                                    {
                                        DrugList.Remove((MasterListItem)dgDrugList.SelectedItem);

                                        //Added by AJ Date 24/11/2016
                                        foreach (var item in DrugList.ToList())
                                        {
                                            if (((MasterListItem)dgDrugList.SelectedItem).ID == item.ID)
                                            {
                                                DrugList.Remove(item);
                                                break;
                                            }
                                        }
                                        //***//---------------------
                                    }
                                }
                                catch (Exception) { }
                            }
                //        }
                //        else
                //        {
                //            CheckBox chk = (CheckBox)sender;
                //            chk.IsChecked = false;
                //            ShowMessageBox("Patient have Allergie for this Drug ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //            return;
                //        }
                //    }
                //};
                //client.ProcessAsync(BizAction, new clsUserVO());
                //client.CloseAsync();
        }
        #endregion

        private void txtBrandName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
              
                FillDrugList();
                DataPagerDoc.PageIndex = 0;
            }
        }

        //private void FillRouteList()
        //{
        //    try
        //    {
        //        //Indicatior.Show();
        //        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_Route;
        //        BizAction.MasterList = new List<MasterListItem>();
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");

        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "-- Select --"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
        //                RouteList = objList;
        //            }

        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

    }
}
