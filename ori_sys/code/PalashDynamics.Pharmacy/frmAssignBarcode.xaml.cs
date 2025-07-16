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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using MessageBoxControl;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.ComponentModel;
namespace PalashDynamics.Pharmacy
{
    public partial class frmAssignBarcode : ChildWindow
    {
        public List<clsItemMasterVO> AssignItemList { get; set; }
        public string ItemName  = null;
        public string BatchCode = null;
        public string BarCode = null;
        public string date = null;
        public string MRP = null;
        public event RoutedEventHandler OnAddButton_Click;

        public frmAssignBarcode()
        {
            InitializeComponent();
            
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtBarCode.SelectedText = BarCode;
            Title = "Item Name : " + ItemName; 
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveBarcode();
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void SaveBarcode()
        {
            if (txtBarCode.Text != "" )
            {
                clsAddAssignedItemBarcodeBizActionVO BizAction = new clsAddAssignedItemBarcodeBizActionVO();
                BizAction.ItemList = new List<clsItemMasterVO>();
                BizAction.ItemName = ItemName;
                BizAction.BatchCode = BatchCode;
                BizAction.BarCode = txtBarCode.Text;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                if (BarCode != txtBarCode.Text )
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                      {
                          if (arg.Error == null)
                          {
                              if (arg.Result != null)
                              {
                                  MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Barcode assigned Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                  mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(mgbx_OnMessageBoxClosed);
                                  mgbx.Show();
                              }
                          }
                      };

                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    this.DialogResult = false;
                }
            }
            else
            {
                //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one file is required.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //mgbx.Show();

            }
        }

        void mgbx_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Do you want to print Barcode", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(mgbxnew_OnMessageBoxClosed);
                mgbx.Show();
            }
        }

        void mgbxnew_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //BarcodeForm_N wind = new BarcodeForm_N();
                BarcodeForm win = new BarcodeForm();
                String strBarCode = txtBarCode.Text;               
                if (!String.IsNullOrEmpty(strBarCode))
                {
                    win.PrintData = "*" + strBarCode.ToUpper() + "*";
                    win.PrintItem = ItemName;
                    win.PrintDate = date;                   
                    win.printMRP = MRP;
                    win.Show();
                }
            }

        }
    }
}

