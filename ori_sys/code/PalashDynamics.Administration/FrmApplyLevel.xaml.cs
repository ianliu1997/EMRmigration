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
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.Administration
{
    public partial class FrmApplyLevel : ChildWindow
    {
        public FrmApplyLevel()
        {
            InitializeComponent();
            this.DataContext = null;

            FillL1();
            FillL2();
            FillL3();
            FillL4();

        }
       
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbL1.SelectedItem != null && cmbL2.SelectedItem != null && cmbL3.SelectedItem != null && cmbL4.SelectedItem != null)
            {
                if (((MasterListItem)cmbL1.SelectedItem).ID == 0 && ((MasterListItem)cmbL2.SelectedItem).ID == 0 && ((MasterListItem)cmbL3.SelectedItem).ID == 0 && ((MasterListItem)cmbL4.SelectedItem).ID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Fill Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                  
                    msgW.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to save OPU details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
            }
        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
        public long ServiceID, ServiceUnitID;
        private void Save()
        {
            clsAddUpdateApplyLevelsToServiceBizActionVO BizAction = new clsAddUpdateApplyLevelsToServiceBizActionVO();
            BizAction.Obj = new clsServiceLevelsVO();
            if (this.DataContext !=null)
            BizAction.Obj.ID = ((clsServiceLevelsVO)this.DataContext).ID;
            BizAction.Obj.ServiceID = ServiceID;
            BizAction.Obj.ServiceUnitID = ServiceUnitID;
          
            if (cmbL1.SelectedItem != null)
                BizAction.Obj.L1ID = ((MasterListItem)cmbL1.SelectedItem).ID;
            if (cmbL2.SelectedItem != null)
                BizAction.Obj.L2ID = ((MasterListItem)cmbL2.SelectedItem).ID;
            if (cmbL3.SelectedItem != null)
                BizAction.Obj.L3ID = ((MasterListItem)cmbL3.SelectedItem).ID;
            if (cmbL4.SelectedItem != null)
                BizAction.Obj.L4ID = ((MasterListItem)cmbL4.SelectedItem).ID;
          

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.OK)
                        {
                            this.DialogResult = true;
                        }
                    };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        long OocyteDonorID = 0;
        long OocyteDonorUnitID = 0;
        private void fillDetails()
        {
            clsGetApplyLevelsToServiceBizActionVO BizAction = new clsGetApplyLevelsToServiceBizActionVO();
            BizAction.Obj = new clsServiceLevelsVO();
            BizAction.Obj.ServiceID = ServiceID;
            BizAction.Obj.ServiceUnitID = ServiceUnitID;
           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj != null)
                    {
                        this.DataContext = ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj;


                        if (((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L1ID != null && ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L1ID != 0)
                        {
                            cmbL1.SelectedValue = ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L1ID;
                        }
                        if (((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L2ID != null && ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L2ID != 0)
                        {
                            cmbL2.SelectedValue = ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L2ID;
                        }
                        if (((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L3ID != null && ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L3ID != 0)
                        {
                            cmbL3.SelectedValue = ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L3ID;
                        }
                        if (((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L4ID != null && ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L4ID != 0)
                        {
                            cmbL4.SelectedValue = ((clsGetApplyLevelsToServiceBizActionVO)arg.Result).Obj.L4ID;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
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
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillDetails();
        }

        

        private void FillL1() 
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ServiceL1;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbL1.ItemsSource = null;
                        cmbL1.ItemsSource = objList.DeepCopy();
                        cmbL1.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbL1.SelectedValue = ((clsServiceLevelsVO)this.DataContext).L1ID;
                      
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
        private void FillL2()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ServiceL2;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbL2.ItemsSource = null;
                        cmbL2.ItemsSource = objList.DeepCopy();
                        cmbL2.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbL2.SelectedValue = ((clsServiceLevelsVO)this.DataContext).L2ID;

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
        private void FillL3()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ServiceL3;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbL3.ItemsSource = null;
                        cmbL3.ItemsSource = objList.DeepCopy();
                        cmbL3.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbL3.SelectedValue = ((clsServiceLevelsVO)this.DataContext).L3ID;

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
        private void FillL4()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ServiceL4;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbL4.ItemsSource = null;
                        cmbL4.ItemsSource = objList.DeepCopy();
                        cmbL4.SelectedItem = objList[0];
                    }
                    if (this.DataContext != null)
                    {
                        cmbL4.SelectedValue = ((clsServiceLevelsVO)this.DataContext).L4ID;

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
    }
}
