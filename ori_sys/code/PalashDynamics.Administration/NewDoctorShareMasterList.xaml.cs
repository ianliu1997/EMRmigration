using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Data;
using PalashDynamics.Collections;
using System.Text;
namespace PalashDynamics.Administration
{
    public partial class NewDoctorShareMasterList : UserControl
    {
        public bool calcshare = false;
        public bool IsCancel = true;
        StringBuilder docnames = new StringBuilder();
        List<long> DIDs = new List<long>();
        public NewDoctorShareMasterList()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsDoctorVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            objAnimation = new SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            FrontPanelDataList = new PagedSortableCollectionView<clsDoctorShareServicesDetailsVO>();
            FrontPanelDataList.OnRefresh += new EventHandler<RefreshEventArgs>(FrontPanelDataList_OnRefresh);
            DataListPageSizeSer = 15;

        }
        void FrontPanelDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
        public int DataListPageSizeSer
        {
            get
            {
                return FrontPanelDataList.PageSize;
            }
            set
            {
                if (value == FrontPanelDataList.PageSize) return;
                FrontPanelDataList.PageSize = value;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillDoctor();
        }
        private SwivelAnimation objAnimation;
        public PagedSortableCollectionView<clsDoctorShareServicesDetailsVO> DataListService { get; private set; }
        public PagedSortableCollectionView<clsDoctorShareServicesDetailsVO> FrontPanelDataList { get; private set; }
        public PagedSortableCollectionView<clsDoctorVO> DataList { get; private set; }
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
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            objAnimation.Invoke(RotationType.Backward);
            FillTariff();
            FillSpecialization();
            FillSubSpecialization();
            FillModality();
            FetchData();
          //  FillDoctor();

        }
        List<clsDoctorShareServicesDetailsVO> AllDoctorShareList = new List<clsDoctorShareServicesDetailsVO>();
        void FetchAllDoctorShare()
        {
            WaitIndicator Pageload = new WaitIndicator();
            Pageload.Show();
            clsGetDoctorShare1DetailsBizActionVO BizActionVO = new clsGetDoctorShare1DetailsBizActionVO();
            BizActionVO.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();

            BizActionVO.FromDoctorShareChildWindow = false;

            BizActionVO.ForAllDoctorShareRecord = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    clsGetDoctorShare1DetailsBizActionVO result = args.Result as clsGetDoctorShare1DetailsBizActionVO;

                    if (result.DoctorShareInfoGetList != null)
                    {
                        foreach (var item in result.DoctorShareInfoGetList)
                        {

                            AllDoctorShareList.Add(item);
                        }
                    }

                }
                Pageload.Close();

            };

            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();
        }
        public bool ClearList = false;
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearControl();
                
                //  FetchAllDoctorShare();
                txtSharePercent.IsReadOnly = true;
                rdbAllDoctor.IsChecked = false;
                rdbApllyToAllservice.IsChecked = false;
                rdbPendingDoctor.IsChecked = false;
                rdbSearchDoctor.IsChecked = false;
                rdbSharePercent1.IsEnabled = false;
                rdbSharePercent1.IsChecked = false;
                SearchDoctorStackPanel.Visibility = Visibility.Collapsed;
                txtSharePercent.IsEnabled = false;
                //if (DataList != null)
                //{
                //    DataList.Clear();
                //}
            //    DataPagerDoc10.PageIndex = 0;
                DataPagerDoc10.Source = null;
                
                //SearchDoctorStackPanel.Visibility = Visibility.Collapsed;
                //dgDoctorList.ItemsSource = null;
                //dgDoctorList.UpdateLayout();

                //dgDoctorList.ItemsSource = null;//Added By umesh
                //dgDoctorList.UpdateLayout();     //Added By umesh           
                AllSelectedDoc = new List<clsDoctorVO>();
                this.SetCommandButtonState("New");

                foreach (var item in objList1)
                {
                    item.Status = false;
                }
                cmbMod1.ItemsSource = null;
                cmbMod1.ItemsSource = objList1;
                ListOfModality = new List<MasterListItem>();
                ListOfModality = objList1;
                ClearList = true;
                //rdbModalityWise.IsChecked = true;
                //if (rdbModalityWise.IsChecked == true)
                //{
                //    rdbSharePercent.IsEnabled = true;
                //    cmbMod1.IsEnabled = true;
                //}
                CheckValidations();
                objAnimation.Invoke(RotationType.Forward);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    btnCancel.IsEnabled = false;
                    btnNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdBack.IsEnabled = true;
                    //IsCancel = true;
                    break;
                case "New":
                    btnNew.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    cmdSave.IsEnabled = true;
                    cmdBack.IsEnabled = false;
                    //IsCancel = false;
                    break;
                case "Save":
                    btnNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    cmdBack.IsEnabled = false;
                    // IsCancel = true;
                    break;

                case "Cancel":
                    btnNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    cmdBack.IsEnabled = true;
                    break;


                default:
                    break;
            }
        }

        #endregion

        List<MasterListItem> ListOfTariff = new List<MasterListItem>();
        List<MasterListItem> SelectedListOfTariff = new List<MasterListItem>();
        List<clsSubSpecializationVO> ListOfSpecializationSubSpecialization = new List<clsSubSpecializationVO>();
        List<MasterListItem> ListOfModality = new List<MasterListItem>();
        private void FillTariff()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.MasterTable = MasterTableNameList.M_TariffMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    //List<MasterListItem> objList = new List<MasterListItem>();
                    //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    //cmbTariff.ItemsSource = null;
                    //cmbTariff.ItemsSource = objList;

                    List<MasterListItem> objList = new List<MasterListItem>();

                    List<MasterListItem> objList1 = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    objList1.Add(new MasterListItem(0, "-- All --"));
                    objList1.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbTariff.ItemsSource = null;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedValue = objList[0].ID;

                    cmbTariff1.ItemsSource = null;
                    cmbTariff1.ItemsSource = objList1;
                    cmbTariff1.SelectedValue = objList1[0].ID;
                    ListOfTariff = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        long ID;
        long SubID;
        private void FillSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Specialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    ID = 1;
                    for (int i = 0; i < objList.Count; i++)
                    {
                        ID = ID + 1;

                    }
                    objList.Add(new MasterListItem(ID, "Not Defined"));
                    //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSpec.ItemsSource = null;
                    cmbSpec.ItemsSource = objList;
                    cmbSpec.SelectedValue = objList[0].ID;

                    cmbSpec1.ItemsSource = null;
                    cmbSpec1.ItemsSource = objList;
                    cmbSpec1.SelectedValue = objList[0].ID;
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();



        }

        private void FillSubSpecialization()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_SubSpecialization;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    SubID = 1;
                    for (int i = 0; i < objList.Count; i++)
                    {
                        SubID = SubID + 1;

                    }
                    objList.Add(new MasterListItem(SubID, "Not Defined"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSubSpec.ItemsSource = null;
                    cmbSubSpec.ItemsSource = objList;
                    cmbSubSpec.SelectedValue = objList[0].ID;

                    cmbSubSpec1.ItemsSource = null;
                    cmbSubSpec1.ItemsSource = objList;
                    cmbSubSpec1.SelectedValue = objList[0].ID;

                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();



        }
        List<MasterListItem> objList1 = new List<MasterListItem>();
        private void FillModality()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RadModalityMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    objList1.Add(new MasterListItem(0, "-- All --"));
                    objList1.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbMod.ItemsSource = null;
                    cmbMod.ItemsSource = objList;
                    cmbMod.SelectedValue = objList[0].ID;

                    cmbMod1.ItemsSource = null;
                    cmbMod1.ItemsSource = objList1;
                    cmbMod1.SelectedValue = objList1[0].ID;
                    ListOfModality = ((clsGetMasterListBizActionVO)arg.Result).MasterList;
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            NewChildDoctorShareMaster child = new NewChildDoctorShareMaster();
            List<clsDoctorShareServicesDetailsVO> ObjList = new List<clsDoctorShareServicesDetailsVO>();
            if (grdDocShare.SelectedItem != null)
            {
                ObjList.Add((clsDoctorShareServicesDetailsVO)grdDocShare.SelectedItem);
                child.DoctorShareList = ObjList;
            }
            child.OnSaveButton_Click += new RoutedEventHandler(child_OnSaveButton_Click);
            child.Show();
        }

        void child_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            FoeUpdateService.IsChecked = false;
            FetchData();
        }

        void child_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            FoeUpdateService.IsChecked = false;
            FetchData();
        }

        void child_OnOkButton_Click(object sender, RoutedEventArgs e)
        {
            //FetchData();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Load");
            objAnimation.Invoke(RotationType.Backward);

            //SetCommandButtonState("Cancel");
            //this.DataContext = null;
            //ClearControl();
            ////  FetchData();
            //UserControl rootPage = Application.Current.RootVisual as UserControl;
            //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            //mElement.Text = "";
        }

        private void lnkExistingDoctorSearch_Click(object sender, RoutedEventArgs e)
        {
            ReferalDoctorSearchChildWindow ExistingDoctorWin = new ReferalDoctorSearchChildWindow(calcshare);
            ExistingDoctorWin.OnSaveButton_Click += new RoutedEventHandler(OnExistingDoctorSaveButton_Click);
         //   ExistingDoctorWin.TempFlagFromDocShare = true;
            ExistingDoctorWin.Show();
        }

        long DID = 0;
        string DoctorIDs = "";
        void OnExistingDoctorSaveButton_Click(object sender, RoutedEventArgs e)
        {
            # region //Already commented
            //((clsVisitVO)grdVisit.DataContext).ReferredDoctorID = ((ReferalDoctorSearchChildWindow)sender).DoctorID;
            //((clsVisitVO)grdVisit.DataContext).ReferredDoctor = ((ReferalDoctorSearchChildWindow)sender).DoctorName;



            //((MasterListItem)cmbReferenceDoctor.SelectedItem).ID = ((ReferalDoctorSearchChildWindow)sender).DoctorID;

            //((MasterListItem)cmbReferenceDoctor.SelectedItem).Description = ((ReferalDoctorSearchChildWindow)sender).DoctorName;
            //FillRefernceDoctor();
# endregion

            # region//Added By Umesh

            DoctorIDs = "";
            txtReferenceDoctor.Text = "";
          //  DoctorShareLinkingTypeID = ((ReferalDoctorSearchChildWindow)sender).DoctorShareLinkingTypeID;
            if (((ReferalDoctorSearchChildWindow)sender).SelectedDoc != null && ((ReferalDoctorSearchChildWindow)sender).SelectedDoc.Count > 0)
            {
                foreach (var item in ((ReferalDoctorSearchChildWindow)sender).SelectedDoc)
                {
                    DoctorIDs = DoctorIDs + item.DoctorId;
                    DoctorIDs = DoctorIDs + ",";
                }

                if (DoctorIDs.EndsWith(","))
                    DoctorIDs = DoctorIDs.Remove(DoctorIDs.Length - 1, 1);

            }

            //DID = ((ReferalDoctorSearchChildWindow)sender).DoctorID;

            docnames = new StringBuilder();

            DIDs = new List<long>();
            foreach (var item in ((ReferalDoctorSearchChildWindow)sender).SelectedDoc)
            {
                docnames = docnames.Append(item.DoctorName);
                docnames = docnames.Append(" ");

                DIDs.Add(item.DoctorId);

                item.IsSelected = true;
                docnames.Append(", ");
                //if (calcShare == true)
                //    break;
            }

            if (docnames.Length > 2)
            {
                docnames.Remove(docnames.Length - 2, 2);
            }

            txtReferenceDoctor.IsEnabled = true;
            txtReferenceDoctor.Text = Convert.ToString(docnames);

            //if (calcShare == true)
            //{
            //    txtReferenceDoctor1.IsEnabled = true;
            //    txtReferenceDoctor1.Text = Convert.ToString(docnames);
            //}
            //dgPaidDRList.ItemsSource = null;
            //lstSelectedDoctor = ((ReferalDoctorSearchChildWindow)sender).SelectedDoc;
            

            #endregion
            //Existing code

            //DID = ((ReferalDoctorSearchChildWindow)sender).DoctorID;
            //txtReferenceDoctor.Text = ((ReferalDoctorSearchChildWindow)sender).DoctorName;


        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Doctor Lists Declaration
        List<clsDoctorVO> AllSelectedDoc = new List<clsDoctorVO>();
        // List<MasterListItem> Obj = new List<MasterListItem>();
        List<clsDoctorVO> selectedListDoc = new List<clsDoctorVO>();
        List<clsDoctorVO> CurrentListDoc = new List<clsDoctorVO>();
        List<clsDoctorVO> CurrentAvailableListDoc = new List<clsDoctorVO>();
        List<clsDoctorVO> RemovedListDoc = new List<clsDoctorVO>();
        List<clsDoctorVO> CurrentRemovedListDoc = new List<clsDoctorVO>();
        List<clsDoctorVO> CurrentSelectedListDoc = new List<clsDoctorVO>();
        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            grdDocShare.SelectedIndex = 0;
            //if (DIDs.Count > 0)
            //{
            //    FetchDataForMultiDoctors();
            //}
            //else
            //{
                FetchData();
          //  }
        }
        private void FillDoctor()
        {
            clsGetDoctorDetailListForDoctorMasterBizActionVO BizAction = new clsGetDoctorDetailListForDoctorMasterBizActionVO();
            BizAction.DoctorDetails = new List<clsDoctorVO>();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                // BizAction.UnitID = 0;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            //else if (cmbClinic.SelectedItem != null)
            //{
            //    cmbClinic.SelectedItem = ((MasterListItem)cmbClinic.SelectedItem).ID;
            //}

            if (txtFirstName1.Text != null)
            {
                BizAction.FirstName = txtFirstName1.Text;
            }
            if (txtLastName2.Text != null)
            {
                BizAction.LastName = txtLastName2.Text;
            }


            if (((MasterListItem)cmbSpec1.SelectedItem).ID != 0)
            {
                if (((MasterListItem)cmbSpec1.SelectedItem).ID == ID)
                {
                    ((MasterListItem)cmbSpec1.SelectedItem).ID = 0;
                }

                BizAction.SpecializationID = BizAction.SpecializationID + ((MasterListItem)cmbSpec1.SelectedItem).ID;
                BizAction.SpecializationID = BizAction.SpecializationID + ",";



                if (BizAction.SpecializationID.EndsWith(","))
                    BizAction.SpecializationID = BizAction.SpecializationID.Remove(BizAction.SpecializationID.Length - 1, 1);

            }

            if (((MasterListItem)cmbSubSpec1.SelectedItem).ID != 0)
            {
                if (((MasterListItem)cmbSubSpec1.SelectedItem).ID == SubID)
                {
                    ((MasterListItem)cmbSubSpec1.SelectedItem).ID = 0;
                }
                BizAction.SubSpecializationID = BizAction.SubSpecializationID + ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                BizAction.SubSpecializationID = BizAction.SubSpecializationID + ",";

                // }

                if (BizAction.SubSpecializationID.EndsWith(","))
                    BizAction.SubSpecializationID = BizAction.SubSpecializationID.Remove(BizAction.SubSpecializationID.Length - 1, 1);

            }

            BizAction.IsPagingEnabled = true;          
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).DoctorDetails != null)
                    {
                        clsGetDoctorDetailListForDoctorMasterBizActionVO result = arg.Result as clsGetDoctorDetailListForDoctorMasterBizActionVO;


                        if (result.DoctorDetails != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetDoctorDetailListForDoctorMasterBizActionVO)arg.Result).TotalRows;
                            //DataList.TotalItemCount = result.TotalRows;
                            //DataList.Clear();
                            //foreach (var item in result.DoctorDetails)
                            //{                                                          
                            //    DataList.Add(item);
                            //}
                            //dgDoctorList.ItemsSource = null;
                            //dgDoctorList.ItemsSource = DataList;


                            //DoctorList = (PagedSortableCollectionView<clsDoctorVO>)dgDoctorList.ItemsSource;
                            foreach (clsDoctorVO item in result.DoctorDetails)
                            {
                                if (dgNewDoctorList.ItemsSource == null)
                                {
                                    item.SelectDoctor = false;
                                    item.IsSelectedDoctor = true;
                                    //AllSelectedStates = new List<clsStateVO>();
                                    selectedListDoc = new List<clsDoctorVO>();
                                }
                                else
                                {
                                    //To Get the Selected State of Previous or Next Page
                                    if (AllSelectedDoc.Count > 0)
                                    {
                                        foreach (var item1 in AllSelectedDoc)
                                        {
                                            if (item1.DoctorId == item.DoctorId)
                                            {
                                                item.SelectDoctor = true;
                                                item.IsSelectedDoctor = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (selectedListDoc.Count > 0)
                                        {
                                            foreach (var item1 in selectedListDoc)
                                            {
                                                if (item1.DoctorId == item.DoctorId)
                                                {
                                                    item.SelectDoctor = true;
                                                    item.IsSelectedDoctor = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                //End
                                DataList.Add(item);
                            }

                        }
                        dgDoctorList.ItemsSource = null;
                        dgDoctorList.ItemsSource = DataList;
                        //dgDoctorList.SelectedIndex = -1;
                        DataPagerDoc10.Source = null;
                        DataPagerDoc10.PageSize = BizAction.MaximumRows;
                        DataPagerDoc10.Source = DataList;
                      //  DataPagerDoc10.PageIndex = -1;

                        //TariffDataList = (List<MasterListItem>)dgTariffList.ItemsSource;

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void chkSelectedDoctor_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                RemovedListDoc.Add((clsDoctorVO)dgNewDoctorList.SelectedItem);

            }
            else
            {
                foreach (var item in RemovedListDoc)
                {
                    long id = item.DoctorId;
                   // if(dgNewDoctorList.SelectedItem = item)
                    if (((clsDoctorVO)dgNewDoctorList.SelectedItem).DoctorId != id)
                    {
                        CurrentRemovedListDoc.Add(item);
                    }
                }
                RemovedListDoc = CurrentRemovedListDoc;

            }

        }

        private void chkMultipleDoctors_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {

                selectedListDoc.Add((clsDoctorVO)dgDoctorList.SelectedItem);
                foreach (var item in selectedListDoc)
                {
                    item.SelectedDoctor = true;
                }
            }
            else
            {
                foreach (var item in selectedListDoc)
                {
                    if (((clsDoctorVO)dgDoctorList.SelectedItem).ID != item.ID)
                    {
                        CurrentListDoc.Add(item);
                    }
                }
                selectedListDoc = CurrentListDoc;
            }

        }
        bool checkforDoctor = false;
        private void cmdAdd6_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctorList.ItemsSource != null)
            {
                if (selectedListDoc.Count == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Doctor From Left Panel", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (dgNewDoctorList.ItemsSource != null)
                {
                    AllSelectedDoc = ((List<clsDoctorVO>)dgNewDoctorList.ItemsSource).DeepCopy();
                }
            }
            foreach (clsDoctorVO item in selectedListDoc)
            {
                item.SelectedDoctor = false;
                item.IsSelectedDoctor = false;
                item.SelectDoctor = true;

                AllSelectedDoc.Add(item);
            }

            foreach (clsDoctorVO item in DataList)
            {
                foreach (clsDoctorVO item1 in selectedListDoc)
                {
                    if (item1.DoctorId == item.DoctorId)
                    {
                        item.IsSelectedDoctor = false;

                        item.SelectDoctor = true;
                    }
                }
            }


            //dgDoctorList.ItemsSource = null;
            //dgDoctorList.ItemsSource = DataList;
            this.dgDoctorList.UpdateLayout();
            this.dgDoctorList.Focus();
            dgNewDoctorList.ItemsSource = null;
            selectedListDoc = new List<clsDoctorVO>();

            dgNewDoctorList.ItemsSource = AllSelectedDoc;
            checkforDoctor = true;
            ExistingDoctorList();

        }

        private void cmdRemove6_Click(object sender, RoutedEventArgs e)
        {
            CurrentAvailableListDoc = new List<clsDoctorVO>();
            CurrentAvailableListDoc = (List<clsDoctorVO>)dgNewDoctorList.ItemsSource;
            CurrentSelectedListDoc = new List<clsDoctorVO>();
            if (CurrentAvailableListDoc != null)
            {
                foreach (var item in CurrentAvailableListDoc)
                {
                    if (item.SelectedDoctor != true)
                    {
                        CurrentSelectedListDoc.Add(item);
                    }
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("PALASH", "No Doctor Available in Right Panel For Selection", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            foreach (var item in DataList)
            {
                foreach (var item1 in RemovedListDoc)
                {
                    if (item1.DoctorId == item.DoctorId)
                    {
                        item.SelectDoctor = false;
                        item.IsSelectedDoctor = true;
                    }
                }
            }


            dgDoctorList.ItemsSource = null;
            dgDoctorList.ItemsSource = DataList;
            dgNewDoctorList.ItemsSource = null;
            dgNewDoctorList.ItemsSource = CurrentSelectedListDoc;
            RemovedListDoc = new List<clsDoctorVO>();
            selectedListDoc = new List<clsDoctorVO>();
            AllSelectedDoc = (List<clsDoctorVO>)dgNewDoctorList.ItemsSource;
            checkforDoctor = true;
            ExistingDoctorList();
        }

        private void cmdAddAll6_Click(object sender, RoutedEventArgs e)
        {
            if (dgDoctorList.ItemsSource == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("PALASH", "No Doctor Available in Left Panel To Add", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                if (dgNewDoctorList.ItemsSource != null)
                {
                    selectedListDoc = new List<clsDoctorVO>();
                    selectedListDoc = (List<clsDoctorVO>)dgNewDoctorList.ItemsSource;
                }
                AllSelectedDoc = new List<clsDoctorVO>();
                if (dgNewDoctorList.ItemsSource != null)
                {
                    AllSelectedDoc = ((List<clsDoctorVO>)dgNewDoctorList.ItemsSource).DeepCopy();
                }
                foreach (var item in DataList)
                {
                    if (selectedListDoc.Count > 0)
                    {
                        if (item.SelectDoctor != true)
                        {
                            item.SelectDoctor = true;
                            item.IsSelectedDoctor = false;
                            item.SelectedDoctor = false;
                            AllSelectedDoc.Add(item);
                        }
                    }
                    else
                    {
                        item.IsSelectedDoctor = false;
                        item.SelectDoctor = true;
                        item.SelectedDoctor = false;
                        AllSelectedDoc.Add(item);
                    }
                }

                dgDoctorList.ItemsSource = null;
                dgDoctorList.ItemsSource = DataList;
                this.dgDoctorList.UpdateLayout();
                this.dgDoctorList.Focus();
                dgNewDoctorList.ItemsSource = null;
                dgNewDoctorList.ItemsSource = AllSelectedDoc;
                selectedListDoc = new List<clsDoctorVO>();
                checkforDoctor = true;
                ExistingDoctorList();
            }
        }

        private void cmdRemoveAll6_Click(object sender, RoutedEventArgs e)
        {
            if (dgNewDoctorList.ItemsSource == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("PALASH", "No Doctor Available in Right Panel To Remove", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            else
            {
                dgNewDoctorList.ItemsSource = null;
                foreach (var item in DataList)
                {
                    item.SelectDoctor = false;
                    item.IsSelectedDoctor = true;
                }
                dgDoctorList.ItemsSource = null;
                dgDoctorList.ItemsSource = DataList;
                RemovedListDoc = new List<clsDoctorVO>();
                selectedListDoc = new List<clsDoctorVO>();
                AllSelectedDoc = new List<clsDoctorVO>();
                //  ExistingDoctorList();
            }
        }

        private void dgNewDoctorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (rdbSharePercent1.IsChecked == true)
            {
                txtSharePercent.IsEnabled = false;
             //   txtShareAmount.IsReadOnly = true;
             //   txtShareAmount.Text = "";
                rdbModalityWise1.IsEnabled = true;
                rdbModalityWise1.IsChecked = true;
            }
            else if (rdbSharePercent1.IsChecked == false)
            {
                txtSharePercent.IsReadOnly = true;
                //txtShareAmount.IsReadOnly = true;
                //txtShareAmount.Text = "";
            }
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            //txtSharePercent.IsReadOnly = true;
            //txtShareAmount.IsReadOnly = false;
            //txtSharePercent.Text = "";
        }
        PagedCollectionView collection;

        private void FetchData()
        {
            WaitIndicator Pageload = new WaitIndicator();
            Pageload.Show();
            clsGetDoctorShare1DetailsBizActionVO BizActionVO = new clsGetDoctorShare1DetailsBizActionVO();
            BizActionVO.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizActionVO.UnitID = 0;
            }

            //foreach (var docid in DIDs)
            //{

                if (cmbTariff.SelectedItem != null)
                {
                    BizActionVO.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                }
                if (cmbSpec.SelectedItem != null)
                {
                    BizActionVO.SpecID = ((MasterListItem)cmbSpec.SelectedItem).ID;

                }
                if (cmbSubSpec.SelectedItem != null)
                {
                    BizActionVO.SubSpecID = ((MasterListItem)cmbSubSpec.SelectedItem).ID;

                }
                if (cmbMod.SelectedItem != null)
                {
                    BizActionVO.ModalityID = ((MasterListItem)cmbMod.SelectedItem).ID;

                }

                if (txtReferenceDoctor.Text !="")
                {
                    BizActionVO.DocIds =DoctorIDs;
                }
                else
                {
                    BizActionVO.DocIds = "";
                }


                BizActionVO.IsPagingEnabled = true;
                BizActionVO.StartRowIndex = FrontPanelDataList.PageIndex * FrontPanelDataList.PageSize;
                BizActionVO.MaximumRows = FrontPanelDataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        clsGetDoctorShare1DetailsBizActionVO result = args.Result as clsGetDoctorShare1DetailsBizActionVO;

                        if (result.DoctorShareInfoGetList != null)
                        {
                            FrontPanelDataList.TotalItemCount = result.TotalRows;
                            FrontPanelDataList.Clear();

                            foreach (var item in result.DoctorShareInfoGetList)
                            {

                                FrontPanelDataList.Add(item);

                            }

                            grdDocShare.ItemsSource = null;
                            collection = new PagedCollectionView(FrontPanelDataList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
                            grdDocShare.ItemsSource = collection;

                            //grdDocShare.ItemsSource = null;
                            //grdDocShare.ItemsSource = FrontPanelDataList;

                            DocSharePager.Source = null;
                            DocSharePager.PageSize = BizActionVO.MaximumRows;
                            DocSharePager.Source = FrontPanelDataList;



                        }

                    }
                    Pageload.Close();

                };

                client.ProcessAsync(BizActionVO, new clsUserVO());
                client.CloseAsync();
          //  }
        }

        //private void FetchDataForMultiDoctors()
        //{
        //    WaitIndicator Pageload = new WaitIndicator();
        //    Pageload.Show();
        //    clsGetDoctorShare1DetailsBizActionVO BizActionVO = new clsGetDoctorShare1DetailsBizActionVO();
        //    BizActionVO.DoctorShareInfoGetList = new List<clsDoctorShareServicesDetailsVO>();
        //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
        //    {
        //        BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    }
        //    else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
        //    {
        //        BizActionVO.UnitID = 0;
        //    }
        //    FrontPanelDataList.Clear();
        //    foreach (var docid in DIDs)
        //    {

        //    if (cmbTariff.SelectedItem != null)
        //    {
        //        BizActionVO.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

        //    }
        //    if (cmbSpec.SelectedItem != null)
        //    {
        //        BizActionVO.SpecID = ((MasterListItem)cmbSpec.SelectedItem).ID;

        //    }
        //    if (cmbSubSpec.SelectedItem != null)
        //    {
        //        BizActionVO.SubSpecID = ((MasterListItem)cmbSubSpec.SelectedItem).ID;

        //    }
        //    if (cmbMod.SelectedItem != null)
        //    {
        //        BizActionVO.ModalityID = ((MasterListItem)cmbMod.SelectedItem).ID;

        //    }

        //    if (txtReferenceDoctor.Text != null)
        //    {
        //        BizActionVO.DoctorId = docid;
        //    }
        //    else
        //    {
        //        BizActionVO.DoctorId = 0;
        //    }


        //    BizActionVO.IsPagingEnabled = true;
        //    BizActionVO.StartRowIndex = FrontPanelDataList.PageIndex * FrontPanelDataList.PageSize;
        //    BizActionVO.MaximumRows = FrontPanelDataList.PageSize;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {
        //            clsGetDoctorShare1DetailsBizActionVO result = args.Result as clsGetDoctorShare1DetailsBizActionVO;

        //            if (result.DoctorShareInfoGetList != null)
        //            {
        //                FrontPanelDataList.TotalItemCount = result.TotalRows;
        //             //   FrontPanelDataList.Clear();

        //                foreach (var item in result.DoctorShareInfoGetList)
        //                {

        //                    FrontPanelDataList.Add(item);

        //                }

        //                grdDocShare.ItemsSource = null;
        //                collection = new PagedCollectionView(FrontPanelDataList);
        //                collection.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
        //                collection.GroupDescriptions.Add(new PropertyGroupDescription("SpecializationName"));
        //                collection.GroupDescriptions.Add(new PropertyGroupDescription("TariffName"));
        //                grdDocShare.ItemsSource = collection;

        //                //grdDocShare.ItemsSource = null;
        //                //grdDocShare.ItemsSource = FrontPanelDataList;

        //                DocSharePager.Source = null;
        //                DocSharePager.PageSize = BizActionVO.MaximumRows;
        //                DocSharePager.Source = FrontPanelDataList;



        //            }

        //        }
        //        Pageload.Close();

        //    };

        //    client.ProcessAsync(BizActionVO, new clsUserVO());
        //    client.CloseAsync();
        //      }
        //}

        private void ClearControl()
        {
            cmbSpec1.SelectedValue = (long)0;
            cmbSubSpec1.SelectedValue = (long)0;
            dgDoctorList.ItemsSource = null;
            dgNewDoctorList.ItemsSource = null;
            cmbTariff1.SelectedValue = (long)0;
            cmbMod1.SelectedValue = (long)0;
            rdbApllyToAllservice.IsChecked = false;
            rdbSpecializationSubspecializationWise.IsChecked = false;
            txtSharePercent.Text = "";
            txtShareAmount.Text = "";
            txtFirstName1.Text = "";
            txtLastName2.Text = "";
          //  dgDoctorList.ItemsSource = null;
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool CheckValidations()
        {

            bool isValid = true;
            if (rdbSearchDoctor.IsChecked == true)
            {
                if ((MasterListItem)cmbSpec1.SelectedItem == null)
                {
                    cmbSpec1.TextBox.SetValidation("Specialization is required For Doctor Selection");
                    cmbSpec1.TextBox.RaiseValidationError();
                    cmbSpec1.Focus();

                    isValid = false;

                }
                // else if (((MasterListItem)cmbSpec1.SelectedItem).ID == 0)
                else if (cmbSpec1.SelectedItem == "{-- Select --}")
                {
                    cmbSpec1.TextBox.SetValidation("Specialization is required For Doctor Selection");
                    cmbSpec1.TextBox.RaiseValidationError();
                    cmbSpec1.Focus();

                    isValid = false;
                }
                else
                    cmbSpec1.TextBox.ClearValidationError();
            }

            if (txtSharePercent.Text == "")
            {
                if (rdbModalityWise1.IsChecked == true)
                {
                    txtSharePercent.SetValidation("Share Percentage is required For Applying Doctor Share");
                    txtSharePercent.RaiseValidationError();
                    txtSharePercent.Focus();

                    isValid = false;
                }
                else
                {
                    txtSharePercent.ClearValidationError();
                }
            }
            else
            {
                txtSharePercent.ClearValidationError();
            }
            if (rdbApllyToAllservice.IsChecked == false)//Added By Umesh
            {
                rdbApllyToAllservice.SetValidation("Select All Services");
                rdbApllyToAllservice.RaiseValidationError();
                rdbApllyToAllservice.Focus();
            }
            else
            {
                rdbApllyToAllservice.ClearValidationError();
            }
            SetCommandButtonState("New");
            return isValid;
        }
        bool Check = true;
        List<clsDoctorShareServicesDetailsVO> ShareForExistinfDoctors = new List<clsDoctorShareServicesDetailsVO>();
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (rdbSpecializationSubspecializationWise.IsChecked == true)
            {
                bool isValid = true;
                cmdSave.IsEnabled = false;

                isValid = CheckValidations();
                if (isValid)
                {
                    //if (Servicelist.Count() > 0)
                    //{
                    foreach (var item in ListOfModality)
                    {
                        if (item.Status == true)
                        {
                            ModalityList.Add(item);
                        }
                    }
                    ListOfModality = new List<MasterListItem>();
                    if (ModalityList.Count > 0)
                    {
                        ListOfModality = ModalityList;
                    }
                    string Message = "";
                    if (dgNewDoctorList.ItemsSource != null)
                    {
                        if (RemovedListDoc.Count > 0)
                        {
                            Check = false;
                            if (Check == false)
                            {
                                string msgText;
                                if (rdbPendingDoctor.IsChecked == true)
                                { msgText = "Are you sure you want to Assign share to The Doctor?"; }
                                else
                                { msgText = "Are you sure you want to Update The Doctor Share of Existing Doctors?"; }

                                MessageBoxControl.MessageBoxChildWindow msgWD1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                msgWD1.OnMessageBoxClosed += (arg1) =>
                                {
                                    if (arg1 == MessageBoxResult.Yes)
                                    {
                                        SaveDoctorShare();
                                    }
                                    else
                                    {
                                        ClearControl();
                                      //  SaveWithOutExistingShare();
                                    }
                                };
                                msgWD1.Show();
                            }
                            else
                            {
                                Save();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }

                    else if (rdbAllDoctor.IsChecked == true)
                    {
                        Check = false;
                        if (Check == false)
                        {
                            string msgText = "Are you sure you want to Apply Share to All Doctor ?";

                            MessageBoxControl.MessageBoxChildWindow msgWD1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgWD1.OnMessageBoxClosed += (arg1) =>
                            {
                                if (arg1 == MessageBoxResult.Yes)
                                {
                                    string msgText1 = "Are you sure you want to Update The Doctor Share of Existing Doctors?";

                                    MessageBoxControl.MessageBoxChildWindow msgWD2 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText1, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                                    msgWD2.OnMessageBoxClosed += (arg2) =>
                                    {
                                        if (arg2 == MessageBoxResult.Yes)
                                        {
                                            SaveAllDoctorShare();
                                        }
                                        else
                                        {
                                            ClearControl();
                                         //   SaveWithOutExistingShare();   //Commented By Umesh
                                        }
                                    };
                                    msgWD2.Show();


                                }
                                else
                                {

                                }
                            };
                            msgWD1.Show();


                        }
                        else
                        {
                            Save();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Must Have to Be Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    // }

                    //else
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //               new MessageBoxControl.MessageBoxChildWindow("Palash", "Apply To All Service Must Have to Be Selected", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW1.Show();
                    //}

                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Specialization.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        public bool IsForallDoctor = false;
        void SaveAllDoctorShare()
        {
            if (Check == false)
            {
                clsDeleteDoctorShareForOverRideExistingShareVO DeleteExtingShareDoctors = new clsDeleteDoctorShareForOverRideExistingShareVO();
                DeleteExtingShareDoctors.ExistingDoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();
                ShareForExistinfDoctors = ExistingDoctorShareList;
                foreach (var item in ShareForExistinfDoctors)
                {
                    DeleteExtingShareDoctors.ExistingDoctorShareInfoList.Add(item);
                }

                if (ExistingDoctorShareList.ToList().Count == 0)
                {
                    IsForallDoctor = true;
                    Save();
                }
                else
                {

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {

                            Save();

                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }

                    };
                    client.ProcessAsync(DeleteExtingShareDoctors, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

            }


        }

        void SaveDoctorShare()
        {
            if (Check == false)
            {
                clsDeleteDoctorShareForOverRideExistingShareVO DeleteExtingShareDoctors = new clsDeleteDoctorShareForOverRideExistingShareVO();
                DeleteExtingShareDoctors.ExistingDoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();
                ShareForExistinfDoctors = ExistingDoctorShareList;
                foreach (var item in ShareForExistinfDoctors)
                {
                    DeleteExtingShareDoctors.ExistingDoctorShareInfoList.Add(item);
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {

                        Save();

                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                };
                client.ProcessAsync(DeleteExtingShareDoctors, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }


        }
        WaitIndicator Indicator1 = new WaitIndicator();
        bool ISCompletated = false;
        List<MasterListItem> ModalityList = new List<MasterListItem>();
        void Save()
        {
            

                if (rdbApllyToAllservice.IsChecked == true)
                {

                    clsAddDoctorShareDetailsBizActionVO BizAction = new clsAddDoctorShareDetailsBizActionVO();

                    //WaitIndicator Indicator = new WaitIndicator();
                    #region For Selected Doctor
                    if (dgNewDoctorList.ItemsSource != null)
                    {
                        BizAction.IsAllDoctorShate = false;
                        BizAction.IsApplyToallDoctorWithAllTariffAndAllModality = false;
                        WaitIndicator Indicator = new WaitIndicator();
                        Indicator.Show();
                        foreach (clsDoctorVO objDoc in dgNewDoctorList.ItemsSource)
                        {

                            if (((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                            {
                                foreach (var item in ListOfTariff)
                                {
                                    //Commented on 29/08/2013
                                    // if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                    #region For All Modality
                                    if (ListOfModality.Count != 1)
                                    {
                                        //Specialization    ///Need Change
                                        if (rdbSpecializationSubspecializationWise.IsChecked == true)    //may be false
                                        {
                                            BizAction.ISShareModalityWise = false;
                                            ISCompletated = true;
                                            foreach (var item1 in ListOfSpecializationSubSpecialization)
                                            {

                                                clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                                Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                                Obj.SpecializationID = item1.SpecializationId;

                                                Obj.SubSpecializationId = item1.SubSpecializationId;

                                                Obj.DoctorId = objDoc.DoctorId;

                                                Obj.TariffID = item.ID;
                                                // Obj.DoctorId = objDoc.DoctorId;
                                                Obj.ModalityID = 0;
                                                Obj.Status = true;

                                                Obj.DoctorSharePercentage = Convert.ToDouble(item1.SharePercentage);

                                                if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                    Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                                Obj.IsSelected = true;
                                                BizAction.IsCompanyForm = false;
                                                BizAction.DoctorShareInfoList.Add(Obj);
                                            }
                                        }
                                        else
                                        {
                                            foreach (var item1 in ListOfModality)
                                            {
                                                //BizAction.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                                clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                                Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                                Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                                if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                                {
                                                    Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                                }
                                                else
                                                {
                                                    Obj.SubSpecializationId = 0;
                                                }

                                                Obj.TariffID = item.ID;
                                                Obj.DoctorId = objDoc.DoctorId;
                                                Obj.ModalityID = item1.ID;
                                                Obj.Status = true;
                                                if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                                    Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                                if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                    Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                                Obj.IsSelected = true;
                                                BizAction.IsCompanyForm = false;
                                                BizAction.DoctorShareInfoList.Add(Obj);
                                            }
                                        }
                                    }
                                    #endregion
                                    #region For selected one modality
                                    else //Selected Modality
                                    {
                                        clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                        Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                        if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                        {
                                            Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                        }
                                        else
                                        {
                                            Obj.SubSpecializationId = 0;
                                        }

                                        Obj.TariffID = item.ID;
                                        Obj.DoctorId = objDoc.DoctorId;
                                        //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                        if (cmbMod1.SelectedItem != "{--ALL--}")
                                        {
                                            if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                            {
                                                Obj.ModalityID = ListOfModality[0].ID;
                                            }
                                            else
                                            {
                                                Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                            }
                                        }
                                        else
                                        {
                                            Obj.ModalityID = ListOfModality[0].ID;
                                        }
                                        Obj.Status = true;
                                        if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                            Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                        if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                            Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                        Obj.IsSelected = true;
                                        BizAction.IsCompanyForm = false;
                                        BizAction.DoctorShareInfoList.Add(Obj);
                                    }
                                    #endregion
                                }
                            }

                            else
                            {

                                if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                {
                                    if (rdbSpecializationSubspecializationWise.IsChecked == true)
                                    {
                                        BizAction.ISShareModalityWise = false;
                                        ISCompletated = true;
                                        foreach (var item1 in ListOfSpecializationSubSpecialization)
                                        {

                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = item1.SpecializationId;

                                            Obj.SubSpecializationId = item1.SubSpecializationId;

                                            Obj.DoctorId = objDoc.DoctorId;

                                            Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                            // Obj.DoctorId = objDoc.DoctorId;
                                            Obj.ModalityID = 0;
                                            Obj.Status = true;

                                            Obj.DoctorSharePercentage = Convert.ToDouble(item1.SharePercentage);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction.IsCompanyForm = false;
                                            BizAction.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var item1 in ListOfModality)
                                        {
                                            //BizAction.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                            if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                            {
                                                Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                            }
                                            else
                                            {
                                                Obj.SubSpecializationId = 0;
                                            }

                                            Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                            Obj.DoctorId = objDoc.DoctorId;
                                            Obj.ModalityID = item1.ID;
                                            Obj.Status = true;
                                            if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                                Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction.IsCompanyForm = false;
                                            BizAction.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                }
                                else
                                {
                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                    {
                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.SubSpecializationId = 0;
                                    }

                                    Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                    Obj.DoctorId = objDoc.DoctorId;
                                    //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                    if (cmbMod1.SelectedItem != null)
                                    {
                                        Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.ModalityID = ListOfModality[0].ID;
                                    }
                                    Obj.Status = true;
                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                    Obj.IsSelected = true;
                                    BizAction.IsCompanyForm = false;
                                    BizAction.DoctorShareInfoList.Add(Obj);
                                }
                            }

                        }
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                SetCommandButtonState("Load");
                                FetchData();
                                objAnimation.Invoke(RotationType.Backward);
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.Show();
                                Indicator1.Close();
                                Indicator.Close();

                            }
                            else
                            {

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }

                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    #endregion

                    #region For All Doctor
                    else
                    {
                        BizAction1.IsAllDoctorShate = true;
                        BizAction1.ISFORALLDOCTOR = IsForallDoctor;
                        Indicator1.Show();

                        if (cmbMod1.SelectedItem != null)
                        {
                            if (cmbTariff1.SelectedItem != null)
                            {
                                if ((((MasterListItem)cmbMod1.SelectedItem).ID == 0) && ((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                                {
                                    BizAction1.IsApplyToallDoctorWithAllTariffAndAllModality = true;
                                }
                            }
                        }
                        if (ForRecursionTariffAndModality == false)
                        {
                            if (((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                            {
                                if (rdbSpecializationSubspecializationWise.IsChecked == false)
                                {

                                    foreach (var item in ListOfTariff)
                                    {
                                        // if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)

                                        if (ListOfModality.Count != 1)
                                        {

                                            if (rdbSpecializationSubspecializationWise.IsChecked == false)
                                            {
                                                foreach (var item1 in ListOfModality)
                                                {
                                                    //BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                                    {
                                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                                    }
                                                    else
                                                    {
                                                        Obj.SubSpecializationId = 0;
                                                    }

                                                    Obj.TariffID = item.ID;
                                                    // Obj.DoctorId = objDoc.DoctorId;
                                                    Obj.ModalityID = item1.ID;
                                                    Obj.Status = true;
                                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                                    Obj.IsSelected = true;
                                                    BizAction1.IsCompanyForm = false;
                                                    BizAction1.DoctorShareInfoList.Add(Obj);
                                                }
                                            }

                                        }
                                        else //Selected Modality
                                        {
                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                            if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                            {
                                                Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                            }
                                            else
                                            {
                                                Obj.SubSpecializationId = 0;
                                            }

                                            Obj.TariffID = item.ID;
                                            // Obj.DoctorId = objDoc.DoctorId;

                                            //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                            if (cmbMod1.SelectedItem != null)
                                            {
                                                if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                                {
                                                    Obj.ModalityID = ListOfModality[0].ID;
                                                }
                                                else
                                                {
                                                    Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;

                                                }
                                            }
                                            else
                                            {
                                                Obj.ModalityID = ListOfModality[0].ID;
                                            }
                                            Obj.Status = true;
                                            if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                                Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction1.IsCompanyForm = false;
                                            BizAction1.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                }
                                else
                                {
                                    if (((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                                    {
                                        //For Specialization SubSpecialization Wise
                                        BizAction1.ISShareModalityWise = false;
                                        ISCompletated = true;
                                        foreach (var item1 in ListOfSpecializationSubSpecialization)
                                        {

                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = item1.SpecializationId;

                                            Obj.SubSpecializationId = item1.SubSpecializationId;



                                            Obj.TariffID = 0;
                                            // Obj.DoctorId = objDoc.DoctorId;
                                            Obj.ModalityID = 0;
                                            Obj.Status = true;

                                            Obj.DoctorSharePercentage = Convert.ToDouble(item1.SharePercentage);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction1.IsCompanyForm = false;
                                            BizAction1.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                    else
                                    {


                                        foreach (var item in SelectedListOfTariff)
                                        {
                                            BizAction1.ISShareModalityWise = false;
                                            ISCompletated = true;
                                            foreach (var item1 in ListOfSpecializationSubSpecialization)
                                            {

                                                clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                                Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                                Obj.SpecializationID = item1.SpecializationId;

                                                Obj.SubSpecializationId = item1.SubSpecializationId;



                                                Obj.TariffID = item.ID;
                                                // Obj.DoctorId = objDoc.DoctorId;
                                                Obj.ModalityID = 0;
                                                Obj.Status = true;

                                                Obj.DoctorSharePercentage = Convert.ToDouble(item1.SharePercentage);

                                                if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                    Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                                Obj.IsSelected = true;
                                                BizAction1.IsCompanyForm = false;
                                                BizAction1.DoctorShareInfoList.Add(Obj);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (rdbSpecializationSubspecializationWise.IsChecked == true)
                                {

                                    foreach (var item2 in ListOfTariff)
                                    {
                                        if (item2.ID == ((MasterListItem)cmbTariff1.SelectedItem).ID)
                                        {
                                            SelectedListOfTariff.Add(item2);
                                        }
                                    }
                                    foreach (var item in SelectedListOfTariff)
                                    {
                                        BizAction1.ISShareModalityWise = false;
                                        ISCompletated = true;
                                        foreach (var item1 in ListOfSpecializationSubSpecialization)
                                        {

                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = item1.SpecializationId;

                                            Obj.SubSpecializationId = item1.SubSpecializationId;



                                            Obj.TariffID = item.ID;
                                            // Obj.DoctorId = objDoc.DoctorId;
                                            Obj.ModalityID = 0;
                                            Obj.Status = true;

                                            Obj.DoctorSharePercentage = Convert.ToDouble(item1.SharePercentage);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction1.IsCompanyForm = false;
                                            BizAction1.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                }
                                else
                                {
                                    ///////////////////////////////////////////////////////////////////////
                                    if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                    {
                                        foreach (var item1 in ListOfModality)
                                        {
                                            //BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                            clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                            Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                            Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                            if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                            {
                                                Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                            }
                                            else
                                            {
                                                Obj.SubSpecializationId = 0;
                                            }

                                            Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                            //  Obj.DoctorId = objDoc.DoctorId;
                                            Obj.ModalityID = item1.ID;
                                            Obj.Status = true;
                                            if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                                Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                            if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                                Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                            Obj.IsSelected = true;
                                            BizAction1.IsCompanyForm = false;
                                            BizAction1.DoctorShareInfoList.Add(Obj);
                                        }
                                    }
                                    else
                                    {
                                        clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                        Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                        if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                        {
                                            Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                        }
                                        else
                                        {
                                            Obj.SubSpecializationId = 0;
                                        }

                                        Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                        //Obj.DoctorId = objDoc.DoctorId;
                                        //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                        if (cmbMod1.SelectedItem != null)
                                        {
                                            if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                            {
                                                Obj.ModalityID = ListOfModality[0].ID;
                                            }
                                            else
                                            {
                                                Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;

                                            }
                                        }
                                        else
                                        {
                                            Obj.ModalityID = ListOfModality[0].ID;
                                        }
                                        Obj.Status = true;
                                        if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                            Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                        if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                            Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                        Obj.IsSelected = true;
                                        BizAction1.IsCompanyForm = false;
                                        BizAction1.DoctorShareInfoList.Add(Obj);
                                    }
                                }
                            }
                        }

                        List = BizAction1.DoctorShareInfoList;

                        for (int i = 0; i < BizAction1.DoctorShareInfoList.Count(); i++)
                        // foreach (var item in BizAction1.DoctorShareInfoList)
                        {
                            BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();

                            BizAction1.DoctorShareDetails.UnitID = BizAction1.DoctorShareInfoList[i].UnitID;
                            BizAction1.DoctorShareDetails.DoctorId = BizAction1.DoctorShareInfoList[i].DoctorId;
                            BizAction1.IsAllDoctorShate = true;
                            BizAction1.IsApplyToallDoctorWithAllTariffAndAllModality = true;
                            BizAction1.DoctorShareDetails.TariffID = BizAction1.DoctorShareInfoList[i].TariffID;
                            BizAction1.DoctorShareDetails.TariffServiceID = BizAction1.DoctorShareInfoList[i].TariffServiceID;
                            BizAction1.DoctorShareDetails.ServiceId = BizAction1.DoctorShareInfoList[i].ServiceId;
                            BizAction1.DoctorShareDetails.SpecializationID = BizAction1.DoctorShareInfoList[i].SpecializationID;
                            BizAction1.DoctorShareDetails.SubSpecializationId = BizAction1.DoctorShareInfoList[i].SubSpecializationId;

                            BizAction1.DoctorShareDetails.DepartmentId = BizAction1.DoctorShareInfoList[i].DepartmentId;
                            BizAction1.DoctorShareDetails.ModalityID = BizAction1.DoctorShareInfoList[i].ModalityID;
                            BizAction1.DoctorShareDetails.ServiceRate = BizAction1.DoctorShareInfoList[i].ServiceRate;
                            BizAction1.DoctorShareDetails.DoctorSharePercentage = BizAction1.DoctorShareInfoList[i].DoctorSharePercentage;
                            BizAction1.DoctorShareDetails.DoctorShareAmount = BizAction1.DoctorShareInfoList[i].DoctorShareAmount;
                            BizAction1.DoctorShareDetails.AddedDateTime = BizAction1.DoctorShareInfoList[i].AddedDateTime;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null)
                                {

                                    if (List.ToList().Count() > 0)
                                    {
                                        if (ISCompletated == true)
                                        {
                                            List = new List<clsDoctorShareServicesDetailsVO>();
                                        }
                                        SaveAll();
                                    }

                                }
                                else
                                {

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW1.Show();
                                }
                                //Indicator.Close();
                            };
                            client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                            List.Remove(BizAction1.DoctorShareInfoList[i]);

                            break;

                        }
                        if (List.Count() == 0)
                        {
                            SetCommandButtonState("Load");

                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            Indicator1.Close();
                        }

                    }
                    #endregion


                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Apply To All Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            
            
        }

        List<clsDoctorShareServicesDetailsVO> List = new List<clsDoctorShareServicesDetailsVO>();
        clsAddDoctorShareDetailsBizActionVO BizAction1 = new clsAddDoctorShareDetailsBizActionVO();
        public bool ForRecursionTariffAndModality = false;
        void SaveAll()
        {
            BizAction1.DoctorShareInfoList = List;
            if (BizAction1.DoctorShareInfoList.Count > 0)
            {
                ForRecursionTariffAndModality = true;

                Save();
            }
            else
            {
                SetCommandButtonState("Load");
                objAnimation.Invoke(RotationType.Backward);
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

                Indicator1.Close();
                FetchData();
            }
        }

        void SaveWithOutExistingShare()
        {

            if (rdbApllyToAllservice.IsChecked == true)
            {
                WaitIndicator Indicator = new WaitIndicator();
                Indicator.Show();
                clsAddDoctorShareDetailsBizActionVO BizAction = new clsAddDoctorShareDetailsBizActionVO();
                BizAction.DoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();

                BizAction.DontChangeTheExistingDoctor = true;
                if (rdbApllyToAllservice.IsChecked == false)
                {
                    foreach (clsDoctorVO objDoc in dgNewDoctorList.ItemsSource)
                    {

                        if (((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                        {
                            foreach (var item in ListOfTariff)
                            {
                                if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                {
                                    foreach (var item1 in ListOfModality)
                                    {
                                        //BizAction.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                        clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                        Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                        if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                        {
                                            Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                        }
                                        else
                                        {
                                            Obj.SubSpecializationId = 0;
                                        }

                                        Obj.TariffID = item.ID;
                                        Obj.DoctorId = objDoc.DoctorId;
                                        Obj.ModalityID = item1.ID;
                                        Obj.Status = true;
                                        if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                            Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                        if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                            Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                        Obj.IsSelected = true;
                                        BizAction.IsCompanyForm = false;
                                        BizAction.DoctorShareInfoList.Add(Obj);
                                    }
                                }
                                else //Selected Modality
                                {
                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                    {
                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.SubSpecializationId = 0;
                                    }

                                    Obj.TariffID = item.ID;
                                    Obj.DoctorId = objDoc.DoctorId;
                                    //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                    if (cmbMod1.SelectedItem != null)
                                    {
                                        Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.ModalityID = ListOfModality[0].ID;
                                    }
                                    Obj.Status = true;
                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                    Obj.IsSelected = true;
                                    BizAction.IsCompanyForm = false;
                                    BizAction.DoctorShareInfoList.Add(Obj);
                                }
                            }
                        }

                        else
                        {
                            if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                            {
                                foreach (var item1 in ListOfModality)
                                {
                                    //BizAction.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                    {
                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.SubSpecializationId = 0;
                                    }

                                    Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                    Obj.DoctorId = objDoc.DoctorId;
                                    Obj.ModalityID = item1.ID;
                                    Obj.Status = true;
                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                    Obj.IsSelected = true;
                                    BizAction.IsCompanyForm = false;
                                    BizAction.DoctorShareInfoList.Add(Obj);
                                }
                            }
                            else
                            {
                                clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                {
                                    Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                }
                                else
                                {
                                    Obj.SubSpecializationId = 0;
                                }

                                Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                Obj.DoctorId = objDoc.DoctorId;
                                //Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                if (cmbMod1.SelectedItem != null)
                                {
                                    Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                }
                                else
                                {
                                    Obj.ModalityID = ListOfModality[0].ID;
                                }
                                Obj.Status = true;
                                if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                    Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                    Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                Obj.IsSelected = true;
                                BizAction.IsCompanyForm = false;
                                BizAction.DoctorShareInfoList.Add(Obj);
                            }
                        }

                    }
                    List<clsDoctorShareServicesDetailsVO> RemovedDoctorShareList = new List<clsDoctorShareServicesDetailsVO>();


                    for (int i = 0; i < ExistingDoctorShareList.Count(); i++)
                    {
                        for (int j = 0; j < BizAction.DoctorShareInfoList.Count(); j++)
                        // foreach (clsDoctorShareServicesDetailsVO item1 in BizAction.DoctorShareInfoList)
                        {
                            if (ExistingDoctorShareList[i].DoctorId == BizAction.DoctorShareInfoList[j].DoctorId)
                            {
                                if (BizAction.DoctorShareInfoList[j].ModalityID == ExistingDoctorShareList[i].ModalityID)
                                {
                                    if (ExistingDoctorShareList[i].TariffID == BizAction.DoctorShareInfoList[j].TariffID)
                                    {
                                        if (ExistingDoctorShareList[i].SpecializationID == BizAction.DoctorShareInfoList[j].SpecializationID)
                                        {
                                            if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                            {
                                                if (ExistingDoctorShareList[i].SubSpecializationId == BizAction.DoctorShareInfoList[j].SubSpecializationId)
                                                {
                                                    BizAction.DoctorShareInfoList.Remove(BizAction.DoctorShareInfoList[j]);
                                                }
                                            }
                                            else
                                            {
                                                BizAction.DoctorShareInfoList.Remove(BizAction.DoctorShareInfoList[j]);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            SetCommandButtonState("Load");
                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            Indicator.Close();

                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW1.Show();
                        }

                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();

                }
                else
                {
                    BizAction1.IsAllDoctorShate = true;
                    Indicator1.Show();

                    if (cmbMod1.SelectedItem != null)
                    {
                        if (cmbTariff1.SelectedItem != null)
                        {
                            if ((((MasterListItem)cmbMod1.SelectedItem).ID == 0) && ((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                            {
                                BizAction1.IsApplyToallDoctorWithAllTariffAndAllModality = true;

                            }
                        }
                    }
                    if (ForRecursionTariffAndModality == false)
                    {
                        if (((MasterListItem)cmbTariff1.SelectedItem).ID == 0)
                        {
                            foreach (var item in ListOfTariff)
                            {
                                //if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                                if (ListOfModality.Count != 1)
                                {
                                    foreach (var item1 in ListOfModality)
                                    {
                                        //BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                        clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                        Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                        Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                        if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                        {
                                            Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                        }
                                        else
                                        {
                                            Obj.SubSpecializationId = 0;
                                        }

                                        Obj.TariffID = item.ID;
                                        // Obj.DoctorId = objDoc.DoctorId;
                                        Obj.ModalityID = item1.ID;
                                        Obj.Status = true;
                                        if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                            Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                        if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                            Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                        Obj.IsSelected = true;
                                        BizAction1.IsCompanyForm = false;
                                        BizAction1.DoctorShareInfoList.Add(Obj);
                                    }
                                }
                                else //Selected Modality
                                {
                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                    {
                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.SubSpecializationId = 0;
                                    }

                                    Obj.TariffID = item.ID;
                                    // Obj.DoctorId = objDoc.DoctorId;
                                    if (cmbMod1.SelectedItem != null)
                                    {
                                        Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.ModalityID = ListOfModality[0].ID;
                                    }
                                    Obj.Status = true;
                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                    Obj.IsSelected = true;
                                    BizAction1.IsCompanyForm = false;
                                    BizAction1.DoctorShareInfoList.Add(Obj);
                                }
                            }
                        }

                        else
                        {
                            if (((MasterListItem)cmbMod1.SelectedItem).ID == 0)
                            {
                                foreach (var item1 in ListOfModality)
                                {
                                    //BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();
                                    clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                    Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                    Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                    {
                                        Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                    }
                                    else
                                    {
                                        Obj.SubSpecializationId = 0;
                                    }

                                    Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                    //  Obj.DoctorId = objDoc.DoctorId;
                                    Obj.ModalityID = item1.ID;
                                    Obj.Status = true;
                                    if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                        Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                    if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                        Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                    Obj.IsSelected = true;
                                    BizAction1.IsCompanyForm = false;
                                    BizAction1.DoctorShareInfoList.Add(Obj);
                                }
                            }
                            else
                            {
                                clsDoctorShareServicesDetailsVO Obj = new clsDoctorShareServicesDetailsVO();

                                Obj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                                Obj.SpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
                                if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
                                {
                                    Obj.SubSpecializationId = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
                                }
                                else
                                {
                                    Obj.SubSpecializationId = 0;
                                }

                                Obj.TariffID = ((MasterListItem)cmbTariff1.SelectedItem).ID;
                                //Obj.DoctorId = objDoc.DoctorId;
                                Obj.ModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
                                Obj.Status = true;
                                if (txtSharePercent.Text != null && txtSharePercent.Text != "")
                                    Obj.DoctorSharePercentage = Convert.ToDouble(txtSharePercent.Text);

                                if (txtShareAmount.Text != null && txtShareAmount.Text != "")
                                    Obj.DoctorShareAmount = Convert.ToDouble(txtShareAmount.Text);

                                Obj.IsSelected = true;
                                BizAction1.IsCompanyForm = false;
                                BizAction1.DoctorShareInfoList.Add(Obj);
                            }
                        }
                    }


                    List = BizAction1.DoctorShareInfoList;

                    for (int i = 0; i < BizAction1.DoctorShareInfoList.Count(); i++)
                    // foreach (var item in BizAction1.DoctorShareInfoList)
                    {
                        BizAction1.DoctorShareDetails = new clsDoctorShareServicesDetailsVO();

                        BizAction1.DoctorShareDetails.UnitID = BizAction1.DoctorShareInfoList[i].UnitID;
                        BizAction1.DoctorShareDetails.DoctorId = BizAction1.DoctorShareInfoList[i].DoctorId;
                        BizAction1.IsAllDoctorShate = true;
                        BizAction1.IsApplyToallDoctorWithAllTariffAndAllModality = true;
                        BizAction1.DoctorShareDetails.TariffID = BizAction1.DoctorShareInfoList[i].TariffID;
                        BizAction1.DoctorShareDetails.TariffServiceID = BizAction1.DoctorShareInfoList[i].TariffServiceID;
                        BizAction1.DoctorShareDetails.ServiceId = BizAction1.DoctorShareInfoList[i].ServiceId;
                        BizAction1.DoctorShareDetails.SpecializationID = BizAction1.DoctorShareInfoList[i].SpecializationID;
                        BizAction1.DoctorShareDetails.SubSpecializationId = BizAction1.DoctorShareInfoList[i].SubSpecializationId;

                        BizAction1.DoctorShareDetails.DepartmentId = BizAction1.DoctorShareInfoList[i].DepartmentId;
                        BizAction1.DoctorShareDetails.ModalityID = BizAction1.DoctorShareInfoList[i].ModalityID;
                        BizAction1.DoctorShareDetails.ServiceRate = BizAction1.DoctorShareInfoList[i].ServiceRate;
                        BizAction1.DoctorShareDetails.DoctorSharePercentage = BizAction1.DoctorShareInfoList[i].DoctorSharePercentage;
                        BizAction1.DoctorShareDetails.DoctorShareAmount = BizAction1.DoctorShareInfoList[i].DoctorShareAmount;
                        BizAction1.DoctorShareDetails.AddedDateTime = BizAction1.DoctorShareInfoList[i].AddedDateTime;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {

                                if (List.ToList().Count() > 0)
                                {
                                    SaveAll();
                                }

                            }
                            else
                            {

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW1.Show();
                            }

                        };
                        client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                        List.Remove(BizAction1.DoctorShareInfoList[i]);

                        break;

                    }
                    if (List.Count() == 0)
                    {
                        SetCommandButtonState("Load");
                        Indicator1.Close();
                        FetchData();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Share Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
                //BizAction.DoctorShareInfoList = new List<clsDoctorShareServicesDetailsVO>();
                //BizAction.DoctorShareInfoList = RemovedDoctorShareList;



            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Apply To All Service.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }


        }

        private void cmdSearchDoctor_Click(object sender, RoutedEventArgs e)
        {
            DocSharePager.PageIndex = 0;
                FillDoctor();
              //  DataPagerDoc10.PageIndex = 0;
        }

        List<clsDoctorShareServicesDetailsVO> Servicelist = new List<clsDoctorShareServicesDetailsVO>();

        private void ApplyToAllServices_Click_1(object sender, RoutedEventArgs e)
        {

            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    WaitIndicator IndicatorForService = new WaitIndicator();
            //    IndicatorForService.Show();
            //    clsGetMasterForServiceBizActionVO BizAction = new clsGetMasterForServiceBizActionVO();
            //    BizAction.ServiceDetails = new List<clsDoctorShareServicesDetailsVO>();

            //    if (((MasterListItem)cmbSpec1.SelectedItem).ID > 0)
            //    {
            //        BizAction.NewSpecializationID = ((MasterListItem)cmbSpec1.SelectedItem).ID;
            //    }

            //    if (((MasterListItem)cmbSubSpec1.SelectedItem).ID > 0)
            //    {
            //        BizAction.NewSubSpecializationID = ((MasterListItem)cmbSubSpec1.SelectedItem).ID;
            //    }
            //    else
            //    {
            //        BizAction.NewSubSpecializationID = 0;
            //    }

            //    if (((MasterListItem)cmbTariff1.SelectedItem).ID > 0)
            //    {
            //        BizAction.NewTariffId = ((MasterListItem)cmbTariff1.SelectedItem).ID;
            //    }
            //    else
            //    {
            //        BizAction.NewTariffId = 0;
            //    }

            //    if (((MasterListItem)cmbMod1.SelectedItem).ID > 0)
            //    {
            //        BizAction.NewModalityID = ((MasterListItem)cmbMod1.SelectedItem).ID;
            //    }
            //    else
            //    {
            //        BizAction.NewModalityID = 0;
            //    }




            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    client.ProcessCompleted += (s, rag) =>
            //    {
            //        if (arg.Error == null)
            //        {
            //            if (((clsGetMasterForServiceBizActionVO)arg.Result).ServiceDetails != null)
            //            {
            //                clsGetMasterForServiceBizActionVO result = arg.Result as clsGetMasterForServiceBizActionVO;

            //                if (result.ServiceDetails != null)
            //                {
            //                    Servicelist = (List<clsDoctorShareServicesDetailsVO>)result.ServiceDetails;

            //                }
            //                MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Apply To All Services Successfully Completed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //                msgW1.Show();
            //            }

            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //            msgW1.Show();
            //        }
            //        IndicatorForService.Close();

            //    };
            //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    client.CloseAsync();
            //}
            //else
            //{
            //    Servicelist = new List<clsDoctorShareServicesDetailsVO>();

            //}


        }

        private void cmbTariff1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rdbApllyToAllservice.IsChecked = false;
            Servicelist = new List<clsDoctorShareServicesDetailsVO>();
        }

        private void cmbMod1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rdbApllyToAllservice.IsChecked = false;
            Servicelist = new List<clsDoctorShareServicesDetailsVO>();
        }

        private void lnkClearDoctor_Click(object sender, RoutedEventArgs e)
        {
            //Existing
            //DID = 0;
            //txtReferenceDoctor.Text = "";

            //By Umesh
         
            string str = docnames.ToString();
            if (str.Contains(","))
            {

                if (str.Length > 0)
                {
                    try
                    {
                        str = str.Substring(0, str.LastIndexOf(','));

                        docnames.Clear();

                        docnames.Append(str);

                        txtReferenceDoctor.Text = Convert.ToString(docnames);
                        DIDs.RemoveAt(DIDs.Count - 1);
                        DoctorIDs = DoctorIDs.Substring(0, DoctorIDs.LastIndexOf(','));
                        lnkClearDoctor.IsChecked = false;
                        //  lstSelectedDoctor.RemoveAt(lstSelectedDoctor.Count - 1);

                        //    dgPaidDRList.ItemsSource = null;
                        //grdDocShare


                        //    dgPaidDRList.UpdateLayout();
                    }
                    catch (Exception ex)
                    {
                        str = "";
                        txtReferenceDoctor.Text = "";
                        docnames.Clear();
                        //  dgPaidDRList.ItemsSource = null;
                        //   dgPaidDRList.UpdateLayout();
                    }
                }
            }
            else
            {
                str = "";
                txtReferenceDoctor.Text = "";
                lnkClearDoctor.IsChecked = false;
            }
        }

        private void AllDoctorRadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            rdbAllDoctor.IsChecked = true;
            rdbPendingDoctor.IsChecked = false;
            rdbSearchDoctor.IsChecked = false;
            if (rdbSearchDoctor.IsChecked == true)
            {
                SearchDoctorStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SearchDoctorStackPanel.Visibility = Visibility.Collapsed;
            }
            //ExistingDoctorList();
            dgDoctorList.ItemsSource = null;
            dgNewDoctorList.ItemsSource = null;
        }

        private void SearchDoctorRadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            ClearControl();
            //dgDoctorList.ItemsSource = null;
            //DataPagerDoc10.Source = null;
           // DataPagerDoc10.PageIndex = 0;
            rdbAllDoctor.IsChecked = false;
            rdbPendingDoctor.IsChecked = false;
            // rdbSearchDoctor.IsChecked = true;
            if (rdbSearchDoctor.IsChecked == true)
            {
                SearchDoctorStackPanel.Visibility = Visibility.Visible;
                dgDoctorList.SelectedIndex = 0;
             //   DataList.PageIndex = 0;
                FillDoctor();
             //   DataPagerDoc10.PageIndex = 0;
            }
            else
            {
                SearchDoctorStackPanel.Visibility = Visibility.Collapsed;
                dgDoctorList.ItemsSource = null;
                DataPagerDoc10.Source = null;
                dgDoctorList.UpdateLayout();
            }
            //FetchAllDoctorShare();

        }

        private void PendingDoctorRadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            ClearControl();
            rdbAllDoctor.IsChecked = false;
            rdbPendingDoctor.IsChecked = true;
            rdbSearchDoctor.IsChecked = false;
            DataList.Clear();
            if (rdbSearchDoctor.IsChecked == true)
            {
                SearchDoctorStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SearchDoctorStackPanel.Visibility = Visibility.Collapsed;
            }

            FetchPendingDoctor();
        }

        void FetchPendingDoctor()
        {
            PendingDoctorList ExistingDoctorWin = new PendingDoctorList();
            ExistingDoctorWin.IsForPendingDoctor = true;
            ExistingDoctorWin.OnSaveButton_Click += new RoutedEventHandler(OnPendingDoctorSaveButton_Click);


            ExistingDoctorWin.Show();
        }
        void OnPendingDoctorSaveButton_Click(object sender, RoutedEventArgs e)
        {
            dgDoctorList.ItemsSource = null;
            if (((PendingDoctorList)sender).DoctorShareSelectList != null)
            {
                dgNewDoctorList.ItemsSource = null;
                dgNewDoctorList.ItemsSource = ((PendingDoctorList)sender).DoctorShareSelectList;
                //dgDoctorList.ItemsSource = null;
                //dgDoctorList.ItemsSource = ((PendingDoctorList)sender).DoctorShareSelectList;
                ExistingDoctorList();
            }
        }
        List<clsDoctorShareServicesDetailsVO> ExistingDoctorShareList = new List<clsDoctorShareServicesDetailsVO>();
        void ExistingDoctorList()
        {
            string DoctorID = "";
            List<clsDoctorVO> ItemList = new List<clsDoctorVO>();
            ItemList = (List<clsDoctorVO>)dgNewDoctorList.ItemsSource;
            //if (((MasterListItem)cmbSubSpecilization.SelectedItem).ID == 0)
            //{
            //    SubSpecilizaionID = null;
            //}
            if (checkforDoctor == true)
            {
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var item in ItemList)
                    {
                        DoctorID = DoctorID + item.DoctorId;
                        DoctorID = DoctorID + ",";
                    }

                    if (DoctorID.EndsWith(","))
                        DoctorID = DoctorID.Remove(DoctorID.Length - 1, 1);

                    // ItemIDs = ItemIDs + (ItemIDs == "" ? (SelectedItemsList[i].ToString()) : "," + (SelectedItemsList[i].ToString()));

                }
            }
            else if (ItemList != null)
            {
                if (ItemList != null && ItemList.Count > 0)
                {
                    foreach (var item in ItemList)
                    {
                        //DoctorID = DoctorID + item.ID;
                        DoctorID = DoctorID + item.DoctorId;
                        DoctorID = DoctorID + ",";
                    }

                    if (DoctorID.EndsWith(","))
                        DoctorID = DoctorID.Remove(DoctorID.Length - 1, 1);

                    // ItemIDs = ItemIDs + (ItemIDs == "" ? (SelectedItemsList[i].ToString()) : "," + (SelectedItemsList[i].ToString()));

                }
            }
            else
            {
                DoctorID = DoctorID + 0;
                DoctorID = DoctorID + ",";
                if (DoctorID.EndsWith(","))
                    DoctorID = DoctorID.Remove(DoctorID.Length - 1, 1);
            }


            clsGetExistingDoctorShareDetails ExtingShareDoctors = new clsGetExistingDoctorShareDetails();
            ExtingShareDoctors.DoctorIDs = DoctorID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetExistingDoctorShareDetails result = arg.Result as clsGetExistingDoctorShareDetails;
                    ExistingDoctorShareList = new List<clsDoctorShareServicesDetailsVO>();

                    if (result.DoctorList != null)
                    {
                        ExistingDoctorShareList = (List<clsDoctorShareServicesDetailsVO>)result.DoctorList;

                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Doctor Share Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }

            };
            client.ProcessAsync(ExtingShareDoctors, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
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

        private void TextName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void rdbSpecializationSubspecializationWise_Click(object sender, RoutedEventArgs e)
        {
            SpecializationSubSpecializationWiseDoctorShareChildWindow ChildWindow = new SpecializationSubSpecializationWiseDoctorShareChildWindow();
            if (ClearList == true)
            {
                if (ListOfSpecializationSubSpecialization != null)
                {
                    if (ListOfSpecializationSubSpecialization.Count > 0)
                    {
                        ChildWindow.DeepCopySubSpecializationList = ListOfSpecializationSubSpecialization;
                    }
                }
            }

            ChildWindow.OnSaveButton_Click += new RoutedEventHandler(ChildWindow_OnSaveButton_Click);
            ChildWindow.OnCloseButton_Click += new RoutedEventHandler(ChildWindow_OnCancelButton_click);
            ChildWindow.Show();
            cmbMod1.IsEnabled = false;
            rdbSharePercent1.IsEnabled = false;

        }
        void ChildWindow_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((SpecializationSubSpecializationWiseDoctorShareChildWindow)sender).SelectedSubSpecializationList != null)
            {
                ListOfSpecializationSubSpecialization = ((SpecializationSubSpecializationWiseDoctorShareChildWindow)sender).SelectedSubSpecializationList;
            }
            rdbSpecializationSubspecializationWise.IsChecked = true;
        }

        void ChildWindow_OnCancelButton_click(object sender, RoutedEventArgs e)
        {
            rdbSpecializationSubspecializationWise.IsChecked = false;
        }

        private void rdbModalityWise_Click(object sender, RoutedEventArgs e)
        {
            if (rdbModalityWise1.IsChecked == true)
            {
                cmbMod1.IsEnabled = true;
                rdbSharePercent1.IsEnabled = true;
            }
            else if (rdbModalityWise1.IsChecked == false)
            {
                cmbMod1.IsEnabled = false;
                rdbSharePercent1.IsEnabled = false;
            }

        }

        //private void rdbModalityWise1_Checked(object sender, EventArgs e)
        //{
        //    cmbMod1.IsEnabled = true;
        //    rdbSharePercent1.IsEnabled = true;
        //}

        //private void rdbModalityWise1_Unchecked(object sender, EventArgs e)
        //{
        //    cmbMod1.IsEnabled = false;
        //    rdbSharePercent1.IsEnabled = false;
        //    txtSharePercent.IsEnabled = false;

        //}

        private void txtFirstName1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillDoctor();
        }

        private void txtLastName2_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FillDoctor();
        }

        private void FoeUpdateService_Checked(object sender, RoutedEventArgs e)
        {

            //if (txtReferenceDoctor.Text == "")
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //       new MessageBoxControl.MessageBoxChildWindow("Palash", "Select at least one doctor for service updatation.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    SetCommandButtonState("Load");
            //    FoeUpdateService.IsChecked = false;
            //    FetchData();
            //}
            //else
            //{
            NewChildDoctorShareMaster child = new NewChildDoctorShareMaster();
            List<clsDoctorShareServicesDetailsVO> ObjList = new List<clsDoctorShareServicesDetailsVO>();
            //if (grdDocShare.SelectedItem != null)
            //{
            //    ObjList.Add((clsDoctorShareServicesDetailsVO)grdDocShare.SelectedItem);
            //    child.DoctorShareList = ObjList;
            //}

            clsDoctorShareServicesDetailsVO ObjDetails = new clsDoctorShareServicesDetailsVO();
            ObjDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            ObjDetails.TariffName = ((MasterListItem)cmbTariff.SelectedItem).Description;
            ObjDetails.SpecializationID = ((MasterListItem)cmbSpec.SelectedItem).ID;
            ObjDetails.SpecializationName = ((MasterListItem)cmbSpec.SelectedItem).Description;
            ObjDetails.SubSpecializationId = ((MasterListItem)cmbSubSpec.SelectedItem).ID;
            ObjDetails.SubSpecializationName = ((MasterListItem)cmbSubSpec.SelectedItem).Description;
            ObjDetails.ModalityID = ((MasterListItem)cmbMod.SelectedItem).ID;
            ObjDetails.Modality = ((MasterListItem)cmbMod.SelectedItem).Description;
            ObjDetails.DoctorId = DID;
            ObjDetails.DoctorName = txtReferenceDoctor.Text;
            ObjList.Add(ObjDetails);
            child.DoctorShareList = ObjList;
            child.OnSaveButton_Click += new RoutedEventHandler(child_OnSaveButton_Click);
            child.OnCancelButton_Click += new RoutedEventHandler(child_OnCancelButton_Click);
            child.Show();
            // }
        }

        private void cmdBack_Click(object sender, RoutedEventArgs e)
        {
            //SetCommandButtonState("Cancel");
            this.DataContext = null;
            ClearControl();
            //  FetchData();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Imaging Center Configuration";
                mElement.Text = "Billing Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmClinicConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        //private void rdbSharePercent_Checked(object sender, RoutedEventArgs e)
        //{
        //    txtSharePercent.IsEnabled = true;
        //}

        //private void rdbSharePercent_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    txtSharePercent.IsEnabled = false;
        //}


        private void txtSearchCriteria_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                grdDocShare.SelectedIndex = 0;               
                FetchData();               
            }
        }
        
    }
}
