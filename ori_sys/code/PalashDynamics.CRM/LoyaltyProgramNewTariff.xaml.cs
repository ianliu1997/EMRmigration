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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Billing;
using CIMS;

namespace PalashDynamics.CRM
{
    public partial class LoyaltyProgramNewTariff : ChildWindow
    {
        public LoyaltyProgramNewTariff()
        {
            InitializeComponent();
        }
        
        public event RoutedEventHandler OnAddButton_Click;
        public List<clsServiceMasterVO> SericeList { get; set; }
        public List<clsServiceMasterVO> ServiceItemSource { get; set; }
        public List<bool> check = new List<bool>();
       public long NewTariff { get; set; }
       
        private void LoyaltyProgramViewTariff_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsTariffMasterBizActionVO();
            SericeList = new List<clsServiceMasterVO>();
            FillService();
            txtTariffCode.Focus();
            cmdAll.IsChecked = true;

        }

        private void chkService_Click(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    check[dgServiceList.SelectedIndex] = true;
            //}
            //else
            //{
            //    check[dgServiceList.SelectedIndex] = false;
            //}

            
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void FillService()
        {
            try
            {

                clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgServiceList.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
                        {
                            //dgServiceList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                            BizAction.ServiceList = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                            List<clsServiceMasterVO> ObjServiceList = new List<clsServiceMasterVO>();

                            foreach (var item in BizAction.ServiceList)
                            {
                                ObjServiceList.Add(new clsServiceMasterVO()
                                {
                                    ServiceID = item.ID,
                                    ServiceCode = item.ServiceCode,
                                    ServiceName = item.ServiceName,
                                    Specialization = item.Specialization,
                                    SubSpecialization = item.SubSpecialization,
                                    Description = item.Description,
                                    ShortDescription = item.ShortDescription,
                                    LongDescription = item.LongDescription,
                                    Rate = item.Rate,
                                    SelectService = false,
                                });
                            }

                            dgServiceList.ItemsSource = ObjServiceList;

                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SaveTariff()
        {
            clsAddTariffMasterBizActionVO BizAction = new clsAddTariffMasterBizActionVO();
            BizAction.TariffDetails = (clsTariffMasterBizActionVO)this.DataContext;
            BizAction.TariffDetails.ServiceMasterList = (List<clsServiceMasterVO>)dgServiceList.ItemsSource; 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    NewTariff = ((clsAddTariffMasterBizActionVO)arg.Result).TariffDetails.ID;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "New Tariff Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                   {
                                       if (res == MessageBoxResult.OK)
                                       {
                                           this.DialogResult = true;
                                           if (OnAddButton_Click != null)
                                               OnAddButton_Click(this, new RoutedEventArgs());
                                       }
                                   };
                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
            
        }

        private void FillTariffServiceMaster(long ServiceID, List<long> lstTariffs, clsAddServiceMasterBizActionVO obj)
        {
            clsAddTariffServiceBizActionVO BizActionObj = new clsAddTariffServiceBizActionVO();
            BizActionObj.ServiceMasterDetails = obj.ServiceMasterDetails;


        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveTariff = true;

            SaveTariff = Validation();

            if (SaveTariff == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure you want to save the New  Tariff?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveTariff();
        }


        private bool Validation()
        {
            bool Result = true;

            if (txtTariffCode.Text == "")
            {
                txtTariffCode.SetValidation("Please Enter Code");
                txtTariffCode.RaiseValidationError();
                txtTariffCode.Focus();

                Result = false;
            }
            else
                txtTariffCode.ClearValidationError();

            if (txtTariffName.Text == "")
            {

                txtTariffName.SetValidation("Please Enter Tariff Name");
                txtTariffName.RaiseValidationError();
                txtTariffName.Focus();

                Result = false;
            }
            else
                txtTariffName.ClearValidationError();

            if (cmdSpecify.IsChecked == true)
            {
                if (txtSpecify.Text == "0")
                {
                    txtSpecify.SetValidation("Please Enter No. Of Visit");
                    txtSpecify.RaiseValidationError();
                    txtSpecify.RaiseValidationError();
                    txtSpecify.Focus();
                    txtSpecify.Focus();
                    Result = false;
                }
                else
                    txtSpecify.ClearValidationError();
            }

            clsServiceMasterVO service = ((List<clsServiceMasterVO>)dgServiceList.ItemsSource).FirstOrDefault(p => p.SelectService == true);

            if (service != null)
            {
                return true;
            }
            else
            {
                string msgTitle = "Palash";
                string msgText = "Please Select service";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW.Show();

                return false;
            }

            //if (dgServiceList.SelectedItem == null)
            //{
            //    string msgTitle = "";
            //    string msgText = "Please Select Service";

            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //    msgW.Show();
            //    Result = false;
            //}

            return Result;
        }
               

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSpecify_Click(object sender, RoutedEventArgs e)
        {
            if (cmdSpecify.IsChecked == true)
            {
                txtSpecify.IsReadOnly = false;
                chkDate.IsEnabled = true;
                dtpEffectiveDate.IsEnabled = true;
                dtpExpiryDate.IsEnabled = true;
            }
        }

        private void cmdSpecify_Unchecked(object sender, RoutedEventArgs e)
        {
            if (cmdSpecify.IsChecked == false)
            {
                txtSpecify.IsReadOnly = true;
                chkDate.IsEnabled = false;
                dtpEffectiveDate.IsEnabled = false;
                dtpExpiryDate.IsEnabled = false;
            }
        }
        List<clsServiceMasterVO> lst = new List<clsServiceMasterVO>();
        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == true)
                {
                    lst = (List<clsServiceMasterVO>)dgServiceList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectService = true;
                            
                        }
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = lst;
                       

                        
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == false)
                {

                    lst = (List<clsServiceMasterVO>)dgServiceList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectService = false;
                        }
                                       
                        dgServiceList.ItemsSource = null;
                        dgServiceList.ItemsSource = lst;

                       

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Commented code
        //private void FillService()
        //{
        //    try
        //    {

        //    clsGetServiceMasterListBizActionVO BizAction = new clsGetServiceMasterListBizActionVO();
        //    BizAction.ServiceList = new List<clsServiceMasterVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    dgServiceList.ItemsSource = null;
        //    client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null)
        //            {
        //                if (((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
        //                {
        //                    dgServiceList.ItemsSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
        //                    ServiceItemSource = ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
        //                    for (int i = 0; i < ((clsGetServiceMasterListBizActionVO)arg.Result).ServiceList.Count; i++)
        //                    {
        //                        bool b = false;
        //                        check.Add(b);
        //                    }
        //                }   
        //            }

        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                       new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

        //                msgW1.Show();
        //            }

        //        };
        //        client.ProcessAsync(BizAction, new clsUserVO());
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //Save()
        //{
        //    for (int i = 0; i < check.Count; i++)
        //    {
        //        if (check[i] == true)
        //        {
        //            SericeList.Add(new clsServiceMasterVO
        //            {
        //                ServiceID=ServiceItemSource[i].ID,
        //                ServiceCode=ServiceItemSource[i].ServiceCode,
        //                Specialization=ServiceItemSource[i].Specialization,
        //                SubSpecialization=ServiceItemSource[i].SubSpecialization,
        //                Description=ServiceItemSource[i].Description,
        //                ServiceName=ServiceItemSource[i].ServiceName,
        //                ShortDescription=ServiceItemSource[i].ShortDescription,
        //                LongDescription=ServiceItemSource[i].LongDescription,
        //                Rate=ServiceItemSource[i].Rate,
        //                Status=ServiceItemSource[i].Status,
        //                SelectService = ServiceItemSource[i].Status,
        //            });
        //        }
        //    }
        //}
        #endregion
    }
}

