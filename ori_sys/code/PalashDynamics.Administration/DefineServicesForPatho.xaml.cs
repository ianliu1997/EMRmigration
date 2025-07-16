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
using PalashDynamics;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;
using System;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.Master;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Pharmacy;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;


namespace PalashDynamics.Administration
{
    public partial class DefineServicesForPatho : ChildWindow
    {
        Boolean IsPageLoded = false;
        clsPathoParameterDefaultValueMasterVO objMasterVO = null;
        Boolean IsRowSelected = false;
        public event RoutedEventHandler OnAddButton_Click;

        public long ParameterId = 0;
        public List<MasterListItem> selectedMasterList;  //to get selected services
        public bool isView = false;
        MasterListItem selectedMaster = new MasterListItem();
        public List<MasterListItem> SelectedTempList;  //to get selected services
        public DefineServicesForPatho()
        {
            InitializeComponent();
            

        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (isView != false && selectedMasterList!=null)
            {
                //txtServiceSearch.Visibility = Visibility.Collapsed;
                //lblServiceName.Visibility = Visibility.Collapsed;
                //cmdParameterSearch.Visibility = Visibility.Collapsed;
                //grdServices.Visibility = Visibility.Collapsed;
                //StockPanel.Visibility = Visibility.Collapsed;
                FetchService();
                OKButton.Content = "Add";
                if (SelectedTempList != null)
                {
                    grdSelectedServices.ItemsSource = SelectedTempList;
                    
                }
                else
                {
                    grdSelectedServices.ItemsSource = selectedMasterList;
                }
                
            }
            else
            {
               
                FetchService();
                OKButton.Content = "Add";
                if (SelectedTempList != null)
                {
                    grdSelectedServices.ItemsSource = SelectedTempList;
                }
            }

        }       
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void FetchService()
        {
            try
            {
                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.Specialization = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;
                BizAction.GetServicesForPathology = true;
                //ADDED BY ROHINI 
                if (txtServiceSearch.Text != string.Empty || txtServiceSearch.Text!=null)
                    BizAction.ServiceName = txtServiceSearch.Text.Trim();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        //objList.Add(new MasterListItem(0, "- Select -"));
                        //objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        clsGetServiceMasterListBizActionVO result = e.Result as clsGetServiceMasterListBizActionVO;
                        if (result.ServiceList != null)
                        {
                            foreach (var item in result.ServiceList)
                            {
                                MasterListItem objServiceItem = new MasterListItem();
                                objServiceItem.ID = item.ID;
                                objServiceItem.Description = item.ServiceName;
                                objServiceItem.Status = false;
                                objList.Add(objServiceItem);
                            }
                        }
                        grdServices.ItemsSource = null;
                        grdServices.ItemsSource = objList;

                        CheckForExistingServices();
                        //if (this.DataContext != null)
                        //{
                        //    CmbServiceName.SelectedValue = ((clsPathoParameterDefaultValueMasterVO)this.DataContext).ServiceID;
                       // }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
       
            catch (Exception ex)
            {
                throw;
            }
        }


        private void CheckForExistingServices()
        {
            if (SelectedTempList != null)
            {
                List<MasterListItem> objList = (List<MasterListItem>)grdServices.ItemsSource;

                if (objList != null && objList.Count > 0)
                {
                    if (SelectedTempList != null && SelectedTempList.Count > 0)
                    {
                        foreach (var items in objList)
                        {
                            foreach (var item in SelectedTempList)
                            {
                                if (items.ID == item.ID)
                                {
                                    items.IsDefault = true;
                                }


                            }
                        }

                    }
                    foreach (var items in objList)
                    {
                        if (items.IsDefault == true)
                        {
                            items.Status = true;
                        }
                        else
                        {
                            items.Status = false;
                        }
                    }
                    grdServices.ItemsSource = objList;
                    grdServices.UpdateLayout();
                }
            }
            else
            {
                List<MasterListItem> objList = (List<MasterListItem>)grdServices.ItemsSource;

                if (objList != null && objList.Count > 0)
                {
                    if (selectedMasterList != null && selectedMasterList.Count > 0)
                    {
                        foreach (var items in objList)
                        {
                            foreach (var item in selectedMasterList)
                            {
                                if (items.ID == item.ID)
                                {
                                    items.IsDefault = true;
                                }


                            }
                        }

                    }
                    foreach (var items in objList)
                    {
                        if (items.IsDefault == true)
                        {
                            items.Status = true;
                        }
                        else
                        {
                            items.Status = false;
                        }
                    }
                    grdServices.ItemsSource = objList;
                   
                }
            }

            //this.DataContext = objList;
        }


        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }
        //public clsAddServiceToParameterbizActionVO BizActionobj;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {               

              //  BizActionobj = new clsAddServiceToParameterbizActionVO();
              //  BizActionobj.ItemSupplier = new clsPathoParameterDefaultValueMasterVO();
              //  BizActionobj.ItemSupplierList = (List<clsPathoParameterDefaultValueMasterVO>)dgServiceList.ItemsSource;
              //  //BizActionobj.ItemSupplier.UpdatedUnitId = 1;
              //  BizActionobj.ItemSupplier.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
              //  BizActionobj.ItemSupplier.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
              //  BizActionobj.ItemSupplier.AddedDateTime = DateTime.Now;

              //  BizActionobj.ItemSupplier.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

              //  BizActionobj.ItemSupplier.ParameterDefaultID = ParameterId;
              ////  BizActionobj.ItemSupplier.ServiceID = objMasterVO.ID;
              //  BizActionobj.ItemSupplier.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
              //  string msgTitle = "";

              //  string msgText = "";

              //  if (IsEditMode == false)
              //  {
              //      msgText = "Are you sure you want to Save the Details?";

              //  }
              //  MessageBoxControl.MessageBoxChildWindow msgW =
              //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
              //  msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

              //  msgW.Show();
              //  selectedMasterList = new List<MasterListItem>();

                //selectedMasterList = obj;
                //frmPathologyParameterMaster winf = new frmPathologyParameterMaster();
                
              
                
                   obj = new List<MasterListItem>();
                   List<MasterListItem> list = new List<MasterListItem>();
                if(grdSelectedServices.ItemsSource!=null)
                {
                    list = (List<MasterListItem>)grdSelectedServices.ItemsSource;
                }
                    foreach (var item in list)
                    {
                        if (((MasterListItem)item).Status == true)
                        {
                            obj.Add(item);
                        }
	                }
                    int ind = grdSelectedServices.SelectedIndex;
                    if (OnAddButton_Click != null)
                        OnAddButton_Click(this, new RoutedEventArgs());
                    this.DialogResult = false;


            }
            catch (Exception)
            {

                throw;
            }
        }
        //void msgW_OnMessageBoxClosed(MessageBoxResult result)
        //{
        //    try
        //    {
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //            client.ProcessAsync(BizActionobj, new clsUserVO());
        //            client.ProcessCompleted += (s, arg) =>
        //            {
        //                if (arg.Error == null && arg.Result != null)
        //                {

        //                    //IF DATA IS SAVED
        //                    if (((clsAddServiceToParameterbizActionVO)(arg.Result)).SuccessStatus == 1)
        //                    {
        //                        if (IsEditMode == true)
        //                        {
        //                            string msgTitle = "";
        //                            string msgText = "Test To Machine Updated Successfully";

        //                            MessageBoxControl.MessageBoxChildWindow msgW =
        //                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


        //                            msgW.Show();
        //                            IsEditMode = false;
        //                            pkID = 0;
        //                        }
        //                        else
        //                        {
        //                            string msgTitle = "";
        //                            string msgText = "Test To Machine Added Successfully";

        //                            MessageBoxControl.MessageBoxChildWindow msgW =
        //                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


        //                            msgW.Show();

        //                        }
        //                        this.Close();



        //                    }


        //                }


        //            };


        //            client.CloseAsync();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}
        //void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        //{

        //}

        //private void GetServiceList()
        //{
        //    clsGetServicesToParameterBizActionVO objBizActionVO = new clsGetServicesToParameterBizActionVO();
        //    objBizActionVO.ItemSupplier = new clsPathoParameterDefaultValueMasterVO();
            
        //    if (objMasterVO.ID != null)
        //    {
        //        objBizActionVO.ItemSupplier.ParameterDefaultID = objMasterVO.ID;
        //    }


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {

        //            _ItemSupplier = null;
        //            _ItemSupplier = new List<MasterListItem>();
        //            _ItemSupplier = ((clsGetServicesToParameterBizActionVO)args.Result).ItemSupplierList;
        //            CheckForExistingSupplier();

        //        }

        //    };

        //    client.ProcessAsync(objBizActionVO, new clsUserVO());
        //    client.CloseAsync();

        //}
   
        //private void CheckForExistingSupplier()
        //{
        //    List<clsPathoParameterDefaultValueMasterVO> objList = (List<clsPathoParameterDefaultValueMasterVO>)dgServiceList.ItemsSource;


        //    if (objList != null && objList.Count > 0)
        //    {
        //        if (_ItemSupplier != null && _ItemSupplier.Count > 0)
        //        {
        //            foreach (var item in _ItemSupplier)
        //            {
        //                foreach (var items in objList)
        //                {
        //                    if (items.ID == item.ID)
        //                    {
        //                        items.MasterListItem.Status = item.Status;                                
        //                    }

        //                }
        //            }

        //        }
        //        dgServiceList.ItemsSource = objList;
        //    }

        //    //this.DataContext = objList;
        //}

     public List<MasterListItem> obj = new List<MasterListItem>();
        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            var chk = sender as CheckBox;
            //var datagridrow = DataGridRow.GetRowContainingElement(chk);
            if (chk.IsChecked == true)
            {
                //   obj = (List<MasterListItem>)dgServiceList.ItemsSource;
                obj = new List<MasterListItem>(); 
                obj.Add((MasterListItem)grdSelectedServices.SelectedItem);
            }
            else
            {
                obj = new List<MasterListItem>();
                obj.Remove((MasterListItem)grdSelectedServices.SelectedItem);
            }

            int ind = grdSelectedServices.SelectedIndex;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void dgItemSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkParamGrid1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdServices.SelectedItem != null)
                {
                    ((MasterListItem)grdServices.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkParamGrid1_Unecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdServices.SelectedItem != null)
                {
                    ((MasterListItem)grdServices.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkParamGrid2_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                if (grdSelectedServices.SelectedItem != null)
                {
                    ((MasterListItem)grdSelectedServices.SelectedItem).Status = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void chkParamGrid2_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdSelectedServices.SelectedItem != null)
                {
                    ((MasterListItem)grdSelectedServices.SelectedItem).Status = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<MasterListItem> lstObjectList = null;
        public List<MasterListItem> grid2ViewList = null;
        private void cmdAdd1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstObjectList = null;

                if (grdSelectedServices.ItemsSource != null)
                {
                    if (lstObjectList == null)
                    {
                        lstObjectList = new List<MasterListItem>();
                    }
                    foreach (var item in grdSelectedServices.ItemsSource)
                    {
                        #region Commented
                        //if (PathoParameterList.Contains((clsPathoTestParameterVO)item))
                        //    continue;
                        //var obj = lstObjectList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                        //if (obj != null)
                        //    continue;
                        //if (lstObjectList.Contains((MasterListItem)item))
                        //    continue;
                        //lstObjectList.Add((MasterListItem)item);
                        #endregion
                        var obj = lstObjectList.FirstOrDefault(q => q.ID == ((MasterListItem)item).ID);
                        if (obj != null)
                            continue;
                        else
                            lstObjectList.Add((MasterListItem)item);
                    }
                }
                else
                    if (lstObjectList == null)
                        lstObjectList = new List<MasterListItem>();

                if (grdServices.ItemsSource != null)
                {
                    List<MasterListItem> list = (List<MasterListItem>)grdServices.ItemsSource;
                    foreach (var item in list)
                    {
                        if (((MasterListItem)item).Status == true)
                        {
                            //clsPathoProfileTestDetailsVO obj = TestList.FirstOrDefault(q => q.TestID == ((clsPathoProfileTestDetailsVO)item).TestID);
                            //if (obj != null)
                            //    continue;
                           // item.Status = false;
                            #region Commented
                            //if (lstObjectList.Contains((MasterListItem)item))
                            //      continue;
                            //foreach (var item1 in lstObjectList)
                            //{
                            //    if (item.ID == item1.ID)
                            //        continue;
                            //    else

                            //    lstObjectList.Add((MasterListItem)item);
                            //}
                            #endregion
                            var obj = lstObjectList.FirstOrDefault(q => q.ID == item.ID);
                            if (obj != null)
                            {
                                //comented by rohini for temp
                                //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //new MessageBoxControl.MessageBoxChildWindow("", "Test Already Exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                //msgW1.Show();
                                //continue;
                            }
                            else
                                lstObjectList.Add((MasterListItem)item);
                        }
                    }
                }

                //added by rohini for cheked selected value
                foreach (var item in lstObjectList)
                {
                    item.Status = true;   
                }
                grdSelectedServices.ItemsSource = null;
                grdSelectedServices.ItemsSource = lstObjectList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lstObjectList = new List<MasterListItem>();
                //lstObjectList = (List<MasterListItem>)dgSearch2.ItemsSource;
                List<MasterListItem> SearchList = null;
                if (grdServices.ItemsSource != null)
                {
                    SearchList = (List<MasterListItem>)grdServices.ItemsSource;
                }
                if (grdSelectedServices.ItemsSource != null)
                {
                    foreach (var item in grdSelectedServices.ItemsSource)
                    {
                        if (((MasterListItem)item).IsEnable == false)
                        {
                            lstObjectList.Add((MasterListItem)item);
                        }
                        else
                        {
                            if (SearchList != null)
                            {
                                foreach (var SearchItems in SearchList.Where(x => x.ID == ((MasterListItem)item).ID))
                                {
                                    SearchItems.isChecked = false;
                                    SearchItems.Status = false;
                                }
                            }
                        }
                    }
                    grdSelectedServices.ItemsSource = null;
                    grdSelectedServices.ItemsSource = lstObjectList;

                    grdServices.ItemsSource = null;
                    grdServices.ItemsSource = SearchList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdParameterSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchService();
        }

        private void txtServiceSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchService();
            }
        }
    
        
    }
}

