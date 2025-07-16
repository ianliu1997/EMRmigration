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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Resources;


namespace PalashDynamics.Administration
{
    public partial class VisitTypeMaster : UserControl, INotifyPropertyChanged
    {

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


        #region VariableDeclaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public bool IsFree = false;
        public int NumberOfDays = 0;

        public PagedSortableCollectionView<clsVisitTypeVO> DataList { get; private set; }

        bool IsCancel = true;
        #endregion

        private void VisitTypeMaster_Loaded(object sender, RoutedEventArgs e)
        {
            grdList.DataContext = new clsVisitTypeVO();
            FillService();
         //   FillVisitType();
            FillVisitTypeMaster();
            //FetchData();
        }

        public VisitTypeMaster()
        {
            InitializeComponent();
            SetCommandButtonState("Load");
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));


            DataList = new PagedSortableCollectionView<clsVisitTypeVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;

            this.dgVisitList.DataContext = DataList;
            //this.dataGrid2Pager.DataContext = DataList;
            
            FetchData();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }
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
                OnPropertyChanged("DataListPageSize");
            }
        }

        private void FillVisitTypeMaster()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_VisitTypeMaster;
            BizAction.Parent = new KeyValue { Key = true, Value = "Status" };
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
                    cmbVisitType.ItemsSource = null;
                    cmbVisitType.ItemsSource = objList;
                    //cmbVisitType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupVisitTypeID;
                }

                if (this.DataContext != null)
                {
                    cmbVisitType.SelectedValue = ((clsVisitTypeVO)this.DataContext).ID;
                    //if (IsFreeFollowup == true)
                    //{
                       // cmbVisitType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupVisitTypeID;
                    //}
                    //else
                    //    cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;

                    //if (((clsVisitVO)this.DataContext).VisitTypeID == 0)
                    //{
                    //    cmbVisitType.TextBox.SetValidation("Please select the Visit Type");

                    //    cmbVisitType.TextBox.RaiseValidationError();
                    //}

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }


        //private void FillVisitType()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_VisitTypeMaster;
        //    BizAction.MasterList = new List<MasterListItem>();
        //    BizAction.IsActive = true;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            cmbVisitType.ItemsSource = null;
        //            cmbVisitType.ItemsSource = objList;


        //        }

        //        if (this.DataContext != null)
        //        {
        //            cmbVisitType.SelectedValue = ((clsVisitTypeVO)this.DataContext).ID;
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();

        //}


        private void FillService()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ServiceMaster;
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsActive = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbService.ItemsSource = null;
                    cmbService.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbService.SelectedValue = ((clsVisitTypeVO)this.DataContext).ID;
                }
             };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("New");
            this.DataContext = new clsVisitTypeVO();
            ClearControl();
            CheckValidations();
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Visit Type";
            objAnimation.Invoke(RotationType.Forward);
        }

        private void FetchData()
        {
            clsGetAllVisitTypeMasetrBizActionVO BizAction = new clsGetAllVisitTypeMasetrBizActionVO();
            BizAction.List = new List<clsVisitTypeVO>();
            BizAction.SearchExpression = txtSearch.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if ((clsGetAllVisitTypeMasetrBizActionVO)arg.Result != null)
                    {

                        //dgVisitList.ItemsSource = ((clsGetAllVisitTypeMasetrBizActionVO)arg.Result).List;


                        clsGetAllVisitTypeMasetrBizActionVO result = arg.Result as clsGetAllVisitTypeMasetrBizActionVO;

                        if (result.List != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetAllVisitTypeMasetrBizActionVO)arg.Result).TotalRows;
                            foreach (clsVisitTypeVO item in result.List)
                            {
                                DataList.Add(item);
                            }
                            
                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.Source = DataList;                            
                        }                     
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
                txtSearch.Focus();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool SaveVisit = true;

            //SaveVisit = CheckValidations();

            SaveVisit = CheckValidations();

            if (SaveVisit == true)
            {

                string msgTitle = "";
                string msgText = "Are you sure,you want to save Visit Type Master?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();

            }

            else {
                
                CheckValidations(); }
            
        }

       private bool CheckValidations()
        {
            bool Result = true;

            if (txtDescription.Text.Trim() == "")
            {

                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError(); 
                txtDescription.Focus();

                Result = false;
            }
            else
                txtDescription.ClearValidationError();

            if (txtCode.Text.Trim() == "")
            {
                txtCode.SetValidation("Please Enter Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();

                Result = false;
            }
            else
                txtCode.ClearValidationError();

            return Result;
        }


        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
       
     
        private void Save()
        {        
            clsAddVisitTypeBizActionVO BizAction = new clsAddVisitTypeBizActionVO();
            BizAction.Details = (clsVisitTypeVO)grdList.DataContext;

            if (cmbService.SelectedItem != null)
            {
                BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
            }

            BizAction.Details.IsFree = IsFree;
            BizAction.Details.FreeDaysDuration = Convert.ToInt64(txtNoOfDays.Text);
            BizAction.Details.ConsultationVisitType = ((MasterListItem)cmbVisitType.SelectedItem).ID;
          

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsAddVisitTypeBizActionVO)arg.Result).SuccessStatus == 1)
                    {
                        FetchData();
                        ClearControl();
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Visit Type Master Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        SetCommandButtonState("Save");
                    }

                    else if (((clsAddVisitTypeBizActionVO)arg.Result).SuccessStatus == 2)
                    {
                       
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because CODE is Duplicate!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW2.Show();
                    }
                    else if (((clsAddVisitTypeBizActionVO)arg.Result).SuccessStatus == 3)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be save because DESCRIPTION is Duplicate!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW2.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyVisit = true;
            ModifyVisit = CheckValidations();
            if (ModifyVisit == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Visit Type Master?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
            else {
                
                CheckValidations(); }

        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }

        private void Modify()
        {
            clsAddVisitTypeBizActionVO BizAction = new clsAddVisitTypeBizActionVO();
            BizAction.Details = (clsVisitTypeVO)grdList.DataContext;
            BizAction.Details.ID = ((clsVisitTypeVO)dgVisitList.SelectedItem).ID;
           
            if (cmbService.SelectedItem != null)
            {
                BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
            }
            if (cmbVisitType.SelectedItem != null)
            {
                BizAction.Details.ConsultationVisitType = ((MasterListItem)cmbVisitType.SelectedItem).ID;
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    if (((clsAddVisitTypeBizActionVO)arg.Result).SuccessStatus == 1)
                    {

                        FetchData();
                        ClearControl();
                        objAnimation.Invoke(RotationType.Backward);



                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Visit Type Master Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();

                        SetCommandButtonState("Modify");
                    }

                    else if (((clsAddVisitTypeBizActionVO)arg.Result).SuccessStatus == 2)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Record cannot be update because CODE is Duplicate!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW2.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            objAnimation.Invoke(RotationType.Backward);

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Patient Configuration";  

                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmPatientConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);

            }
            else
            {
                IsCancel = true;
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCode.Text = txtCode.Text.ToTitleCase();
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescription.Text = txtDescription.Text.ToTitleCase();
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdModify.IsEnabled = false;
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
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    cmdModify.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                   
                case "View":
                    cmdModify.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dgVisitList.SelectedItem != null)
            {
                clsAddVisitTypeBizActionVO BizAction = new clsAddVisitTypeBizActionVO();
                BizAction.Details = (clsVisitTypeVO)dgVisitList.SelectedItem;
                BizAction.Details.ID = ((clsVisitTypeVO)dgVisitList.SelectedItem).ID;
                BizAction.Details.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);

                if (cmbService.SelectedItem != null)
                {
                    BizAction.Details.ServiceID = ((MasterListItem)cmbService.SelectedItem).ID;
                }


                 Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Update the Visit Type Master?";

                    MessageBoxControl.MessageBoxChildWindow msgW5 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW5.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW5_OnMessageBoxClosed);
                    msgW5.Show();
                  

                }
            }; 
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

            }

        }

        void msgW5_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Status Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW2.Show();
            }
               
        }

        private void hlbViewVisitTypeMaster_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("View");
            ClearControl();
            if (((clsVisitTypeVO)dgVisitList.SelectedItem).Status == false)
                cmdModify.IsEnabled = false;
            else { cmdModify.IsEnabled = true; }
            if (dgVisitList.SelectedItem != null)
            {
                grdList.DataContext = ((clsVisitTypeVO)dgVisitList.SelectedItem).DeepCopy() ;
                cmbService.SelectedValue = ((clsVisitTypeVO)dgVisitList.SelectedItem).ServiceID;
                cmbVisitType.SelectedValue = ((clsVisitTypeVO)dgVisitList.SelectedItem).ConsultationVisitType;
                objAnimation.Invoke(RotationType.Forward);
            }
        }

        private void ClearControl()
        {
            //txtCode.Text = "";
            //txtDescription.Text = "";
            //cmbService.SelectedValue = ((clsVisitTypeVO)this.DataContext).ServiceID;
            //chkIsClinical.IsChecked = false;

            grdList.DataContext = new clsVisitTypeVO();
            cmbService.SelectedValue = ((clsVisitTypeVO)grdList.DataContext).ServiceID;
            cmbVisitType.SelectedValue = ((clsVisitTypeVO)grdList.DataContext).ConsultationVisitType;

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsVisitTypeVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            FetchData();
            dgVisitList.ItemsSource = null;
            dgVisitList.ItemsSource = DataList;           
        }

        private void txtSearchCriteria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FetchData();
            }
        }

        private void txtNoOfDays_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chkIsFree_Checked(object sender, RoutedEventArgs e)
        {
            IsFree = true;
            txtNoOfDays.IsEnabled = true;
            cmbVisitType.IsEnabled = true;
        }

        private void chkIsFree_Unchecked(object sender, RoutedEventArgs e)
        {
            IsFree = false;
            txtNoOfDays.IsEnabled = false;
            cmbVisitType.IsEnabled = false;
        }

        private void cmbVisitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
