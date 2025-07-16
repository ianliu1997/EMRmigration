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
using System.ComponentModel;
using CIMS;
using PalashDynamics.Animations;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
namespace PalashDynamics.Administration
{
    public partial class frmAutoCharges : UserControl
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        #region  Variables
        private SwivelAnimation objAnimation;
        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        private long UnitId;
        int PageIndex = 0;
        public PagedSortableCollectionView<clsIPDAutoChargesVO> MasterList { get; private set; }
        bool IsCancel = true;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        List<clsIPDAutoChargesVO> AddServiceList = null;
        #endregion

        #region Properties

        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }

        #endregion

        public frmAutoCharges()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmAutoCharges_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            SetCommandButtonAuto("Load");
            MasterList = new PagedSortableCollectionView<clsIPDAutoChargesVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);

            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdAutoChargesDetails.DataContext = MasterList;
            FillUnitList();

            FillUnitforSearch();


            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);

        }

        #region Set Command Button State New/Save/Modify/Cancel
        private void SetCommandButtonAuto(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdAdd.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "Modify":
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "FrontPanel":

                    cmdAdd.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    IsCancel = true;

                    break;
                case "View":
                    cmdSave.IsEnabled = false;
                    cmdAdd.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }
        #endregion

        void frmAutoCharges_Loaded(object sender, RoutedEventArgs e)
        {
            AddServiceList = new List<clsIPDAutoChargesVO>();
            if (grdAutoCharges != null)
            {
                if (grdAutoCharges.Columns.Count > 0)
                {
                    grdAutoCharges.Columns[0].Header = "Service Name";
                }
            }
            if (grdAutoChargesDetails != null)
            {
                if (grdAutoChargesDetails.Columns.Count > 0)
                {
                    grdAutoChargesDetails.Columns[0].Header = "Service Name";
                }
            }
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            PageIndex = dataGrid2Pager.PageIndex;
        }

        private void cmdDeleteService_Click(object sender, RoutedEventArgs e)
        {
            DeleteService();
        }

        public void DeleteService()
        {
            clsDeleteServiceBizActionVO BizAction = new clsDeleteServiceBizActionVO();
            try
            {
                if (grdAutoCharges.SelectedItem != null)
                {
                    clsIPDAutoChargesVO objDischarge = (clsIPDAutoChargesVO)grdAutoCharges.SelectedItem;
                    BizAction.Id = objDischarge.Id;

                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    Client1.ProcessCompleted += (s, arg) =>
                    {

                        if (arg.Error == null && arg.Result != null)
                        {
                            msgText = "Service Deleted successfully.";
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                                GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
                        }
                        else
                        {

                        }
                    };
                    Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            grdAutoChargesDetails.DataContext = new clsIPDAutoChargesVO();
            SetCommandButtonAuto("New");

            grdAutoChargesDetails.ItemsSource = null;
            AddServiceList = new List<clsIPDAutoChargesVO>();
            FillService();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                GetAutoCharges(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {


            if (Validation())
            {

                msgText = "Are you sure you want to Save ?";
                MessageBoxControl.MessageBoxChildWindow SavemsgWin =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                SavemsgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(SavemsgWin_OnMessageBoxClosed);
                SavemsgWin.Show();
            }
        }

        private bool Validation()
        {
            if (cmbListUnit1.ItemsSource == null)
            {
                msgText = "Please select Unit";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbListUnit1.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            if (cmbListUnit1.SelectedItem == null)
            {
                msgText = "Please select Unit";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbListUnit1.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            if (((MasterListItem)cmbListUnit1.SelectedItem).ID == 0)
            {
                msgText = "Please select Unit";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbListUnit1.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }

            if (cmbListUnit1.SelectedItem == null)
            {
                msgText = "Please select Service";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbServicePackage.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            if (((MasterListItem)cmbServicePackage.SelectedItem).ID == 0)
            {
                msgText = "Please select Service";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbServicePackage.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            cmbListUnit1.ClearValidationError();
            cmbServicePackage.ClearValidationError();
            cmbServicePackage.BorderBrush = new SolidColorBrush(Colors.Gray);
            cmbListUnit1.BorderBrush = new SolidColorBrush(Colors.Gray);

            return true;
        }
        void SavemsgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                clsAddIPDAutoChargesServiceListBizActionVO bizactionVO = new clsAddIPDAutoChargesServiceListBizActionVO();
                clsIPDAutoChargesVO AddChargesVO = new clsIPDAutoChargesVO();
                try
                {
                    if (cmbListUnit1.SelectedItem != null)
                    {
                        long unitID = Convert.ToInt64(((MasterListItem)cmbListUnit1.SelectedItem).ID);
                        if (unitID > 0)
                        {
                            bizactionVO.UnitId = ((MasterListItem)cmbListUnit1.SelectedItem).ID;
                        }
                    }
                    bizactionVO.ChargesMasterList.AddRange(AddServiceList);
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {

                        if (args.Error == null && args.Result != null)
                        {
                            msgText = "Record Saved successfully.";
                            SetCommandButtonAuto("Save");
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                                GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
                            UnitId = 0;
                        }
                    };
                    client.ProcessAsync(bizactionVO, new clsUserVO());
                    client.CloseAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }


        private void GetAutoChargesServiceList(long unitID)
        {
            try
            {
                clsGetIPDAutoChargesServiceListBizActionVO BizAction = new clsGetIPDAutoChargesServiceListBizActionVO();
                BizAction.GetChargesMasterList = new List<clsIPDAutoChargesVO>();
                BizAction.UnitId = unitID;
                BizAction.SearchExpression = cmbListUnit1.SearchText;
                BizAction.PagingEnabled = true;
                BizAction.MaximumRows = MasterList.PageSize;
                BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.GetChargesMasterList = (((clsGetIPDAutoChargesServiceListBizActionVO)args.Result).GetChargesMasterList);
                        MasterList.Clear();
                        MasterList.TotalItemCount = (int)(((clsGetIPDAutoChargesServiceListBizActionVO)args.Result).TotalRows);
                        foreach (clsIPDAutoChargesVO item in BizAction.GetChargesMasterList)
                        {
                            MasterList.Add(item);
                        }
                        grdAutoCharges.ItemsSource = null;
                        grdAutoCharges.ItemsSource = MasterList;
                        dataGrid2Pager.Source = null;
                        grdAutoCharges.SelectedItem = null;
                    }
                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonAuto("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Admission Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmIPDAdmissionConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            MasterList = new PagedSortableCollectionView<clsIPDAutoChargesVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dataGrid2Pager.DataContext = MasterList;
            this.grdAutoCharges.DataContext = MasterList;
            if (cmbListUnit.ItemsSource != null)
            {
                if (cmbListUnit.SelectedItem != null)
                {
                    if (((MasterListItem)cmbListUnit.SelectedItem).ID > 0)
                    {
                        GetAutoChargesServiceList(((MasterListItem)cmbListUnit.SelectedItem).ID);
                    }
                    else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                        GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                    GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                GetAutoChargesServiceList(((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID);
        }


        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            AddService();

        }
        private void FillServices()
        {
            try
            {
                clsGetServiceListByUnitIDBizActionVO BizActionObj = new clsGetServiceListByUnitIDBizActionVO();
                BizActionObj.ServiceList = new List<clsServiceMasterVO>();
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetServiceListByUnitIDBizActionVO result = args.Result as clsGetServiceListByUnitIDBizActionVO;

                        if (result.ServiceList != null)
                        {
                            BizActionObj.ServiceList = result.ServiceList.Where(S => S.HealthPlan.Equals(true)).ToList<clsServiceMasterVO>();
                            BizActionObj.ServiceList = (from x in result.ServiceList
                                                        where x.HealthPlan == false && x.BasePackage == false
                                                        select x).ToList<clsServiceMasterVO>();

                            if (BizActionObj.ServiceList != null)
                            {
                                List<MasterListItem> objList = new List<MasterListItem>();
                                objList.Add(new MasterListItem(0, "- Select -"));
                                foreach (var item in BizActionObj.ServiceList)
                                {
                                    objList.Add(new MasterListItem(item.ID, item.Description));
                                }
                                cmbServicePackage.ItemsSource = null;
                                cmbServicePackage.ItemsSource = objList;
                                cmbServicePackage.SelectedValue = objList[0].ID;
                            }
                        }
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void AddService()
        {
            if (cmbListUnit1.ItemsSource != null)
            {
                if (cmbListUnit1.SelectedItem != null)
                {
                    long unitID = Convert.ToInt64(((MasterListItem)cmbListUnit1.SelectedItem).ID);
                    if (unitID > 0)
                    {
                        if (CheckDuplicasyPre())
                        {
                            if (cmbServicePackage.SelectedItem != null)
                            {
                                if (((MasterListItem)cmbServicePackage.SelectedItem).ID > 0)
                                {
                                    clsIPDAutoChargesVO objAdd = new clsIPDAutoChargesVO();
                                    objAdd.Service = ((MasterListItem)cmbServicePackage.SelectedItem).Description;
                                    objAdd.ServiceId = ((MasterListItem)cmbServicePackage.SelectedItem).ID;
                                    AddServiceList.Add(objAdd);
                                    grdAutoChargesDetails.ItemsSource = null;
                                    grdAutoChargesDetails.ItemsSource = AddServiceList;


                                }

                                else
                                {
                                    msgText = "Please Select Service.";
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    cmbServicePackage.BorderBrush = new SolidColorBrush(Colors.Red);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Unit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    cmbListUnit1.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }

        }

        private bool CheckDuplicasyPre()
        {
            PalashDynamics.ValueObjects.Administration.IPD.clsIPDAutoChargesVO Item;
            if (grdAutoChargesDetails.ItemsSource != null)
            {
                if (cmbServicePackage.SelectedItem != null)
                {

                    long id = ((MasterListItem)cmbServicePackage.SelectedItem).ID;
                    Item = ((List<clsIPDAutoChargesVO>)grdAutoChargesDetails.ItemsSource).FirstOrDefault(p => p.ServiceId.Equals(id));
                    if (Item != null)
                    {
                        msgText = "Service already exists.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
                else
                    return true;

            }
            else
                return true;
        }

        private void cmbListUnit1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbListUnit1.ItemsSource != null)
            {
                if (cmbListUnit1.SelectedItem != null)
                {
                    long unitID = Convert.ToInt64(((MasterListItem)cmbListUnit1.SelectedItem).ID);
                    if (unitID > 0)
                    {
                        GetAutoCharges(((MasterListItem)cmbListUnit1.SelectedItem).ID);
                    }
                }
            }
        }

        private void GetAutoCharges(long unitID)
        {
            clsGetIPDAutoChargesServiceListBizActionVO BizAction = new clsGetIPDAutoChargesServiceListBizActionVO();
            BizAction.GetChargesMasterList = new List<clsIPDAutoChargesVO>();
            BizAction.UnitId = unitID;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            try
            {

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.GetChargesMasterList = (((clsGetIPDAutoChargesServiceListBizActionVO)args.Result).GetChargesMasterList);
                        AddServiceList = new List<clsIPDAutoChargesVO>();
                        foreach (clsIPDAutoChargesVO item in BizAction.GetChargesMasterList)
                        {
                            clsIPDAutoChargesVO objAdd = new clsIPDAutoChargesVO();
                            objAdd.ServiceId = item.ServiceId;
                            objAdd.Service = item.Description;
                            AddServiceList.Add(objAdd);
                        }
                        grdAutoChargesDetails.ItemsSource = null;
                        grdAutoChargesDetails.ItemsSource = AddServiceList;
                    }

                };
                client.ProcessAsync(BizAction, User);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private void FillUnitList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
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

                    cmbListUnit1.ItemsSource = null;
                    cmbListUnit1.ItemsSource = objList;

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                    {
                        cmbListUnit1.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                    }
                    else
                    {
                        cmbListUnit1.SelectedItem = objList[0];

                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillUnitforSearch()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- All --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbListUnit.ItemsSource = null;
                    cmbListUnit.ItemsSource = objList;

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID > 0)
                    {
                        cmbListUnit.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                        cmbListUnit.SelectedItem = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillService()
        {
            try
            {
                clsGetServiceMasterListBizActionVO objVo = new clsGetServiceMasterListBizActionVO();
                objVo.GetServicesForTariffMaster = true;
                objVo.ServiceList = new List<clsServiceMasterVO>();
                objVo.ServiceMaster = new clsServiceMasterVO();
                objVo.ServiceMaster.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                cmbServicePackage.ItemsSource = null;
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((PalashDynamics.ValueObjects.Administration.clsGetServiceMasterListBizActionVO)arg.Result).ServiceList != null)
                        {
                            objVo.ServiceList = ((PalashDynamics.ValueObjects.Administration.clsGetServiceMasterListBizActionVO)arg.Result).ServiceList;
                            cmbServicePackage.ItemsSource = objVo.ServiceList;
                            cmbServicePackage.SelectedItem = objVo.ServiceList.Where(z => z.ID == 0).FirstOrDefault();
                            //cmbServicePackage.SelectedValue = ((MasterListItem)objVo.MasterList.Where(z => z.ID == 0).FirstOrDefault()).ID;
                        }
                    }
                    else
                    {
                        msgText = "Error occurred while processing.";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }

                };
                client.ProcessAsync(objVo, User);// new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

