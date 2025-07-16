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
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Collections;

namespace OPDModule.Forms
{
    public partial class LoyaltyProgramWindow : ChildWindow
    {
        public LoyaltyProgramWindow()
        {
            InitializeComponent();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;


        }
        #region Paging

        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }

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

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();

        }
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public bool UnIssue=true;
        public clsPatientGeneralVO ObjPatient = new clsPatientGeneralVO();
        public clsPatientFamilyDetailsVO SelectedMember { get; set; }

        public event RoutedEventHandler OnSaveButton_Click;
        
        private void LoyaltyProgramWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsPatientVO();
                FillLoyaltyProgram();
                FillUnitList();
                SetComboboxValue();
                FetchData();
                dtpFromDate.Focus();
                Indicatior.Close();

            }
            IsPageLoded = true;
            dtpFromDate.Focus();
            dtpFromDate.UpdateLayout();

        }

        #region FillCombobox
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

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        #endregion

        private void SetComboboxValue()
        {
            cmbClinic.SelectedValue = (clsPatientVO)this.DataContext;
        }

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMember.MemberRegistered == true)
            {
                string msgTitle = "";
                string msgText = "Selected member is already registered";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWD.Show();
            }
            else
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
            
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FetchData();

        }

        private void FetchData()
        {
            clsGetPatientForLoyaltyCardIssueBizActionVO BizAction = new clsGetPatientForLoyaltyCardIssueBizActionVO();
            BizAction.PatientDetails = new List<clsPatientGeneralVO>();
            
            BizAction.FromDate = dtpFromDate.SelectedDate;
            BizAction.ToDate = dtpToDate.SelectedDate;

            if (txtMrno.Text != null)
                BizAction.MrNo = txtMrno.Text;

            if (txtLoyaltyCardNo.Text != null)
                BizAction.LoyaltyCardNo = txtLoyaltyCardNo.Text;

            if (txtFirstName.Text != null)
                BizAction.FirstName = txtFirstName.Text;

            if (txtMiddleName.Text != null)
                BizAction.MiddleName = txtMiddleName.Text;

            if (txtLastName.Text != null)
                BizAction.LastName = txtLastName.Text;

            if (cmbClinic.SelectedItem != null)
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;

           BizAction.IsLoyaltymember = UnIssue;
           BizAction.IssuDate = true;

           if ((MasterListItem)cmbLoyaltyProgram.SelectedItem != null)
               BizAction.LoyaltyProgramID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;
           else
               BizAction.LoyaltyProgramID = 0;

           BizAction.IsPagingEnabled = true;
           BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
           BizAction.MaximumRows = DataList.PageSize;
        
          
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPatientList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Result != null && arg.Error == null)
                {

                    if (((clsGetPatientForLoyaltyCardIssueBizActionVO)arg.Result).PatientDetails != null)
                    {
                        //dgPatientList.ItemsSource = ((clsGetPatientForLoyaltyCardIssueBizActionVO)arg.Result).PatientDetails;
                        clsGetPatientForLoyaltyCardIssueBizActionVO result = arg.Result as clsGetPatientForLoyaltyCardIssueBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;

                        if (result.PatientDetails != null)
                        {
                            DataList.Clear();

                            foreach (var item in result.PatientDetails)
                            {
                                DataList.Add(item);
                            }

                            dgPatientList.ItemsSource = null;
                            dgPatientList.ItemsSource = DataList;

                            dataGrid2Pager.Source = null;
                            dataGrid2Pager.PageSize = BizAction.MaximumRows;
                            dataGrid2Pager.Source = DataList;
                            dgPatientList.SelectedIndex = -1;

                        }

                    }
                }
                else
                    HtmlPage.Window.Alert("Error occured while processing.");

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsPatientGeneralVO)dgPatientList.SelectedItem != null)
            {
                ObjPatient = ((clsPatientGeneralVO)dgPatientList.SelectedItem);
                SelectedMember = new clsPatientFamilyDetailsVO();
                FillFamilyDetails(ObjPatient.PatientID);
            }

        }


        private void FillLoyaltyProgram()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbLoyaltyProgram.ItemsSource = null;
                    cmbLoyaltyProgram.ItemsSource = objList;
                    cmbLoyaltyProgram.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillFamilyDetails(long iID)
        {
            clsGetPatientFamilyDetailsBizActionVO BizAction = new clsGetPatientFamilyDetailsBizActionVO();
            BizAction.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
            BizAction.PatientID = iID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; 


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgFamilyList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Result != null && arg.Error == null)
                {

                    if (((clsGetPatientFamilyDetailsBizActionVO)arg.Result).FamilyDetails != null)
                    {
                        dgFamilyList.ItemsSource = ((clsGetPatientFamilyDetailsBizActionVO)arg.Result).FamilyDetails;

                    }
                }
                else
                    HtmlPage.Window.Alert("Error occured while processing.");

            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dgFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem != null)
            {
                SelectedMember = ((clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem);
                SelectedMember.UnitId = ObjPatient.UnitId;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            dgFamilyList.ItemsSource = null;
            FetchData();
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();

        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void chkRegistered_Click(object sender, RoutedEventArgs e)
        {
            

        }



    }
}

