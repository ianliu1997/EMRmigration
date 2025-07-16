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
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Windows.Controls.Data;
using System.ComponentModel;
using System.Windows.Browser;
using System.Globalization;
using System.Windows.Data;
using PalashDynamics.Animations;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.CRM.LoyaltyProgram;
using System.IO;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.CRM
{
    public partial class LoyaltyProgram : UserControl
    {

        #region Paging

        public PagedSortableCollectionView<clsLoyaltyProgramVO> DataList { get; private set; }

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



        #endregion
        
        public LoyaltyProgram()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.DataContext = new clsLoyaltyProgramVO();
            DataList = new PagedSortableCollectionView<clsLoyaltyProgramVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            this.dgDataPager.DataContext = DataList;
            this.dgLoyaltyProgramList.DataContext = DataList;
            FetchData();

            
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }

        #region Variable Declaration
        private SwivelAnimation objAnimation;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public List<clsLoyaltyProgramFamilyDetails> FamilyList { get; set; }
        public List<clsLoyaltyAttachmentVO> AttachmentList { get; set; }
        FileInfo Attachment;
        byte[] data;
        clsLoyaltyAttachmentVO AttachmentVO { get; set; }
        clsLoyaltyProgramFamilyDetails FamilyVO { get; set; }
        long AppRelationID { get; set; }
        bool MainTabTariff = false;
        bool FamilyTabTariff = false;
        bool IsNew = false;
        bool IsUpdate = true;
        int ClickedFlag = 0;
        #endregion

        private void LoyaltyProgram_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                CheckValidation();
                SetCommandButtonState("New");
                FillPatientCategory();
                FillTariff();
                FillFamilyMember();
                FillApplicableTariff();
                SetComboboxValue();

                FillClinicGrid();
                FillPatientCategoryGrid();
                FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                AttachmentList = new List<clsLoyaltyAttachmentVO>();
                AppRelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID;
                
                    
               //txtProgram.Focus();
                Indicatior.Close();
            }
            txtProgram.Focus();
            txtProgram.UpdateLayout();
            IsPageLoded = true;
        
        }

        #region FillCombobox
        private void FillPatientCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
 
            BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
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

                    //cmbPatientCategory.ItemsSource = null;
                    //cmbPatientCategory.ItemsSource = objList;
                    //cmbPatientCategory.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    //cmbPatientCategory.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).PatientCategoryID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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

                    cmbTariff.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).TariffID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillFamilyMember()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
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

                    cmbFamilyMember.ItemsSource = null;
                    cmbFamilyMember.ItemsSource = objList;

                    cmbFamilyMember.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbFamilyMember.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).MemberRelationID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillApplicableTariff()
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

                    cmbApplicableTariff.ItemsSource = null;
                    cmbApplicableTariff.ItemsSource = objList;

                    cmbApplicableTariff.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbApplicableTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).MemberTariffID;
                    

                }
                            
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
    

        #endregion

        private void FillClinicGrid()
        {
            clsGetLoyaltyClinicBizActionVO BizAction = new clsGetLoyaltyClinicBizActionVO();
            BizAction.ClinicList = new List<clsLoyaltyClinicVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgClinic.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetLoyaltyClinicBizActionVO)arg.Result).ClinicList != null)
                    {
                        BizAction.ClinicList = ((clsGetLoyaltyClinicBizActionVO)arg.Result).ClinicList;
                        List<clsLoyaltyClinicVO> ClinicList = new List<clsLoyaltyClinicVO>();

                        foreach (var item in BizAction.ClinicList)
                        {
                            ClinicList.Add(new clsLoyaltyClinicVO()
                            {
                                LoyaltyUnitID=item.LoyaltyUnitID,
                                LoyaltyUnitDescription=item.LoyaltyUnitDescription,
                                Status = false
                                
                            });
                        }

                        dgClinic.ItemsSource = ClinicList;

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

        private void FillPatientCategoryGrid()
        {
            clsGetLoyaltyPatientCategoryBizActionVO BizAction = new clsGetLoyaltyPatientCategoryBizActionVO();
            BizAction.CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPatientCatagories.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetLoyaltyPatientCategoryBizActionVO)arg.Result).CategoryList != null)
                    {
                        BizAction.CategoryList = ((clsGetLoyaltyPatientCategoryBizActionVO)arg.Result).CategoryList;
                        List<clsLoyaltyProgramPatientCategoryVO> CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();

                        foreach (var item in BizAction.CategoryList)
                        {
                            CategoryList.Add(new clsLoyaltyProgramPatientCategoryVO()
                            {
                               PatientCategoryID=item.PatientCategoryID,
                               Description=item.Description,
                               Status = false

                            });
                        }

                        dgPatientCatagories.ItemsSource = CategoryList;

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
        private void SetComboboxValue()
        {
            //cmbPatientCategory.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).PatientCategoryID;
            cmbTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).TariffID;
            cmbApplicableTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).MemberTariffID;
        }
         
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            this.SetCommandButtonState("Save");
            this.DataContext = new clsLoyaltyProgramVO();
            FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
            AttachmentList = new List<clsLoyaltyAttachmentVO>();
            ClearControl();
            IsUpdate = true;
       

            IsNew = true;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Loyalty Program";
            TabControlMain.SelectedIndex = 0;

            if (chkFamily.IsChecked == false)
            {
                tabFamilyDetails.Visibility = Visibility.Collapsed;
            }

            objAnimation.Invoke(RotationType.Forward);

            txtProgramName.Focus();



           
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            txtProgramName.ClearValidationError();
            dtpExpiryDate.ClearValidationError();
            dtpEffectiveDate.ClearValidationError();

            ClearControl();

            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";
            objAnimation.Invoke(RotationType.Backward);

        }

        /// <summary>
        /// Purpose:Save New loyalty program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {

                bool SaveLoyaltyProgram = true;
                SaveLoyaltyProgram = CheckValidation();
                if (SaveLoyaltyProgram == true)
                {

                    string msgTitle = "";
                    string msgText = "Are you sure you want to save the Loyalty Program";

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
                if (CheckDuplicasy())
                {
                    SaveLoyaltyProgram();
                }
            }
            else
                ClickedFlag = 0;
        }

        private void SaveLoyaltyProgram()
        {
            clsAddLoyaltyProgramBizActionVO BizAction = new clsAddLoyaltyProgramBizActionVO();
            BizAction.LoyaltyProgramDetails = (clsLoyaltyProgramVO)this.DataContext;
            
            if (cmbTariff.SelectedItem != null)
                BizAction.LoyaltyProgramDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
   
            BizAction.LoyaltyProgramDetails.ClinicList=(List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
            BizAction.LoyaltyProgramDetails.CategoryList = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
            if (FamilyList !=null && FamilyList.Count > 0)
            {
                BizAction.LoyaltyProgramDetails.FamilyList = FamilyList;
            }
            if (AttachmentList != null && AttachmentList.Count > 0)
            {
                BizAction.LoyaltyProgramDetails.AttachmentList = AttachmentList;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null)
                {
                    SetCommandButtonState("New");
                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();
                 
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Loyalty Program Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Loyalty Program .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void hlTariff_Click(object sender, RoutedEventArgs e)
        {
            LoyaltyProgramNewTariff Win = new LoyaltyProgramNewTariff();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
            MainTabTariff = true;
   

            

        }

        private void hlViewTariff_Click(object sender, RoutedEventArgs e)
        {
            LoyaltyProgramViewTariff WinView = new LoyaltyProgramViewTariff();

            if (((MasterListItem)cmbTariff.SelectedItem).ID != 0)
            {

                WinView.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                WinView.Show();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please select tariff.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }
        }
      
        /// <summary>
        /// Purpose:Modify existing Loyalty program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            bool ModifyLoyaltyProgram = true;
            ModifyLoyaltyProgram = CheckValidation();
            if (ModifyLoyaltyProgram == true)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Update the Loyalty Program";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

                msgW1.Show();

            }
           
        }

        void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (CheckDuplicasy())
                {
                    ModifyLoyaltyProgram();
                }
            }

        }

        private void ModifyLoyaltyProgram()
        {
            clsUpdateLoyaltyProgramBizActionVO BizAction = new clsUpdateLoyaltyProgramBizActionVO();
            BizAction.LoyaltyProgram = (clsLoyaltyProgramVO)this.DataContext;
            
            if (cmbTariff.SelectedItem != null)
                BizAction.LoyaltyProgram.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            BizAction.LoyaltyProgram.CategoryList = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
            BizAction.LoyaltyProgram.ClinicList = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;

            if (FamilyList != null && FamilyList.Count > 0)
            {
                BizAction.LoyaltyProgram.FamilyList = FamilyList;
            }

            if (AttachmentList != null && AttachmentList.Count > 0)
            {
                BizAction.LoyaltyProgram.AttachmentList = AttachmentList;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    SetCommandButtonState("New");
                    FetchData();
                    ClearControl();
                    objAnimation.Invoke(RotationType.Backward);
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Loyalty Program Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Updating Loyalty Program .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void CmdClose1_Click(object sender, RoutedEventArgs e)
        {

            objAnimation.Invoke(RotationType.Backward);
     
        }

        /// <summary>
        /// Purpose:Getting List of loyalty program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {

                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {

                    dtpFromDate.SetValidation("From Date should be greater than To Date");
                    dtpFromDate.RaiseValidationError();

                    string strMsg = "From Date should be greater than To Date";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                  
                    dtpFromDate.ClearValidationError();
                }
            }
            if (dtpFromDate.SelectedDate > DateTime.Now.Date.Date)
            {
                dtpFromDate.SetValidation("From Date should not be greater than today's date");
                dtpFromDate.RaiseValidationError();

                string strMsg = "From Date should not be greater than today's date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }
            else
            {


                dtpFromDate.ClearValidationError();
            }
            
            if (dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
            {

                dtpFromDate.SetValidation("To Date should not be less than From Date");
                dtpFromDate.RaiseValidationError();

                string strMsg = "To Date should not be less than From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }

            else
            {

                dtpToDate.ClearValidationError();
            }
            



            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Plase Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();

                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
              
                FetchData();
            }
           
        }

        private void FetchData()
        {
            clsGetLoyaltyProgramListBizActionVO BizAction = new clsGetLoyaltyProgramListBizActionVO();
            BizAction.LoyaltyProgramList = new List<clsLoyaltyProgramVO>();

            if (txtProgram.Text != null)
                BizAction.LoyaltyProgramName = txtProgram.Text;

            BizAction.FromDate = dtpFromDate.SelectedDate;
            BizAction.ToDate = dtpToDate.SelectedDate;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
         
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetLoyaltyProgramListBizActionVO)arg.Result).LoyaltyProgramList != null)
                    {
                        //dgLoyaltyProgramList.ItemsSource = ((clsGetLoyaltyProgramListBizActionVO)arg.Result).LoyaltyProgramList;


                        clsGetLoyaltyProgramListBizActionVO result = arg.Result as clsGetLoyaltyProgramListBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.LoyaltyProgramList != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.LoyaltyProgramList)
                            {
                                DataList.Add(item);
                            }

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
        
        /// <summary>
        ///Purpose:View Loyalty program details. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void hlbViewLoyaltyProgram_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Modify");
            IsNew = false;


            clsLoyaltyProgramVO objLoyaltyProgram = new clsLoyaltyProgramVO();

            if ((clsLoyaltyProgramVO)dgLoyaltyProgramList.SelectedItem != null) ;
            {
                objLoyaltyProgram = ((clsLoyaltyProgramVO)dgLoyaltyProgramList.SelectedItem);
                FillLoyaltyProgram(objLoyaltyProgram.ID);
            }

            IsUpdate = false;
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + objLoyaltyProgram.LoyaltyProgramName;
            TabControlMain.SelectedIndex = 0;
            objAnimation.Invoke(RotationType.Forward);

        }

        private void FillLoyaltyProgram(long iID)
        {
            try
            {
                clsGetLoyaltyProgramByIDBizActionVO BizAction = new clsGetLoyaltyProgramByIDBizActionVO();
                BizAction.LoyaltyProgramDetails = new clsLoyaltyProgramVO();
                BizAction.ID = iID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                AttachmentList = new List<clsLoyaltyAttachmentVO>();
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        SetCommandButtonState("Modify");
                        if (dgLoyaltyProgramList.SelectedItem != null)
                            objAnimation.Invoke(RotationType.Forward);
                        clsGetLoyaltyProgramByIDBizActionVO ObjList = new clsGetLoyaltyProgramByIDBizActionVO();
                        ObjList = (clsGetLoyaltyProgramByIDBizActionVO)arg.Result;
                        this.DataContext = null;
                        this.DataContext = ObjList.LoyaltyProgramDetails;
                        //((clsLoyaltyProgramVO)this.DataContext).ID = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.ID;
                        //((clsLoyaltyProgramVO)this.DataContext).LoyaltyProgramName = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.LoyaltyProgramName;
                        //((clsLoyaltyProgramVO)this.DataContext).EffectiveDate = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.EffectiveDate;
                        //((clsLoyaltyProgramVO)this.DataContext).ExpiryDate = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.ExpiryDate;
                        //((clsLoyaltyProgramVO)this.DataContext).Tariff = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.Tariff;
                        //((clsLoyaltyProgramVO)this.DataContext).Remark = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.Remark;
                        //((clsLoyaltyProgramVO)this.DataContext).Status = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.Status;
                        //((clsLoyaltyProgramVO)this.DataContext).IsFamily = ((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails.IsFamily;

                        ((clsLoyaltyProgramVO)this.DataContext).ID = ObjList.LoyaltyProgramDetails.ID;
                        ((clsLoyaltyProgramVO)this.DataContext).LoyaltyProgramName = ObjList.LoyaltyProgramDetails.LoyaltyProgramName;
                        ((clsLoyaltyProgramVO)this.DataContext).EffectiveDate = ObjList.LoyaltyProgramDetails.EffectiveDate;
                        ((clsLoyaltyProgramVO)this.DataContext).ExpiryDate = ObjList.LoyaltyProgramDetails.ExpiryDate;
                        ((clsLoyaltyProgramVO)this.DataContext).Tariff = ObjList.LoyaltyProgramDetails.Tariff;
                        ((clsLoyaltyProgramVO)this.DataContext).Remark = ObjList.LoyaltyProgramDetails.Remark;
                        ((clsLoyaltyProgramVO)this.DataContext).Status = ObjList.LoyaltyProgramDetails.Status;
                        ((clsLoyaltyProgramVO)this.DataContext).IsFamily = ObjList.LoyaltyProgramDetails.IsFamily;
                        cmbTariff.SelectedValue = ObjList.LoyaltyProgramDetails.TariffID;



                        if (ObjList.LoyaltyProgramDetails.CategoryList != null)
                        {
                            List<clsLoyaltyProgramPatientCategoryVO> lstCategory = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
                            foreach (var item1 in ObjList.LoyaltyProgramDetails.CategoryList)
                            {
                                foreach (var item in lstCategory)
                                {
                                    if (item.PatientCategoryID == item1.PatientCategoryID)
                                    {
                                        item.Status = item1.Status;
                                    }

                                }
                            }
                            dgPatientCatagories.ItemsSource = null;
                            dgPatientCatagories.ItemsSource = lstCategory;
                        }

                        if (ObjList.LoyaltyProgramDetails.ClinicList != null)
                        {
                            List<clsLoyaltyClinicVO> lstClinic = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
                            foreach (var item1 in ObjList.LoyaltyProgramDetails.ClinicList)
                            {
                                foreach (var item in lstClinic)
                                {
                                    if (item.LoyaltyUnitID == item1.LoyaltyUnitID)
                                    {
                                        item.Status = item1.Status;
                                    }

                                }
                            }

                            dgClinic.ItemsSource = null;
                            dgClinic.ItemsSource = lstClinic;
                        }

                        if (ObjList.LoyaltyProgramDetails.FamilyList != null)
                        {
                            List<clsLoyaltyProgramFamilyDetails> Family;
                            Family = ObjList.LoyaltyProgramDetails.FamilyList;
                            foreach (var item4 in Family)
                            {
                                FamilyList.Add(item4);
                            }
                            dgFamilyList.ItemsSource = FamilyList;
                            dgFamilyList.Focus();
                            dgFamilyList.UpdateLayout();

                        }

                        if (ObjList.LoyaltyProgramDetails.AttachmentList != null)
                        {
                            List<clsLoyaltyAttachmentVO> Attachment;
                            Attachment = ObjList.LoyaltyProgramDetails.AttachmentList;
                            foreach (var item5 in Attachment)
                            {
                                AttachmentList.Add(item5);
                            }
                            dgAttachmentList.ItemsSource = AttachmentList;
                            dgAttachmentList.Focus();
                            dgAttachmentList.UpdateLayout();
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

        List<clsLoyaltyProgramPatientCategoryVO> lst = new List<clsLoyaltyProgramPatientCategoryVO>();
        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == true)
                {

                    lst = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.Status = true;
                        }
                        dgPatientCatagories.ItemsSource = null;
                        dgPatientCatagories.ItemsSource = lst;

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

                    lst = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.Status = false;
                        }
                        dgPatientCatagories.ItemsSource = null;
                        dgPatientCatagories.ItemsSource = lst;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        List<clsLoyaltyClinicVO> lstClinic = new List<clsLoyaltyClinicVO>();
        private void chkSelectAllClinic_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAllClinic.IsChecked == true)
                {

                    lstClinic = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lstClinic)
                        {
                            item.Status = true;
                        }
                        dgClinic.ItemsSource = null;
                        dgClinic.ItemsSource = lstClinic;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void chkSelectAllClinic_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAllClinic.IsChecked == false)
                {

                    lstClinic = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lstClinic)
                        {
                            item.Status = false;
                        }
                        dgClinic.ItemsSource = null;
                        dgClinic.ItemsSource = lstClinic;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;

          

            if (txtProgramName.Text == "")
            {
                txtProgramName.SetValidation("Please Enter Program Name");
                txtProgramName.RaiseValidationError();
                txtProgramName.Focus();
                result = false;
            }
            else
                txtProgramName.ClearValidationError();


            if (IsPageLoded)
            {

                if (IsUpdate == true)
                {
                    if (((clsLoyaltyProgramVO)this.DataContext).EffectiveDate < DateTime.Today)
                    {

                        //dtpEffectiveDate.SetValidation("Effective Date can not be less than Today");
                        //dtpEffectiveDate.RaiseValidationError();

                        MessageBoxControl.MessageBoxChildWindow msgW11 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Effective Date can not be less than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW11.Show();
                        dtpEffectiveDate.Focus();
                        result = false;
                        return result;
                    }
                    else if (dtpEffectiveDate.SelectedDate == null)
                    {
                        //dtpEffectiveDate.SetValidation("Please Select EffectiveDate ");
                        //dtpEffectiveDate.RaiseValidationError();
                        MessageBoxControl.MessageBoxChildWindow msgW11 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select EffectiveDate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW11.Show();
                        dtpEffectiveDate.Focus();
                        result = false;
                        return result;
                    }
                    else
                        dtpEffectiveDate.ClearValidationError();
                }


                if (dtpExpiryDate.SelectedDate < dtpEffectiveDate.SelectedDate)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW12 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry Date can not be less than effective date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW12.Show();
                    //dtpExpiryDate.SetValidation("Expiry Date can not be less than effective date");
                    //dtpExpiryDate.RaiseValidationError();
                    dtpExpiryDate.Focus();
                    result = false;
                    return result;
                }
                else if (dtpExpiryDate.SelectedDate == null)
                {
                    //dtpExpiryDate.SetValidation("Please Select Expiry Date ");
                    //dtpExpiryDate.RaiseValidationError();

                    MessageBoxControl.MessageBoxChildWindow msgW13 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Expiry Date .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW13.Show();

                    dtpExpiryDate.Focus();
                    result = false;
                    return result;
                }
                else
                    dtpExpiryDate.ClearValidationError();



                if ((MasterListItem)cmbTariff.SelectedItem == null)
               {

                   cmbTariff.TextBox.SetValidation("Please Select Tariff");
                   cmbTariff.TextBox.RaiseValidationError();
                   cmbTariff.Focus();
                   result = false;

               }
               else if (((MasterListItem)(cmbTariff.SelectedItem)).ID ==0)
                {

                    cmbTariff.TextBox.SetValidation("Please Select Tariff");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    result = false;

                }
                else
                    cmbTariff.ClearValidationError();


                if (txtProgramName.Text == "")
                {
                    txtProgramName.SetValidation("Please Enter Program Name");
                    txtProgramName.RaiseValidationError();
                    txtProgramName.Focus();
                    result = false;
                }
                else
                    txtProgramName.ClearValidationError();

               //bool Chk = false;
               //List<clsLoyaltyProgramPatientCategoryVO> lList = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
               //foreach (var item in lList)
               //{
               //    if (item.Status == false)
               //    {
               //        Chk = true;
               //    }
               //    else
               //    {
               //        Chk = false;
               //        break;
               //    }

               //}
               //if (Chk == true)
               //{
               //    string msgTitle = "";
               //    string msgText = "Please Select Patient Category";

               //    MessageBoxControl.MessageBoxChildWindow msgW =
               //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

               //    msgW.Show();
               //    result = false;
               //}

               //bool ChkClinic = false;
               //List<clsLoyaltyClinicVO> lList1 = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
               //foreach (var item1 in lList1)
               //{
               //    if (item1.Status == false)
               //    {
               //        ChkClinic = true;
               //    }
               //    else
               //    {
               //        ChkClinic = false;
               //        break;
               //    }

               //}
               //if (ChkClinic == true)
               //{
               //    string msgTitle = "";
               //    string msgText = "Please Select clinic";

               //    MessageBoxControl.MessageBoxChildWindow msgW =
               //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

               //    msgW.Show();
               //    result = false;
               //}

               if (chkFamily.IsChecked == true)
               {
                   if (dgFamilyList.ItemsSource == null)
                   {
                       string msgTitle = "";
                       string msgText = "Please enter family details";

                       MessageBoxControl.MessageBoxChildWindow msgW =
                           new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                       msgW.Show();
                       result = false;
                       return result;

                   }
               }
            }

            return result;
        }

        private void txtProgram_LostFocus(object sender, RoutedEventArgs e)
        {
            txtProgram.Text = txtProgram.Text.ToTitleCase();
        }

        private void txtProgramName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtProgramName.Text = txtProgramName.Text.ToTitleCase();
            
        }

        #endregion

        #region Reset All Controls
        private void ClearControl()
        {
            txtProgramName.Text = string.Empty;
            dtpEffectiveDate.SelectedDate = null;
            dtpExpiryDate.SelectedDate = null;
            cmbTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).TariffID;
            txtRemark.Text = string.Empty;
            chkFamily.IsChecked = false;

            List<clsLoyaltyProgramPatientCategoryVO> lList = (List<clsLoyaltyProgramPatientCategoryVO>)dgPatientCatagories.ItemsSource;
            if (lList != null)
            {
                foreach (var item in lList)
                {
                    item.Status = false;

                }
                dgPatientCatagories.ItemsSource = null;
                dgPatientCatagories.ItemsSource = lList;
            }


            List<clsLoyaltyClinicVO> lList2 = (List<clsLoyaltyClinicVO>)dgClinic.ItemsSource;
            if (lList2 != null)
            {
                foreach (var item1 in lList2)
                {
                    item1.Status = false;

                }
                dgClinic.ItemsSource = null;
                dgClinic.ItemsSource = lList2;
            }

            dgFamilyList.ItemsSource = null;
            cmbApplicableTariff.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).MemberTariffID;
            cmbFamilyMember.SelectedValue = ((clsLoyaltyProgramVO)this.DataContext).MemberRelationID;

            dgAttachmentList.ItemsSource = null;
            txtDocumentName.Text = string.Empty;
            txtFilePath.Text = string.Empty;
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

        #region Family Details
        private void chkFamily_Checked(object sender, RoutedEventArgs e)
        {
            if (chkFamily.IsChecked == true)
            {
                tabFamilyDetails.Visibility = Visibility.Visible;
            }
        }
        private void chkFamily_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkFamily.IsChecked == false)
            {
                tabFamilyDetails.Visibility = Visibility.Collapsed;
            }

        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            bool IsExists = false;
            if (((MasterListItem)cmbFamilyMember.SelectedItem).ID != 0 && ((MasterListItem)cmbApplicableTariff.SelectedItem).ID != 0)
            {
                if (((MasterListItem)cmbFamilyMember.SelectedItem).ID != AppRelationID)
                {
                    if (FamilyList.Count == 0)
                    {
                        FamilyList.Add(new clsLoyaltyProgramFamilyDetails
                        {

                            RelationID = ((MasterListItem)cmbFamilyMember.SelectedItem).ID,
                            Relation = ((MasterListItem)cmbFamilyMember.SelectedItem).Description,
                            TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID,
                            Tariff = ((MasterListItem)cmbApplicableTariff.SelectedItem).Description,
                            Status = true
                        }
                            );

                    }

                    else
                    {
                        for (int i = 0; i < FamilyList.Count; i++)
                        {
                            if (FamilyList[i].RelationID.Equals(((MasterListItem)cmbFamilyMember.SelectedItem).ID))
                            {
                                IsExists = true;
                                break;
                            }
                        }
                        if (IsExists == false)
                        {
                            FamilyList.Add(new clsLoyaltyProgramFamilyDetails
                            {
                                RelationID = ((MasterListItem)cmbFamilyMember.SelectedItem).ID,
                                Relation = ((MasterListItem)cmbFamilyMember.SelectedItem).Description,
                                TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID,
                                Tariff = ((MasterListItem)cmbApplicableTariff.SelectedItem).Description,
                                Status = true
                            }
                            );


                        }
                        else
                        {
                            string msgTitle = "";
                            string msgText = "Family member already exists";

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWD.Show();
                        }
                    }

                    dgFamilyList.ItemsSource = FamilyList;
                    dgFamilyList.Focus();
                    dgFamilyList.UpdateLayout();
                    dgFamilyList.SelectedIndex = FamilyList.Count - 1;

                }
            }


        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            
            if (((MasterListItem)cmbFamilyMember.SelectedItem).ID != 0 && ((MasterListItem)cmbApplicableTariff.SelectedItem).ID != 0)
            {
                if (((MasterListItem)cmbFamilyMember.SelectedItem).ID != AppRelationID)
                {
                    if (FamilyList.Count > 0)
                    {
                        int var = dgFamilyList.SelectedIndex;
                        FamilyList.RemoveAt(dgFamilyList.SelectedIndex);
                        
                            FamilyList.Insert(var, new clsLoyaltyProgramFamilyDetails
                            {
                                RelationID = ((MasterListItem)cmbFamilyMember.SelectedItem).ID,
                                Relation = ((MasterListItem)cmbFamilyMember.SelectedItem).Description,
                                TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID,
                                Tariff = ((MasterListItem)cmbApplicableTariff.SelectedItem).Description,
                                Status = true

                            }
                            );
                        
                        dgFamilyList.ItemsSource = FamilyList;
                        dgFamilyList.Focus();
                        dgFamilyList.UpdateLayout();
                        dgFamilyList.SelectedIndex = FamilyList.Count - 1;



                    }
                }
            }


        }

        private void cmdDeleteMember_Click(object sender, RoutedEventArgs e)
        {

            if (dgFamilyList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected member ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        FamilyList.RemoveAt(dgFamilyList.SelectedIndex);
                        dgFamilyList.Focus();
                        dgFamilyList.UpdateLayout();
                        dgFamilyList.SelectedIndex = FamilyList.Count - 1;
                    }
                };
                msgWD.Show();
            }
        }

        private void hlbEditMember_Click(object sender, RoutedEventArgs e)
        {
            cmbFamilyMember.SelectedValue = FamilyVO.RelationID;
            cmbApplicableTariff.SelectedValue = FamilyVO.TariffID;
        }

        private void dgFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsLoyaltyProgramFamilyDetails)dgFamilyList.SelectedItem != null)
            {
                FamilyVO = new clsLoyaltyProgramFamilyDetails();
                FamilyVO = (clsLoyaltyProgramFamilyDetails)dgFamilyList.SelectedItem;
            }
        }
        #endregion

        #region Attachment Details
        private clsLoyaltyAttachmentVO CreateData()
        {
            clsLoyaltyAttachmentVO ObjAttachment = new clsLoyaltyAttachmentVO();
            ObjAttachment.DocumentName = txtDocumentName.Text.Trim();

            if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            {

                ObjAttachment.AttachmentFileName = txtFilePath.Text.Trim();
                ObjAttachment.Attachment = data;
            }
            return ObjAttachment;
        }
        int DoubleClickedFlag = 0;
        private void cmdAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            DoubleClickedFlag += 1;
            bool IsExists = false;
       
            if (txtDocumentName.Text == "" )
            {
                txtDocumentName.SetValidation("Please Enter Document Name");
                txtDocumentName.RaiseValidationError();
                txtDocumentName.Focus();

            }
            else if (txtFilePath.Text == "")
            {
                txtFilePath.SetValidation("Please Attach File");
                txtFilePath.RaiseValidationError();
                txtFilePath.Focus();

            }
            else
            {
                txtDocumentName.ClearValidationError();
                txtFilePath.ClearValidationError();

                if (AttachmentList.Count > 0)
                {
                    for (int i = 0; i < AttachmentList.Count; i++)
                    {
                        if (AttachmentList[i].DocumentName == txtDocumentName.Text || AttachmentList[i].AttachmentFileName == (txtFilePath.Text))
                        {
                            IsExists = true;
                            break;
                        }
                    }
                }
                if (DoubleClickedFlag == 1)
                {
                    if (IsExists == false)
                    {
                        //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                        //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                        
                        Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                        DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                        client.AttachmentFileCompleted += (s, args) =>
                        {
                            DoubleClickedFlag = 0;
                            if (args.Error == null)
                            {
                                if (txtDocumentName.Text != "" && txtFilePath.Text != "")
                                {
                                    for (int i = 0; i <= AttachmentList.Count; i++)
                                    {
                                        AttachmentList.Add(new clsLoyaltyAttachmentVO
                                        {
                                            DocumentName = txtDocumentName.Text,
                                            AttachmentFileName = txtFilePath.Text,
                                            Attachment = data,
                                            Status = true
                                        }
                                        );
                                        break;
                                    }
                                    dgAttachmentList.ItemsSource = AttachmentList;
                                    dgAttachmentList.Focus();
                                    dgAttachmentList.UpdateLayout();
                                    dgAttachmentList.SelectedIndex = AttachmentList.Count - 1;
                                    DoubleClickedFlag = 0;

                                    //txtDocumentName.Text = string.Empty;
                                    //txtFilePath.Text = string.Empty;
                                }
                            }
                            else
                            {

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while adding the attachment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                msgW1.Show();
                                DoubleClickedFlag = 0;
                            }


                        };
                        client.AttachmentFileAsync(txtFilePath.Text, data);
                    }

                    else
                    {
                        string msgTitle = "";
                        string msgText = "Attachment is already exists";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWD.Show();
                        DoubleClickedFlag = 0;
                    }
                }
                //}
            }
            
        }

        private void cmdEditAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (txtDocumentName.Text == "" )
            {
                txtDocumentName.SetValidation("Please Enter Document Name");
                txtDocumentName.RaiseValidationError();
                txtDocumentName.Focus();

            }
            else if (txtFilePath.Text == "")
            {
                txtFilePath.SetValidation("Please Add Attachment File");
                txtFilePath.RaiseValidationError();
                txtFilePath.Focus();

            }
            else
            {
                txtDocumentName.ClearValidationError();
                txtFilePath.ClearValidationError();
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);


                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.AttachmentFileCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        if (txtDocumentName.Text != "" && txtFilePath.Text != "")
                        {
                            if (AttachmentList.Count > 0)
                            {
                                int var = dgAttachmentList.SelectedIndex;
                                AttachmentList.RemoveAt(dgAttachmentList.SelectedIndex);
                                AttachmentList.Insert(var, new clsLoyaltyAttachmentVO
                                {
                                    DocumentName = txtDocumentName.Text,
                                    AttachmentFileName = txtFilePath.Text,
                                    Attachment = data,
                                    Status = true
                                }
                                );
                                dgAttachmentList.ItemsSource = AttachmentList;
                                dgAttachmentList.Focus();
                                dgAttachmentList.UpdateLayout();
                                dgAttachmentList.SelectedIndex = AttachmentList.Count - 1;
                            }
                        }
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while adding the attachment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }


                };
                client.AttachmentFileAsync(txtFilePath.Text, data);
            }
        }
          
        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
           OpenFileDialog OpenFile = new OpenFileDialog();
           bool? showdialog = OpenFile.ShowDialog();
           if (showdialog == true)
           {
            
                try
                {
                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        if (stream.Length < 5120000)
                        {
                            data = new byte[stream.Length];
                            stream.Read(data, 0, (int)stream.Length);
                            Attachment = OpenFile.File;
                            txtFilePath.Text = OpenFile.File.Name;
                        }
                        else
                        {
                            MessageBox.Show("File must be less than 5 MB");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
        }

        private void hlbViewAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgLoyaltyProgramList.SelectedItem == null)
            {
                clsLoyaltyAttachmentVO ObjAttachment =CreateData();
                if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
                }
               
            }
            else
            {
                string FileName="";
                bool Flag = false;
                for (int i = 0; i < AttachmentList.Count; i++)
                {
                    FileName = AttachmentList[i].AttachmentFileName;
                    data = AttachmentList[i].Attachment;
                    Flag = true;
                }
                if (Flag == true)
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { FileName });
                }

            }



        }

        private void hlbEditAttachment_Click(object sender, RoutedEventArgs e)
        {
            txtDocumentName.Text = AttachmentVO.DocumentName;
            txtFilePath.Text = AttachmentVO.AttachmentFileName;
        }

        private void dgAttachmentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsLoyaltyAttachmentVO)dgAttachmentList.SelectedItem != null)
            {
                AttachmentVO = new clsLoyaltyAttachmentVO();
                AttachmentVO = (clsLoyaltyAttachmentVO)dgAttachmentList.SelectedItem;
            }
        }

        

        private void cmdDeleteAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgAttachmentList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected attachment ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        AttachmentList.RemoveAt(dgAttachmentList.SelectedIndex);
                        dgAttachmentList.Focus();
                        dgAttachmentList.UpdateLayout();
                        dgAttachmentList.SelectedIndex = AttachmentList.Count - 1;
                    }
                };
                msgWD.Show();
            }

        }
        #endregion

        private void hlNewApplicableTariff_Click(object sender, RoutedEventArgs e)
        {
            LoyaltyProgramNewTariff Win = new LoyaltyProgramNewTariff();
            Win.OnAddButton_Click += new RoutedEventHandler(Win_OnAddButton_Click);
            Win.Show();
            FamilyTabTariff = true;
        }

        private void hlViewApplicableTariff_Click(object sender, RoutedEventArgs e)
        {
            LoyaltyProgramViewTariff WinView = new LoyaltyProgramViewTariff();
          
            if (((MasterListItem)cmbApplicableTariff.SelectedItem).ID != 0)
            {

                WinView.TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID;
               
                WinView.Show();
                
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Please select tariff.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }
            
        }

        void Win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((LoyaltyProgramNewTariff)sender).DialogResult == true)
            {
                if (MainTabTariff == true)
                {
                    ((clsLoyaltyProgramVO)this.DataContext).TariffID = ((LoyaltyProgramNewTariff)sender).NewTariff;
                    FillTariff();
                    FillApplicableTariff();
                    MainTabTariff = false;
                    

                }
                else if (FamilyTabTariff == true)
                {
                    ((clsLoyaltyProgramVO)this.DataContext).MemberTariffID = ((LoyaltyProgramNewTariff)sender).NewTariff;
                    FillTariff();
                    FillApplicableTariff();
                    FamilyTabTariff = false;
                }
            }
        }

        private void ViewTariff(long iTariffID)
        {
            

        }
        private bool CheckDuplicasy()
        {
            clsLoyaltyProgramVO Prog;

            if (IsNew)
            {
                Prog = ((PagedSortableCollectionView<clsLoyaltyProgramVO>)dgLoyaltyProgramList.ItemsSource).FirstOrDefault(p => p.LoyaltyProgramName.ToUpper().Equals(txtProgramName.Text.ToUpper()));
                
            }
            else
            {
                Prog = ((PagedSortableCollectionView<clsLoyaltyProgramVO>)dgLoyaltyProgramList.ItemsSource).FirstOrDefault(p => p.LoyaltyProgramName.ToUpper().Equals(txtProgramName.Text.ToUpper()) && p.ID != ((clsLoyaltyProgramVO)dgLoyaltyProgramList.SelectedItem).ID);
                
            }

            if (Prog != null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Record cannot be save because Loyalty Program Name already exist!.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return false;
            }
           
            else
            {
                return true;
            }
        }
     

        

        #region Commented code

        //if (dgLoyaltyProgramList.SelectedItem != null)
        //{
        //    try
        //    {
        //        clsGetLoyaltyProgramAttachmentByIdBizActionVO BizAction = new clsGetLoyaltyProgramAttachmentByIdBizActionVO();
        //        BizAction.AttachmentDetails = new clsLoyaltyAttachmentVO();
        //        BizAction.ID = ((clsLoyaltyAttachmentVO)dgAttachmentList.SelectedItem).ID;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //        client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null)
        //            {
        //                if (((clsGetLoyaltyProgramAttachmentByIdBizActionVO)arg.Result).AttachmentDetails != null)
        //                {
        //                    clsLoyaltyAttachmentVO ObjAttachment = ((clsGetLoyaltyProgramAttachmentByIdBizActionVO)arg.Result).AttachmentDetails;
        //                    txtDocumentName.Text = ObjAttachment.DocumentName;
        //                    txtFilePath.Text = ObjAttachment.AttachmentFileName;
        //                    data = ObjAttachment.Attachment;


        //                }
        //            }
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


         //clsGetLoyaltyProgramFamilyByIdBizActionVO BizAction = new clsGetLoyaltyProgramFamilyByIdBizActionVO();
         //   BizAction.FamilyDetails =new clsLoyaltyProgramFamilyDetails();
         //   BizAction.ID=((clsLoyaltyProgramFamilyDetails)dgFamilyList.SelectedItem).ID;
             
         //   Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
         //   PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

         //   client.ProcessCompleted += (s, arg) =>
         //   {
         //       if (arg.Error == null && arg.Result != null)
         //       {
         //           if (((clsGetLoyaltyProgramFamilyByIdBizActionVO)arg.Result).FamilyDetails != null)
         //           {
         //               clsLoyaltyProgramFamilyDetails ObjFamily = ((clsGetLoyaltyProgramFamilyByIdBizActionVO)arg.Result).FamilyDetails;
         //               cmbFamilyMember.SelectedValue = ObjFamily.RelationID;
         //               cmbApplicableTariff.SelectedValue = ObjFamily.TariffID;


         //           }
         //       }
         //   };
         //   client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
         //   client.CloseAsync();


        #endregion

    }
}
