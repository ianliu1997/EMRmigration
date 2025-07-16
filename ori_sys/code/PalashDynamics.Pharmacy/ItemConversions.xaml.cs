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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemConversions : ChildWindow
    {

        #region Variable Declarations

        Boolean IsPageLoded = false;
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public clsItemMasterVO objItemVO { get; set; }
        List<clsConversionsVO> ConversionFactList;
        WaitIndicator objIndicator;
        bool IsModify = false;
        long lID = 0;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        #endregion

        #region Constructor and Loaded
        public ItemConversions()
        {
            InitializeComponent();
            ConversionFactList = new List<clsConversionsVO>();
            objIndicator = new WaitIndicator();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                FillUnitOfMeasurement();
                itemName.Text = "Item Name: " + objItemVO.ItemName.ToString();
                GetConversionFactors(objItemVO.ID);
            }
            IsPageLoded = true;
        }
        #endregion

        #region Private Methods
        private void FillUnitOfMeasurement()
        {
            try
            {
                if (objIndicator == null) objIndicator = new WaitIndicator();
                objIndicator.Show();
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                        //if (objItemVO.BaseUM > 0)
                        //{
                        //    foreach (var item in objList.ToList())
                        //    {
                        //        if (item.ID == objItemVO.BaseUM)
                        //        {
                        //            objList.Remove(item);
                        //        }
                        //    }
                        //}
                        cmbFromUOM.ItemsSource = null;
                        cmbFromUOM.ItemsSource = objList.DeepCopy();
                        cmbFromUOM.SelectedItem = objList[0];

                        cmbToUOM.ItemsSource = null;
                        cmbToUOM.ItemsSource = objList.DeepCopy();
                        if (objItemVO.BaseUM > 0)
                        {
                            cmbToUOM.SelectedValue = objItemVO.BaseUM;
                            cmbToUOM.IsEnabled = false;
                        }
                        else
                        {
                            cmbToUOM.SelectedItem = objList[0];
                            cmbToUOM.IsEnabled = true;
                        }

                        cmbPurchaseUOM.ItemsSource = null;
                        cmbPurchaseUOM.ItemsSource = objList.DeepCopy();
                        cmbPurchaseUOM.SelectedValue = objItemVO.PUM;

                        cmbstockUOM.ItemsSource = null;
                        cmbstockUOM.ItemsSource = objList.DeepCopy();
                        cmbstockUOM.SelectedValue = objItemVO.SUM;

                        cmbSaleUOM.ItemsSource = null;
                        cmbSaleUOM.ItemsSource = objList.DeepCopy();
                        cmbSaleUOM.SelectedValue = objItemVO.SellingUM;

                        cmbBaseUOM.ItemsSource = null;
                        cmbBaseUOM.ItemsSource = objList.DeepCopy();
                        cmbBaseUOM.SelectedValue = objItemVO.BaseUM;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                objIndicator.Close();
                throw;
            }
        }

        public void Save()
        {
            try
            {
                clsAddUpdateItemConversionFactorListBizActionVO BizActionVO = new clsAddUpdateItemConversionFactorListBizActionVO();
                BizActionVO.UOMConversionVO = new clsConversionsVO();
                BizActionVO.IsForDelete = false;
                BizActionVO.UOMConversionVO.ID = 0;
                if (this.IsModify)
                {
                    BizActionVO.IsModify = true;
                    BizActionVO.UOMConversionVO.ID = this.lID;
                }
                else
                {
                    BizActionVO.IsModify = false;
                    BizActionVO.UOMConversionVO.ID = 0;
                }
                BizActionVO.UOMConversionVO.ItemID = objItemVO.ID;
                if (cmbFromUOM.SelectedItem != null)
                    BizActionVO.UOMConversionVO.FromUOMID = (cmbFromUOM.SelectedItem as MasterListItem).ID;
                if (cmbToUOM.SelectedItem != null)
                    BizActionVO.UOMConversionVO.ToUOMID = (cmbToUOM.SelectedItem as MasterListItem).ID;
                if (!string.IsNullOrEmpty(txtConversionFactor.Text))
                    BizActionVO.UOMConversionVO.ConversionFactor = Convert.ToSingle(txtConversionFactor.Text);

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsAddUpdateItemConversionFactorListBizActionVO)arg.Result).SuccessStatus == 0)
                        {
                            GetConversionFactors(objItemVO.ID);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Selected Record is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                        }
                        else if (((clsAddUpdateItemConversionFactorListBizActionVO)arg.Result).SuccessStatus == 1)
                        {
                            GetConversionFactors(objItemVO.ID);
                            cmbFromUOM.SelectedValue = (long)0;
                            txtConversionFactor.Text = string.Empty;
                            if (cmbToUOM.IsEnabled == true) cmbToUOM.SelectedValue = (long)0;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else if (((clsAddUpdateItemConversionFactorListBizActionVO)arg.Result).SuccessStatus == 2)
                        {
                            GetConversionFactors(objItemVO.ID);
                            cmbFromUOM.SelectedValue = (long)0;
                            txtConversionFactor.Text = string.Empty;
                            if (cmbToUOM.IsEnabled == true) cmbToUOM.SelectedValue = (long)0;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Record Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            this.IsModify = false;
                            cmdAddConversionFactor.Content = "Save";
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public void GetConversionFactors(long ItemID)
        {
            try
            {
                if (objIndicator == null) objIndicator = new WaitIndicator();
                objIndicator.Show();
                clsGetItemConversionFactorListBizActionVO BizActionVO = new clsGetItemConversionFactorListBizActionVO();
                BizActionVO.GetSavedData = false;
                BizActionVO.ItemID = ItemID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        ConversionFactList = ((clsGetItemConversionFactorListBizActionVO)arg.Result).UOMConversionList;
                        dgConversionList.ItemsSource = null;
                        dgConversionList.ItemsSource = ConversionFactList;
                        dgConversionList.UpdateLayout();
                    }
                    objIndicator.Close();
                };
                client.ProcessAsync(BizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception EX)
            {
                objIndicator.Close();
                throw EX;
            }

        }

        public Boolean SaveValidation()
        {
            bool result = true;
            if (cmbFromUOM.SelectedItem == null || (cmbFromUOM.SelectedItem as MasterListItem).ID == 0)
            {
                cmbFromUOM.TextBox.SetValidation("Please select From UOM");
                cmbFromUOM.TextBox.RaiseValidationError();
                cmbFromUOM.TextBox.Focus();
                result = false;
            }
            else
                cmbFromUOM.TextBox.ClearValidationError();


            if (cmbToUOM.SelectedItem == null || (cmbToUOM.SelectedItem as MasterListItem).ID == 0)
            {
                cmbToUOM.TextBox.SetValidation("Please select To UOM");
                cmbToUOM.TextBox.RaiseValidationError();
                cmbToUOM.TextBox.Focus();
                result = false;
            }
            else
                cmbToUOM.TextBox.ClearValidationError();

            if (string.IsNullOrEmpty(txtConversionFactor.Text.Trim()))
            {
                txtConversionFactor.SetValidation("Please add Conversion Factor");
                txtConversionFactor.RaiseValidationError();
                txtConversionFactor.Focus();
                result = false;
            }
            else
                txtConversionFactor.ClearValidationError();

            return result;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        #region Click Events
        private void hlkDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgConversionList.SelectedItem != null)
            {
                clsConversionsVO objVo = (clsConversionsVO)dgConversionList.SelectedItem;
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to delete the selected Record", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsAddUpdateItemConversionFactorListBizActionVO BizActionVo = new clsAddUpdateItemConversionFactorListBizActionVO();
                        BizActionVo.IsForDelete = true;
                        BizActionVo.UOMConversionVO = new clsConversionsVO();
                        BizActionVo.UOMConversionVO.ItemID = objItemVO.ID;
                        BizActionVo.UOMConversionVO.FromUOMID = objVo.FromUOMID;
                        BizActionVo.UOMConversionVO.ToUOMID = objVo.ToUOMID;
                        BizActionVo.UOMConversionVO.ConversionFactor = objVo.ConversionFactor;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsAddUpdateItemConversionFactorListBizActionVO)arg.Result).SuccessStatus == 1)
                                {
                                    GetConversionFactors(objItemVO.ID);
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "Record Deleted Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    this.IsModify = false;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            }
                        };
                        client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();

                    }
                };

                msgWD.Show();
            }
        }

        private void hlkEditRecord_Click(object sender, RoutedEventArgs e)
        {
            if (dgConversionList.SelectedItem != null)
            {
                cmdAddConversionFactor.Content = "Modify";
                cmbFromUOM.SelectedValue = (dgConversionList.SelectedItem as clsConversionsVO).FromUOMID;
                cmbToUOM.SelectedValue = (dgConversionList.SelectedItem as clsConversionsVO).ToUOMID;
                txtConversionFactor.Text = Convert.ToString((dgConversionList.SelectedItem as clsConversionsVO).ConversionFactor);
                this.IsModify = true;
                this.lID = (dgConversionList.SelectedItem as clsConversionsVO).ID;
            }
            else
            {
                cmdAddConversionFactor.Content = "Save";
                this.IsModify = false;
            }
        }

        private void cmdAddConversionFactor_Click(object sender, RoutedEventArgs e)
        {
            if (SaveValidation())
            {
                string strMsg = string.Empty;
                if (this.IsModify) strMsg = "Update?"; else strMsg = "Save?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure, you want to " + strMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        Save();
                    }
                };
                msgWD.Show();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        #region Other Events(KeyDown, TextChanged)
        private void txtConversionFactor_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtConversionFactor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtConversionFactor.Text) && !txtConversionFactor.Text.IsPositiveDoubleValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }
        #endregion

        private void cmbFromUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFromUOM.SelectedItem != null && cmbToUOM.SelectedItem != null)
            {
                if (((MasterListItem)cmbFromUOM.SelectedItem).ID > 0 && ((MasterListItem)cmbToUOM.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbFromUOM.SelectedItem).ID == ((MasterListItem)cmbToUOM.SelectedItem).ID)
                    {
                        txtConversionFactor.Text = Convert.ToString(1);
                        txtConversionFactor.IsEnabled = false;

                    }
                    else
                    {
                        txtConversionFactor.Text = string.Empty;
                        txtConversionFactor.IsEnabled = true;
                    }
                }
                else
                {
                    txtConversionFactor.Text = string.Empty;
                    txtConversionFactor.IsEnabled = true;
                }
            }
        }
    }
}

