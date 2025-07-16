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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Administration.IPD;
using PalashDynamics.Administration;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using System.Reflection;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;

namespace OPDModule.Forms
{
    public partial class PatientDietPlan : UserControl,IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    else if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }
                    else
                    {
                       

                        IsPatientExist = true;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                        mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;

                        pnlPersonalDetails.DataContext = ((IApplicationConfiguration)App.Current).SelectedPatient;
                        cmbGender.SelectedItem = (((IApplicationConfiguration)App.Current).SelectedPatient).Gender;

                        txtYY.Text = ConvertDate(((clsPatientGeneralVO)pnlPersonalDetails.DataContext).DateOfBirth, "YY");
                        txtMM.Text = ConvertDate(((clsPatientGeneralVO)pnlPersonalDetails.DataContext).DateOfBirth, "MM");
                        txtDD.Text = ConvertDate(((clsPatientGeneralVO)pnlPersonalDetails.DataContext).DateOfBirth, "DD");

                    }
                    break;


            }
        }

        #endregion

        public PatientDietPlan()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            DataList = new PagedSortableCollectionView<clsPatientDietPlanVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
        }

        #region Variable Declaration
        public bool IsPatientExist = false;

        SwivelAnimation objAnimation;
        ObservableCollection<clsPatientDietPlanDetailVO> FoodItemList{ get; set; }
        bool IsCancel = true;

        long VisitDoctorID { get; set; }
        long VisitID { get; set; }
   

        #endregion

        #region Paging

        public PagedSortableCollectionView<clsPatientDietPlanVO> DataList { get; private set; }


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


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //FetchData();

        }
        #endregion

        #region FillComboBox
        private void FillPlan()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DietPlanMaster;
            BizAction.IsActive = true;
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

                    cmbPlan.ItemsSource = null;
                    cmbPlan.ItemsSource = objList;
                    cmbPlan.SelectedItem = objList[0];
                   

                    if (this.DataContext != null)
                    {
                        cmbPlan.SelectedValue = ((clsPatientDietPlanVO)this.DataContext).PlanID;
                    }
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
            BizAction.IsActive = true;
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

                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                   
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

            }
            else
            {
                SetCommandButtonState("Load");
                this.DataContext = new clsPatientDietPlanVO()
                {
                    Date=DateTime.Now
                };
            
               
                FoodItemList = new ObservableCollection<clsPatientDietPlanDetailVO>();
                FillPlan();
                CheckVisit();
            }

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            
            ClearData();
            
            if (VisitID > 0)
            {
                objAnimation.Invoke(RotationType.Forward);
                SetCommandButtonState("New");
            }
            
        }

        #region SaveData
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to save the Patient Diet?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Save();
        }
        private void Save()
        {
            clsAddPatientDietPlanBizActionVO BizAction = new clsAddPatientDietPlanBizActionVO();
            try
            {
                BizAction.DietPlan = (clsPatientDietPlanVO)this.DataContext;
                BizAction.DietPlan.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.DietPlan.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.DietPlan.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                BizAction.DietPlan.VisitDoctorID = VisitDoctorID;

                if (cmbPlan.SelectedItem != null)
                    BizAction.DietPlan.PlanID = ((MasterListItem)cmbPlan.SelectedItem).ID;

                BizAction.DietPlan.DietDetails = FoodItemList.ToList();
                BizAction.DietPlan.GeneralInformation = richTextEditor.Html;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Save");
                    if (arg.Error == null)
                    {
                        if (((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan != null)
                        {
                            ClearData();
                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Diet plan added successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                    if (((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan.ID > 0)
                                    {
                                        Print(((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan.ID);
                                    }
                                }
                            };
                            msgW1.Show();
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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ModifyData
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (chkValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to update the Patient Diet?";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();
            }
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Modify();
        }
        private void Modify()
        {
            clsAddPatientDietPlanBizActionVO BizAction = new clsAddPatientDietPlanBizActionVO();
            try
            {
                BizAction.DietPlan = (clsPatientDietPlanVO)this.DataContext;
                BizAction.DietPlan.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.DietPlan.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.DietPlan.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                if (cmbPlan.SelectedItem != null)
                    BizAction.DietPlan.PlanID = ((MasterListItem)cmbPlan.SelectedItem).ID;

                BizAction.DietPlan.DietDetails = FoodItemList.ToList();
                BizAction.DietPlan.GeneralInformation = richTextEditor.Html;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    SetCommandButtonState("Modify");
                    if (arg.Error == null)
                    {
                        if (((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan != null)
                        {
                            ClearData();
                            FetchData();
                            objAnimation.Invoke(RotationType.Backward);
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Diet plan Updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                    if (((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan.ID > 0)
                                    {
                                        Print(((clsAddPatientDietPlanBizActionVO)arg.Result).DietPlan.ID);
                                    }
                                }
                            };
                            msgW1.Show();

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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");

            if (IsCancel == true)
            {

                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

            }
            else
            {
                IsCancel = true;
            }
            objAnimation.Invoke(RotationType.Backward);
        }

        #region View data
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            SetCommandButtonState("View");
            if (dgPlan.SelectedItem != null)
            {
                this.DataContext = (clsPatientDietPlanVO)dgPlan.SelectedItem;
                cmbPlan.SelectedValue = ((clsPatientDietPlanVO)dgPlan.SelectedItem).PlanID;
                richTextEditor.Html = ((clsPatientDietPlanVO)dgPlan.SelectedItem).GeneralInformation;
                FillFood(((clsPatientDietPlanVO)dgPlan.SelectedItem).ID);
                TabDietPlan.SelectedIndex = 0;
                objAnimation.Invoke(RotationType.Forward);
            }
           
        }
        
        private void FillFood(long iID)
        {
            clsGetPatientDietPlanDetailsBizActionVO BizAction = new clsGetPatientDietPlanDetailsBizActionVO();
            BizAction.ID = iID;
            BizAction.DietDetailsList = new List<clsPatientDietPlanDetailVO>();
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetPatientDietPlanDetailsBizActionVO)arg.Result).DietDetailsList != null)
                        {
                            List<clsPatientDietPlanDetailVO> objlist = ((clsGetPatientDietPlanDetailsBizActionVO)arg.Result).DietDetailsList;
                           
                            foreach (var item in objlist)
                            {
                                FoodItemList.Add(item);

                            }

                            PagedCollectionView collection = new PagedCollectionView(FoodItemList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("FoodItemCategory"));
                            dgFoodList.ItemsSource = collection;
                            dgFoodList.UpdateLayout();
                            dgFoodList.Focus();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Data
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

      

        #endregion

        #region Get food list from food master
        private void FillFoodItemList(long iID)
        {
            clsGeDietPlanMasterBizActionVO BizAction = new clsGeDietPlanMasterBizActionVO();
            BizAction.PlanID = iID;
            BizAction.DietDetailList = new List<clsPatientDietPlanDetailVO>();
            try
            {   Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGeDietPlanMasterBizActionVO)arg.Result).DietDetailList != null)
                        {
                            List<clsPatientDietPlanDetailVO> objlist = ((clsGeDietPlanMasterBizActionVO)arg.Result).DietDetailList;
                            if (((clsGeDietPlanMasterBizActionVO)arg.Result).GeneralInfo != null)
                                richTextEditor.Html = ((clsGeDietPlanMasterBizActionVO)arg.Result).GeneralInfo;
                            foreach (var item in objlist)
                            {
                                FoodItemList.Add(item);
                                
                            }

                            PagedCollectionView collection = new PagedCollectionView(FoodItemList);
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("FoodItemCategory"));
                            dgFoodList.ItemsSource = collection;
                            dgFoodList.UpdateLayout();
                            dgFoodList.Focus();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                            msgW1.Show();
                        }

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private void cmbPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cmdSearchPlan_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPlan.SelectedItem != null && ((MasterListItem)cmbPlan.SelectedItem).ID > 0)
            {
                FoodItemList = new ObservableCollection<clsPatientDietPlanDetailVO>();
                dgFoodList.ItemsSource = null;

                FillFoodItemList(((MasterListItem)cmbPlan.SelectedItem).ID);
            }
            else
            {
                FoodItemList = new ObservableCollection<clsPatientDietPlanDetailVO>();
                dgFoodList.ItemsSource = null;
            }
        }
        #endregion

        #region Clear UI
        private void ClearData()
        {
            this.DataContext = new clsPatientDietPlanVO()
            {
                Date=DateTime.Now
            };
            FoodItemList = new ObservableCollection<clsPatientDietPlanDetailVO>();
            dgFoodList.ItemsSource = null;
            cmbPlan.SelectedValue = (long)0;
            richTextEditor.Html = "";
            
        }
        #endregion

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

        #region Add food items
        private void btnSelectFood_Click(object sender, RoutedEventArgs e)
        {
            frmFoodItemList win = new frmFoodItemList();
            win.OnAddButton_Click += new RoutedEventHandler(AddFood_OnAddButton_Click);
            win.Show();
        }
        
        void AddFood_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmFoodItemList)sender).DialogResult == true)
            {
                for (int i = 0; i < ((frmFoodItemList)sender).check.Count; i++)
                {
                    if (((frmFoodItemList)sender).check[i] == true)
                    {
                        var item1 = from r in FoodItemList
                                    where (r.FoodItemID == (((frmFoodItemList)sender).FoodItemSource[i]).ID
                                    && r.FoodItemCategoryID == (((frmFoodItemList)sender).FoodItemSource[i]).ItemID)
                                    select new clsPatientDietPlanDetailVO
                                    {
                                        FoodItemID = r.FoodItemID
                                       
                                    };
                        if (item1.ToList().Count == 0)
                        {

                            clsPatientDietPlanDetailVO objDetails = new clsPatientDietPlanDetailVO();
                            objDetails.FoodItemID = (((frmFoodItemList)sender).FoodItemSource[i].ID);
                            objDetails.FoodItem = (((frmFoodItemList)sender).FoodItemSource[i].Description);
                            objDetails.FoodItemCategoryID = (((frmFoodItemList)sender).FoodItemSource[i].ItemID);
                            objDetails.FoodItemCategory = (((frmFoodItemList)sender).FoodItemSource[i].ItemName);
                            FoodItemList.Add(objDetails);
                        }
                        else
                        {
                            string strMsg =  "Food "+ ((frmFoodItemList)sender).FoodItemSource[i].Description+" already exists";
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                }
                PagedCollectionView collection = new PagedCollectionView(FoodItemList);
                collection.GroupDescriptions.Add(new PropertyGroupDescription("FoodItemCategory"));
                dgFoodList.ItemsSource = collection;
                dgFoodList.UpdateLayout();
                dgFoodList.Focus();
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgFoodList.SelectedItem != null)
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        FoodItemList.RemoveAt(dgFoodList.SelectedIndex);
                        dgFoodList.Focus();
                        dgFoodList.UpdateLayout();
                        dgFoodList.SelectedIndex = FoodItemList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        #endregion

        #region Fetch data

        private void FetchData()
        {
            clsGetPatientDietPlanBizActionVO BizAction = new clsGetPatientDietPlanBizActionVO();
            try
            {
                BizAction.DietList = new List<clsPatientDietPlanVO>();

                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null )
                    {
                        if (((clsGetPatientDietPlanBizActionVO)arg.Result).DietList != null)
                        {
                            clsGetPatientDietPlanBizActionVO result = arg.Result as clsGetPatientDietPlanBizActionVO;

                            DataList.TotalItemCount = result.TotalRows;

                            if (result.DietList != null)
                            {
                                DataList.Clear();

                                foreach (var item in result.DietList)
                                {
                                    DataList.Add(item);
                                }

                                dgPlan.ItemsSource = null;
                                dgPlan.ItemsSource = DataList;

                                dataPager.Source = null;
                                dataPager.PageSize = BizAction.MaximumRows;
                                dataPager.Source = DataList;

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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Validation
        private bool chkValidation()
        {
            bool result = true;

            if (cmbPlan.SelectedItem == null)
            {
                cmbPlan.TextBox.SetValidation("Please select Plan");
                cmbPlan.TextBox.RaiseValidationError();
                result = false;
                cmbPlan.Focus();
            }
            else if (((MasterListItem)cmbPlan.SelectedItem).ID == 0)
            {
                cmbPlan.TextBox.SetValidation("Please select Plan");
                cmbPlan.TextBox.RaiseValidationError();
                result = false;
                cmbPlan.Focus();
            }
            else
                cmbPlan.TextBox.RaiseValidationError();


            if (dgFoodList.ItemsSource == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("", "You can not save diet plan without food items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                result = false;
                return result;
                TabDietPlan.SelectedIndex = 0;
                return result;
            }

            if (FoodItemList.Where(Items => Items.Timing == null).Any() == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter timing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW14.Show();
                TabDietPlan.SelectedIndex = 0;
                result = false;
                return result;
            }
            else if (FoodItemList.Where(Items => Items.Timing == "").Any() == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW14 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter timing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW14.Show();
                TabDietPlan.SelectedIndex = 0;
                result = false;
                return result;
            }



            //if (richTextEditor.Html == "")
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                   new MessageBoxControl.MessageBoxChildWindow("", "Please enter general information", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //    result = false;
            //    return result;
            //    TabDietPlan.SelectedIndex = 1;
            //    return result;
            //}


            return result;
        }

        #endregion

        #region Print 


        private void Print(long iID)
        {
            if (iID > 0)
            {
                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                string URL = "../Reports/OPD/PatientDietReport.aspx?ID=" + iID + "&UnitID=" + UnitID; ;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlan.SelectedItem != null)
            {
                if (((clsPatientDietPlanVO)dgPlan.SelectedItem).ID > 0)
                {
                    Print(((clsPatientDietPlanVO)dgPlan.SelectedItem).ID);
                }
            }
        }
        #endregion

        #region Patient information
        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            PatientSearch winObj = new PatientSearch();
            winObj.VisitWise = true;

            winObj.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            winObj.Show();
        }
        
        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                {

                    pnlPersonalDetails.DataContext = ((IApplicationConfiguration)App.Current).SelectedPatient;

                    CheckVisit();
                   
                    cmbGender.SelectedItem = (((IApplicationConfiguration)App.Current).SelectedPatient).Gender;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                }
               
            }
            catch (Exception ex)
            {
                throw;
               
            }

        }

        #endregion

        #region Calculate age from birth date
        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;

                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }
        #endregion

        private void CheckVisit()
        {
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
      
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.GetLatestVisit = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {
                       
                      
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.VisitID > 0)
                        {
                            VisitDoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                            VisitID = ((clsGetVisitBizActionVO)arg.Result).Details.ID;

                            if (VisitID > 0)
                            {
                                FetchData();
                            }
                            TabDietPlan.SelectedIndex = 0;
                          
                        }
                    }
                    else
                    {
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit is not marked for the Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                       
                        return;
                       
                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

    }
}
