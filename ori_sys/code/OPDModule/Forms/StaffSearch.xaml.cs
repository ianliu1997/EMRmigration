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
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;

namespace OPDModule.Forms
{
    public partial class StaffSearch : ChildWindow
    {
        public bool isfromCouterSaleStaff = false;
        public event RoutedEventHandler OnSaveButton_Click;
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }        
        public int PageSize
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

        public StaffSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
            this.DataContext = new clsPatientGeneralVO();

            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            PageSize = 15;
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }
     

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            FillUnitList();                  
            cmbClinic.IsEnabled = false;                     
            GetData();          

        }

        public void GetData()
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            try
            {
                clsGetPatientGeneralDetailsListBizActionVO BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
                BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

              
                BizActionObject.isfromCouterSaleStaff = true;          

                if (txtFirstName.Text != "")
                    BizActionObject.FirstName = txtFirstName.Text;         
                if (txtLastName.Text != "")
                    BizActionObject.LastName = txtLastName.Text;
                if (txtEmpNO.Text != "")
                    BizActionObject.MRNo = txtEmpNO.Text;            
         
                 BizActionObject.RegistrationTypeID = 10;
              
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizActionObject.UnitID = 0;
                }
                else if ((MasterListItem)cmbClinic.SelectedItem != null)
                {
                    BizActionObject.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                }
                else
                {
                    BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizActionObject.IsPagingEnabled = true;
                BizActionObject.MaximumRows = DataList.PageSize; ;
                BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;
            

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    indicator.Close();
                    if (ea.Error == null)
                    {
                        if (ea.Result != null)
                        {

                            clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;
                            DataList.Clear();
                            foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                            {

                                DataList.Add(person);
                            }

                            dataGrid2.ItemsSource = null;
                            dataGrid2.ItemsSource = DataList;

                            peopleDataPager.Source = null;
                            peopleDataPager.PageSize = BizActionObject.MaximumRows;
                            peopleDataPager.Source = DataList;
                        }                       
                    }

                };
                client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
            }
            finally
            {

            }
        }

        private void FillUnitList()
        {
            clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsUserWise = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];
                }
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID != null)
                {
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName = ((clsPatientGeneralVO)dataGrid2.SelectedItem).FirstName;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.LastName = ((clsPatientGeneralVO)dataGrid2.SelectedItem).LastName;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName = ((clsPatientGeneralVO)dataGrid2.SelectedItem).MiddleName;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).GenderID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth = ((clsPatientGeneralVO)dataGrid2.SelectedItem).DateOfBirth;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo = ((clsPatientGeneralVO)dataGrid2.SelectedItem).MRNo;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientUnitID;
                    OnSaveButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }


        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }
       
        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                //   dataGrid2.SelectedIndex = 0;
                peopleDataPager.PageIndex = 0;
            }

        }


        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }


        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                try
                {
                    GetData();
                    dataGrid2.SelectedIndex = 0;               
                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error While Processing...", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Clinic.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }
        }

        private void cmdOK_Checked(object sender, RoutedEventArgs e)
        {
            //if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            //{
            //    this.DialogResult = true;
            //    if (OnSaveButton_Click != null)
            //    {
            //        OnSaveButton_Click(this, new RoutedEventArgs());

            //        this.Close();
            //    }
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW.Show();
            //}
        }
               

        private void txtEmpNO_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
                peopleDataPager.PageIndex = 0;
            }

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

