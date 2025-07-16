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
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using OPDModule;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Collections;
using OPDModule.Forms;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using System.Text;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.Forms.PatientView
{


    public partial class FrmDonarLinking : ChildWindow
    {

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCloseButton_Click;
        bool isLoaded = false;
        public long PatientCategoryID { get; set; }
        public string Mrno { get; set; }
        public string PatientName { get; set; }
        public string SearchKeyword { get; set; }
        public bool IsSurrogate = false;
        public clsPatientGeneralVO SelectedPatientObj;

        #region Properties
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
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
                //RaisePropertyChanged("PageSize");
            }
        }
        #endregion

        public List<clsPatientGeneralVO> Selectedpatient;
        public List<clsPatientGeneralVO> SurrogatePatientList;

        public List<clsPatientGeneralVO> SelectedCheckedPatient = new List<clsPatientGeneralVO>();
        public FrmDonarLinking()
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
            isLoaded = true;
            txtFirstName.Focus();
            if (PatientName != null)
                txtFirstName.Text = PatientName;
            if (Mrno != null)
                txtLastName.Text = Mrno;


            if (IsSurrogate)
            {
                dataGrid2.Columns[0].Visibility = Visibility.Visible;
                if (SelectedCheckedPatient != null && SelectedCheckedPatient.Count > 0)
                    Selectedpatient = SelectedCheckedPatient;
                GetSurrogatePatientList();
            }
            else
            {
                dataGrid2.Columns[0].Visibility = Visibility.Collapsed;
                GetData();
            }
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            if (!IsSurrogate)
                SelectedPatientObj = (clsPatientGeneralVO)dataGrid2.SelectedItem;
        }

        #region Validation
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

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        string textBefore = null;

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;


                }
            }
        }

        #endregion

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        public void GetSurrogatePatientList()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();
            BizActionObject.IsSurrogacy = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        SurrogatePatientList = new List<clsPatientGeneralVO>();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            SurrogatePatientList.Add(person);
                        }
                    }
                    GetData();
                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }

        public void GetData()
        {
            GetPatientListForDashboardBizActionVO BizActionObject = new GetPatientListForDashboardBizActionVO();

            BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();

            BizActionObject.VisitWise = false;

            // BizActionObject.PatientCategoryID = PatientCategoryID;
            //if (PatientCategoryID == 8)
            BizActionObject.PatientCategoryID = 0;
            BizActionObject.IsDonorLink = true;


            if (txtFirstName.Text != "")
                BizActionObject.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != "")
                BizActionObject.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != "")
                BizActionObject.LastName = txtLastName.Text;
            if (txtFamilyName.Text != "")
                BizActionObject.FamilyName = txtFamilyName.Text;

            if (txtMrno.Text != "")
                BizActionObject.MRNo = txtMrno.Text;

            if (txtOPDNo.Text != "")
                BizActionObject.OPDNo = txtOPDNo.Text;

            if (txtContactNo.Text != "")
                BizActionObject.ContactNo = txtContactNo.Text;

            if (txtCivilID.Text != "")
                BizActionObject.CivilID = txtCivilID.Text;
            if (SearchKeyword != null)
                BizActionObject.SearchKeyword = SearchKeyword;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizActionObject.UnitID = 0;
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
                if (ea.Error == null)
                {
                    if (ea.Result != null)
                    {
                        GetPatientListForDashboardBizActionVO result = ea.Result as GetPatientListForDashboardBizActionVO;
                        DataList.TotalItemCount = result.TotalRows;
                        DataList.Clear();
                        foreach (clsPatientGeneralVO person in result.PatientDetailsList)
                        {
                            DataList.Add(person);
                        }

                        if (SurrogatePatientList != null && SurrogatePatientList.Count > 0)
                        {
                            (from a1 in DataList
                             join a2 in SurrogatePatientList on a1.PatientID equals a2.PatientID
                             select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => x.A1.IsPatientEnabled = true);
                        }

                        if (Selectedpatient != null && Selectedpatient.Count > 0)
                        {
                            (from a1 in DataList
                             join a2 in Selectedpatient on a1.PatientID equals a2.PatientID
                             select new { A1 = a1, A2 = a2 }).ToList().ForEach(x => x.A1.IsPatientChecked = true);
                        }

                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList;

                        DataPager.Source = null;
                        DataPager.PageSize = BizActionObject.MaximumRows;
                        DataPager.Source = DataList;
                    }
                }
            };
            client.ProcessAsync(BizActionObject, new clsUserVO());
            client.CloseAsync();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null && ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID != 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCloseButton_Click != null)
            {
                OnCloseButton_Click(this, new RoutedEventArgs());
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetData();
            }
        }

        private void chkSelectPatient_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                if (Selectedpatient == null)
                    Selectedpatient = new List<clsPatientGeneralVO>();
                CheckBox chk = (CheckBox)sender;
                StringBuilder strError = new StringBuilder();
                if (chk.IsChecked == true)
                {
                    if (Selectedpatient.Count > 0)
                    {
                        var item = from r in Selectedpatient
                                   where r.PatientID == ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID
                                   select new clsPatientGeneralVO
                                   {
                                       PatientID = r.PatientID
                                   };
                        if (item.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientName);

                            if (!string.IsNullOrEmpty(strError.ToString()))
                            {
                                string strMsg = "";
                                strMsg = "Patient already Selected : " + strError.ToString();
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            Selectedpatient.Add((clsPatientGeneralVO)dataGrid2.SelectedItem);
                        }
                    }
                    else
                    {
                        Selectedpatient.Add((clsPatientGeneralVO)dataGrid2.SelectedItem);
                    }
                }
                else
                {
                    Selectedpatient = Selectedpatient.Where(x => x.PatientID != ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID).ToList();
                }

                //Selectedpatient.Remove((clsPatientGeneralVO)dataGrid2.SelectedItem);
            }
        }
    }




    //public partial class FrmDonarLinking : ChildWindow
    //{
    //    public clsPatientGeneralVO ObjPatient = new clsPatientGeneralVO();
    //    public event RoutedEventHandler OnSaveButton_Click;
    //    public long PatientCategoryID { get; set; }
    //    public FrmDonarLinking()
    //    {
    //        InitializeComponent();
    //    }

    //    private void OKButton_Click(object sender, RoutedEventArgs e)
    //    {
    //    //    if (txtDonarCode.Text.Trim() == string.Empty)
    //    //    {
    //    //        MessageBoxControl.MessageBoxChildWindow msgW1 =
    //    //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Donar Code.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
    //    //        msgW1.RaiseValidationError();
    //    //        msgW1.Show();
    //    //    }
    //    //    else
    //    //    {
    //    //        clsAddDonorCodeBizActionVO BizAction = new clsAddDonorCodeBizActionVO();
    //    //        BizAction.PatientDetails = new clsPatientVO();
    //    //        BizAction.PatientDetails.GeneralDetails = new clsPatientGeneralVO();
    //    //        BizAction.PatientDetails.GeneralDetails = ObjPatient;
    //    //        BizAction.PatientDetails.GeneralDetails.DonarCode = txtDonarCode.Text;

    //    //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
    //    //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
    //    //        Client.ProcessCompleted += (s, args) =>
    //    //        {
    //    //            if (args.Error == null)
    //    //            {
    //    //                if (((clsAddDonorCodeBizActionVO)args.Result).SuccessStatus == 1)
    //    //                {
    //    //                    this.DialogResult = false;                          
    //    //                    if (OnSaveButton_Click != null)
    //    //                        OnSaveButton_Click(this, new RoutedEventArgs());
    //    //                }
    //    //            }
    //    //            else
    //    //            {                      
    //    //                MessageBoxControl.MessageBoxChildWindow msgW1 =
    //    //                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
    //    //                msgW1.Show();
    //    //            }
    //    //        };

    //    //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
    //    //        Client.CloseAsync();
    //    //    }

    //    }

    //    private void CancelButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        this.DialogResult = false;
    //    }

    //    protected override void OnClosed(EventArgs e)
    //    {
    //        base.OnClosed(e);
    //        Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
    //    }

        
    //}
}

