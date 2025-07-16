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
using PalashDynamics.ValueObjects.Pathology;
using CIMS;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pathology
{
    public partial class SampleCollectionTimeChildWindow : ChildWindow
    {
        public SampleCollectionTimeChildWindow()
        {
            InitializeComponent();
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
            this.Loaded += new RoutedEventHandler(SampleCollectionTimeChildWindow_Loaded);
        }
        
        public event RoutedEventHandler OnSaveButtonClick;

        public event RoutedEventHandler OnCancelButtonClick;
        
        #region Public Variables
        public string msgText;
        public string msgTitle;
        clsUserVO UserVo = new clsUserVO();
        public List<clsPathOrderBookingDetailVO> SelectedTests { get; set; }
        #endregion

        #region Variables
        List<long> TestID = new List<long>();
        //public ObservableCollection<clsPathoTestItemDetailsVO> ItemList
        //{
        //    get;
        //    set;
        //}
        #endregion

        void SampleCollectionTimeChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dtpSampColleDate.SelectedDate = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplicationDateTime; //DateTime.Now;
            dtpSampReceDate.SelectedDate = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplicationDateTime; //DateTime.Now;
            dtpSampCollTime.Value = DateTime.Now;
            dtpSampReceTime.Value = DateTime.Now;
            chkIsOutSourced.IsChecked = true;            
            txtCollCenter.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName;
            FillAgencyApplicableUnitList();

            TestID = new List<long>();

            if (SelectedTests != null && SelectedTests.Count > 0)
            {
                foreach (var item in SelectedTests)
                {
                    if (item.TestID > 0)
                        TestID.Add(item.TestID);
                }
                if (TestID != null && TestID.Count > 0)
                {
                    //FillItemList();
                }
            }
            //FillCollectionCenter();
            //ItemList = new ObservableCollection<clsPathoTestItemDetailsVO>();

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
                    //List<clsServiceAgencyMasterVO> objList = new List<clsServiceAgencyMasterVO>();
                    //// objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    //objList.Add(new clsServiceAgencyMasterVO { ID = 0, AgencyName = "- Select -" });  //(0, "- Select -"));
                    //objList.AddRange(((clsGetPathoAgencyApplicableUnitListBizActionVO)args.Result).ServiceAgencyMasterDetails);
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
                        txtAgencyName.SelectedItem = ((MasterListItem)result.First());
                    }
                    else
                    {
                        txtAgencyName.SelectedItem = objList[0];
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
                if (Convert.ToDateTime(dtpSampColleDate.SelectedDate) > DateTime.Now || dtpSampColleDate.SelectedDate==null)
                {
                    msgText = " Please Select Proper Sample Collection Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampColleDate.Focus();
                    result = false;
                }
                else if (Convert.ToDateTime(dtpSampReceDate.SelectedDate) > DateTime.Now || dtpSampReceDate.SelectedDate==null)
                {
                    msgText = " Please Select Proper Sample Received Date.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampReceDate.Focus();
                    result = false;
                }

                else if (Convert.ToDateTime(dtpSampCollTime.Value) > DateTime.Now || dtpSampCollTime.Value==null)
                {
                    msgText = " Please Select Proper Sample Collection Time.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampCollTime.Focus();
                    result = false;
                }
                else if (Convert.ToDateTime(dtpSampReceTime.Value) > DateTime.Now || dtpSampReceTime.Value==null)
                {
                    msgText = " Please Select Proper Sample Received Time.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampReceTime.Focus();
                    result = false;
                }

                if (chkIsOutSourced.IsChecked == true)
                {
                    if (((MasterListItem)txtAgencyName.SelectedItem).ID == 0)
                    {
                        txtAgencyName.TextBox.SetValidation("Agency Name is required");
                        txtAgencyName.TextBox.RaiseValidationError();
                        txtAgencyName.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)txtAgencyName.SelectedItem) == null)
                    {
                        txtAgencyName.TextBox.SetValidation("Agency Name is required");
                        txtAgencyName.TextBox.RaiseValidationError();
                        txtAgencyName.Focus();
                        result = false;
                    }
                    else
                        txtAgencyName.TextBox.ClearValidationError();
                }

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkval())
            {
                //string strcollDateTime = Convert.ToDateTime(dtpSampColleDate.SelectedDate).ToString("MM/dd/yyyy") + "" + Convert.ToDateTime(dtpSampCollTime.Value).ToString("hh:mm:ss");
                //string strrecDateTime =Convert.ToDateTime(dtpSampReceDate.SelectedDate).ToString("MM/dd/yyyy") + "" + Convert.ToDateTime(dtpSampReceTime.Value).ToString("hh:mm:ss");   
                //DateTime collDateTime = Convert.ToDateTime(Convert.ToDateTime(dtpSampColleDate.SelectedDate).ToString("MM/dd/yyyy") + "" + Convert.ToDateTime(dtpSampCollTime.Value).ToString("hh:mm:ss"));
                //DateTime recDateTime = Convert.ToDateTime(Convert.ToDateTime(dtpSampReceDate.SelectedDate).ToString("MM/dd/yyyy") + "" + Convert.ToDateTime(dtpSampReceTime.Value).ToString("hh:mm:ss"));   
                DateTime collDate = Convert.ToDateTime(dtpSampColleDate.SelectedDate);
                DateTime recDate =Convert.ToDateTime(dtpSampReceDate.SelectedDate);
                DateTime collTime =Convert.ToDateTime(dtpSampCollTime.Value);
                DateTime recTime =  Convert.ToDateTime(dtpSampReceTime.Value);

                DateTime dtCombinedColl = new DateTime(collDate.Year, collDate.Month, collDate.Day, collTime.Hour, collTime.Minute, collTime.Second);
                DateTime dtCombinedrec = new DateTime(recDate.Year, recDate.Month, recDate.Day, recTime.Hour, recTime.Minute, recTime.Second);
                
                clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)this.DataContext;
                if (dtpSampColleDate.SelectedDate != null)
                    item.SampleCollectedDateTime = dtCombinedColl;

                if (dtpSampReceDate.SelectedDate != null)
                    item.SampleReceivedDateTime = dtCombinedrec;

                if (txtAgencyName.SelectedItem != null && chkIsOutSourced.IsChecked == true)
                {
                    item.IsOutSourced = true;
                    item.AgencyID = ((MasterListItem)txtAgencyName.SelectedItem).ID;
                    item.AgencyName = ((MasterListItem)txtAgencyName.SelectedItem).Description;
                }
                if (OnSaveButtonClick != null)
                {
                    OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
                }
                this.DialogResult = true; 
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
            }
            this.DialogResult = false;
        }
        
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkIsOutSourced_Click(object sender, RoutedEventArgs e)
        {
            //if ((bool)(((CheckBox)sender).IsChecked))
            //{
            //    tblAgency.Visibility = Visibility.Visible;
            //    txtAgencyName.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    txtAgencyName.Text = "";
            //    tblAgency.Visibility = Visibility.Collapsed;
            //    txtAgencyName.Visibility = Visibility.Collapsed;
            //}
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
