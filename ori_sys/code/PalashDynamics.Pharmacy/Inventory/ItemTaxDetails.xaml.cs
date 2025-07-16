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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class ItemTaxDetails : ChildWindow
    {
        Boolean IsPageLoded = false;
        public clsItemMasterVO objMasterVO;
        List<MasterListItem> objList = new List<MasterListItem>();
        public List<clsItemStoreVO> ItemStoreList;
        public List<clsItemMasterVO> lstItemStore;
        List<clsItemTaxVO> ItemTaxList = null;
        public bool IsModify = false;
        public string msgText = String.Empty;
        clsItemTaxVO objSelectedTax;
        public List<clsItemStoreVO> SelectedStoreList = null;
        WaitIndicator indicator = new WaitIndicator();
        public ItemTaxDetails()
        {
            InitializeComponent();
            SelectedStoreList = new List<clsItemStoreVO>();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ViewerStore_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        List<clsItemTaxVO> objTaxList = new List<clsItemTaxVO>();
        private void cmdAddTax_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAddTax())
            {
             
                List<clsItemTaxVO> lstItemTax = dgTaxList.ItemsSource as List<clsItemTaxVO>;             
           
                lstItemTax.Clear();               
                clsItemTaxVO objTax = new clsItemTaxVO();
                objTax.TaxID = ((MasterListItem)cmbTax.SelectedItem).ID;
                objTax.TaxName = ((MasterListItem)cmbTax.SelectedItem).Description;
                objTax.ApplicableForId = ((MasterListItem)cmbApplicableFor.SelectedItem).ID;
                objTax.ApplicableOnId = ((MasterListItem)cmbApplicableOn.SelectedItem).ID;
                objTax.ApplicableForDesc = ((MasterListItem)cmbApplicableFor.SelectedItem).Description;
                objTax.ApplicableOnDesc = ((MasterListItem)cmbApplicableOn.SelectedItem).Description;
                objTax.status = true;
                objTax.Percentage = Convert.ToDecimal(txtPercentage.Text);                     

                objTax.ApplicableFrom = Convert.ToDateTime(DateTime.Now);
                objTax.ApplicableTo = Convert.ToDateTime(DateTime.Now);
                objTax.IsGST = Convert.ToBoolean(chkIsGST.IsChecked);

                if (IsModify == true)
                {

                    objTax.ID = objSelectedTax.ID;
                }

                if (taxseletion.IsChecked == true)
                {
                    objTax.TaxType = 1;
                    objTax.TaxTypeName = "Inclusive";
                }
                else
                {
                    objTax.TaxType = 2;
                    objTax.TaxTypeName = "Exclusive";
                }

                if (ValidateTaxUI())
                {                   
                   lstItemTax.Add(objTax);                 

                    cmdAddTax.Content = "Add Tax";
                    cmbTax.IsEnabled = true;
                    taxseletion.IsEnabled = true;
                    taxseletion1.IsEnabled = true;

                   
                    if (IsModify == true)
                    {
                        objTaxList.Clear();
                        objTaxList.Add(objTax);
                    }
                    else
                    {
                        objTaxList.Clear();
                        objTaxList.Add(objTax);
                    }
                    chkTaxForAllStores.IsChecked = false;
                    ApplyStore();                   
                }
                ClearTaxUI();
             
                cmbTax.SelectedItem = objList[0];
            }
        }

        private void dgTaxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public bool IsFromNewItem;
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (IsFromNewItem == false)
                {
                    lblItemName.Text = "";
                    if (objMasterVO.ItemName != null)
                    {
                        lblItemName.Text = "Item Name: " + objMasterVO.ItemName.ToString();
                    }
                }
                GetItemStoreList();
                FillTax();
                ClearTaxUI();
            }
            IsPageLoded = true;
            // count = 0;
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if ((clsItemTaxVO)dgTaxList.SelectedItem != null)
            {
                List<clsItemTaxVO> objList = new List<clsItemTaxVO>();
                msgText = "Do You Want To Remove Selected Tax For Item ?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {                   

                        clsAddItemClinicBizActionVO BizActionobj = new clsAddItemClinicBizActionVO();
                        BizActionobj.IsForDelete = true;
                        BizActionobj.ISMultipleStoreTax = true;
                        BizActionobj.DeleteTaxID = ((clsItemTaxVO)dgTaxList.SelectedItem).ID;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                objList = ((List<clsItemTaxVO>)dgTaxList.ItemsSource).ToList();
                                objList.Remove((clsItemTaxVO)dgTaxList.SelectedItem);
                                dgTaxList.ItemsSource = null;
                                dgTaxList.ItemsSource = objList;
                                objTaxList = objList;

                                foreach (clsItemStoreVO item in SelectedStoreList)
                                {
                                    //if (item.StoreID == ((clsItemStoreVO)dgStoreList.SelectedItem).StoreID)
                                    //{
                                        item.objStoreTaxList = new List<clsItemTaxVO>();
                                        item.objStoreTaxList = objTaxList.DeepCopy();
                                    //}
                                }
                            }
                            ClearTaxUI();
                            //cmbStore.SelectedItem = SelectedStoreList[0];
                            //   cmbTax.SelectedItem = objList[0];
                        };
                        client.ProcessAsync(BizActionobj, new clsUserVO());
                        client.CloseAsync();
                    }
                };
                msgW.Show();
            }
        }

       
      
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {

            if (dgTaxList.SelectedItem != null)
            {
                objSelectedTax = ((clsItemTaxVO)dgTaxList.SelectedItem);

                List<MasterListItem> objTaxList = ((List<MasterListItem>)cmbTax.ItemsSource).ToList();
                List<MasterListItem> objTaxList1 = ((List<MasterListItem>)cmbApplicableFor.ItemsSource).ToList();
                List<MasterListItem> objTaxList2 = ((List<MasterListItem>)cmbApplicableOn.ItemsSource).ToList();
                             

                foreach (MasterListItem item in objTaxList)
                {
                    if (item.ID == objSelectedTax.TaxID)
                    {
                        cmbTax.SelectedItem = item;
                        break;
                    }
                }
                foreach (MasterListItem item in objTaxList1)
                {
                    if (item.ID == objSelectedTax.ApplicableForId)
                    {
                        cmbApplicableFor.SelectedItem = item;
                        break;
                    }
                }
                foreach (MasterListItem item in objTaxList2)
                {
                    if (item.ID == objSelectedTax.ApplicableOnId)
                    {
                        cmbApplicableOn.SelectedItem = item;
                        break;
                    }
                }
                if (objSelectedTax.TaxType == 2)
                {
                    taxseletion.IsChecked = false;
                    taxseletion1.IsChecked = true;
                }
                else if (objSelectedTax.TaxType == 1)
                {
                    taxseletion1.IsChecked = false;
                    taxseletion.IsChecked = true;
                }

                dtpApplicableFromDate.Text = objSelectedTax.ApplicableFrom.ToString("dd/MM/yyyy");
                dtpApplicableToDate.Text = objSelectedTax.ApplicableTo.ToString("dd/MM/yyyy");
                chkIsGST.IsChecked = Convert.ToBoolean(objSelectedTax.IsGST);
                txtPercentage.Text = Convert.ToString(objSelectedTax.Percentage);
                cmdAddTax.Content = "Modify Tax";
                IsModify = true;
                cmbTax.IsEnabled = false;
                cmbTax.UpdateLayout();
                taxseletion.IsEnabled = false;
                taxseletion1.IsEnabled = false;

            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FillTax()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TaxMaster;
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
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbTax.ItemsSource = null;
                        cmbTax.ItemsSource = objList;
                        cmbTax.SelectedItem = objList[0];
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

        private void ClearTaxUI()
        {
            clsItemTaxVO obj = new clsItemTaxVO();
            cmbApplicableFor.ItemsSource = null;
            cmbApplicableFor.ItemsSource = obj.ApplicableForList;
            cmbApplicableFor.SelectedItem = obj.ApplicableForList[0];
            cmbApplicableOn.ItemsSource = null;
            cmbApplicableOn.ItemsSource = obj.ApplicableOnList;
            cmbApplicableOn.SelectedItem = obj.ApplicableOnList[0];
            txtPercentage.Text = obj.Percentage.ToString();

        }

        private void GetItemStoreList()
        {
            try
            {
                clsGetItemClinicBizActionVO BizActionObj = new clsGetItemClinicBizActionVO();
                long clinicid = 0;
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.ItemDetails = new clsItemMasterVO();
                BizActionObj.ItemDetails.RetrieveDataFlag = false;
                if (objMasterVO != null)
                    BizActionObj.ItemDetails.ItemID = objMasterVO.ID;
                BizActionObj.ItemDetails.UnitID = 0;
                BizActionObj.ISGteMultipleStoreList = true;
                BizActionObj.ItemDetails.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObj.ItemDetails.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObj.ItemDetails.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionObj.ItemDetails.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionObj.ItemDetails.AddedDateTime = DateTime.Now;
                BizActionObj.ItemDetails.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && ((clsGetItemClinicBizActionVO)(args.Result)) != null)
                    {
                        lstItemStore = new List<clsItemMasterVO>();
                        lstItemStore = ((clsGetItemClinicBizActionVO)(args.Result)).ItemList;
                        CheckForExistingTax(true);
                    }
                };
                client.ProcessAsync(BizActionObj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

      

        private bool ValidateTaxUI()
        {
            bool blnValidate = true;
            List<clsItemTaxVO> lstItemTax = (List<clsItemTaxVO>)dgTaxList.ItemsSource;

            clsItemTaxVO objItemTax = new clsItemTaxVO();            
            objItemTax.ApplicableForDesc = ((MasterListItem)cmbApplicableFor.SelectedItem).Description;
            objItemTax.ApplicableOnDesc = ((MasterListItem)cmbApplicableOn.SelectedItem).Description;
            objItemTax.Percentage = Convert.ToDecimal(txtPercentage.Text);
            objItemTax.TaxName = ((MasterListItem)cmbTax.SelectedItem).Description;   

            if (taxseletion.IsChecked == true)
            {
                objItemTax.TaxType = 1;                
            }
            else
            {
                objItemTax.TaxType = 2;                
            }
           
            if (objItemTax != null)
            {
                if (IsModify == true)                                                                      
                {
                    objItemTax.ID = objSelectedTax.ID;                    
                    if (lstItemTax.Where(z => z.TaxName.Equals(objItemTax.TaxName) && z.ApplicableForDesc.Equals(objItemTax.ApplicableForDesc) && z.StoreID.Equals(objItemTax.StoreID) && z.ID != (objItemTax.ID)).Any())  //&& z.Percentage.Equals(objItemTax.Percentage)
                    {                        
                        msgText = "You can not Add Same Tax Details";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("PALASH", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                cmdAddTax.Content = "Modify Tax";
                            }
                        };
                        msgW.Show();
                        blnValidate = false;                        
                    }

                    else
                    {
                        blnValidate = true;                      
                        objItemTax.ID = objSelectedTax.ID;
                    }
                }               
              
            }
            return blnValidate;
        }


        private bool ValidateAddTax()
        {
            bool IsValidate = true;
            if (((MasterListItem)cmbTax.SelectedItem).ID == 0)
            {
                cmbTax.TextBox.SetValidation("Please Select Tax.");
                cmbTax.TextBox.RaiseValidationError();
                cmbTax.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbTax.TextBox.ClearValidationError();
            if (((MasterListItem)cmbApplicableFor.SelectedItem).ID == 0)
            {
                cmbApplicableFor.TextBox.SetValidation("Please Select Applicable For.");
                cmbApplicableFor.TextBox.RaiseValidationError();
                cmbApplicableFor.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbApplicableFor.TextBox.ClearValidationError();
            if (((MasterListItem)cmbApplicableOn.SelectedItem).ID == 0)
            {
                cmbApplicableOn.TextBox.SetValidation("Please Select Applicable On.");
                cmbApplicableOn.TextBox.RaiseValidationError();
                cmbApplicableOn.TextBox.Focus();
                IsValidate = false;
            }
            else
                cmbApplicableOn.TextBox.ClearValidationError();

            if (String.IsNullOrEmpty(txtPercentage.Text))
            {
                txtPercentage.SetValidation("Please Add Tax Percentage");
                txtPercentage.RaiseValidationError();
                txtPercentage.Focus();
            }
            if (Convert.ToDouble(txtPercentage.Text) > 100)
            {
                txtPercentage.SetValidation("Tax Percentage Should Be Less Than 100");
                txtPercentage.RaiseValidationError();
                txtPercentage.Focus();
                IsValidate = false;
            }
            
            else
                txtPercentage.ClearValidationError();             

            return IsValidate;
        }


        private void ApplyStore()
        {
            try
            {
                indicator.Show();

                clsAddItemClinicBizActionVO BizActionobj = new clsAddItemClinicBizActionVO();
                BizActionobj.ItemStore = new clsItemStoreVO();
                BizActionobj.ItemStore.StoreList = new List<clsItemStoreVO>();
                BizActionobj.ItemTaxList = new List<clsItemTaxVO>();
                BizActionobj.ItemStore.DeletedStoreList = new List<clsItemStoreVO>();
                BizActionobj.ItemLinkStoreList = new List<clsItemMasterVO>();
                if (IsModify)
                {
                    BizActionobj.ISAddMultipleStoreTax = false;
                }
                else
                {
                     BizActionobj.ISAddMultipleStoreTax = true;
                }               
                BizActionobj.ISMultipleStoreTax = true;

                    BizActionobj.ItemTaxList.Clear();
                    if (SelectedStoreList.Where(z => z.StoreName.Equals("--Select--")).Any())
                    { SelectedStoreList.RemoveAt(0); }
                    BizActionobj.ItemStore.StoreList = SelectedStoreList;
               
                    BizActionobj.ItemStore.StoreList.Clear();
                    BizActionobj.ItemTaxList = objTaxList;
                    BizActionobj.ItemLinkStoreList = lstItemStore;
              
                BizActionobj.ItemStore.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemStore.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionobj.ItemStore.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionobj.ItemStore.AddedDateTime = DateTime.Now;
                BizActionobj.ItemStore.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                BizActionobj.ItemStore.ItemID = objMasterVO.ID;
            
                BizActionobj.IsForDelete = false;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddItemClinicBizActionVO)(arg.Result)).SuccessStatus == 1)
                        {
                            string msgtxt = "";
                         
                                if (((clsAddItemClinicBizActionVO)(arg.Result)).ResultStatus == 2)
                                {
                                    msgtxt = "Tax Already Define For This Item";                                    
                                }
                                else
                                {
                                    msgtxt = "Tax Added/Modify Successfully";                                   
                                }
                          
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                new MessageBoxControl.MessageBoxChildWindow("PALASH", msgtxt, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    CheckForExistingTax(true);
                                    ClearTaxUI();                                   
                                }
                            };
                            msgW.Show();
                        }
                    }
                };
                indicator.Close();
                client.ProcessAsync(BizActionobj, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void CheckForExistingTax(bool IsForAllStore)
        {
            try
            {
                
                clsGetItemStoreTaxListBizActionVO BizAction = new clsGetItemStoreTaxListBizActionVO();
                BizAction.StoreItemTaxList = new List<clsItemTaxVO>();
                BizAction.StoreItemTaxDetails = new clsItemTaxVO();            
                BizAction.StoreItemTaxDetails.ItemID = objMasterVO.ID;
                BizAction.IsForAllStore = IsForAllStore;
                BizAction.ISGetAllStoreTax = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetItemStoreTaxListBizActionVO)e.Result;
                        BizAction.StoreItemTaxList = ((clsGetItemStoreTaxListBizActionVO)e.Result).StoreItemTaxList;
                        objTaxList.Clear();

                        foreach (var item in BizAction.StoreItemTaxList)
                        {                          
                                objTaxList.Add(item);
                        }
                        dgTaxList.ItemsSource = null;
                        dgTaxList.ItemsSource = objTaxList;                       
                      
                    }
                };               
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

