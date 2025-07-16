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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.Agency;
using CIMS;
using System.Reflection;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.ComponentModel;



namespace PalashDynamics.Administration.Agency
{
    public partial class frmAgencyMaster : UserControl
    {
        #region List and Variables Declaration
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private SwivelAnimation objAnimation;
        private bool IsModify = false;
        private bool IsCancel = true;
        private bool IsUpdateStatus = false;
        private List<clsSpecializationVO> SpecializationList;
        private List<clsSpecializationVO> SelectedSpecializationList;
        private List<clsSpecializationVO> ModifySpecializationList;

        #endregion
        public clsAddAgencyMasterBizActionVO objBizActionVO;
        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
        

        public List<long> lstApplicableUnits = new List<long>();
        public List<MasterListItem> SelectedUnitList;
        public List<MasterListItem> AddSelectedUnitList;
        public List<MasterListItem> UnSelectedUnitList;
        public List<MasterListItem> SelectedSpecilList;
        //public List<MasterListItem> SelectedUnitList = new List<MasterListItem>();
        //public List<MasterListItem> AddSelectedUnitList = new List<MasterListItem>();
        //public List<MasterListItem> UnSelectedUnitList = new List<MasterListItem>();
        //public List<MasterListItem> SelectedSpecilList = new List<MasterListItem>();
        public PagedSortableCollectionView<clsAgencyMasterVO> MasterList { get; private set; }

        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public string msgText;
        public string msgTitle;
        bool Edit = false;
        bool IsNew = false;
         public bool UpdatedStatus = false;
        public bool isView = false;
        public Boolean isPageLoaded = false;
        public bool EditMode { get; set; }
        public bool ApplySpcial = false;
        public long pkAgencyID { get; set; }

        long SpecializationID = 0;
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
      

        List<MasterListItem> objList = null;
        List<MasterListItem> objspecialList = null;

        List<MasterListItem> objList2 = null;
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
       
        #region Class Constructor and Loaded Events
        public frmAgencyMaster()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            SetCommandButtonState("Load");
            SpecializationList = new List<clsSpecializationVO>();
            SelectedSpecializationList = new List<clsSpecializationVO>();
            ModifySpecializationList = new List<clsSpecializationVO>();
            MasterList = new PagedSortableCollectionView<clsAgencyMasterVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 15;
            this.dgDataPager.DataContext = MasterList;
            dgAgencyList.DataContext = MasterList;

        }

        private void frmAgencyMaster_Loaded(object sender, RoutedEventArgs e)
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            MasterListItem objM = new MasterListItem(0, "-- Select --");
            objList.Add(objM);
            txtCountry.ItemsSource = objList;
            txtCountry.SelectedItem = objM;
            txtState.ItemsSource = objList;
            txtState.SelectedItem = objM;
            txtCity.ItemsSource = objList;
            txtCity.SelectedItem = objM;
            FillCountry();
            FillAgencyList();
        }
        #endregion
        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillAgencyList();
        }
        #region Clicked Events(Toggle Buttons and CheckBoxes)
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            FillSpecialization();        
            ClearUI();
            try
            {
                objAnimation.Invoke(RotationType.Forward);
                IsModify = false;
            }
            catch (Exception)
            {
                throw;
            }
            txtCountry.SelectedValue = Country;
            txtState.SelectedValue = city;
            txtCity.SelectedValue = state;
            CheckValidation1();

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                if (chkmail == true && chkName==true)
                {
                    string msgTitle = "Palash";
                    string msgText = "Are you sure you want to Save the Agency Master?";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            SaveAgencyMaster();
                        }
                    };
                    msgW1.Show();
                }
            }
            else
            {

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Save The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
          
        }
        //rohinee
   
      
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            if (((clsAgencyMasterVO)dgAgencyList.SelectedItem).Status == true)
            {
                if (CheckValidation())
                {
                    if (chkmail == true && chkName == true)
                    {

                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to Modify Agency Master?";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                SaveAgencyMaster();
                            }
                        };
                        msgW1.Show();
                    }
                }
                else
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Required Fields To Update The Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                string msgText = "YOU CAN NOT MODIFY " + ((clsAgencyMasterVO)dgAgencyList.SelectedItem).Description + " \n BECAUSE STATUS OF " + ((clsAgencyMasterVO)dgAgencyList.SelectedItem).Description + " IS INACTIVE.";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            ClearUI();
            if (IsCancel == true)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Clinic Configuration";
                UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.Agency.frmAgencyConfiguration") as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            else
            {
                FillAgencyList();
                IsCancel = true;
            }
            state = 0;
            city = 0;
            Country = 0;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillAgencyList();
        }

        long city = 0;
        long state = 0;
        long Country = 0;
       
        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
           
            //rohinee
            if (dgAgencyList.SelectedItem != null)
            {
                try
                {
                 
                    IsModify = true;
                    CheckValidation1();
                    SetCommandButtonState("View");
                    FillSpecialization();

                    clsAgencyMasterVO obj=new clsAgencyMasterVO();
                    obj = (clsAgencyMasterVO)dgAgencyList.SelectedItem;
                    cmdModify.IsEnabled = ((clsAgencyMasterVO)dgAgencyList.SelectedItem).Status;
                    
                    txtCode.Text = obj.Code; 
                    txtName.Text = obj.Description;

                    if (obj.Address1 != null)
                    {
                        txtAddressLine1.Text = obj.Address1;
                    }
                    if (obj.Address1 != null)
                    {
                        txtAddressLine2.Text = obj.Address2;
                    }
                    txtAddressLine3.Text = obj.Address3;

                  
                    txtCode.Text = obj.Code;
                    txtContactperson1Email.Text = obj.ContactPerson1Email;
                    txtContactperson1MobileNo.Text = obj.ContactPerson1MobileNo;
                    txtContactperson1Name.Text = obj.ContactPerson1Name;

                    txtContactperson2Email.Text = obj.ContactPerson2Email;
                    txtContactperson2MobileNo.Text = obj.ContactPerson2MobileNo;
                    txtContactperson2Name.Text = obj.ContactPerson2Name;

                    txtPhoneNo.Text = obj.PhoneNo;

                     city = obj.CityId;
                     state = obj.StateId;
                     Country = obj.CountryId;
                     if (Country != 0)
                     {
                         FillCountry();
                     }
                     else
                     {

                         txtCountry.SelectedValue = Country;
                         txtState.SelectedValue = city;
                         txtCity.SelectedValue = state;

                     }

                    
                    //txtState.SelectedValue = obj.StateId;
                    //txtCity.SelectedValue = obj.CityId;
                   // txtCountry.Text = obj.Country;
                    //txtState.Text = obj.State;
                    //txtCity.Text = obj .City;

                    txtFax.Text = obj.Fax;
                    txtPincode.Text = obj.Pincode;                 
                  
                    objAnimation.Invoke(RotationType.Forward);
                  
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ch = sender as CheckBox;
            IsUpdateStatus = true;
            if (dgAgencyList.SelectedItem != null)
            {
                if(ch.IsChecked == true)
                {
                    UpdatedStatus = true;
                }
                else
                {
                    UpdatedStatus = false;
                }
            
               SaveAgencyMaster();

            }
        }

        private void chkSpecialization_Click(object sender, RoutedEventArgs e)
        {
            clsSpecializationVO objVO = new clsSpecializationVO { SpecializationId = ((MasterListItem)dgSpecialization.SelectedItem).ID };
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked == true)
            {
                if (IsModify == true && dgAgencyList.SelectedItem != null)
                {
                    ModifySpecializationList.Add(objVO);
                }
                else
                {
                    SpecializationList.Add(objVO);
                }
            }
            else
            {
                clsSpecializationVO obj;
                if (IsModify == true)
                    obj = ModifySpecializationList.Where(z => z.SpecializationId == objVO.SpecializationId).FirstOrDefault();
                else
                    obj = SpecializationList.Where(z => z.SpecializationId == objVO.SpecializationId).FirstOrDefault();
                if (IsModify == true && dgAgencyList.SelectedItem != null)
                {
                    ModifySpecializationList.Remove(obj);
                }
                else
                {
                    SpecializationList.Remove(obj);
                }
            }
        }
        #endregion

        #region TextBox Events
        private void txtNo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNo_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtAgencyName_KeyUp(object sender, KeyEventArgs e)
        {

        }
        #endregion

        
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
                  

                    dgSpecialization.ItemsSource = null;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdModify.IsEnabled = false;
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
                case "View1":
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

       
        private void ClearUI()
        {
            txtCode.Text = String.Empty;
            txtName.Text = String.Empty;
            state = 0;
            city = 0;
            Country = 0;

            txtCountry.SelectedValue = (long)0;
            txtCity.SelectedValue = (long)0;
            txtState.SelectedValue = (long)0;
         
            txtAddressLine1.Text = String.Empty;
             txtAddressLine2.Text = String.Empty;
            txtAddressLine3.Text = String.Empty;

            txtContactperson1Name.Text = String.Empty;
             txtContactperson2Name.Text = String.Empty;
            txtContactperson1Email.Text = String.Empty;
             txtContactperson2Email.Text = String.Empty;
            

            txtContactperson1MobileNo.Text=String.Empty;
              txtContactperson2MobileNo.Text=String.Empty;

            //for clear validation
            txtContactperson1Email.ClearValidationError();
            txtContactperson2Email.ClearValidationError();
            txtCode.ClearValidationError();
            txtName.ClearValidationError();
            txtContactperson1Name.ClearValidationError();
            txtContactperson2Name.ClearValidationError();
        

            txtPhoneNo.Text = String.Empty;
            txtPincode.Text = String.Empty;
    
            txtFax.Text = String.Empty;
          }

        private bool CheckValidation()
        {
            bool IsValidate = true;
            if (String.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Please Enter Agency Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                IsValidate = false;
            }
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.SetValidation("Please Enter Agency Description");
                txtName.RaiseValidationError();
                txtName.Focus();
                IsValidate = false;
            }
            if (SpecializationList != null && SpecializationList.Count <= 0 && IsModify == false) //|| (ModifySpecializationList != null && ModifySpecializationList.Count <= 0))
            {
                string msgText = "Select At Least One Specialization.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                IsValidate = false;
            }

            else if ((ModifySpecializationList != null && ModifySpecializationList.Count <= 0) && IsModify == true)
            {
          
                string msgText = "Select At Least One Specialization.";
                ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                IsValidate = false;
            }
            if (txtContactperson1Email.Text.Length > 0 && txtContactperson1Email.Text.IsEmailValid() == false)
            {
                txtContactperson1Email.SetValidation("Please Enter Valid Email Adderess");
                txtContactperson1Email.RaiseValidationError();
                txtContactperson1Email.Focus();
                IsValidate = false;
            }
            else
                txtContactperson1Email.ClearValidationError();

            if (txtContactperson2Email.Text.Length > 0 && txtContactperson2Email.Text.IsEmailValid() == false)
            {
                txtContactperson2Email.SetValidation("Please Enter Valid Email Adderess");
                txtContactperson2Email.RaiseValidationError();
                txtContactperson2Email.Focus();
                IsValidate = false;
            }
            else
                txtContactperson2Email.ClearValidationError();
         
            return IsValidate;
        }
        private bool CheckValidation1()
        {
            bool IsValidate = true;
            if (String.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                txtCode.SetValidation("Please Enter Agency Code");
                txtCode.RaiseValidationError();
                txtCode.Focus();
                IsValidate = false;
            }
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtName.SetValidation("Please Enter Agency Description");
                txtName.RaiseValidationError();
                txtName.Focus();
                IsValidate = false;
            }
            return IsValidate;
        }

        private void FillSpecialization()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Specialization;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        dgSpecialization.ItemsSource = null;
                        if (dgAgencyList.SelectedItem != null && IsModify == true)
                        {
                            foreach (var item in objList)
                            {
                                foreach (clsSpecializationVO item1 in SelectedSpecializationList)
                                {
                                    if (item1.AgencyID == ((clsAgencyMasterVO)dgAgencyList.SelectedItem).ID && item.ID == item1.SpecializationId)
                                    {
                                        item.Status = true;
                                        ModifySpecializationList.Add(item1);
                                    }
                                }
                            }
                        }
                        dgSpecialization.ItemsSource = objList;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillAgencyList()
        {

            try
            {
                clsGetAgencyMasterListBizActionVO bizActionObj = new clsGetAgencyMasterListBizActionVO();
                bizActionObj.AgencyMasterDetails = new clsAgencyMasterVO();
                if (!String.IsNullOrEmpty(txtAgencyName.Text))
                    bizActionObj.AgencyMasterDetails.Description = txtAgencyName.Text;

             
                bizActionObj.PagingEnabled = true;
                bizActionObj.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
                bizActionObj.MaximumRows = MasterList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetAgencyMasterListBizActionVO result = args.Result as clsGetAgencyMasterListBizActionVO;
                        MasterList.Clear();
                        SelectedSpecializationList.Clear();
                        SelectedSpecializationList = result.AgencyMasterDetails.SelectedSpecializationList;
                        if (result.AgencyMasterList != null)
                        {
                            MasterList.TotalItemCount = (int)((clsGetAgencyMasterListBizActionVO)args.Result).TotalRows;
                            foreach (clsAgencyMasterVO item in result.AgencyMasterList)
                            {
                                MasterList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            return;

        }


        //Rohinee
        private void FillStateList(string pCountry)
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();


        }
        //Rohinee
        private void FillCityList(string pState)
        {
           clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();
                }

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
    
        private void txtAddressLine1_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine1.Text = txtAddressLine1.Text.ToTitleCase();
        }

        private void txtAddressLine2_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine2.Text = txtAddressLine2.Text.ToTitleCase();
        }

        private void txtAddressLine3_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAddressLine3.Text = txtAddressLine3.Text.ToTitleCase();
        }
        bool chkName = true;
        private void txtContactperson1Name_LostFocus(object sender, RoutedEventArgs e)
        {
           
            txtContactperson1Name.Text = txtContactperson1Name.Text.ToTitleCase();

            //valiodation for only char--rohini
            if (txtContactperson1Name.Text.Length != null)
            {
                if (txtContactperson1Name.Text.IsNameValid())
                {
                    txtContactperson1Name.ClearValidationError();
                    chkName = true;
                 
                }
                else
                {
                    txtContactperson1Name.SetValidation("Please Enter valid Name");
                    txtContactperson1Name.RaiseValidationError();
                    chkName = false;
                }
            }
         
        }


        
        private void txtContactperson1MobileNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {


                if (!((TextBox)sender).Text.IsNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 10)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtContactperson1MobileNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        bool chkmail = true;
        private void txtContactperson1Email_LostFocus(object sender, RoutedEventArgs e)
        {
            
            //if (txtContactperson1Email.Text.Length > 0)
            //{
            //    if (txtContactperson1Email.Text.IsEmailValid())
            //    {
            //        txtContactperson1Email.ClearValidationError();
            //        chkmail = true;
            //    }
            //    else
            //    {
            //        txtContactperson1Email.SetValidation("Please Enter valid Email Adderess");
            //        txtContactperson1Email.RaiseValidationError();
            //        chkmail = false;
            //    }
            //}

            if (!string.IsNullOrEmpty(txtContactperson1Email.Text))
            {
                if (Extensions.IsEmailValid(txtContactperson1Email.Text))
                {
                    txtContactperson1Email.ClearValidationError();
                    chkmail = true;
                }
                else
                {
                    txtContactperson1Email.SetValidation("Please Enter Valid Email Address");
                    txtContactperson1Email.RaiseValidationError();
                    txtContactperson1Email.Focus();
                    chkmail = false;
                }
            }
            else
            {
                txtContactperson1Email.ClearValidationError();
                chkmail = true;
            }
        }

        private void txtContactperson2Name_LostFocus(object sender, RoutedEventArgs e)
        {
            txtContactperson2Name.Text = txtContactperson2Name.Text.ToTitleCase();

            if (txtContactperson2Name.Text.Length != null)
            {
                if (txtContactperson2Name.Text.IsNameValid())
                {
                    txtContactperson2Name.ClearValidationError();
                    chkName = true;

                }
                else
                {
                    txtContactperson2Name.SetValidation("Please Enter valid Name");
                    txtContactperson2Name.RaiseValidationError();
                    chkName = false;
                }
            }



        }

        private void txtContactperson2MobileNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

   
        private void txtContactperson2MobileNo_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {


                if (!((TextBox)sender).Text.IsNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 10)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        

        private void txtPhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPhoneNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {


                if (!((TextBox)sender).Text.IsNumberValid())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((TextBox)sender).Text.Length > 10)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

    
      
        //rohinee
        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                 
                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
                }
        }
        //rohinee
        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = objList.DeepCopy();
                  //  txtCountry.SelectedItem = objList[0];
                    if(Country == 0)
                    {
                      txtCountry.SelectedItem = objList[0];
                    }

                
                }
                if (this.DataContext != null)
                {
                    txtCountry.SelectedValue = ((clsAgencyMasterVO)this.DataContext).CountryId;

                }
                if (Country != 0)
                {
                    txtCountry.SelectedValue = Country;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();

        }

        //rohinee
        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();                
                

                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = ((clsAgencyMasterVO)this.DataContext).StateId;
                      //  txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                        else if(state!=0)
                    {
                        txtState.SelectedValue = state;
                       // txtSpouseState.SelectedItem = objM;
                    }
                    else
                    {
                        txtState.SelectedItem = objM;
                       // txtSpouseState.SelectedItem = objM;
                    }

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        //rohinee
        public void FillCity(long StateID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();

                  

                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = ((clsAgencyMasterVO)this.DataContext).CityId;
                       
                    }
                         else if(city!=0)
                    {
                        txtCity.SelectedValue = city;
                    
                    }
                    else
                    {
                        txtCity.SelectedItem = objM;
                    
                    }

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        //rohinee
        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    //((clsAgencyMasterVO)this.DataContext).StateId = ((MasterListItem)txtState.SelectedItem).ID;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                   
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
        }
        //rohinee
        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    //if (((MasterListItem)txtCity.SelectedItem).ID > 0)
                    //    ((clsAgencyMasterVO)this.DataContext).CityId = ((MasterListItem)txtCity.SelectedItem).ID;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                   
                   }
        }

        private void txtContactperson1Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!System.Text.RegularExpressions.Regex.IsMatch(txtContactperson1Name.Text, "^[a-zA-Z]"))
            //{
            //    MessageBox.Show("This textbox accepts only alphabetical characters");
            //    //if (txtContactperson1Name.Text != "")
            //    //{
            //    //    txtContactperson1Name.Text.Remove(txtContactperson1Name.Text.Length - 1);
            //    //}
            //    txtContactperson1Name.Text = "";
            //}
        }

       
             private void txtContactperson2Email_LostFocus(object sender, RoutedEventArgs e)
             {
            //if (txtContactperson2Email.Text.Length > 0)
            //{
            //    if (txtContactperson2Email.Text.IsEmailValid())
            //    {
            //        txtContactperson2Email.ClearValidationError();
            //        chkmail = true;
            //    }
            //    else
            //    {
            //        txtContactperson2Email.SetValidation("Please Enter valid Email Adderess");
            //        txtContactperson2Email.RaiseValidationError();
            //        chkmail = false;
            //    }
            //}
                 if (!string.IsNullOrEmpty(txtContactperson2Email.Text))
                 {
                     if (Extensions.IsEmailValid(txtContactperson2Email.Text))
                     {
                         txtContactperson2Email.ClearValidationError();
                         chkmail = true;
                     }
                     else
                     {
                         txtContactperson2Email.SetValidation("Please Enter Valid Email Address");
                         txtContactperson2Email.RaiseValidationError();
                         txtContactperson2Email.Focus();
                         chkmail = false;
                     }
                 }
                 else
                 {
                     txtContactperson2Email.ClearValidationError();
                     chkmail = true;
                 }
        }

        private void txtContactperson1Name_KeyDown(object sender, KeyEventArgs e)
        {
            //if (((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Tab))
            //{
            //    e.Handled = false;
            //    txtContactperson1Name.SetValidation("Please Enter valid Name");
            //    txtContactperson1Name.RaiseValidationError();
            //    chkName = false;
            //}
          
        }

        private void txtContactperson2Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Tab))
            {
                e.Handled = false;
                txtContactperson2Name.SetValidation("Please Enter valid Name");
                txtContactperson2Name.RaiseValidationError();
                chkName = false;
            }
            //else
            //{
            //    e.Handled = true;
            //    txtContactperson2Name.ClearValidationError();
            //    chkName = true;

            //}
        }
        private void SaveAgencyMaster()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                clsAddAgencyMasterBizActionVO BizAction = new clsAddAgencyMasterBizActionVO();
                BizAction.AgencyMasterDetails = new clsAgencyMasterVO();
                if (IsModify == true && dgAgencyList.SelectedItem != null)
                {
                    BizAction.AgencyMasterDetails.ID = ((clsAgencyMasterVO)dgAgencyList.SelectedItem).ID;
                }

                if (IsUpdateStatus == true && dgAgencyList.SelectedItem != null)
                {
                    BizAction.IsStatusChanged = true;

                    BizAction.AgencyMasterDetails.Status = UpdatedStatus;
                    BizAction.AgencyMasterDetails.ID = ((clsAgencyMasterVO)dgAgencyList.SelectedItem).ID;
                }
                else
                {
                    BizAction.AgencyMasterDetails.Status = true;
                }

                if (!String.IsNullOrEmpty(txtCode.Text))
                    BizAction.AgencyMasterDetails.Code = txtCode.Text;
                if (!String.IsNullOrEmpty(txtName.Text))
                    BizAction.AgencyMasterDetails.Description = txtName.Text;
                if (!String.IsNullOrEmpty(txtAddressLine1.Text))
                    BizAction.AgencyMasterDetails.Address1 = txtAddressLine1.Text;
                if (!String.IsNullOrEmpty(txtAddressLine2.Text))
                    BizAction.AgencyMasterDetails.Address2 = txtAddressLine2.Text;
                if (!String.IsNullOrEmpty(txtAddressLine3.Text))
                    BizAction.AgencyMasterDetails.Address3 = txtAddressLine3.Text;

                if (txtCountry.SelectedItem != null)
                {
                    BizAction.AgencyMasterDetails.Country = ((MasterListItem)txtCountry.SelectedItem).Description;
                    BizAction.AgencyMasterDetails.CountryId = ((MasterListItem)txtCountry.SelectedItem).ID;
                    // ((MasterListItem)txtCountry.SelectedItem).ID;
                }

                if (txtState.SelectedItem != null)
                {
                    BizAction.AgencyMasterDetails.State = ((MasterListItem)txtState.SelectedItem).Description;
                    BizAction.AgencyMasterDetails.StateId = ((MasterListItem)txtState.SelectedItem).ID;
                }

                if (txtCity.SelectedItem != null)
                {
                    BizAction.AgencyMasterDetails.City = ((MasterListItem)txtCity.SelectedItem).Description;
                    BizAction.AgencyMasterDetails.CityId = ((MasterListItem)txtCity.SelectedItem).ID;
                }

                //BizAction.AgencyMasterDetails.CityId = ((MasterListItem)txtCity.SelectedItem).ID;
                //BizAction.AgencyMasterDetails.CountryId = ((MasterListItem)txtCountry.SelectedItem).ID;
                //BizAction.AgencyMasterDetails.StateId = ((MasterListItem)txtState.SelectedItem).ID;


                if (!String.IsNullOrEmpty(txtPincode.Text))
                    BizAction.AgencyMasterDetails.Pincode = txtPincode.Text;
                if (!String.IsNullOrEmpty(txtPhoneNo.Text))
                    BizAction.AgencyMasterDetails.PhoneNo = txtPhoneNo.Text;
                if (!String.IsNullOrEmpty(txtContactperson1Name.Text))
                    BizAction.AgencyMasterDetails.ContactPerson1Name = txtContactperson1Name.Text;
                if (!String.IsNullOrEmpty(txtContactperson2Name.Text))
                    BizAction.AgencyMasterDetails.ContactPerson2Name = txtContactperson2Name.Text;
                if (!String.IsNullOrEmpty(txtContactperson1Email.Text))
                    BizAction.AgencyMasterDetails.ContactPerson1Email = txtContactperson1Email.Text;
                if (!String.IsNullOrEmpty(txtContactperson2Email.Text))
                    BizAction.AgencyMasterDetails.ContactPerson2Email = txtContactperson2Email.Text;
                if (!String.IsNullOrEmpty(txtContactperson1MobileNo.Text))
                    BizAction.AgencyMasterDetails.ContactPerson1MobileNo = txtContactperson1MobileNo.Text;
                if (!String.IsNullOrEmpty(txtContactperson2MobileNo.Text))
                    BizAction.AgencyMasterDetails.ContactPerson2MobileNo = txtContactperson2MobileNo.Text;
                if (!String.IsNullOrEmpty(txtFax.Text))
                    BizAction.AgencyMasterDetails.Fax = txtFax.Text;
                if (IsModify == true && SelectedSpecializationList != null)
                {
                    BizAction.AgencyMasterDetails.SpecializationList = ModifySpecializationList;
                }
                else
                {
                    if (SpecializationList != null)
                        BizAction.AgencyMasterDetails.SpecializationList = SpecializationList;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsAddAgencyMasterBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Agency master saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                objAnimation.Invoke(RotationType.Backward);
                                FillAgencyList();
                                SetCommandButtonState("Save");
                                SpecializationList.Clear();
                                ModifySpecializationList.Clear();
                                IsUpdateStatus = false;
                                ClearUI();
                            };
                        }
                        else if (((clsAddAgencyMasterBizActionVO)args.Result).SuccessStatus == 4)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Agency master updated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                            msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                objAnimation.Invoke(RotationType.Backward);
                                FillAgencyList();
                                SetCommandButtonState("Save");
                                SpecializationList.Clear();
                                ModifySpecializationList.Clear();                            
                                IsModify = false;
                                IsUpdateStatus = false;
                                ClearUI();
                            };
                        }
                        else if (((clsAddAgencyMasterBizActionVO)args.Result).SuccessStatus == 2)
                        {
                            string msgText = "Record Cannot Be Saved Because Code Already Exist! ";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //ClearUI();
                            IsUpdateStatus = false;
                        }
                        else if (((clsAddAgencyMasterBizActionVO)args.Result).SuccessStatus == 3)
                        {
                            string msgText = "Record Cannot Be Saved Because Description Already Exist!";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                           // ClearUI();
                            IsUpdateStatus = false;
                        }
                        else if (IsUpdateStatus == true)
                        {
                            string msgText = "Status Updated Successfully.";
                            ShowMessageBox(msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            FillAgencyList();
                            IsUpdateStatus = false;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
            return;
        }

        private void txtAgencyName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillAgencyList();
            }


        }

     
        private void txtAgeTo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtAgeTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() || Convert.ToInt64(((TextBox)sender).Text) <= 0 && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

    
    }

}
