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
using PalashDynamics.Animations;
using CIMS;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using System.Collections.ObjectModel;
using PalashDynamics.Pathology.ItemSearch;
using PalashDynamics.Pharmacy.ItemSearch;
using System.Windows.Browser;
using System.Windows.Data;
using System.ComponentModel;

namespace PalashDynamics.Pathology
{
    public partial class PathoResultEntry : UserControl, IInitiateCIMS
    {
        private SwivelAnimation objAnimation;
        bool IsPatientExist = false;
        public ObservableCollection<clsPathoTestItemDetailsVO> ItemList { get; set; }
        public ObservableCollection<clsPathoTestParameterVO> TestList { get; set; }
        List<MasterListItem> HelpValueList = new List<MasterListItem>();
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        string ReferredBy { get; set; }
        bool IsNumeric { get; set; }
        bool IsUpdate = false;

        public PathoResultEntry()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            dgTestList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgTestList_CellEditEnded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPathOrderBookingVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }
        
        #region Color Code
        void dgTestList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (((clsPathoTestParameterVO)dgTestList.SelectedItem).IsNumeric == true)
            {
                if (e.Column.DisplayIndex == 2)
                {
                    if (((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue != "")
                    {
                        if (!((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue.IsItNumber())
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Result Value in correct format.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW14.Show();
                            TabControlMain.SelectedIndex = 0;
                        }
                        else
                        {
                            Color C = new Color();
                            C.R = 198;
                            C.B = 24;
                            C.G = 15;
                            C.A = 255;
                            if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) < (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).DefaultValue)))
                            {

                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = new SolidColorBrush(C);

                            }
                            else if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) == (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).DefaultValue)))
                            {
                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = new SolidColorBrush(Colors.Green);
                            }
                            else if (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue) > (Convert.ToDouble(((clsPathoTestParameterVO)dgTestList.SelectedItem).DefaultValue)))
                            {
                                ((clsPathoTestParameterVO)dgTestList.SelectedItem).ApColor = new SolidColorBrush(C);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        public void Initiate(string Mode)
        {
            
            switch (Mode)
            {
                case "New":
                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Pathology work order is not selected.\nPlease Select a work order then click on Result Entry.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder != null && ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).SampleType == false)
                    {
                        MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Sample is not Collected for Selected Work Order.\nPlease select the Pathology Work Order whose Sample is Collected.", MessageBoxButtons.Ok, MessageBoxIcon.Warning);
                        msgbox.Show();
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPathOrderBookingVO)((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder).PatientName;
                   

                    break;
            }
        }

        private void ResultEntry_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
                    ((IApplicationConfiguration)App.Current).OpenMainContent(new PathologyWorkOrderGeneration());

                }
                else
                {
                    this.DataContext = new clsPathPatientReportVO();
                    FillOrderBookingList();
                    ItemList = new ObservableCollection<clsPathoTestItemDetailsVO>();
                    TestList = new ObservableCollection<clsPathoTestParameterVO>();
                    FillPathologist();
                    FillStore();
                    FillCategory();

                }
            }
            IsPageLoded = true;
            txtReferenceDoctor.Focus();
        }

        #region 'Paging'

        public PagedSortableCollectionView<clsPathOrderBookingVO> DataList { get; private set; }

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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillOrderBookingList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
      
        #endregion

        #region Fill Combobox
        private void FillDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    txtReferenceDoctor.ItemsSource = null;
                    txtReferenceDoctor.ItemsSource = objList;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillPathologist()
        {

            clsGetPathologistBizActionVO BizAction = new clsGetPathologistBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetPathologistBizActionVO)arg.Result).MasterList);

                    cmbPathologist1.ItemsSource = null;
                    cmbPathologist1.ItemsSource = objList;
                    cmbPathologist1.SelectedItem = objList[0];

                    cmbPathologist2.ItemsSource = null;
                    cmbPathologist2.ItemsSource = objList;
                    cmbPathologist2.SelectedItem = objList[0];


                    cmbPathologist3.ItemsSource = null;
                    cmbPathologist3.ItemsSource = objList;
                    cmbPathologist3.SelectedItem = objList[0];

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillStore()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;
                    cmbStore.SelectedItem = objList[0];

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_PathoParameterCategoryMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbCategory.ItemsSource = null;
                    cmbCategory.ItemsSource = objList;
                    cmbCategory.SelectedItem = objList[0];

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void txtReferenceDoctor_GotFocus(object sender, RoutedEventArgs e)
        {
            FillDoctor();
        }
        #endregion

        #region WorkOrder
        private void FillOrderBookingList()
        {
            if (!(((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder == null))
            {
                try
                {
                    DataList.Clear();
                    DataList.Add(((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder);
                    dgOrdertList.ItemsSource = null;
                    dgOrdertList.ItemsSource = DataList;

                    DataPager.Source = null;
                    DataPager.PageSize = 1;
                    DataPager.Source = DataList;

                    dgTest.ItemsSource = null;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private void FillOrderBookingDetailsList()
        {
            clsGetPathOrderBookingDetailListBizActionVO BizAction = new clsGetPathOrderBookingDetailListBizActionVO();
            BizAction.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.UnitID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                       
                        List<clsPathOrderBookingDetailVO> ObjList = new List<clsPathOrderBookingDetailVO>();

                        ObjList = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;
                        
                        var results = from r in ObjList
                                      where r.IsSampleCollected == true && r.IsFinalized ==false && r.IsCompleted==false
                                      select r;
                        if (results.ToList().Count > 0)
                        {
                            ObjList = results.ToList();

                            dgTest.ItemsSource = null;
                            dgTest.ItemsSource = ObjList;
                        }
                        else
                        {
                            dgTest.ItemsSource = null;
                        }
                        //dgTest.ItemsSource = ((clsGetPathOrderBookingDetailListBizActionVO)arg.Result).OrderBookingDetailList;

                      

                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        #region Get Data
        
        private void FillItemList()
        {
            try
            {
                clsGetPathoTestItemDetailsBizActionVO BizAction = new clsGetPathoTestItemDetailsBizActionVO();
                BizAction.ItemList = new List<clsPathoTestItemDetailsVO>();

                BizAction.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoTestItemDetailsBizActionVO)arg.Result).ItemList != null)
                        {
                            dgItemDetailsList.ItemsSource = ((clsGetPathoTestItemDetailsBizActionVO)arg.Result).ItemList;

                            List<clsPathoTestItemDetailsVO> ObjItem;
                            ObjItem = ((clsGetPathoTestItemDetailsBizActionVO)arg.Result).ItemList; ;
                            foreach (var item4 in ObjItem)
                            {
                                ItemList.Add(item4);
                            }
                            dgItemDetailsList.ItemsSource = null;
                            dgItemDetailsList.ItemsSource = ItemList;

                        }



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
            catch(Exception ex)
            {
                throw;
            }
        }

        private void FillTestList()
        {
            try
            {
                clsGetPathoTestDetailsForResultEntryBizActionVO BizAction = new clsGetPathoTestDetailsForResultEntryBizActionVO();
                BizAction.TestList = new List<clsPathoTestParameterVO>();

                BizAction.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoTestDetailsForResultEntryBizActionVO)arg.Result).TestList != null)
                        {
                           

                                List<clsPathoTestParameterVO> ObjTest;
                                ObjTest = ((clsGetPathoTestDetailsForResultEntryBizActionVO)arg.Result).TestList;
                                foreach (var item4 in ObjTest)
                                {
                                    IsNumeric = item4.IsNumeric;
                                    if(item4.FootNote!=null)
                                        txtFootNote.Text = item4.FootNote;
                                    if(item4.Note!=null)
                                        txtSuggestion.Text = item4.Note;
                                    TestList.Add(item4);

                                }

                                PagedCollectionView Collection = new PagedCollectionView(TestList);
                                Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));

                                dgTestList.ItemsSource = null;
                                dgTestList.ItemsSource = Collection;
                      

                            if (IsNumeric == false)
                            {
                                HelpValueList = ((clsGetPathoTestDetailsForResultEntryBizActionVO)arg.Result).HelpValueList;
                                dgTestList.Columns[4].Visibility = Visibility.Collapsed;
                                
                            }
                            else
                            {  
                                dgTestList.Columns[5].Visibility = Visibility.Collapsed;
                               dgTestList.Columns[4].Visibility = Visibility.Visible;
                           
                               
                            }
                            if (IsUpdate == true)
                            {
                                GetResultEntry();
                            }

                        }



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
            catch(Exception ex)
            {
                throw;
            }
        }

        private void GetResultEntry()
        {
            try
            {
        
            clsGetPathoResultEntryBizActionVO BizAction = new clsGetPathoResultEntryBizActionVO();
            BizAction.ResultEntryDetails=new clsPathPatientReportVO();
            BizAction.ID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
            BizAction.DetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;

            dgTestList.ItemsSource = null;
            dgItemDetailsList.ItemsSource = null;
            TestList = new ObservableCollection<clsPathoTestParameterVO>();
            ItemList = new ObservableCollection<clsPathoTestItemDetailsVO>();
             Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
             PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails != null)
                        {
                            clsPathPatientReportVO ObjDetails;
                            ObjDetails = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails;
                            if (ObjDetails != null)
                            {
                                this.DataContext = ObjDetails;
                                cmbPathologist1.SelectedValue = ObjDetails.PathologistID1;
                            }
                            if (ObjDetails.TestList != null)
                            {
                                List<clsPathoTestParameterVO> ObjTest;
                                ObjTest = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails.TestList;

                                foreach (var item5 in ObjTest)
                                {
                                    if(item5.CategoryID !=null )
                                        cmbCategory.SelectedValue = item5.CategoryID;
                                    IsNumeric = item5.IsNumeric;
                                  
                                    if (IsNumeric == true)
                                    {
                                        Color C = new Color();
                                        C.R = 198;
                                        C.B = 24;
                                        C.G = 15;
                                        C.A = 255;
                                        if (Convert.ToDouble(item5.ResultValue) < (Convert.ToDouble(item5.DefaultValue)))
                                        {

                                           item5.ApColor = new SolidColorBrush(C);

                                        }
                                        else if (Convert.ToDouble(item5.ResultValue) == (Convert.ToDouble(item5.DefaultValue)))
                                        {
                                            item5.ApColor = new SolidColorBrush(Colors.Green);
                                        }
                                        else if (Convert.ToDouble(item5.ResultValue) > (Convert.ToDouble(item5.DefaultValue)))
                                        {
                                            item5.ApColor = new SolidColorBrush(C);
                                        }
                                     
                                    }
                                    txtFootNote.Text = item5.FootNote;
                                    txtSuggestion.Text = item5.Note;
                                    TestList.Add(item5);
                                }
                                PagedCollectionView Collection = new PagedCollectionView(TestList);
                                Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));

                                dgTestList.ItemsSource = null;
                                dgTestList.ItemsSource = Collection;
                            }

                            if (ObjDetails.ItemList != null)
                            {
                                List<clsPathoTestItemDetailsVO> ObjItem;
                                ObjItem = ((clsGetPathoResultEntryBizActionVO)arg.Result).ResultEntryDetails.ItemList;
                                foreach (var item4 in ObjItem)
                                {
                                    ItemList.Add(item4);
                                }
                                dgItemDetailsList.ItemsSource = null;
                                dgItemDetailsList.ItemsSource = ItemList;
                            }
                        }
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
            catch(Exception ex)
            {
                throw;
            }    

        }
        
        private void dgOrdertList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTest.ItemsSource = null;
            if (dgOrdertList.SelectedItem != null)
            {
                if (((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID > 0)
                {
                    FillOrderBookingDetailsList();

                }
            }
        }
        
        #endregion

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Save");
            ClearData();
            if (dgTest.SelectedItem != null)
            {
                

                if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID > 0)
                {
                    if (((clsPathOrderBookingDetailVO)dgTest.SelectedItem).IsResultEntry == false)
                    {
                        txtSampleNo.Text = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SampleNo.ToString();
                        txtReferenceDoctor.Text = ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredBy;
                        tpCollectionTime.Value = (DateTime?)DateTime.Now;
                        cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyStoreID;
                        FillItemList();
                        TabControlMain.SelectedIndex = 0;
                        IsUpdate = false;
                        cmdGetTest.IsEnabled = true;

                    }
                    else
                    {
                        IsUpdate = true;
                        txtReferenceDoctor.Text = ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder.ReferredBy;
                        cmbStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyStoreID;
                        cmdGetTest.IsEnabled = false;
                        GetResultEntry();
                        TabControlMain.SelectedIndex = 0;

                    }

                    //Get test details 
                    //Commented on 25/11/2011
                    //FillTestList();
                }

                objAnimation.Invoke(RotationType.Forward);
            }
        }
                      
        #region Save Data


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                bool SaveTemplate = true;

                SaveTemplate = CheckValidation();

                if (SaveTemplate == true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to save the Result Entry?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                    ClickedFlag = 0;

            }
        }
        
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsUpdate == false)
                {
                    Save();
                }
                else
                {
                    Update();
                }
            }

            else
            {
                ClickedFlag = 0;
            }
        }
        
        private void Save()
        {
            clsAddPathPatientReportBizActionVO BizAction = new clsAddPathPatientReportBizActionVO();
            try
            {
                BizAction.OrderPathPatientReportList = (clsPathPatientReportVO)this.DataContext;
                BizAction.OrderPathPatientReportList.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.OrderPathPatientReportList.PathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                BizAction.OrderPathPatientReportList.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromPathology;

                BizAction.OrderPathPatientReportList.SampleCollectionTime = tpCollectionTime.Value;

                if (cmbPathologist1.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID;

                if (cmbPathologist2.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID2 = ((MasterListItem)cmbPathologist2.SelectedItem).ID;

                if (cmbPathologist3.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID3 = ((MasterListItem)cmbPathologist3.SelectedItem).ID;

                foreach (var item in TestList)
                {
                    item.ApColor = null;
                    item.FootNote = txtFootNote.Text;
                    item.Note = txtSuggestion.Text;
                }
                BizAction.OrderPathPatientReportList.TestList = TestList.ToList();
                BizAction.OrderPathPatientReportList.ItemList = ItemList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Load");
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        FillOrderBookingList();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Result Entry Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    if (((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.IsFinalized == true)
                                    {
                                        PrintReport(((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.ID);
                                    }
                                }
                            };
                        msgW1.Show();


                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Result Entry .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }

            catch (Exception ex)
            {
                throw;
            }

        }


        private void Update()
        {
            clsAddPathPatientReportBizActionVO BizAction = new clsAddPathPatientReportBizActionVO();
           
            try
            {
                BizAction.OrderPathPatientReportList = (clsPathPatientReportVO)this.DataContext;
                BizAction.OrderPathPatientReportList.OrderID = ((clsPathOrderBookingVO)dgOrdertList.SelectedItem).ID;
                BizAction.OrderPathPatientReportList.PathOrderBookingDetailID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).ID;
                BizAction.OrderPathPatientReportList.TestID = ((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID;
                BizAction.AutoDeductStock = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoDeductStockFromPathology;

                BizAction.OrderPathPatientReportList.SampleCollectionTime = tpCollectionTime.Value;

                if (cmbPathologist1.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID1 = ((MasterListItem)cmbPathologist1.SelectedItem).ID;

                if (cmbPathologist2.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID2 = ((MasterListItem)cmbPathologist2.SelectedItem).ID;

                if (cmbPathologist3.SelectedItem != null)
                    BizAction.OrderPathPatientReportList.PathologistID3 = ((MasterListItem)cmbPathologist3.SelectedItem).ID;

                foreach (var item in TestList)
                {
                    item.ApColor = null;
                    item.FootNote = txtFootNote.Text;
                    item.Note = txtSuggestion.Text;
                }
                BizAction.OrderPathPatientReportList.TestList = TestList.ToList();
                BizAction.OrderPathPatientReportList.ItemList = ItemList.ToList();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Load");
                    ClickedFlag = 0;

                    if (arg.Error == null)
                    {
                        FillOrderBookingList();
                        ClearData();
                        objAnimation.Invoke(RotationType.Backward);

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Result Entry Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.OK)
                            {
                                if (((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.IsFinalized == true)
                                {
                                    PrintReport(((clsAddPathPatientReportBizActionVO)arg.Result).OrderPathPatientReportList.ID);
                                }
                            }
                        };
                        msgW1.Show();


                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Result Entry .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }

            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion
        
        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;

                    break;
                case "New":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;

                    break;

                case "Save":


                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                case "Cancel":


                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;

                    break;

                default:
                    break;
            }

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
        }

        #endregion

        #region Add ITem Details

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).ItemID > 0 && ((MasterListItem)cmbStore.SelectedItem).ID > 0)
            {
                cmbStore.ClearValidationError();
                AssignBatch BatchWin = new AssignBatch();
                BatchWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                BatchWin.SelectedItemID = ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).ItemID;
                BatchWin.ItemName = ((clsPathoTestItemDetailsVO)dgItemDetailsList.SelectedItem).ItemName;

                BatchWin.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButton_Click);
                BatchWin.Show();
            }
            else
            {
                if (cmbStore.SelectedItem == null)
                {
                    cmbStore.TextBox.SetValidation("Please select the store");
                    cmbStore.TextBox.RaiseValidationError();

                }
                else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                {
                    cmbStore.TextBox.SetValidation("Please select the store");
                    cmbStore.TextBox.RaiseValidationError();

                }



            }
        }

        void OnAddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            AssignBatch AssignBatchWin = (AssignBatch)sender;

            if (AssignBatchWin.DialogResult == true)
            {
                if (AssignBatchWin.SelectedBatches != null)
                {
                    foreach (var item in AssignBatchWin.SelectedBatches)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsPathoTestItemDetailsVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,

                                    };
                        if (item1.ToList().Count == 0)
                        {

                            foreach (var BatchItems in ItemList.Where(x => x.ItemID == item.ItemID))
                            {
                                BatchItems.ItemID = item.ItemID;
                                BatchItems.ItemCode = item.ItemCode;
                                BatchItems.BatchID = item.BatchID;
                                BatchItems.BatchCode = item.BatchCode;
                                BatchItems.ExpiryDate = item.ExpiryDate;
                                BatchItems.BalanceQuantity = item.AvailableStock;
                                BatchItems.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                            }


                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();

                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                {
                    cmbStore.ClearValidationError();

                    ItemListNew ItemsWin = new ItemListNew();

                    // ItemsWin.StoreID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RadiologyStoreID;
                    ItemsWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    ItemsWin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                    ItemsWin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    ItemsWin.ShowBatches = true;
                    ItemsWin.cmbStore.IsEnabled = false;


                    ItemsWin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    ItemsWin.Show();
                }
                else
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();

                    }
                    else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
                    {
                        cmbStore.TextBox.SetValidation("Please select the store");
                        cmbStore.TextBox.RaiseValidationError();

                    }



                }


            }
            catch (Exception ex)
            {
                throw;
            }

        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemListNew Itemswin = (ItemListNew)sender;
            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.ItemBatchList != null)
                {
                    foreach (var item in Itemswin.ItemBatchList)
                    {
                        var item1 = from r in ItemList
                                    where (r.BatchID == item.BatchID)
                                    select new clsPathoTestItemDetailsVO
                                    {
                                        Status = r.Status,
                                        ItemID = r.ItemID,
                                        ItemName = r.ItemName,

                                    };
                        if (item1.ToList().Count == 0)
                        {

                            clsPathoTestItemDetailsVO objItem = new clsPathoTestItemDetailsVO();

                            objItem.ItemID = item.ItemID;
                            objItem.ItemCode = item.ItemCode;
                            objItem.ItemName = item.ItemName;
                            objItem.BatchID = item.BatchID;
                            objItem.BatchCode = item.BatchCode;
                            objItem.ExpiryDate = item.ExpiryDate;
                            objItem.BalanceQuantity = item.AvailableStock;
                            objItem.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

                            ItemList.Add(objItem);
                            dgItemDetailsList.ItemsSource = ItemList;
                            dgItemDetailsList.Focus();
                            dgItemDetailsList.UpdateLayout();
                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemDetailsList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ItemList.RemoveAt(dgItemDetailsList.SelectedIndex);
                        dgItemDetailsList.Focus();
                        dgItemDetailsList.UpdateLayout();
                        dgItemDetailsList.SelectedIndex = ItemList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }
        #endregion

        #region Clear UI
        /// <summary>
        /// Purpose:For Clear data.
        /// </summary>
        private void ClearData()
        {
            this.DataContext = new clsPathPatientReportVO();
            
            txtReferenceDoctor.Text = string.Empty;
            txtSampleNo.Text = string.Empty;
            tpCollectionTime.Value = null;
            cmbPathologist1.SelectedValue = (long)0;
            cmbPathologist2.SelectedValue = (long)0;
            cmbPathologist3.SelectedValue = (long)0;
            cmbCategory.SelectedValue = (long)0;
            cmbStore.SelectedValue = (long)0;
            ItemList = new ObservableCollection<clsPathoTestItemDetailsVO>();
            TestList = new ObservableCollection<clsPathoTestParameterVO>();
            dgTestList.ItemsSource = null;
            IsUpdate = false;


        }

        #endregion

        #region validation
        /// <summary>
        /// Purpose:Assign validation to controls.
        /// </summary>
        /// <returns></returns>
        private bool CheckValidation()
        {
            bool result = true;


            if (txtSampleNo.Text == "")
            {
                txtSampleNo.SetValidation("Please enter Sample no.");
                txtSampleNo.RaiseValidationError();
                txtSampleNo.Focus();
                TabControlMain.SelectedIndex = 0;
                result = false;

            }
            else
                txtSampleNo.ClearValidationError();
             

            
            if (txtReferenceDoctor.Text == "")
            {
                txtReferenceDoctor.SetValidation("Please enter Referred By");
                txtReferenceDoctor.RaiseValidationError();
                TabControlMain.SelectedIndex = 0;
                result = false;
                txtReferenceDoctor.Focus();

            }
            else
                txtReferenceDoctor.ClearValidationError();


            if (IsPageLoded)
            {

                if (ItemList.Where(Items => Items.ActualQantity == 0).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Actual Qantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW14.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;
                }

                if (TestList.Where(Tests =>  Tests.ResultValue == null).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW15 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Result Value.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW15.Show();
                    TabControlMain.SelectedIndex = 1;
                    result = false;
                    return result;
                }

                if (ItemList.Where(Items => Items.BatchID == 0).Any() == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please assign Batch.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW13.Show();
                    TabControlMain.SelectedIndex = 1;
                    result = false;
                    return result;
                }

                if (IsNumeric == true)
                {
                    if (!((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue.IsItNumber())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW15=
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter Result Value in correct format.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW15.Show();
                        TabControlMain.SelectedIndex = 0;
                        result = false;
                        return result;
                    }
                }

                if (dgTestList.ItemsSource==null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can not save result entry  without test Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW13.Show();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    return result;
                }




            }

            return result;
        }


  
        #endregion

        #region Get Help Value
        private void HelpValueDetails_Click(object sender, RoutedEventArgs e)
        {
            
                HelpValueDetails WinHelpValue = new HelpValueDetails();
                WinHelpValue.ParameterID = ((clsPathoTestParameterVO)dgTestList.SelectedItem).ParameterID;
            
                //WinHelpValue.HelpValueList = HelpValueList;
                WinHelpValue.OnSaveButton_Click += new RoutedEventHandler(WinHelpValue_OnSaveButton_Click);
                WinHelpValue.Show();
            
        }
        
        void WinHelpValue_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            HelpValueDetails ObjHelpValueDetails = (HelpValueDetails)sender;


            if (ObjHelpValueDetails.SelectedItems != null)
            {
                foreach (var item in ObjHelpValueDetails.SelectedItems)
                {
                    if (TestList.Where(Items => Items.ParameterID == item.ParameterID).Any() == true)
                    {
                        ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue = item.HelpValue ;
                    }
                }

              
            }




            //if (ObjHelpValueDetails.DialogResult == true)
            //{
            //    if (ObjHelpValueDetails.cmbHelpValue.SelectedItem != null)
            //    {
            //        if (((MasterListItem)((ObjHelpValueDetails).cmbHelpValue.SelectedItem)).ID != 0)
            //        {

            //            ((clsPathoTestParameterVO)dgTestList.SelectedItem).ResultValue = ((MasterListItem)((ObjHelpValueDetails).cmbHelpValue.SelectedItem)).Description;
            //            ((clsPathoTestParameterVO)dgTestList.SelectedItem).HelpValue = ((MasterListItem)((ObjHelpValueDetails).cmbHelpValue.SelectedItem)).Description;
            //            if (((MasterListItem)((ObjHelpValueDetails).cmbHelpValue.SelectedItem)).Status == true)
            //            {
            //                ((clsPathoTestParameterVO)dgTestList.SelectedItem).DefaultValue = ((MasterListItem)((ObjHelpValueDetails).cmbHelpValue.SelectedItem)).Description;
            //            }
            //            else
            //            {
            //                ((clsPathoTestParameterVO)dgTestList.SelectedItem).DefaultValue = null;
            //            }
            //        }
            //    }
            //}
        }

#endregion

        /// <summary>
        /// Purpose:To print report.
        /// </summary>
        /// <param name="ResultID"></param>
        private void PrintReport(long ResultID)
        {
            if (ResultID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/Pathology/PathoResultEntry.aspx?ID=" + ResultID + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void cmdGetTest_Click(object sender, RoutedEventArgs e)
        {
            dgTestList.ItemsSource = null;
            TestList = new ObservableCollection<clsPathoTestParameterVO>();
            GetParameters();
        }

        private void GetParameters()
        {
            clsGetPathoTestParameterAndSubTestDetailsBizActionVO BizAction = new clsGetPathoTestParameterAndSubTestDetailsBizActionVO();
            try
            {
                BizAction.TestList = new List<clsPathoTestParameterVO>();
                BizAction.TestID = Convert.ToString(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).TestID);

                if(cmbCategory.SelectedItem !=null)
                    BizAction.CategoryID = ((MasterListItem)cmbCategory.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        dgTestList.ItemsSource = null;
                        if (((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList != null)
                        {


                            List<clsPathoTestParameterVO> TestDetails;
                            TestDetails = ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).TestList;
                           
                            foreach (var item in TestDetails)
                            {
                                TestList.Add(item);

                            }

                            PagedCollectionView Collection = new PagedCollectionView(TestList);
                            Collection.GroupDescriptions.Add(new PropertyGroupDescription("PathoTestName"));

                            dgTestList.ItemsSource = null;
                            dgTestList.ItemsSource = Collection;

                            if (((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).FootNote != null && ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).FootNote != "")
                                txtFootNote.Text = ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).FootNote;
                            if (((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).Note != null && ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).Note != "")
                                txtSuggestion.Text = ((clsGetPathoTestParameterAndSubTestDetailsBizActionVO)arg.Result).Note;
                            
                        }

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
            catch (Exception ex)
            {
                throw;
            }


        }
    }


    
}
