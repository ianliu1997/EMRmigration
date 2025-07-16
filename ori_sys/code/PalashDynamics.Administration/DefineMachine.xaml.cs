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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.UserControls;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.Pharmacy;


namespace PalashDynamics.Administration   
{
    public partial class DefineMachine : ChildWindow           // by rohini dated 19.1.16 as per disscion with nilesh sir
    {
        Boolean IsPageLoded = false;
        clsPathoTestMasterVO objMasterVO = null;
        clsPathoTestMasterVO objMasterVOSub = null;
        Boolean IsRowSelected = false;
        public bool IsFromSubTest = false;
        public List<clsPathoTestMasterVO> ItemSupplierList;
        public List<clsPathoTestMasterVO> _ItemSupplier;
        public List<clsPathoTestMasterVO> ItemSupplierListSub;
        public List<clsPathoTestMasterVO> _ItemSupplierSub;
        public DefineMachine()
        {
            InitializeComponent();
            this.DataContext = new clsPathoTestMasterVO();
        }

        public clsAddMachineToTestbizActionVO BizActionobj;
        public Boolean IsEditMode { get; set; }
        public long pkID { get; set; }

        public clsAddMachineToSubTestbizActionVO BizActionobjSub;
        public Boolean IsEditModeSub { get; set; }
        public long pkIDSub { get; set; }
        //for test master
        public void GetItemDetails(clsPathoTestMasterVO objItemMasterVO)
        {
            objMasterVO = objItemMasterVO;
        }
        //for sub test
        public void GetItemDetailsSub(clsPathoTestMasterVO objItemMasterVOSub)
        {
            objMasterVOSub = objItemMasterVOSub;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (IsFromSubTest == true)
            {
                try
                {
                    BizActionobjSub = new clsAddMachineToSubTestbizActionVO();
                    BizActionobjSub.ItemSupplier = new clsPathoTestMasterVO();
                    BizActionobjSub.ItemSupplierList = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;
                    BizActionobjSub.ItemSupplier.CreatedUnitID = 1;
                    BizActionobjSub.ItemSupplier.UpdatedUnitID = 1;
                    BizActionobjSub.ItemSupplier.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                    BizActionobjSub.ItemSupplier.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    BizActionobjSub.ItemSupplier.AddedDateTime = DateTime.Now;

                    BizActionobjSub.ItemSupplier.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    BizActionobjSub.ItemSupplier.SubTestID = objMasterVOSub.ID;
                    BizActionobjSub.ItemSupplier.MachineID = objMasterVOSub.MachineID;
                    BizActionobjSub.ItemSupplier.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                    string msgTitle = "";

                    string msgText = "";

                    if (IsEditMode == false)
                    {
                        msgText = "Are you sure you want to Save the Details?";

                    }
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);

                    msgW.Show();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {

                try
                {
                    BizActionobj = new clsAddMachineToTestbizActionVO();
                    BizActionobj.ItemSupplier = new clsPathoTestMasterVO();
                    BizActionobj.ItemSupplierList = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;
                    BizActionobj.ItemSupplier.CreatedUnitID = 1;
                    BizActionobj.ItemSupplier.UpdatedUnitID = 1;
                    BizActionobj.ItemSupplier.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
                    BizActionobj.ItemSupplier.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                    BizActionobj.ItemSupplier.AddedDateTime = DateTime.Now;

                    BizActionobj.ItemSupplier.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;

                    BizActionobj.ItemSupplier.TestID = objMasterVO.ID;
                    BizActionobj.ItemSupplier.MachineID = objMasterVO.MachineID;
                    BizActionobj.ItemSupplier.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                    string msgTitle = "";

                    string msgText = "";

                    if (IsEditMode == false)
                    {
                        msgText = "Are you sure you want to Save the  Details?";

                    }
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                catch (Exception)
                {

                    throw;
                }
            }
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
                            if (((clsAddMachineToTestbizActionVO)(arg.Result)).SuccessStatus == 1)
                            {
                                if (IsEditMode == true)
                                {
                                    string msgTitle = "";
                                    string msgText = "Test To Machine Updated Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();
                                    IsEditMode = false;
                                    pkID = 0;
                                }
                                else
                                {
                                    string msgTitle = "";
                                    string msgText = "Test To Machine Added Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);


                                    msgW.Show();

                                }
                                this.Close();



                            }


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
        void msgW_OnMessageBoxClosed1(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessAsync(BizActionobjSub, new clsUserVO());
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)                        {

                            //IF DATA IS SAVED
                            if (((clsAddMachineToSubTestbizActionVO)(arg.Result)).SuccessStatus == 1)
                            {
                                if (IsEditMode == true)
                                {
                                    string msgTitle = "";
                                    string msgText = "Sub Test To Machine Updated Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed1);


                                    msgW.Show();
                                    IsEditMode = false;
                                    pkID = 0;
                                }
                                else
                                {
                                    string msgTitle = "";
                                    string msgText = "Sub Test To Machine Added Successfully";

                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed1);


                                    msgW.Show();

                                }
                                this.Close();
                            }
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
        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {

        }
        void msgW1_OnMessageBoxClosed1(MessageBoxResult result)
        {

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //objMasterVO = new clsItemMasterVO();
            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (IsFromSubTest == false)
                {
                    if (objMasterVO.PrintTestName != null)
                    {
                        lblItemName.Text = objMasterVO.Description.ToString();
                        lbl.Text = "Test Name";
                    }

                }
                else
                {
                    if (objMasterVOSub.PrintTestName != null)
                    {
                        lblItemName.Text = objMasterVOSub.Description.ToString();
                        lbl.Text = "Sub Test Name";
                    }
                }
                FillDataGrid();
            }
            IsPageLoded = true;
        }
        //by rohini dated 19.1 16
        private void FillDataGrid()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_MachineMaster;
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
                        ItemSupplierList = new List<clsPathoTestMasterVO>();
                        clsPathoTestMasterVO obj = new clsPathoTestMasterVO();

                        foreach (var item in objList)
                        {

                            ItemSupplierList.Add(new clsPathoTestMasterVO { ID = item.ID, SupplierName = item.Description, HPLevelList = obj.HPLevelList, status = item.Status });
                        }


                        dgItemSupplier.ItemsSource = null;
                        dgItemSupplier.ItemsSource = ItemSupplierList;
                        GetItemSupplierlist();
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

        private void GetItemSupplierlist()
        {
            if (IsFromSubTest == false)
            {
                clsGetMachineToTestBizActionVO objBizActionVO = new clsGetMachineToTestBizActionVO();
                objBizActionVO.ItemSupplier = new clsPathoTestMasterVO();

                objBizActionVO.ItemSupplier.TestID = objMasterVO.ID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        _ItemSupplier = null;
                        _ItemSupplier = new List<clsPathoTestMasterVO>();
                        _ItemSupplier = ((clsGetMachineToTestBizActionVO)args.Result).ItemSupplierList;
                        CheckForExistingSupplier();

                    }

                };

                client.ProcessAsync(objBizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            else
            {
                clsGetMachineToSubTestBizActionVO objBizActionVO = new clsGetMachineToSubTestBizActionVO();
                objBizActionVO.ItemSupplier = new clsPathoTestMasterVO();

                objBizActionVO.ItemSupplier.SubTestID = objMasterVOSub.ID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {

                        _ItemSupplierSub = null;
                        _ItemSupplierSub = new List<clsPathoTestMasterVO>();
                        _ItemSupplierSub = ((clsGetMachineToSubTestBizActionVO)args.Result).ItemSupplierList;
                        CheckForExistingSupplierSub();
                    }

                };

                client.ProcessAsync(objBizActionVO, new clsUserVO());
                client.CloseAsync();
            }

        }

        private void CheckForExistingSupplier()
        {
            List<clsPathoTestMasterVO> objList = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;


            if (objList != null && objList.Count > 0)
            {
                if (_ItemSupplier != null && _ItemSupplier.Count > 0)
                {
                    foreach (var item in _ItemSupplier)
                    {
                        foreach (var items in objList)
                        {
                            if (items.ID == item.MachineID)
                            {
                                items.status = item.status;
                               // items.SelectedHPLevel = item.SelectedHPLevel;
                            }

                        }
                    }

                }
                dgItemSupplier.ItemsSource = objList;
            }

            //this.DataContext = objList;
        }
        private void CheckForExistingSupplierSub()
        {
            List<clsPathoTestMasterVO> objList = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;

            if (objList != null && objList.Count > 0)
            {
                if (_ItemSupplierSub != null && _ItemSupplierSub.Count > 0)
                {
                    foreach (var item in _ItemSupplierSub)
                    {
                        foreach (var items in objList)
                        {
                            if (items.ID == item.MachineID)
                            {
                                items.status = item.status;
                                // items.SelectedHPLevel = item.SelectedHPLevel;
                            }
                        }
                    }

                }
                dgItemSupplier.ItemsSource = objList;
            }

            //this.DataContext = objList;
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromSubTest == false)
            {
                var chk = sender as CheckBox;
                //var datagridrow = DataGridRow.GetRowContainingElement(chk);
                List<clsPathoTestMasterVO> obj = new List<clsPathoTestMasterVO>();
                obj = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;
                int ind = dgItemSupplier.SelectedIndex;
            }
            else
            {
                var chk = sender as CheckBox;
                List<clsPathoTestMasterVO> obj = new List<clsPathoTestMasterVO>();
                obj = (List<clsPathoTestMasterVO>)dgItemSupplier.ItemsSource;
                int ind = dgItemSupplier.SelectedIndex;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void dgItemSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}

