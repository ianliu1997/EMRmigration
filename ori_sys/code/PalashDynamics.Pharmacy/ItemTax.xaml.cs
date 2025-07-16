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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemTax : ChildWindow
    {
        
        bool IsPageLoded = false;
        clsItemMasterVO objMasterVO=null;
        List<clsItemTaxVO> ItemTaxList = null;
        List<clsItemTaxVO> _ItemTax = null;
        public string ItemName { get; set; }
        public ItemTax()
        {
            InitializeComponent();
            this.DataContext = new clsItemTaxVO();
        }

      
        public void GetItemDetails(clsItemMasterVO objItemMasterVO)
        {
            //lblItemName.Text = objItemMasterVO.ItemName;
            objMasterVO = objItemMasterVO;
        }
        public clsAddItemTaxBizActionVO BizActionobj;
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                foreach(var item in (List<clsItemTaxVO>)dgItemTax.ItemsSource)
                {
                    if (item.status == true)
                    {
                        if (item.ApplicableFor == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW5 =
                                                                                              new MessageBoxControl.MessageBoxChildWindow("", "Please Select Applicable For", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW5.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW5.Show();
                            return;
                        }
                        else if (item.ApplicableFor.ID == 0 )
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Applicable For", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW2.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW2.Show();
                            return;
                        }
                        if (item.ApplicableOn == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Please Select Applicable On", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW3.Show();
                            return;
                        }
                        else if (item.ApplicableOn.ID == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                                    new MessageBoxControl.MessageBoxChildWindow("", "Please Select Applicable On", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW4.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW4.Show();
                            return;
                        }
                        if (item.Percentage == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                                               new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Percentage", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                            return;
                        }
                    }
                }
              
                BizActionobj = new clsAddItemTaxBizActionVO();
                BizActionobj.ItemTax = new clsItemTaxVO();
                BizActionobj.ItemTaxList = (List<clsItemTaxVO>)dgItemTax.ItemsSource;
                BizActionobj.ItemTax.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemTax.UpdatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionobj.ItemTax.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                BizActionobj.ItemTax.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                BizActionobj.ItemTax.AddedDateTime = DateTime.Now;

                BizActionobj.ItemTax.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                BizActionobj.ItemTax.ItemID = objMasterVO.ID;
                BizActionobj.ItemTax.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                string msgTitle = "";

                string msgText = "";

                if (IsEditMode == false)
                {
                    msgText = "Are you sure you want to Save the  Details";

                }
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW1.Show();






            }
            catch (Exception)
            {

                throw;
            }
        }




        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {




                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessAsync(BizActionobj, new clsUserVO());



                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {

                            //IF DATA IS SAVED
                            if (((PalashDynamics.ValueObjects.Inventory.clsAddItemTaxBizActionVO)(arg.Result)).SuccessStatus == 1)
                            {
                                
                                    string msgTitle = "";
                                    string msgText = "Item Tax Saved Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();

                              



                            }

                            this.Close();


                        }


                    };


                    client.CloseAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
       
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (objMasterVO.ItemName != null)
                {
                    lblItemName.Text = objMasterVO.ItemName.ToString();
                }

                
             
                FillDataGrid();
            }
            IsPageLoded = true;
          
           
            
        }

        private void FillDataGrid()
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

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                        ItemTaxList = new List<clsItemTaxVO>();
                        clsItemTaxVO obj = new clsItemTaxVO();

                        foreach (var item in objList)
                        {

                            ItemTaxList.Add(new clsItemTaxVO { ID = item.ID, TaxName = item.Description, ApplicableForList = obj.ApplicableForList, ApplicableOnList = obj.ApplicableOnList, status = item.Status });
                        }


                        dgItemTax.ItemsSource = null;
                        dgItemTax.ItemsSource = ItemTaxList;
                        GetItemTaxList();




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

        private void GetItemTaxList()
        {
            clsGetItemTaxBizActionVO objBizActionVO = new clsGetItemTaxBizActionVO();
            objBizActionVO.ItemTaxDetails = new clsItemTaxVO();

            objBizActionVO.ItemTaxDetails.ItemID = objMasterVO.ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    _ItemTax = null;
                    _ItemTax = new List<clsItemTaxVO>();
                    _ItemTax = ((clsGetItemTaxBizActionVO)args.Result).ItemTaxList;
                    CheckForExistingTax();

                }

            };

            client.ProcessAsync(objBizActionVO, new clsUserVO());
            client.CloseAsync();
        }

        private void CheckForExistingTax()
        {
            List<clsItemTaxVO> objList = (List<clsItemTaxVO>)dgItemTax.ItemsSource;


            if (objList != null && objList.Count > 0)
            {
                if (_ItemTax != null && _ItemTax.Count > 0)
                {
                    foreach (var item in _ItemTax)
                    {
                        foreach (var items in objList)
                        {
                            if (items.ID == item.TaxID)
                            {
                                items.status = item.status;
                                items.ApplicableFor = item.ApplicableFor;
                                items.ApplicableOn = item.ApplicableOn;
                                items.Percentage = item.Percentage;

                            }

                        }
                    }

                }
                dgItemTax.ItemsSource = objList;
            }
             
           
        }

     

      


        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }
        

      

     
       
    }
}

