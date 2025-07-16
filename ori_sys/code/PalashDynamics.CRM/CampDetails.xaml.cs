
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Windows.Controls.Data;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using System.Collections.Generic;
using PalashDynamics.Service.PalashTestServiceReference;
using System;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.CRM;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using System.Text;


namespace PalashDynamics.CRM
{
    public partial class CampDetails : UserControl
    {
        int ClickedFlag1 = 0;
        public PagedSortableCollectionView<clsCampMasterVO> DataList { get; private set; }

        public List<CampserviceDetailsVO> CampServiceDetail { get; set; }
        public ObservableCollection<CampserviceDetailsVO> FreeCampServiceList { get; set; }
        public ObservableCollection<CampserviceDetailsVO> ConcessionCampServiceList { get; set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        public CampDetails()
        {
            InitializeComponent();
            dgConServiceList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgConServiceList_CellEditEnded);

            DataList = new PagedSortableCollectionView<clsCampMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;


            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

        }

        void dgConServiceList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 3)
            {
                if (((CampserviceDetailsVO)dgConServiceList.SelectedItem).ConcessionPercentage > 100)
                {

                    ((CampserviceDetailsVO)dgConServiceList.SelectedItem).ConcessionPercentage = 100;
                    string msgTitle = "";
                    string msgText = "Percentage should not be greater than 100";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
            else if (e.Column.DisplayIndex == 4)
            {
                double ServiceRate = ((CampserviceDetailsVO)dgConServiceList.SelectedItem).Rate;

                if (((CampserviceDetailsVO)dgConServiceList.SelectedItem).ConcessionAmount > ServiceRate)
                {

                    ((CampserviceDetailsVO)dgConServiceList.SelectedItem).ConcessionAmount = ServiceRate;
                    string msgTitle = "";
                    string msgText = "ConcessionAmount should not be greater than Service Rate";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsUpdate = true;
        bool IsPageLoded = false;

        #endregion

        private void CampDetails_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                //Indicatior = new WaitIndicator();
                //Indicatior.Show();

                this.DataContext = new clsCampMasterVO()
                {
                    City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City
                };
                CampServiceDetail = new List<CampserviceDetailsVO>();
                FreeCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
                ConcessionCampServiceList = new ObservableCollection<CampserviceDetailsVO>();

                CheckValidation();
                SetCommandButtonState("New");
                FetchData();
                FillCamp();
                FillTariff();
                FillCityList();
                FillCampName();
                FillEmail();
                FillSMS();


                SetComboboxValue();
                cmbCampName.Focus();

                dtpSelectFromDate.Visibility = Visibility.Collapsed;
                dtpSelectToDate.Visibility = Visibility.Collapsed;
                //hblEmail.IsEnabled = false;
                //hblSms.IsEnabled = false;
                //Indicatior.Close();
            }
            cmbCampName.Focus();
            cmbCampName.UpdateLayout();
            IsPageLoded = true;

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");

            ClearControl();
            dtpFromDate.ClearValidationError();
            dtpToDate.ClearValidationError();
            txtArea.ClearValidationError();
            txtCity.ClearValidationError();



            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            TabControlMain.SelectedIndex = 0;
            objAnimation.Invoke(RotationType.Backward);
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            this.DataContext = new clsCampMasterVO();
            FreeCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
            ConcessionCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
            ClearControl();

            IsUpdate = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Camp Details";

            TabControlMain.SelectedIndex = 0;
            objAnimation.Invoke(RotationType.Forward);

            cmbCamp.IsEnabled = true;
            btnService.IsEnabled = true;
            cmbCamp.Focus();
            cmbCamp.UpdateLayout();
        }

        #region FillCombobox
        private void FillCamp()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CampMaster;
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

                    cmbCamp.ItemsSource = null;
                    cmbCamp.ItemsSource = objList;

                }
                if (this.DataContext != null)
                {
                    cmbCamp.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;


                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillTariff()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
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

                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbTariff.SelectedValue = ((clsCampMasterVO)this.DataContext).TariffID;


                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillCampName()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CampMaster;
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

                    cmbCampName.ItemsSource = null;
                    cmbCampName.ItemsSource = objList;


                }
                if (this.DataContext != null)
                {
                    cmbCampName.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillCityList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

            BizAction.TableName = "T_CampDetails";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = false;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

            BizAction.TableName = "T_CampDetails";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillEmail()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //   BizAction.MasterTable = MasterTableNameList.
            BizAction.MasterTable = MasterTableNameList.T_EmailTemplate;

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

                    cmbEmail.ItemsSource = null;
                    cmbEmail.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillSMS()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //   BizAction.MasterTable = MasterTableNameList.None;
            BizAction.MasterTable = MasterTableNameList.T_SMSTemplate;
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

                    cmbSms.ItemsSource = null;
                    cmbSms.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSms.SelectedValue = ((clsCampMasterVO)this.DataContext).SmsTemplateID;
                }
                //if ((clsCampMasterVO)dgCampDetailsList.SelectedItem != null)
                //{
                //    clsCampMasterVO objCampMaster = new clsCampMasterVO();
                //    cmbSms.SelectedValue = objCampMaster.SmsTemplateID;
                //}
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        #endregion

        private void SetComboboxValue()
        {

            cmbCampName.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;
            cmbTariff.SelectedValue = ((clsCampMasterVO)this.DataContext).TariffID;
            cmbCamp.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;
            cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
            cmbSms.SelectedValue = ((clsCampMasterVO)this.DataContext).SmsTemplateID;

        }

        private void SetPosition_Click(object sender, RoutedEventArgs e)
        {
            //customHost.InvokeScript("showVehicle", new object[] { txtLat.Text, txtLon.Text });

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://maps.google.com/", UriKind.RelativeOrAbsolute), "_blank");
            //customHost.NavigationUrl = "http://maps.google.com/";
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbTariff.SelectedItem).ID == 0)
            {
                cmbTariff.TextBox.SetValidation("Please Select Tariff");
                cmbTariff.TextBox.RaiseValidationError();
                cmbTariff.Focus();
                TabControlMain.SelectedIndex = 0;
            }
            else
            {
                cmbTariff.ClearValidationError();
                ServiceDetails objservice = new ServiceDetails();
                objservice.OnAddButton_Click += new RoutedEventHandler(objservice_OnAddButton_Click);
                objservice.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                objservice.Show();

            }

        }

        /// <summary>
        /// Purpose:Add camp service.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void objservice_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
           
            CampserviceDetailsVO ObjTemp = null;

            long currentServiceID = 0;

            if (((ServiceDetails)sender).DialogResult == true)
            {
                for (int i = 0; i < ((ServiceDetails)sender).check.Count; i++)
                {

                    if (((ServiceDetails)sender).check[i] == true)
                    {
                        currentServiceID = (((ServiceDetails)sender).ServiceItemSource[i]).ID;

                        if (((ServiceDetails)sender).CmdAddFreeService.IsChecked == true)
                        {
                            if (ConcessionCampServiceList.Where(ServiceItems => ServiceItems.ServiceID == currentServiceID).Any() == true)
                            {
                                string msgTitle = "";
                                string msgText = "Service is already exists in Concession Service List";

                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgWD.Show();
                                return;
                            }
                            if (FreeCampServiceList.Where(ServiceItems => ServiceItems.ServiceID == currentServiceID).Any() == false)
                            {
                                ObjTemp = new CampserviceDetailsVO();
                                ObjTemp.ServiceID = (((ServiceDetails)sender).ServiceItemSource[i]).ID;
                                ObjTemp.ServiceName = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceName;
                                ObjTemp.ServiceCode = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceCode;
                                ObjTemp.IsFree = (bool)((ServiceDetails)sender).CmdAddFreeService.IsChecked;
                                ObjTemp.IsConcession = (bool)((ServiceDetails)sender).CmdconcessionService.IsChecked;
                                ObjTemp.Rate = (double)((ServiceDetails)sender).ServiceItemSource[i].Rate;
                                ObjTemp.ConcessionAmount = (double)((ServiceDetails)sender).ServiceItemSource[i].Rate;
                                FreeCampServiceList.Add(ObjTemp);
                            }
                            else
                            {
                                string msgTitle = "";
                                string msgText = "Service is already exists in Free Service List";

                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgWD.Show();
                            }
                        }
                        else
                        {
                            if (FreeCampServiceList.Where(ServiceItems => ServiceItems.ServiceID == currentServiceID).Any() == true)
                            {
                                string msgTitle = "";
                                string msgText = "Service is already exists in Free Service List";

                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgWD.Show();
                                return;
                            }
                            if (ConcessionCampServiceList.Where(ServiceItems => ServiceItems.ServiceID == currentServiceID).Any() == false)
                            {
                                ObjTemp = new CampserviceDetailsVO();
                                ObjTemp.ServiceID = (((ServiceDetails)sender).ServiceItemSource[i]).ID;
                                ObjTemp.ServiceName = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceName;
                                ObjTemp.ServiceCode = (((ServiceDetails)sender).ServiceItemSource[i]).ServiceCode;
                                ObjTemp.IsFree = (bool)((ServiceDetails)sender).CmdAddFreeService.IsChecked;
                                ObjTemp.IsConcession = (bool)((ServiceDetails)sender).CmdconcessionService.IsChecked;
                                ObjTemp.Rate = (double)((ServiceDetails)sender).ServiceItemSource[i].Rate;
                                ObjTemp.ConcessionAmount = (double)((ServiceDetails)sender).ServiceItemSource[i].Rate;
                                ConcessionCampServiceList.Add(ObjTemp);
                            }
                            else
                            {
                                string msgTitle = "";
                                string msgText = "Service is already exists in Concession Service List";

                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgWD.Show();
                            }

                        }
                    }

                }


                if (((ServiceDetails)sender).CmdAddFreeService.IsChecked == true)
                {

                    dgFreeServiceList.ItemsSource = FreeCampServiceList;
                    dgFreeServiceList.Focus();
                    dgFreeServiceList.UpdateLayout();
                    dgFreeServiceList.SelectedIndex = FreeCampServiceList.Count - 1;

                    TabMain.SelectedIndex = 0;
                }
                else
                {

                    dgConServiceList.ItemsSource = ConcessionCampServiceList;
                    dgConServiceList.Focus();
                    dgConServiceList.UpdateLayout();
                    dgConServiceList.SelectedIndex = ConcessionCampServiceList.Count - 1;

                    TabMain.SelectedIndex = 1;

                }
            }



        }

        private void hlNewTariff_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow chform = new ChildWindow();
            NewTariff win = new NewTariff();

            chform.Content = win;
            chform.Show();

        }

        private void CmdSendEmail_Click(object sender, RoutedEventArgs e)
        {
            EmailTemplate win = new EmailTemplate();
            win.Show();
        }

        private void CmdSendSms_Click(object sender, RoutedEventArgs e)
        {
            SmsTemplate win = new SmsTemplate();
            win.Show();
        }
        /// <summary>
        /// Purpose:View email template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void hblEmail_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbEmail.SelectedItem).ID != 0)
            {
                cmbEmail.ClearValidationError();
                ChildWindow chform = new ChildWindow();
                chform.Title = "Email Template";
                ViewEmailTemplateDetails win = new ViewEmailTemplateDetails();

                win.TemplateId = ((MasterListItem)cmbEmail.SelectedItem).ID;
                chform.Content = win;
                chform.Show();
            }
            else
            {
                if ((MasterListItem)cmbEmail.SelectedItem == null)
                {
                    cmbEmail.TextBox.SetValidation("Please select Email template");
                    cmbEmail.TextBox.RaiseValidationError();
                }
                else if (((MasterListItem)cmbEmail.SelectedItem).ID == 0)
                {
                    cmbEmail.TextBox.SetValidation("Please select Email template");
                    cmbEmail.TextBox.RaiseValidationError();
                }
                else
                    cmbEmail.ClearValidationError();
            }
        }

        /// <summary>
        /// Purpose:View sms template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hblSms_Click(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbSms.SelectedItem).ID != 0)
            {
                cmbSms.ClearValidationError();
                ChildWindow chform = new ChildWindow();
                chform.Title = "SMS Template";
                SmsTemplate win = new SmsTemplate();

                win.TemplateId = ((MasterListItem)cmbSms.SelectedItem).ID;
                chform.Content = win;
                chform.Show();
            }
            else
            {
                if ((MasterListItem)cmbSms.SelectedItem == null)
                {
                    cmbSms.TextBox.SetValidation("Please select SMS Template");
                    cmbSms.TextBox.RaiseValidationError();
                }
                else if (((MasterListItem)cmbSms.SelectedItem).ID == 0)
                {
                    cmbSms.TextBox.SetValidation("Please select SMS Template");
                    cmbSms.TextBox.RaiseValidationError();
                }
                else
                    cmbSms.ClearValidationError();
            }
        }

        /// <summary>
        /// Purpose:Getting list of Camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetCampDetailsListBizActionVO BizAction = new clsGetCampDetailsListBizActionVO();
            BizAction.CampDetailsList = new List<clsCampMasterVO>();

            if (cmbCampName.SelectedItem != null)
                BizAction.Camp = ((MasterListItem)cmbCampName.SelectedItem).ID;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgCampDetailsList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetCampDetailsListBizActionVO)arg.Result).CampDetailsList != null)
                    {
                        clsGetCampDetailsListBizActionVO result = arg.Result as clsGetCampDetailsListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        if (result.CampDetailsList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.CampDetailsList)
                            {
                                DataList.Add(item);
                            }

                            dgCampDetailsList.ItemsSource = null;
                            dgCampDetailsList.ItemsSource = DataList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;

                        }

                        //dgCampDetailsList.ItemsSource = ((clsGetCampDetailsListBizActionVO)arg.Result).CampDetailsList;

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        /// <summary>
        /// Purpose:Add new camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 += 1;
            if (ClickedFlag1 == 1)
            {
                bool SaveCampDetails = true;
                SaveCampDetails = CheckValidation();
                if (SaveCampDetails == true)
                {

                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Camp Details?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();


                }
                else
                    ClickedFlag1 = 0;

            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {


            if (result == MessageBoxResult.Yes)
            {
                if (ClickedFlag1 == 1)
                {
                    SaveCampDetails();
                }
            }
            else
                ClickedFlag1 = 0;

        }

        private void SaveCampDetails()
        {

            clsAddCampDetailsBizActionVO BizAction = new clsAddCampDetailsBizActionVO();
            BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;

            if (cmbCamp.SelectedItem != null)
                BizAction.CampMasterDetails.CampID = ((MasterListItem)cmbCamp.SelectedItem).ID;

            if (cmbTariff.SelectedItem != null)
                BizAction.CampMasterDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            if (cmbEmail.SelectedItem != null)
                BizAction.CampMasterDetails.EmailTemplateID = ((MasterListItem)cmbEmail.SelectedItem).ID;

            if (cmbSms.SelectedItem != null)
                BizAction.CampMasterDetails.SmsTemplateID = ((MasterListItem)cmbSms.SelectedItem).ID;

            //BizAction.CampMasterDetails.CampServiceDetails = CampServiceDetail; //Add CampSevice in T_CampService table.
            BizAction.CampMasterDetails.FreeCampServiceList = FreeCampServiceList.ToList();
            BizAction.CampMasterDetails.ConcessionServiceList = ConcessionCampServiceList.ToList();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Camp Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                    msgW1.Show();

                    SetCommandButtonState("New");
                    objAnimation.Invoke(RotationType.Backward);

                    ClearControl();
                    FetchData();
                    ClickedFlag1 = 0;

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Camp Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                    ClickedFlag1 = 0;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtValidDays.Text = string.Empty;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (((clsCampMasterVO)this.DataContext).ToDate >= dtpFromDate.SelectedDate)
                {
                    System.DateTime dtFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                    System.DateTime dtToDate = Convert.ToDateTime(dtpToDate.SelectedDate).AddDays(1);
                    System.TimeSpan diffResult = dtToDate.Subtract(dtFromDate);
                    txtValidDays.Text = Convert.ToString(diffResult.TotalDays);
                }
            }



        }


        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtValidDays.Text = string.Empty;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (((clsCampMasterVO)this.DataContext).FromDate < dtpToDate.SelectedDate)
                {
                    System.DateTime dtFromDate = Convert.ToDateTime(dtpFromDate.SelectedDate);
                    System.DateTime dtToDate = Convert.ToDateTime(dtpToDate.SelectedDate).AddDays(1);
                    System.TimeSpan diffResult = dtToDate.Subtract(dtFromDate);
                    txtValidDays.Text = Convert.ToString(diffResult.TotalDays);
                }
            }

        }

        /// <summary>
        /// Purpose:View selected camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void hlbViewCampDetails_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Modify");
            ClearControl();

            clsCampMasterVO objCampMaster = new clsCampMasterVO();
            if ((clsCampMasterVO)dgCampDetailsList.SelectedItem != null) ;
            {
                objCampMaster = ((clsCampMasterVO)dgCampDetailsList.SelectedItem);
                FillCampDetails(objCampMaster.CampDetailID);
            }

            if (dgCampDetailsList.SelectedItem != null)
            {

                clsGetCampFreeAndConServiceListBizActionVO BizActionObj = new clsGetCampFreeAndConServiceListBizActionVO();
                BizActionObj.CampFreeServiceList = new List<CampserviceDetailsVO>();
                BizActionObj.CampConServiceList = new List<CampserviceDetailsVO>();
                BizActionObj.CampID = objCampMaster.CampDetailID;



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                dgFreeServiceList.ItemsSource = null;
                dgConServiceList.ItemsSource = null;
                FreeCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
                ConcessionCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampFreeServiceList != null && ((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampFreeServiceList.Count != 0)
                        {
                            List<CampserviceDetailsVO> FreeList;
                            FreeList = ((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampFreeServiceList;
                            foreach (var ObjList in FreeList)
                            {
                                FreeCampServiceList.Add(ObjList);
                            }
                            dgFreeServiceList.ItemsSource = null;
                            dgFreeServiceList.ItemsSource = FreeCampServiceList;
                            dgFreeServiceList.Focus();
                            dgFreeServiceList.UpdateLayout();
                            TabMain.SelectedIndex = 0;

                        }

                        if (((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampConServiceList != null && ((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampConServiceList.Count != 0)
                        {
                            List<CampserviceDetailsVO> ConList;
                            ConList = ((clsGetCampFreeAndConServiceListBizActionVO)arg.Result).CampConServiceList;
                            foreach (var ObjConList in ConList)
                            {
                                ConcessionCampServiceList.Add(ObjConList);
                            }
                            dgConServiceList.ItemsSource = null;
                            dgConServiceList.ItemsSource = ConcessionCampServiceList;
                            dgConServiceList.Focus();
                            dgConServiceList.UpdateLayout();
                            TabMain.SelectedIndex = 1;

                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }


            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + objCampMaster.Description;
            //cmbCamp.IsEnabled = false;
            //btnService.IsEnabled = false;

            objAnimation.Invoke(RotationType.Forward);

            cmbEmail.SelectedItem = objCampMaster.EmailTemplateDetails.Description;
            cmbEmail.SelectedValue = objCampMaster.EmailTemplateDetails.Description;
            cmbSms.SelectedItem = objCampMaster.SMSTemplateDetails.Description;
            cmbSms.SelectedValue = objCampMaster.SMSTemplateDetails.Description;
            TabControlMain.SelectedIndex = 0;
            IsUpdate = false;
        }

        private void FillCampDetails(long iID)
        {
            clsGetCampDetailsByIDBizActionVO BizAction = new clsGetCampDetailsByIDBizActionVO();
            BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;
            BizAction.CampMasterDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.ID = iID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if (dgCampDetailsList.SelectedItem != null)
                        objAnimation.Invoke(RotationType.Forward);

                    ((clsCampMasterVO)this.DataContext).CampDetailID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.CampDetailID;
                    ((clsCampMasterVO)this.DataContext).FromDate = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.FromDate;
                    ((clsCampMasterVO)this.DataContext).ToDate = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.ToDate;
                    ((clsCampMasterVO)this.DataContext).PatientRegistrationValidTillDate = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.PatientRegistrationValidTillDate;
                    ((clsCampMasterVO)this.DataContext).City = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.City;

                    txtCity.Text = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.City;
                    txtArea.Text = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Area;
                    ((clsCampMasterVO)this.DataContext).Area = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Area;
                    ((clsCampMasterVO)this.DataContext).ValidDays = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.ValidDays;

                    //if (((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Concession != null)
                    //{
                    //    txtConcession.Text = Convert.ToString(((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Concession);
                    //}
                    ((clsCampMasterVO)this.DataContext).Reason = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Reason;
                    ((clsCampMasterVO)this.DataContext).Description = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Description;
                    ((clsCampMasterVO)this.DataContext).CampID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.CampID;
                    ((clsCampMasterVO)this.DataContext).TariffID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.TariffID;
                    ((clsCampMasterVO)this.DataContext).Tariff = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Tariff;
                    ((clsCampMasterVO)this.DataContext).Status = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.Status;

                    if (((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.EmailTemplateID != null)
                    {
                        ((clsCampMasterVO)this.DataContext).EmailTemplateID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.EmailTemplateID;
                        ((clsCampMasterVO)this.DataContext).EmailTemplateDetails.ID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.EmailTemplateDetails.ID;
                        ((clsCampMasterVO)this.DataContext).EmailTemplateDetails.Description = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.EmailTemplateDetails.Description;
                    }

                    if (((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.SmsTemplateID != null)
                    {
                        ((clsCampMasterVO)this.DataContext).SmsTemplateID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.SmsTemplateID;
                        ((clsCampMasterVO)this.DataContext).SMSTemplateDetails.ID = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.SMSTemplateDetails.ID;
                        ((clsCampMasterVO)this.DataContext).SMSTemplateDetails.Description = ((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.SMSTemplateDetails.Description;
                    }


                    IEnumerator<MasterListItem> list = (IEnumerator<MasterListItem>)cmbCamp.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.CampID)
                        {
                            cmbCamp.SelectedItem = objMsterListItem;
                            break;
                        }

                    }

                    list = (IEnumerator<MasterListItem>)cmbTariff.ItemsSource.GetEnumerator();
                    while (list.MoveNext())
                    {
                        MasterListItem objMsterListItem = list.Current;
                        if (objMsterListItem.ID == (Int64)((clsGetCampDetailsByIDBizActionVO)arg.Result).CampMasterDetails.TariffID)
                        {
                            cmbTariff.SelectedItem = objMsterListItem;
                            break;
                        }
                    }

                    FillEmail();
                    FillSMS();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        /// <summary>
        /// Purpose:Modify existing camp details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (dtpToDate.SelectedDate > DateTime.Now.Date.Date)
            {
                bool ModifyCampDetails = true;
                ModifyCampDetails = CheckValidation();
                if (ModifyCampDetails == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Update the Camp Details?";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                    msgW1.Show();

                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW5 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "You can not update this camp details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW5.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();

        }

        private void Modify()
        {
            try
            {
                clsUpdateCampDetailsBizActionVO BizAction = new clsUpdateCampDetailsBizActionVO();

                BizAction.CampMasterDetails = (clsCampMasterVO)this.DataContext;
                if (cmbCamp.SelectedItem != null)
                    BizAction.CampMasterDetails.CampID = ((MasterListItem)cmbCamp.SelectedItem).ID;

                if (cmbTariff.SelectedItem != null)
                    BizAction.CampMasterDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                if (cmbEmail.SelectedItem != null)
                    BizAction.CampMasterDetails.EmailTemplateID = ((MasterListItem)cmbEmail.SelectedItem).ID;

                if (cmbSms.SelectedItem != null)
                    BizAction.CampMasterDetails.SmsTemplateID = ((MasterListItem)cmbSms.SelectedItem).ID;

                //BizAction.CampMasterDetails.CampServiceDetails = CampServiceDetail; //Add CampSevice in T_CampService table.

                if (FreeCampServiceList != null && FreeCampServiceList.Count != 0)
                {

                    BizAction.CampMasterDetails.FreeCampServiceList = FreeCampServiceList.ToList();
                }
                if (ConcessionCampServiceList != null && ConcessionCampServiceList.Count != 0)
                {
                    BizAction.CampMasterDetails.ConcessionServiceList = ConcessionCampServiceList.ToList();
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null)
                    {


                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Camp Details Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while updating Camp Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                    SetCommandButtonState("New");

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        #region Validation

        private bool CheckValidation()
        {
            bool result = true;

            if (txtArea.Text == "")
            {
                txtArea.SetValidation("Please Enter Area");
                txtArea.RaiseValidationError();
                txtArea.Focus();
                TabControlMain.SelectedIndex = 0;
                result = false;


            }
            else
                txtArea.ClearValidationError();

            if (txtCity.Text == "")
            {
                txtCity.SetValidation("Please Enter City");
                txtCity.RaiseValidationError();
                txtCity.Focus();
                TabControlMain.SelectedIndex = 0;
                result = false;


            }
            else
                txtCity.ClearValidationError();

            if (((clsCampMasterVO)this.DataContext).ToDate < dtpFromDate.SelectedDate)
            {

                dtpToDate.SetValidation("To Date can not be less than From date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                TabControlMain.SelectedIndex = 0;
                result = false;

            }
            else if (dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Select To Date ");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                TabControlMain.SelectedIndex = 0;
                result = false;

            }
            else
                dtpToDate.ClearValidationError();



            if (IsUpdate == true)
            {
                if (((clsCampMasterVO)this.DataContext).FromDate < DateTime.Today)
                {

                    dtpFromDate.SetValidation("From Date can not be less than Today's date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;


                }
                else if (dtpFromDate.SelectedDate == null)
                {
                    dtpFromDate.SetValidation("Please Select From Date ");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;

                }
                else
                    dtpFromDate.ClearValidationError();
            }


            if (IsPageLoded)
            {
                if (dtpPatientRegistrationValidTillDate.SelectedDate < dtpFromDate.SelectedDate)
                {
                    //dtpPatientRegistrationValidTillDate.SetValidation("Registration Valid Till Date can not be less than From Date ");
                    //dtpPatientRegistrationValidTillDate.RaiseValidationError();
                    //dtpPatientRegistrationValidTillDate.Focus();

                    MessageBoxControl.MessageBoxChildWindow msgW11 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Registration Valid Till Date can not be less than From Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW11.OnMessageBoxClosed += (MessageBoxResult res1) =>
                    {
                        if (res1 == MessageBoxResult.OK)
                        {
                            dtpPatientRegistrationValidTillDate.Focus();

                        }
                    };


                    msgW11.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;

                }
                else if (dtpPatientRegistrationValidTillDate.SelectedDate == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient Registration Valid Till Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                    {
                        if (res1 == MessageBoxResult.OK)
                        {
                            dtpPatientRegistrationValidTillDate.Focus();

                        }
                    };

                    msgW1.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;

                }
                else
                    dtpPatientRegistrationValidTillDate.ClearValidationError();


                if ((MasterListItem)cmbTariff.SelectedItem == null)
                {

                    cmbTariff.TextBox.SetValidation("Please Select Tariff");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;


                }
                else if (((MasterListItem)cmbTariff.SelectedItem).ID == 0)
                {
                    cmbTariff.TextBox.SetValidation("Please Select Tariff");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;

                }
                else
                    cmbTariff.TextBox.ClearValidationError();

                if ((MasterListItem)cmbCamp.SelectedItem == null)
                {
                    cmbCamp.TextBox.SetValidation("Please Select Camp");
                    cmbCamp.TextBox.RaiseValidationError();
                    cmbCamp.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;

                }
                else if (((MasterListItem)cmbCamp.SelectedItem).ID == 0)
                {
                    cmbCamp.TextBox.SetValidation("Please Select Camp");
                    cmbCamp.TextBox.RaiseValidationError();
                    cmbCamp.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;

                }
                else
                    cmbCamp.TextBox.ClearValidationError();


                if (dgFreeServiceList.ItemsSource == null && dgConServiceList.ItemsSource == null)
                {
                    string msgTitle = "Palash";
                    string msgText = "Please Select Service";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW.Show();
                    TabControlMain.SelectedIndex = 1;
                    result = false;
                    return result;
                }
                else if (FreeCampServiceList.Count == 0 && ConcessionCampServiceList.Count == 0)
                {
                    string msgTitle = "Palash";
                    string msgText = "Please Select Service";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW.Show();
                    TabControlMain.SelectedIndex = 1;
                    result = false;
                    return result;

                }
            }

            return result;
        }


        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();

            FillAreaList(txtCity.Text);

            if (txtCity.Text == "")
            {
                txtCity.SetValidation("Please Enter City");
                txtCity.RaiseValidationError();
            }
            else
                txtCity.ClearValidationError();


        }

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();

            if (txtArea.Text == "")
            {
                txtArea.SetValidation("Please Enter Area");
                txtArea.RaiseValidationError();
            }
            else
                txtArea.ClearValidationError();
        }

        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtValidDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore == null)
                    textBefore = "0";
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtValidDays_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        #endregion

        #region Reset All Controls
        private void ClearControl()
        {

            cmbCamp.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;
            cmbTariff.SelectedValue = ((clsCampMasterVO)this.DataContext).TariffID;
            cmbEmail.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateID;
            cmbSms.SelectedValue = ((clsCampMasterVO)this.DataContext).SmsTemplateID;

            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
            txtArea.Text = string.Empty;
            txtCity.Text = string.Empty; ;
            txtValidDays.Text = string.Empty; ;
            //txtConcession.Text = string.Empty; ;
            dgConServiceList.ItemsSource = null;
            dgFreeServiceList.ItemsSource = null;
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void LocationLink_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txtCity.Text.Trim()))
            {
                string strAddress = txtArea.Text.Trim() + " " + txtCity.Text.Trim();
                Uri address1 = new Uri(Application.Current.Host.Source, "../GoogleMap.aspx?Address=" + strAddress);
                HtmlPage.Window.Navigate(address1, "_blank");
            }

        }


        private void cmdDeleteFreeService_Click(object sender, RoutedEventArgs e)
        {
            if (dgFreeServiceList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected service ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        FreeCampServiceList.RemoveAt(dgFreeServiceList.SelectedIndex);
                        dgFreeServiceList.Focus();
                        dgFreeServiceList.UpdateLayout();
                        dgFreeServiceList.SelectedIndex = FreeCampServiceList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void cmdDeleteConcessionService_Click(object sender, RoutedEventArgs e)
        {
            if (dgConServiceList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected service ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        if (dgConServiceList.ItemsSource != null)
                        {
                            ConcessionCampServiceList.RemoveAt(dgConServiceList.SelectedIndex);
                            dgConServiceList.Focus();
                            dgConServiceList.UpdateLayout();
                            dgConServiceList.SelectedIndex = ConcessionCampServiceList.Count - 1;
                        }
                    }
                };
                msgWD.Show();
            }

        }

        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgFreeServiceList.ItemsSource != null)
            {
                FreeCampServiceList = new ObservableCollection<CampserviceDetailsVO>();

                dgFreeServiceList.ItemsSource = FreeCampServiceList;
                dgFreeServiceList.Focus();
                dgFreeServiceList.UpdateLayout();
                dgFreeServiceList.SelectedIndex = FreeCampServiceList.Count - 1;
            }
            if (dgConServiceList.ItemsSource != null)
            {
                ConcessionCampServiceList = new ObservableCollection<CampserviceDetailsVO>();
                dgConServiceList.ItemsSource = ConcessionCampServiceList;
                dgConServiceList.Focus();
                dgConServiceList.UpdateLayout();
                dgConServiceList.SelectedIndex = ConcessionCampServiceList.Count - 1;
            }

        }

        private void Autocomplete_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void Autocomplete_TextChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {

            }
            else  if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
               
            }
        }



    }
}


