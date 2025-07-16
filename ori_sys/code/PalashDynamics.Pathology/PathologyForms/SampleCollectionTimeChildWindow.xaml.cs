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
using PalashDynamics.ValueObjects.Pathology;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.ValueObjects.Master;



namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class SampleCollectionTimeChildWindow : ChildWindow
    {
        #region Public Variables
        public string msgText;
        public string msgTitle;
        clsUserVO UserVo = new clsUserVO();
        public bool IsFromSampleCollection;
        public bool IsFromSampleDispatch;
        public bool IsFromSampleReceive;
        public bool IsFromSampleAccept;
        public bool IsSampleReject;
        public event RoutedEventHandler OnSaveButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;
        public List<clsPathOrderBookingDetailVO> itemList { get; set; }
        public bool SendRejectEmail;
        public string strUserName = null;
        public string UserName { get; set; }
      

        #endregion

        public SampleCollectionTimeChildWindow()
        {
            InitializeComponent(); Counter = 0;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    cmdSave.IsEnabled = false;
            }
          //  this.Loaded += new RoutedEventHandler(SampleCollectionTimeChildWindow_Loaded);
        }

        public long Counter { get; set; }
      //  public string UserName = null;
        void SampleCollectionTimeChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

           // string UserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;

            string UnitName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName;

            if (IsFromSampleCollection == true)
            {
               
                lblSampleReceiveDate.Visibility = Visibility.Collapsed;
                lblSampleReceiveTime.Visibility = Visibility.Collapsed;
                lblRejectionRemark.Visibility = Visibility.Collapsed;
                lblSampleDispatchDate.Visibility = Visibility.Collapsed;
                lblSampleDispatchTime.Visibility = Visibility.Collapsed;

                lblSampleAcceptDate.Visibility = Visibility.Collapsed;
                lblSampleAcceptTime.Visibility = Visibility.Collapsed;
                dtpSampAcceptDate.Visibility = Visibility.Collapsed;
                dtpSampAcceptTime.Visibility = Visibility.Collapsed;

                dtpSampReceDate.Visibility = Visibility.Collapsed;
                dtpSampReceTime.Visibility = Visibility.Collapsed;

                dtpDispatchDate.Visibility = Visibility.Collapsed;
                dtpDispatchTime.Visibility = Visibility.Collapsed;
                txtRejectionRemark.Visibility = Visibility.Collapsed;
                //comented by rohini
                //dtpSampColleDate.SelectedDate = DateTime.Now;
                //dtpSampCollTime.Value = DateTime.Now;
                //by rohini dated 4/2/16
                //comented by rohini
                //dtpSampColleDate.DisplayDateStart = DateTime.Now.Date.AddDays(-1);


                fillCollection();
                //((MasterListItem)CmbStatus.SelectedValue).ID = 0;
                //((MasterListItem)CmbCollnCenter.SelectedValue).ID = 0;
                txtStatus.IsEnabled = false;
                this.Title = "Sample Collection";
                lblDispatchTo.Visibility = Visibility.Collapsed;
                lblDispatchBy.Visibility = Visibility.Collapsed;
                CmbDispatchTo.Visibility = Visibility.Collapsed;
                txtDispatchBy.Visibility = Visibility.Collapsed;
                lblAcceptedBy.Visibility = Visibility.Collapsed;
                lblRemark.Visibility = Visibility.Collapsed;
                txtAcceptedBy.Visibility = Visibility.Collapsed;
                // rbRejected.Visibility = Visibility.Collapsed;
                rbAccepted.Visibility = Visibility.Collapsed;
                chkSubOptimal.Visibility = Visibility.Collapsed;
                txtAcceptRemark.Visibility = Visibility.Collapsed;
                chkResendSample.Visibility = Visibility.Collapsed;
                txtReceivedBy.Visibility = Visibility.Collapsed;
                lblReceivedBy.Visibility = Visibility.Collapsed;

                //
                if (UserName == null || UserName == "")
                {
                    UserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                }
                //txtSampleBy.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                txtSampleBy.Text = UserName;

            }
            else if (IsFromSampleReceive == true)
            {
                
                lblsampleCollectionDate.Visibility = Visibility.Collapsed;
                lblSampleCollectionTime.Visibility = Visibility.Collapsed;
                lblRejectionRemark.Visibility = Visibility.Collapsed;
                lblSampleAcceptDate.Visibility = Visibility.Collapsed;
                lblSampleAcceptTime.Visibility = Visibility.Collapsed;
                dtpSampAcceptDate.Visibility = Visibility.Collapsed;
                dtpSampAcceptTime.Visibility = Visibility.Collapsed;
                lblSampleDispatchDate.Visibility = Visibility.Collapsed;
                lblSampleDispatchTime.Visibility = Visibility.Collapsed;
                dtpSampColleDate.Visibility = Visibility.Collapsed;
                dtpSampCollTime.Visibility = Visibility.Collapsed;
                dtpDispatchDate.Visibility = Visibility.Collapsed;
                dtpDispatchTime.Visibility = Visibility.Collapsed;
                txtRejectionRemark.Visibility = Visibility.Collapsed;
                //dtpSampReceDate.SelectedDate = DateTime.Now;
                //dtpSampReceTime.Value = DateTime.Now;

                //by rohini dated 4/2/16
                lblStatus.Visibility = Visibility.Collapsed;
                lblGestation.Visibility = Visibility.Collapsed;
                lblCollnCenter.Visibility = Visibility.Collapsed;
                lblCollectedBy.Visibility = Visibility.Collapsed;
                CmbStatus.Visibility = Visibility.Collapsed;
                txtStatus.Visibility = Visibility.Collapsed;
                txtGestation.Visibility = Visibility.Collapsed;
                lblStatus2.Visibility = Visibility.Collapsed;
                lblStatus1.Visibility = Visibility.Collapsed;
                CmbCollnCenter.Visibility = Visibility.Collapsed;
                txtCollnCenter.Visibility = Visibility.Collapsed;
                txtSampleBy.Visibility = Visibility.Collapsed;
                lblDispatchTo.Visibility = Visibility.Collapsed;
                lblDispatchBy.Visibility = Visibility.Collapsed;
                CmbDispatchTo.Visibility = Visibility.Collapsed;
                txtDispatchBy.Visibility = Visibility.Collapsed;
                lblAcceptedBy.Visibility = Visibility.Collapsed;
                lblRemark.Visibility = Visibility.Collapsed;
                txtAcceptedBy.Visibility = Visibility.Collapsed;
                //rbRejected.Visibility = Visibility.Collapsed;
                chkSubOptimal.Visibility = Visibility.Collapsed;
                txtAcceptRemark.Visibility = Visibility.Collapsed;
                rbAccepted.Visibility = Visibility.Collapsed;
                chkResendSample.Visibility = Visibility.Collapsed;
                //added by rohini
                getServerDate();
                this.Title = "Sample Receipt";
                //dtpSampReceDate.DisplayDateStart = DateTime.Now;
                //dtpSampReceDate.DisplayDateStart = DateTime.Now.Date;

                //

                // txtReceivedBy.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (UserName == null || UserName == "")
                {
                    UserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                }
                txtReceivedBy.Text = UserName;
            }
            else if (IsFromSampleDispatch == true)
            {

                // txtDispatchBy.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                
                lblSampleAcceptDate.Visibility = Visibility.Collapsed;
                lblSampleAcceptTime.Visibility = Visibility.Collapsed;
                dtpSampAcceptDate.Visibility = Visibility.Collapsed;
                dtpSampAcceptTime.Visibility = Visibility.Collapsed;
                lblRejectionRemark.Visibility = Visibility.Collapsed;
                lblsampleCollectionDate.Visibility = Visibility.Collapsed;
                lblSampleCollectionTime.Visibility = Visibility.Collapsed;
                lblSampleReceiveDate.Visibility = Visibility.Collapsed;
                lblSampleReceiveTime.Visibility = Visibility.Collapsed;

                dtpSampColleDate.Visibility = Visibility.Collapsed;
                dtpSampCollTime.Visibility = Visibility.Collapsed;
                dtpSampReceDate.Visibility = Visibility.Collapsed;
                dtpSampReceTime.Visibility = Visibility.Collapsed;
                txtRejectionRemark.Visibility = Visibility.Collapsed;
                //dtpDispatchDate.SelectedDate = DateTime.Now;
                //dtpDispatchTime.Value = DateTime.Now;

                //by rohini dated 4/2/16
                lblStatus.Visibility = Visibility.Collapsed;
                lblGestation.Visibility = Visibility.Collapsed;
                lblCollnCenter.Visibility = Visibility.Collapsed;
                lblCollectedBy.Visibility = Visibility.Collapsed;
                CmbStatus.Visibility = Visibility.Collapsed;
                txtStatus.Visibility = Visibility.Collapsed;
                lblStatus2.Visibility = Visibility.Collapsed;
                lblStatus1.Visibility = Visibility.Collapsed;
                txtGestation.Visibility = Visibility.Collapsed;
                CmbCollnCenter.Visibility = Visibility.Collapsed;
                txtCollnCenter.Visibility = Visibility.Collapsed;
                txtSampleBy.Visibility = Visibility.Collapsed;
                lblAcceptedBy.Visibility = Visibility.Collapsed;
                lblRemark.Visibility = Visibility.Collapsed;
                txtAcceptedBy.Visibility = Visibility.Collapsed;
                // rbRejected.Visibility = Visibility.Collapsed;
                chkSubOptimal.Visibility = Visibility.Collapsed;
                txtAcceptRemark.Visibility = Visibility.Collapsed;
                rbAccepted.Visibility = Visibility.Collapsed;
                chkResendSample.Visibility = Visibility.Collapsed;
                txtReceivedBy.Visibility = Visibility.Collapsed;
                lblReceivedBy.Visibility = Visibility.Collapsed;
                //added by rohini 5.2.16
                FillDispatch();
                //dtpDispatchDate.DisplayDateStart = DateTime.Now;
                dtpDispatchDate.DisplayDateStart = DateTime.Now.Date;
                this.Title = "Sample Dispatch";
                //
                if (UserName == null || UserName == "")
                {
                    UserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                }
                txtDispatchBy.Text = UserName;
            }
            if (IsFromSampleAccept == true)
            {
                
                
                txtCollCenter.Text = UnitName;
                lblsampleCollectionDate.Visibility = Visibility.Collapsed;
                lblSampleCollectionTime.Visibility = Visibility.Collapsed;
                lblSampleDispatchDate.Visibility = Visibility.Collapsed;
                lblSampleDispatchTime.Visibility = Visibility.Collapsed;
                lblSampleReceiveDate.Visibility = Visibility.Collapsed;
                lblSampleReceiveTime.Visibility = Visibility.Collapsed;
                dtpSampColleDate.Visibility = Visibility.Collapsed;
                dtpSampCollTime.Visibility = Visibility.Collapsed;
                dtpDispatchDate.Visibility = Visibility.Collapsed;
                dtpDispatchTime.Visibility = Visibility.Collapsed;
                dtpSampReceDate.Visibility = Visibility.Collapsed;
                dtpSampReceTime.Visibility = Visibility.Collapsed;

                if (IsSampleReject == true)
                {

                    //dtpSampAcceptDate.SelectedDate = DateTime.Now;
                    //dtpSampAcceptTime.Value = DateTime.Now;

                    txtRejectionRemark.Visibility = Visibility.Visible;
                    lblRejectionRemark.Visibility = Visibility.Visible;
                    //comented by rohini for tem dated 9.2.16
                    //chkSendSMS.Visibility = Visibility.Visible;
                    //chkSendEmail.Visibility = Visibility.Visible;
                    //
                    //by rohini dated 4/2/16
                    lblStatus.Visibility = Visibility.Collapsed;
                    lblGestation.Visibility = Visibility.Collapsed;
                    lblCollnCenter.Visibility = Visibility.Collapsed;
                    lblCollectedBy.Visibility = Visibility.Collapsed;
                    CmbStatus.Visibility = Visibility.Collapsed;
                    txtStatus.Visibility = Visibility.Collapsed;
                    lblStatus2.Visibility = Visibility.Collapsed;
                    lblStatus1.Visibility = Visibility.Collapsed;
                    txtGestation.Visibility = Visibility.Collapsed;
                    CmbCollnCenter.Visibility = Visibility.Collapsed;
                    txtCollnCenter.Visibility = Visibility.Collapsed;
                    txtSampleBy.Visibility = Visibility.Collapsed;
                    lblDispatchTo.Visibility = Visibility.Collapsed;
                    lblDispatchBy.Visibility = Visibility.Collapsed;
                    CmbDispatchTo.Visibility = Visibility.Collapsed;
                    txtDispatchBy.Visibility = Visibility.Collapsed;
                    chkResendSample.Visibility = Visibility.Visible;
                    lblRemark.Visibility = Visibility.Collapsed;
                    chkSubOptimal.Visibility = Visibility.Collapsed;
                    txtAcceptRemark.Visibility = Visibility.Collapsed;
                    rbAccepted.Visibility = Visibility.Collapsed;
                    txtReceivedBy.Visibility = Visibility.Collapsed;
                    lblReceivedBy.Visibility = Visibility.Collapsed;
                    this.Title = "Sample Rejection ";
                    lblSampleAcceptDate.Text = "Sample Rejection Date :";
                    lblSampleAcceptTime.Text = "Sample Rejection Time :";
                    lblAcceptedBy.Text = "Rejected By";
                    dtpSampAcceptDate.DisplayDateStart = DateTime.Now.Date;
                }
                else
                {

                    //dtpSampAcceptDate.SelectedDate = DateTime.Now;
                    //dtpSampAcceptTime.Value = DateTime.Now;
                    //by rohini dated 4/2/16
                    lblStatus.Visibility = Visibility.Collapsed;
                    chkResendSample.Visibility = Visibility.Collapsed;
                    lblGestation.Visibility = Visibility.Collapsed;
                    lblCollnCenter.Visibility = Visibility.Collapsed;
                    lblCollectedBy.Visibility = Visibility.Collapsed;
                    CmbStatus.Visibility = Visibility.Collapsed;
                    txtStatus.Visibility = Visibility.Collapsed;
                    lblStatus2.Visibility = Visibility.Collapsed;
                    lblStatus1.Visibility = Visibility.Collapsed;
                    txtGestation.Visibility = Visibility.Collapsed;
                    CmbCollnCenter.Visibility = Visibility.Collapsed;
                    txtCollnCenter.Visibility = Visibility.Collapsed;
                    txtSampleBy.Visibility = Visibility.Collapsed;
                    lblDispatchTo.Visibility = Visibility.Collapsed;
                    lblDispatchBy.Visibility = Visibility.Collapsed;
                    CmbDispatchTo.Visibility = Visibility.Collapsed;
                    txtDispatchBy.Visibility = Visibility.Collapsed;
                    txtReceivedBy.Visibility = Visibility.Collapsed;
                    lblReceivedBy.Visibility = Visibility.Collapsed;
                    chkResendSample.Visibility = Visibility.Collapsed;
                    this.Title = "Sample Acceptance";
                    dtpSampAcceptDate.DisplayDateStart = DateTime.Now;

                }
                //
                //txtAcceptedBy.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (UserName == null || UserName == "")
                {
                    UserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                }
                txtAcceptedBy.Text = UserName == null ? this.strUserName : UserName;  //txtAcceptedBy.Text = //UserName;
            }
            chkIsOutSourced.IsChecked = false;
            //itemList = new List<clsPathOrderBookingDetailVO>();
            tblAgency.Visibility = Visibility.Collapsed;
            txtAgencyName.Visibility = Visibility.Collapsed;

            FillAgencyMaster();
        }
        DateTime ServerDateTime;
        private void getServerDate()
        {
            clsGetServerDateTimeBizActionVO BizAction = new clsGetServerDateTimeBizActionVO();
            // BizAction.id = ((MasterListItem)CmbStatus.SelectedItem).ID;

            // BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    ServerDateTime = new DateTime();
                    ServerDateTime = (((clsGetServerDateTimeBizActionVO)args.Result).ServerDateTime);
                    //added by rohini for server date time
                    dtpSampColleDate.SelectedDate = ServerDateTime;
                    dtpSampCollTime.Value = ServerDateTime;
                    //
                    //added by rohini for server date time
                    dtpSampColleDate.SelectedDate = ServerDateTime;
                    dtpSampCollTime.Value = ServerDateTime;
                    //
                    //added by rohini for server date time
                    dtpDispatchDate.SelectedDate = ServerDateTime;
                    dtpDispatchTime.Value = ServerDateTime;
                    //
                    //added by rohini for server date time
                    dtpSampAcceptDate.SelectedDate = ServerDateTime;
                    dtpSampAcceptTime.Value = ServerDateTime;
                    //

                    //added by rohini for server date time
                    dtpSampAcceptDate.SelectedDate = ServerDateTime;
                    dtpSampAcceptTime.Value = ServerDateTime;
                    //
                    dtpSampReceDate.SelectedDate = ServerDateTime;
                    dtpSampReceTime.Value = ServerDateTime;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //
        //by rohini dated 4/2/16 for ACCEPT
        private void rbAccepted_Checked(object sender, RoutedEventArgs e)
        {
            if (rbAccepted != null)
            {
                if (rbAccepted.IsChecked == true)
                {
                    chkSubOptimal.IsEnabled = true;
                }
                else
                {
                    chkSubOptimal.IsEnabled = false;
                }
            }
        }

        //by rohini dated 4/2/16 for sample        
        private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbStatus.SelectedValue != null || ((MasterListItem)CmbStatus.SelectedItem).ID != (long)0)
            {
                clsGetPathoFastingBizActionVO BizAction = new clsGetPathoFastingBizActionVO();
                BizAction.id = ((MasterListItem)CmbStatus.SelectedItem).ID;

                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        foreach (var i in ((clsGetPathoFastingBizActionVO)args.Result).MasterList)
                        {
                            if (i.IsHrs == true)
                            {
                                txtStatus.IsEnabled = true;
                            }
                            else
                            {
                                txtStatus.IsEnabled = false;
                                txtStatus.Text = "";
                            }

                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }
        private void CmbCollnCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)CmbCollnCenter.SelectedItem).ID == 1)
            {
                txtCollnCenter.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName;
            }
            else
            {
                txtCollnCenter.Text = string.Empty;
            }

        }
        private void fillCollection()
        {
            List<MasterListItem> UOMConvertLIst1 = new List<MasterListItem>();
            MasterListItem objConversion = new MasterListItem();
            UOMConvertLIst1.Add(new MasterListItem(0, "- Select -"));
            UOMConvertLIst1.Add(new MasterListItem(1, "In House"));
            UOMConvertLIst1.Add(new MasterListItem(2, "Clinic"));
            UOMConvertLIst1.Add(new MasterListItem(3, "Hospital"));
            UOMConvertLIst1.Add(new MasterListItem(4, "Home"));
            CmbCollnCenter.ItemsSource = null;
            CmbCollnCenter.ItemsSource = UOMConvertLIst1;
            CmbCollnCenter.SelectedItem = UOMConvertLIst1[0];
            FillStatus();
            //((MasterListItem)CmbCollnCenter.SelectedItem).ID = (long)0;
        }
        private void FillStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathoFastingStatus;
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
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    CmbStatus.ItemsSource = null;
                    CmbStatus.ItemsSource = objList;
                    CmbStatus.SelectedItem = objList[0];
                    // ((MasterListItem)CmbStatus.SelectedItem).ID = (long)0;
                    //added by rohini
                    getServerDate();

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        //

        //by rohini dated 4/2/16 for dispatch
        private void FillDispatch()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "IsProcessingUnit";

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    CmbDispatchTo.ItemsSource = null;
                    CmbDispatchTo.ItemsSource = objList;
                    CmbDispatchTo.SelectedValue = (long)0;
                    //added by rohini
                    getServerDate();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillAgencyMaster()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtAgencyName.ItemsSource = null;
                    txtAgencyName.ItemsSource = objList;
                    //added by rohini
                    getServerDate();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillAgencyApplicableUnitList()
        {
            clsGetPathoAgencyApplicableUnitListBizActionVO BizAction = new clsGetPathoAgencyApplicableUnitListBizActionVO();
            BizAction.ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();
            BizAction.ServiceId = ((clsPathOrderBookingDetailVO)this.DataContext).ServiceID;
            BizAction.ApplicableUnitID = ((clsPathOrderBookingDetailVO)this.DataContext).UnitId;
            BizAction.UnitId = ((clsPathOrderBookingDetailVO)this.DataContext).UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem { ID = 0, Description = "- Select -" });
                    foreach (clsServiceAgencyMasterVO item in ((clsGetPathoAgencyApplicableUnitListBizActionVO)args.Result).ServiceAgencyMasterDetails)
                    {
                        MasterListItem obj = new MasterListItem();
                        obj.ID = item.AgencyID;
                        obj.Description = item.AgencyName;
                        obj.isChecked = item.IsDefaultAgency;
                        objList.Add(obj);
                    }
                    txtAgencyName.ItemsSource = null;
                    txtAgencyName.ItemsSource = objList;
                    var result = from r in (objList)
                                 where r.isChecked == true
                                 select r;

                    if (result != null && result.ToList().Count > 0)
                    {
                        if (((clsPathOrderBookingDetailVO)this.DataContext).AgencyID == null || ((clsPathOrderBookingDetailVO)this.DataContext).AgencyID == 0)
                        {
                            txtAgencyName.SelectedItem = ((MasterListItem)result.First());
                            txtAgencyName.IsEnabled = true;
                        }
                        else
                        {
                            var result1 = from r1 in (objList)
                                          where r1.ID == ((clsPathOrderBookingDetailVO)this.DataContext).AgencyID
                                          select r1;
                            if (result1 != null && result1.ToList().Count > 0)
                            {
                                txtAgencyName.SelectedItem = ((MasterListItem)result1.First());
                                txtAgencyName.IsEnabled = false;
                            }
                            else
                            {
                                txtAgencyName.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        if (((clsPathOrderBookingDetailVO)this.DataContext).AgencyID == null || ((clsPathOrderBookingDetailVO)this.DataContext).AgencyID == 0)
                        {
                            txtAgencyName.SelectedItem = objList[0];
                            txtAgencyName.IsEnabled = true;
                        }
                        else
                        {
                            var result2 = from r2 in (objList)
                                          where r2.ID == ((clsPathOrderBookingDetailVO)this.DataContext).AgencyID
                                          select r2;
                            if (result2 != null && result2.ToList().Count > 0)
                            {
                                txtAgencyName.SelectedItem = ((MasterListItem)result2.First());
                                txtAgencyName.IsEnabled = false;
                            }
                            else
                            {
                                txtAgencyName.SelectedItem = objList[0];
                                txtAgencyName.IsEnabled = true;
                            }
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private bool chkval()
        {
            bool result = true;
            try
            {
                if (IsFromSampleCollection)
                {
                    //if (Convert.ToDateTime(dtpSampColleDate.SelectedDate) < DateTime.Now.Date || dtpSampColleDate.SelectedDate == null || Convert.ToDateTime(dtpSampColleDate.SelectedDate) > DateTime.Now.Date)
                    //{


                    //    msgText = "Please Select Proper Sample Collection Date";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow =
                    //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgWindow.Show();
                    //    dtpSampColleDate.Focus();
                    //    result = false;

                    //}
                    //else if (Convert.ToDateTime(dtpSampCollTime.Value) > DateTime.Now || dtpSampCollTime.Value == null)
                    //{

                    //    msgText = "Please Select Proper Sample Collection Time";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow =
                    //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgWindow.Show();
                    //    dtpSampCollTime.Focus();
                    //    result = false;
                    //}


                    if (string.IsNullOrEmpty(txtSampleBy.Text.Trim()))
                    {
                        //msgText = "Please Select Sample Collected By";
                        //MessageBoxControl.MessageBoxChildWindow msgWindow =
                        //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgWindow.Show();
                        //dtpSampCollTime.Focus();
                        //result = false;
                        txtSampleBy.SetValidation("Please Select Sample Collected By");
                        txtSampleBy.RaiseValidationError();
                        txtSampleBy.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        txtSampleBy.ClearValidationError();
                    }


                    if (((MasterListItem)CmbCollnCenter.SelectedItem).ID <= 0)
                    {
                        CmbCollnCenter.TextBox.SetValidation("Please Select Collection Center");
                        CmbCollnCenter.TextBox.RaiseValidationError();
                        CmbCollnCenter.TextBox.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        CmbCollnCenter.TextBox.ClearValidationError();
                    }
                    if (((MasterListItem)CmbStatus.SelectedItem).ID <= 0)
                    {
                        CmbStatus.TextBox.SetValidation("Please Select Collection Status");
                        CmbStatus.TextBox.RaiseValidationError();
                        CmbStatus.TextBox.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        CmbStatus.TextBox.ClearValidationError();
                    }

                }
                else if (IsFromSampleReceive)
                {
                    if (Convert.ToDateTime(dtpSampReceDate.SelectedDate) < DateTime.Now.Date || dtpSampReceDate.SelectedDate == null || Convert.ToDateTime(dtpSampReceDate.SelectedDate) > DateTime.Now.Date)
                    {

                        //comented by rohini for tem perpose

                        //msgText = "Please Select Proper Sample Received Date";
                        //MessageBoxControl.MessageBoxChildWindow msgWindow =
                        //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgWindow.Show();
                        //dtpSampReceDate.Focus();
                        //result = false;


                        if (string.IsNullOrEmpty(txtReceivedBy.Text.Trim()))
                        {
                            txtReceivedBy.SetValidation("Please Select Sample Received By");
                            txtReceivedBy.RaiseValidationError();
                            txtReceivedBy.Focus();
                            result = false;
                            Counter = 0;
                        }
                        else
                        {
                            txtReceivedBy.ClearValidationError();
                        }

                    }
                    else if (Convert.ToDateTime(dtpSampReceTime.Value) > DateTime.Now || dtpSampReceTime.Value == null)
                    {

                        //msgText = "Please Select Proper Sample Received Time";
                        //MessageBoxControl.MessageBoxChildWindow msgWindow =
                        //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgWindow.Show();
                        //dtpSampReceTime.Focus();
                        //result = false;
                        if (string.IsNullOrEmpty(txtReceivedBy.Text.Trim()))
                        {
                            txtReceivedBy.SetValidation("Please Select Sample Received By");
                            txtReceivedBy.RaiseValidationError();
                            txtReceivedBy.Focus();
                            result = false;
                            Counter = 0;
                        }
                        else
                        {
                            txtReceivedBy.ClearValidationError();
                        }
                    }
                }
                else if (IsFromSampleDispatch)
                {

                    //else if (((MasterListItem)cmbSpecialization.SelectedItem).ID == 0)
                    //{
                    //    cmbSpecialization.TextBox.SetValidation("Please Select Specialization");
                    //    cmbSpecialization.TextBox.RaiseValidationError();
                    //    cmbSpecialization.Focus();
                    //    result = false;


                    //}

                    if (((MasterListItem)CmbDispatchTo.SelectedItem).ID <= 0)
                    {
                        CmbDispatchTo.TextBox.SetValidation("Please Select Dispatch To Clinic");
                        CmbDispatchTo.TextBox.RaiseValidationError();
                        CmbDispatchTo.TextBox.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        CmbDispatchTo.TextBox.ClearValidationError();
                    }
                    if (string.IsNullOrEmpty(txtDispatchBy.Text.Trim()))
                    {
                        txtDispatchBy.SetValidation("Please Select Sample Dispatched By");
                        txtDispatchBy.RaiseValidationError();
                        txtDispatchBy.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        txtDispatchBy.ClearValidationError();
                    }
                    if (Convert.ToDateTime(dtpDispatchDate.SelectedDate) < DateTime.Now.Date || dtpDispatchDate.SelectedDate == null || Convert.ToDateTime(dtpDispatchDate.SelectedDate) > DateTime.Now.Date)
                    {

                    }
                    else if (Convert.ToDateTime(dtpDispatchTime.Value) > DateTime.Now || dtpDispatchTime.Value == null)
                    {
                        //comented by rohini for tem perpose
                        //msgText = "Please Select Proper Sample Dispatch Time";
                        //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgWindow.Show();
                        //dtpDispatchTime.Focus();
                        //result = false;
                    }
                    return result;


                }
                else if (IsFromSampleAccept)
                {

                    if (string.IsNullOrEmpty(txtAcceptedBy.Text.Trim()))
                    {
                        if (IsSampleReject == true)
                        {
                            txtAcceptedBy.SetValidation("Please Select Sample Rejected By");
                        }
                        else
                        {
                            txtAcceptedBy.SetValidation("Please Select Sample Accepted By");

                        }
                        txtAcceptedBy.RaiseValidationError();
                        txtAcceptedBy.Focus();
                        result = false;
                        Counter = 0;
                    }
                    else
                    {
                        txtAcceptedBy.ClearValidationError();
                    }

                    if (string.IsNullOrEmpty(txtAcceptRemark.Text.Trim()) || string.IsNullOrEmpty(txtRejectionRemark.Text.Trim()))
                    {
                        if (IsSampleReject == true && string.IsNullOrEmpty(txtRejectionRemark.Text.Trim()))
                        {
                            txtRejectionRemark.SetValidation("Please Enter Remark");
                            txtRejectionRemark.RaiseValidationError();
                            txtRejectionRemark.Focus();
                            result = false;
                            Counter = 0;
                        }
                        else if (chkSubOptimal.IsChecked == true && string.IsNullOrEmpty(txtAcceptRemark.Text.Trim()))
                        {
                            txtAcceptRemark.SetValidation("Please Enter Remark In Case Of Sub Optimal");
                            txtAcceptRemark.RaiseValidationError();
                            txtAcceptRemark.Focus();
                            result = false;
                            Counter = 0;
                        }

                    }
                    else
                    {
                        txtAcceptRemark.ClearValidationError();
                        txtRejectionRemark.ClearValidationError();
                    }



                    //if (rbAccepted.IsChecked==false)
                    //{
                    //    //comented by rohini for tem perpose

                    //    msgText = "Please Check Is Accepted";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgWindow.Show();
                    //    dtpSampAcceptDate.Focus();
                    //    result = false;
                    //    Counter = 0;
                    //}
                    //else if (Convert.ToDateTime(dtpSampAcceptTime.Value) > DateTime.Now || dtpSampAcceptTime.Value == null)
                    //{
                    //    //comented by rohini for tem perpose

                    //    //msgText = "Please Select Proper Sample Accept Time";
                    //    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    //msgWindow.Show();
                    //    //dtpSampAcceptTime.Focus();
                    //    //result = false;
                    //}



                }
                //if (chkIsOutSourced.IsChecked == true)
                //{
                //    if (((MasterListItem)txtAgencyName.SelectedItem).ID == 0)
                //    {
                //        msgText = "Agency Name is required";
                //        txtAgencyName.TextBox.SetValidation(msgText);
                //        txtAgencyName.TextBox.RaiseValidationError();
                //        txtAgencyName.Focus();
                //        result = false;
                //    }
                //    else if (((MasterListItem)txtAgencyName.SelectedItem) == null)
                //    {
                //        msgText = "Agency Name is required";
                //        txtAgencyName.TextBox.SetValidation(msgText);
                //        txtAgencyName.TextBox.RaiseValidationError();
                //        txtAgencyName.Focus();
                //        result = false;
                //    }
                //    else
                //        txtAgencyName.TextBox.ClearValidationError();
                //}
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        private bool chkDispatchVal()
        {
            //added by rohini
            bool result = true;

            if ((MasterListItem)CmbDispatchTo.SelectedItem == null)
            {
                CmbDispatchTo.TextBox.SetValidation("Please Select Dispatched To Clinic");
                CmbDispatchTo.TextBox.RaiseValidationError();
                CmbDispatchTo.Focus();
                result = false;
            }
            else if (dtpDispatchDate.SelectedDate == null)
            {
                dtpDispatchDate.SetValidation("Please Select Dispatched To Clinic");
                dtpDispatchDate.RaiseValidationError();
                dtpDispatchDate.Focus();
                result = false;
            }
            return result;
        }
        public DateTime collDate;

        public DateTime collTime;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Counter = Counter + 1;
            if (Counter <= 1)
            {
                if (chkval())
                {
                    foreach (var item in itemList)
                    {
                        if (IsFromSampleCollection == true)
                        {
                            collDate = Convert.ToDateTime(dtpSampColleDate.SelectedDate);
                            collTime = Convert.ToDateTime(dtpSampCollTime.Value.Value);
                            DateTime dtCombinedColl = new DateTime(collDate.Year, collDate.Month, collDate.Day, collTime.Hour, collTime.Minute, collTime.Second);
                            if (dtpSampColleDate.SelectedDate != null)
                            {
                                item.SampleCollectedDateTime = dtCombinedColl;
                                item.IsSampleCollected = true;
                                item.IsFromSampleColletion = true;
                                item.IsOutSourced = false;
                                //added by rohini dated 5.1.16 as per client requirement
                                item.FastingStatusID = ((MasterListItem)CmbStatus.SelectedItem).ID;
                                if (!String.IsNullOrEmpty(txtStatus.Text))
                                {
                                    item.FastingStatusHrs = Convert.ToInt16(txtStatus.Text);
                                }
                                item.CollectionID = ((MasterListItem)CmbCollnCenter.SelectedItem).ID;
                                if (!String.IsNullOrEmpty(txtGestation.Text))
                                {
                                    item.Gestation = txtGestation.Text;
                                }
                                item.FastingStatusName = ((MasterListItem)CmbStatus.SelectedItem).Description;
                                item.CollectionName = ((MasterListItem)CmbCollnCenter.SelectedItem).Description;
                                item.CollectionCenter = txtCollnCenter.Text;
                                item.SampleCollectedBy = txtSampleBy.Text;


                            }
                        }
                        else if (IsFromSampleReceive == true)
                        {
                            DateTime recDate = Convert.ToDateTime(dtpSampReceDate.SelectedDate);
                            DateTime recTime = Convert.ToDateTime(dtpSampReceTime.Value);
                            DateTime dtCombinedrec = new DateTime(recDate.Year, recDate.Month, recDate.Day, recTime.Hour, recTime.Minute, recTime.Second);
                            if (dtpSampReceDate.SelectedDate != null)
                            {
                                item.SampleReceivedDateTime = dtCombinedrec;
                                item.IsSampleReceive = true;
                                item.IsFromSampleReceive = true;
                                item.SampleReceiveBy = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                            }
                        }
                        else if (IsFromSampleDispatch == true)
                        {
                            if (chkDispatchVal() == true)
                            {
                                DateTime dispatchDate = Convert.ToDateTime(dtpDispatchDate.SelectedDate);
                                DateTime dispatchTime = Convert.ToDateTime(dtpDispatchTime.Value);
                                DateTime dtCombinedDispatch = new DateTime(dispatchDate.Year, dispatchDate.Month, dispatchDate.Day, dispatchTime.Hour, dispatchTime.Minute, dispatchTime.Second);
                                if (dtpDispatchDate.SelectedDate != null)
                                {
                                    item.SampleDispatchDateTime = dtCombinedDispatch;
                                    item.IsSampleDispatch = true;
                                    item.IsFromSampleDispatch = true;
                                    //added by rohini dated 5.1.16
                                    item.DispatchToID = ((MasterListItem)CmbDispatchTo.SelectedItem).ID;
                                    item.DispatchToName = ((MasterListItem)CmbDispatchTo.SelectedItem).Description;
                                    item.DispatchBy = txtDispatchBy.Text.Trim();
                                    //
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required fields to save Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                            }
                        }
                        else if (IsFromSampleAccept == true)
                        {
                            DateTime AcceptDate = Convert.ToDateTime(dtpSampAcceptDate.SelectedDate);
                            DateTime AcceptTime = Convert.ToDateTime(dtpSampAcceptTime.Value);
                            DateTime dtCombinedAccept = new DateTime(AcceptDate.Year, AcceptDate.Month, AcceptDate.Day, AcceptTime.Hour, AcceptTime.Minute, AcceptTime.Second);



                            if (dtpSampAcceptDate.SelectedDate != null)
                            {
                                if (IsSampleReject == true)
                                {

                                    item.SampleRejectionDateTime = dtCombinedAccept;
                                    //for reveive flag true as per milann requirement
                                    item.SampleReceivedDateTime = dtCombinedAccept;
                                    item.IsSampleReceive = true;
                                    //item.IsFromSampleReceive = true;

                                    //
                                    if (!String.IsNullOrEmpty(txtRejectionRemark.Text))
                                    {
                                        item.Remark = txtRejectionRemark.Text;
                                    }
                                    if (!String.IsNullOrEmpty(txtAcceptedBy.Text))
                                    {
                                        item.AcceptedOrRejectedByName = txtAcceptedBy.Text;
                                        item.SampleReceiveBy = txtReceivedBy.Text.Trim();
                                    }
                                    if (chkResendSample.IsChecked == true)
                                    {
                                        item.IsResendForNewSample = true;
                                        item.IsSampleAccepted = false;
                                        item.IsRejected = true;
                                    }
                                    else
                                    {
                                        item.IsResendForNewSample = false;
                                        item.IsRejected = true;
                                        item.IsSampleAccepted = false;

                                    }
                                   // item.Remark = txtRejectionRemark.Text.Trim();
                                }
                                else
                                {
                                    item.SampleAcceptanceDateTime = dtCombinedAccept;
                                    item.IsSampleAccepted = true;
                                    //for reveive flag true as per milann requirement
                                    item.SampleReceivedDateTime = dtCombinedAccept;
                                    item.IsSampleReceive = true;
                                    //item.IsFromSampleReceive = true;

                                    //
                                    if (!String.IsNullOrEmpty(txtAcceptedBy.Text))
                                    {
                                        item.AcceptedOrRejectedByName = txtAcceptedBy.Text;
                                        item.SampleReceiveBy = txtReceivedBy.Text.Trim();
                                    }
                                    if (rbAccepted.IsChecked == true)
                                        item.IsAccepted = true;
                                    else
                                        item.IsAccepted = false;

                                    if (chkSubOptimal.IsChecked == true)
                                        item.IsSubOptimal = true;
                                    else
                                        item.IsSubOptimal = false;
                                    item.Remark = txtAcceptRemark.Text.Trim();
                                }
                                item.IsFromSampleAcceptReject = true;
                            }
                        }
                    }
                    if (OnSaveButtonClick != null)
                    {
                        OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
                    }
                    this.DialogResult = true;
                }
            }
        }

        //added by rohini dated 16.2.16 for name validation
        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSampleBy.Text = txtSampleBy.Text.ToTitleCase();
        }
        //private void txtDispatchBy_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtDispatchBy.Text = txtDispatchBy.Text.ToTitleCase();
        //}

        //private void txtAcceptedBy_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtAcceptedBy.Text = txtAcceptedBy.Text.ToTitleCase();
        //}
        private void txtReceivedBy_LostFocus(object sender, RoutedEventArgs e)
        {
            txtReceivedBy.Text = txtReceivedBy.Text.ToTitleCase();
        }

        //--------------------------------
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
            }
            this.DialogResult = false;
            //added for page check box get hand on cancel click
            //    this.DialogResult = false;
            //  Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkIsOutSourced_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)(((CheckBox)sender).IsChecked))
            {
                tblAgency.Visibility = Visibility.Visible;
                txtAgencyName.Visibility = Visibility.Visible;
            }
            else
            {
                txtAgencyName.Text = "";
                tblAgency.Visibility = Visibility.Collapsed;
                txtAgencyName.Visibility = Visibility.Collapsed;
            }
        }

        #region Send SMS and Email
        private void chkSendSMS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (chkSendEmail.IsChecked == true)
                SendRejectEmail = true;
            else
                SendRejectEmail = false;
        }
        #endregion
        string textBefore = "";
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }






    }
}
