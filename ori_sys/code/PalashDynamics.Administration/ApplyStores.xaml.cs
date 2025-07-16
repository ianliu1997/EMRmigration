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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.Administration
{
    public partial class ApplyStores : ChildWindow
    {
        public ApplyStores()
        {
            InitializeComponent();
        }

       public List<clsUserUnitDetailsVO> UnitList = new List<clsUserUnitDetailsVO>();
       public List<clsItemStoreVO> SelectedStoreList = new List<clsItemStoreVO>();
       string msgText = "";
       public event RoutedEventHandler OnCloseButton_Click;
       public event RoutedEventHandler OnSaveButton_Click;

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // FillStores(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            UnitList.Insert(0,new clsUserUnitDetailsVO{UnitID=0,UnitName="Select"});
            cmbStore.ItemsSource = UnitList;
            cmbStore.SelectedItem = UnitList[0];
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsUserUnitDetailsVO)cmbStore.SelectedItem).UnitID !=0)
            FillUnitStoreFirst();
        }

        private void FillUnitStoreFirst()
        {
            List<clsItemStoreVO> StoreUnitList = new List<clsItemStoreVO>();
            try
            {
                clsGetUnitStoreBizActionVO BizAction = new clsGetUnitStoreBizActionVO();

                if ((clsUserUnitDetailsVO)cmbStore.SelectedItem != null)
                {

                  //  grbUnitStore.Header = ((clsUserUnitDetailsVO)dgUnitList.SelectedItem).UnitName + " Stores";
                    long SelectedUnitId = ((clsUserUnitDetailsVO)cmbStore.SelectedItem).UnitID;
                    BizAction.ID = SelectedUnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, ea) =>
                    {
                        if (ea.Result != null && ea.Error == null)
                        {
                            List<clsItemStoreVO> StoreList = new List<clsItemStoreVO>();
                            if (((clsGetUnitStoreBizActionVO)ea.Result).Details != null)
                            {
                                StoreList = ((clsGetUnitStoreBizActionVO)ea.Result).Details;

                                dgUnitStoreList.ItemsSource = null;
                                dgUnitStoreList.ItemsSource = StoreList;
                                //foreach (var item in StoreList)
                                //{
                                //    //  if(item.status==true)
                                //    StoreUnitList.Add(new clsItemStoreVO() { ID = item.ID, StoreName = item.StoreName, status = item.StoreStatus });
                                //}

                              //  FillUnitStore(StoreUnitList);
                            }
                            else
                            {
                                //clear Store Details.
                                //grbUnitStore.Visibility = Visibility.Collapsed;
                                string msgTitle = "";
                                msgText = "No Store Assigned to the Selected Unit.";
                                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWindow.Show();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, new clsUserVO());
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                //   throw;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //base.OnClosed(e);
            //Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);

            this.DialogResult = false;
            if (OnCloseButton_Click != null)
                OnCloseButton_Click(this, new RoutedEventArgs());
        }

        private void chkStoreStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                ((clsItemStoreVO)dgUnitStoreList.SelectedItem).StoreStatus = true;
                SelectedStoreList.Add((clsItemStoreVO)dgUnitStoreList.SelectedItem);
            }
            else
            {
                ((clsItemStoreVO)dgUnitStoreList.SelectedItem).StoreStatus = false;
                SelectedStoreList.Remove((clsItemStoreVO)dgUnitStoreList.SelectedItem);
            }
        }

        private void cmdSetStore_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        
    }
}

