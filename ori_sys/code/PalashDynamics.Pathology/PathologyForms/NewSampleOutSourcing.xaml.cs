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
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Pathology;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewSampleOutSourcing : UserControl
    {
        #region Variables And List Declarions
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public PagedSortableCollectionView<clsPathoTestOutSourceDetailsVO> MasterList { get; private set; }
        public int DataListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }
        PagedCollectionView collection;
        ChangeTestAgency ChangeAgency;
        clsPathoTestOutSourceDetailsVO objOutSourceTestDetails;
        public List<clsPathoTestOutSourceDetailsVO> UnAssignedAgnecyTestList;
        public List<clsChangePathoTestAgencyBizActionVO> SelectedTestDetails = new List<clsChangePathoTestAgencyBizActionVO>();
        private bool IsAssignAgency = false;
        private bool IsOutsourcedFlag = false;
        #endregion

        #region Constructor
        public NewSampleOutSourcing()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(NewOutSourceTest_Loaded);
            /*==========================================================*/
            MasterList = new PagedSortableCollectionView<clsPathoTestOutSourceDetailsVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            DataListPageSize = 15;

            DataPager.PageSize = DataListPageSize;
            DataPager.Source = MasterList;

            this.DataPager.DataContext = MasterList;
            dgOutSourcedTestList.DataContext = MasterList;
            /*==========================================================*/
            this.dtpFromDate.SelectedDate = DateTime.Now;
            this.dtpToDate.SelectedDate = DateTime.Now;
            UnAssignedAgnecyTestList = new List<clsPathoTestOutSourceDetailsVO>();
        }
        #endregion

        #region Loaded and Refresh Events
        private void NewOutSourceTest_Loaded(object sender, RoutedEventArgs e)
        {
            FillAgency();
            FillTestList();
            dgOutSourcedTestList.Columns[0].Visibility = Visibility.Collapsed;
            dtpFromDate.Focus();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillTestList();
        }
        #endregion

        #region ToggleButton Click Events
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (IsAssignAgency == true && UnAssignedAgnecyTestList.Count > 0)
            {
                var item = from r in UnAssignedAgnecyTestList
                           where r.IsSampleCollected == false
                           select r;
                if (item != null && item.ToList().Count == 0)
                {
                    ChangeAgency = new ChangeTestAgency();
                    ChangeAgency.IsAssignAgency = true;
           
                   
                    ChangeAgency.AssignAgencyToTestList = UnAssignedAgnecyTestList;
                    ChangeAgency.OnSaveButtonClick += new RoutedEventHandler(OnSaveButtonClick);
                    ChangeAgency.OnCancelButtonClick += new RoutedEventHandler(OnCancelButtonClick);
                    ChangeAgency.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST WHOSE SAMPLE IS COLLECTED.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else if (IsAssignAgency == false && dgOutSourcedTestList.SelectedItem != null)
            {
                if (((clsPathoTestOutSourceDetailsVO)dgOutSourcedTestList.SelectedItem).IsSampleCollected)
                {
                    objOutSourceTestDetails = dgOutSourcedTestList.SelectedItem as clsPathoTestOutSourceDetailsVO;
                    ChangeAgency = new ChangeTestAgency();
                    ChangeAgency.IsAssignAgency = false;
                    ChangeAgency.TestOutSourceDetails = objOutSourceTestDetails;
                    ChangeAgency.OnSaveButtonClick += new RoutedEventHandler(OnSaveButtonClick);
                    ChangeAgency.OnCancelButtonClick += new RoutedEventHandler(OnCancelButtonClick);
                    ChangeAgency.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST WHOSE SAMPLE IS COLLECTED.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            FillTestList();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            MasterList.Clear();
            FillTestList();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillTestList();
        }
        #endregion

        #region Check Box Click and Selection Changed Events
        private void chkSelect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgOutSourcedTestList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgOutSourcedTestList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Collapsed;
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            docHeader.Visibility = Visibility.Visible;
        }

        private void PART_MaximizeToggle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkBox_Click(object sender, RoutedEventArgs e)
        {
            if (chkBox.IsChecked == true)
            {
                dgOutSourcedTestList.Columns[0].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[4].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[5].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[6].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[7].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[8].Visibility = Visibility.Collapsed;
                cmdModify.MinWidth = 100;
                cmdModify.Content = "Assign Agency";
                IsAssignAgency = true;
            }
            else
            {
                dgOutSourcedTestList.Columns[0].Visibility = Visibility.Collapsed;
                dgOutSourcedTestList.Columns[4].Visibility = Visibility.Visible;
                dgOutSourcedTestList.Columns[5].Visibility = Visibility.Visible;
                dgOutSourcedTestList.Columns[6].Visibility = Visibility.Visible;
                dgOutSourcedTestList.Columns[7].Visibility = Visibility.Visible;
                dgOutSourcedTestList.Columns[8].Visibility = Visibility.Visible;
                cmdModify.MinWidth = 50;
                cmdModify.Content = "Modify";
                IsAssignAgency = false;
            }
            FillTestList();
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            IsAssignAgency = true;
            clsPathoTestOutSourceDetailsVO objVO = dgOutSourcedTestList.SelectedItem as clsPathoTestOutSourceDetailsVO;
            if (chk.IsChecked == true)
            {
              //  UnAssignedAgnecyTestList.Clear();
                FillUnAssignedAgencyTestList();
                if (dgOutSourcedTestList.SelectedItem != null)
                {
                    try
                    {

                        if (chk.IsChecked == true)
                        {
                            // Added By Anumani on 14.04.2016
                            UnAssignedAgnecyTestList.Add(objVO);

                            var item1 = from r in UnAssignedAgnecyTestList
                                        where r.IsSampleCollected == false
                                        select r;
                            if (item1 != null && item1.ToList().Count == 0)
                            {
                                //  UnAssignedAgnecyTestList.Add(objVO);




                                clsChangePathoTestAgencyBizActionVO BizAction = new clsChangePathoTestAgencyBizActionVO();
                                BizAction.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();

                                foreach (var item in UnAssignedAgnecyTestList)
                                {
                                    BizAction.OutSourceID = BizAction.OutSourceID + item.ID;
                                    BizAction.OutSourceID = BizAction.OutSourceID + ",";


                                }


                                if (BizAction.OutSourceID.EndsWith(","))
                                    BizAction.OutSourceID = BizAction.OutSourceID.Remove(BizAction.OutSourceID.Length - 1, 1);
                                BizAction.IsOutsource = true;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Error == null)
                                    {
                                        if (arg.Result != null)
                                        {
                                            MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Service OutSourced Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                            msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                            {
                                                //if (res == MessageBoxResult.OK)
                                                //{
                                                //    this.DialogResult = false;
                                                //    if (OnSaveButtonClick != null)
                                                //        OnSaveButtonClick(this, new RoutedEventArgs());
                                                //}
                                            };
                                            msgbox.Show();
                                        }
                                    }
                                };
                                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST WHOSE SAMPLE IS COLLECTED.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                chk.IsChecked = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        //this.DialogResult = false;
                    }
                }
            }
            else
            {
                UnAssignedAgnecyTestList.Clear();
                MasterList.Clear();
                FillTestList();
            }
        }

        private void chk_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Are You Sure You Want To Outsource The Test.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
          //  mgbx.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed1);
            mgbx.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        UnAssignedAgnecyTestList.Clear();
                        CheckBox chk = sender as CheckBox;
                        IsAssignAgency = true;
                        clsPathoTestOutSourceDetailsVO objVO = dgOutSourcedTestList.SelectedItem as clsPathoTestOutSourceDetailsVO;
                        if (dgOutSourcedTestList.SelectedItem != null)
                        {
                            try
                            {

                                if (chk.IsChecked == true)
                                {
                                    // Added By Anumani on 14.04.2016
                                    UnAssignedAgnecyTestList.Add(objVO);

                                    var item1 = from r in UnAssignedAgnecyTestList
                                                where r.IsSampleCollected == false
                                                select r;
                                    if (item1 != null && item1.ToList().Count == 0)
                                    {
                                        //  UnAssignedAgnecyTestList.Add(objVO);




                                        clsChangePathoTestAgencyBizActionVO BizAction = new clsChangePathoTestAgencyBizActionVO();
                                        BizAction.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();

                                        foreach (var item in UnAssignedAgnecyTestList)
                                        {
                                            BizAction.OutSourceID = BizAction.OutSourceID + item.ID;
                                            BizAction.OutSourceID = BizAction.OutSourceID + ",";
                                            BizAction.PathoOutSourceTestDetails.TestID = item.TestID;


                                        }

                                      
                                        if (BizAction.OutSourceID.EndsWith(","))
                                            BizAction.OutSourceID = BizAction.OutSourceID.Remove(BizAction.OutSourceID.Length - 1, 1);
                                        BizAction.IsOutsource = true;

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                        client.ProcessCompleted += (s, arg) =>
                                        {
                                            if (arg.Error == null)
                                            {
                                                if (arg.Result != null)
                                                {
                                                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Service OutSourced Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                                    //msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                                    //{
                                                    //    //if (res == MessageBoxResult.OK)
                                                    //    //{
                                                    //    //    this.DialogResult = false;
                                                    //    //    if (OnSaveButtonClick != null)
                                                    //    //        OnSaveButtonClick(this, new RoutedEventArgs());
                                                    //    //}
                                                    //};
                                                    msgbox.Show();
                                                    chk.IsEnabled = false;
                                                }
                                            }
                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST WHOSE SAMPLE IS COLLECTED.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                        chk.IsChecked = false;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                //this.DialogResult = false;
                            }
                        }
                        else
                        {
                            clsPathoTestOutSourceDetailsVO obj;
                            obj = UnAssignedAgnecyTestList.Where(z => z.TestID == objVO.TestID).FirstOrDefault();
                            UnAssignedAgnecyTestList.Remove(obj);
                        }
                    }
                };

            mgbx.Show();



          
            
        }
        #endregion


        //void msgW_OnMessageBoxClosed1(MessageBoxResult result)
        //{
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        UnAssignedAgnecyTestList.Clear();
        //        CheckBox chk = sender as CheckBox;
        //        IsAssignAgency = true;
        //        clsPathoTestOutSourceDetailsVO objVO = dgOutSourcedTestList.SelectedItem as clsPathoTestOutSourceDetailsVO;
        //        if (dgOutSourcedTestList.SelectedItem != null)
        //        {
        //            try
        //            {

        //                if (chk.IsChecked == true)
        //                {
        //                    // Added By Anumani on 14.04.2016
        //                    UnAssignedAgnecyTestList.Add(objVO);

        //                    var item1 = from r in UnAssignedAgnecyTestList
        //                                where r.IsSampleCollected == false
        //                                select r;
        //                    if (item1 != null && item1.ToList().Count == 0)
        //                    {
        //                        //  UnAssignedAgnecyTestList.Add(objVO);




        //                        clsChangePathoTestAgencyBizActionVO BizAction = new clsChangePathoTestAgencyBizActionVO();
        //                        BizAction.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();

        //                        foreach (var item in UnAssignedAgnecyTestList)
        //                        {
        //                            BizAction.OutSourceID = BizAction.OutSourceID + item.ID;
        //                            BizAction.OutSourceID = BizAction.OutSourceID + ",";


        //                        }


        //                        if (BizAction.OutSourceID.EndsWith(","))
        //                            BizAction.OutSourceID = BizAction.OutSourceID.Remove(BizAction.OutSourceID.Length - 1, 1);
        //                        BizAction.IsOutsource = true;

        //                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //                        client.ProcessCompleted += (s, arg) =>
        //                        {
        //                            if (arg.Error == null)
        //                            {
        //                                if (arg.Result != null)
        //                                {
        //                                    MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Service OutSourced Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        //                                    msgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                                    {
        //                                        //if (res == MessageBoxResult.OK)
        //                                        //{
        //                                        //    this.DialogResult = false;
        //                                        //    if (OnSaveButtonClick != null)
        //                                        //        OnSaveButtonClick(this, new RoutedEventArgs());
        //                                        //}
        //                                    };
        //                                    msgbox.Show();
        //                                }
        //                            }
        //                        };
        //                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                    }
        //                    else
        //                    {
        //                        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                               new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE, SELECT TEST WHOSE SAMPLE IS COLLECTED.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                        msgW1.Show();
        //                        chk.IsChecked = false;
        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //            finally
        //            {
        //                //this.DialogResult = false;
        //            }
        //        }
        //        else
        //        {
        //            clsPathoTestOutSourceDetailsVO obj;
        //            obj = UnAssignedAgnecyTestList.Where(z => z.TestID == objVO.TestID).FirstOrDefault();
        //            UnAssignedAgnecyTestList.Remove(obj);
        //        }
            

        //    }
        //}

        #region Private Methods
        private void FillTestList()
        {
            try
            {
                clsGetPathoOutSourceTestListBizActionVO bizActionObj = new clsGetPathoOutSourceTestListBizActionVO();
                bizActionObj.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();
                bizActionObj.PathoOutSourceTestList = new List<clsPathoTestOutSourceDetailsVO>();
                if (dtpFromDate.SelectedDate != null)
                    bizActionObj.PathoOutSourceTestDetails.FromDate = dtpFromDate.SelectedDate;
                if (dtpToDate.SelectedDate != null)
                    bizActionObj.PathoOutSourceTestDetails.ToDate = dtpToDate.SelectedDate;
                if (!String.IsNullOrEmpty(txtFrontFirstName.Text))
                    bizActionObj.PathoOutSourceTestDetails.FirstName = txtFrontFirstName.Text;
                if (!String.IsNullOrEmpty(txtFrontLastName.Text))
                    bizActionObj.PathoOutSourceTestDetails.LastName = txtFrontLastName.Text;
                if (!String.IsNullOrEmpty(txtTestName.Text))
                    bizActionObj.PathoOutSourceTestDetails.TestName = txtTestName.Text;
                if (!String.IsNullOrEmpty(txtFrontMRNO.Text))
                    bizActionObj.PathoOutSourceTestDetails.MRNo = txtFrontMRNO.Text;
                if (cmbAgency.SelectedItem != null && ((MasterListItem)cmbAgency.SelectedItem).ID > 0)
                    bizActionObj.PathoOutSourceTestDetails.AgencyID = ((MasterListItem)cmbAgency.SelectedItem).ID;
                if (chkBox.IsChecked == true)
                    bizActionObj.PathoOutSourceTestDetails.OutSourceType = false;
                else
                    bizActionObj.PathoOutSourceTestDetails.OutSourceType = true;
                bizActionObj.IsPagingEnabled = true;
                bizActionObj.StartIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionObj.MaximumRows = MasterList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetPathoOutSourceTestListBizActionVO result = args.Result as clsGetPathoOutSourceTestListBizActionVO;
                        MasterList.Clear();
                        if (result.PathoOutSourceTestList != null && result.PathoOutSourceTestList.Count > 0)
                        {
                            MasterList.TotalItemCount = (int)((clsGetPathoOutSourceTestListBizActionVO)args.Result).TotalRows;
                            foreach (clsPathoTestOutSourceDetailsVO item in result.PathoOutSourceTestList)
                            {
                                if (item.IsChangedAgency == true)
                                    item.AgencyChangedImage = "../Icons/tick.png";
                                else
                                    item.AgencyChangedImage = "../Icons/error.png";
                                if (item.IsOutSourced == true)//&& String.IsNullOrEmpty(item.AgencyAssignReason))
                                    item.AgencyAssignedImage = "../Icons/tick.png";
                                else
                                    item.AgencyAssignedImage = "../Icons/error.png";
                                if (item.IsSampleCollected)
                                    item.SampleCollectedImage = "../Icons/tick.png";
                                else
                                    item.SampleCollectedImage = "../Icons/error.png";

                           //     item.IsOutSourced = false;
                                if (item.IsOutSourced1 == true)
                                {
                                    item.IsForOutSourceChecked = false;
                                }
                                MasterList.Add(item);
                            }
                            if (UnAssignedAgnecyTestList != null && UnAssignedAgnecyTestList.Count > 0)
                            {
                                foreach (clsPathoTestOutSourceDetailsVO item in UnAssignedAgnecyTestList)
                                {
                                    foreach (clsPathoTestOutSourceDetailsVO item1 in MasterList)
                                    {
                                        if (item.TestID == item1.TestID && item.PatientID == item1.PatientID)
                                        {
                                            item1.IsOutSourced = true;
                                        }
                                    }
                                }
                            }
                            dgOutSourcedTestList.ItemsSource = null;
                            collection = new PagedCollectionView(MasterList);
                            MasterList.ToList().Where(x => x.SampleDispatchDateTime.Equals(DateTime.MinValue)).ToList().ForEach(x => { x.SampleDispatchDateTime = null; });
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("PatientName"));
                            dgOutSourcedTestList.ItemsSource = collection;
                            DataPager.Source = null;
                            DataPager.PageSize = MasterList.PageSize;
                            DataPager.Source = MasterList;
                            txtTotalCountRecords.Text = "";
                            txtTotalCountRecords.Text = MasterList.TotalItemCount.ToString();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return;
        }

        private void FillAgency()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
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
                    cmbAgency.ItemsSource = null;
                    cmbAgency.ItemsSource = objList.DeepCopy();
                    cmbAgency.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillUnAssignedAgencyTestList()
        {
            try
            {
                clsGetPathoOutSourceTestListBizActionVO bizActionObj = new clsGetPathoOutSourceTestListBizActionVO();
                bizActionObj.PathoOutSourceTestDetails = new clsPathoTestOutSourceDetailsVO();
                bizActionObj.PathoOutSourceTestDetails.IsForUnassignedAgencyTest = true;
                bizActionObj.UnAssignedAgnecyTestList = new List<clsPathoTestOutSourceDetailsVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetPathoOutSourceTestListBizActionVO result = args.Result as clsGetPathoOutSourceTestListBizActionVO;
                        if (result.UnAssignedAgnecyTestList != null && result.UnAssignedAgnecyTestList.Count > 0)
                        {
                            UnAssignedAgnecyTestList = result.UnAssignedAgnecyTestList;
                        }
                        FillTestList();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "ERROR OCCURRED WHILE PROCESSING.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return;
        }

        #endregion



        public bool DialogResult { get; set; }
    }
}
