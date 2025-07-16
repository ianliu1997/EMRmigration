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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using PalashDynamics;
using PalashDynamics.OutPatientDepartment.ViewModels;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.IVF;
using PalashDynamics.Forms.PatientView;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.Administration.UserRights;
using EMR;

namespace CIMS.Forms
{
    public partial class PatientList : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "REG":
                    isReg = true;
                      break;
                case "VISIT":
                      isReg = false;
                      break;
                case "Surrogacy":
                      isReg = true;
                      isSurrogacy = true;
                      break;
                default:
                    
                    break;
            }
        }

        #endregion
        bool isSurrogacy = false;
        bool isLoaded = false;
        bool isReg = true;
        public bool IsPatientList = false;
        clsGetPatientGeneralDetailsListBizActionVO BizActionObject;//added by akshays
        PagedSortableCollectionView<clsPatientGeneralVO> DataList { get; set; }//added by akshays
        List<clsPatientGeneralVO> DataList1 { get; set; }//added by akshays
        //public int PageSize { get; set; }//added by akshays
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

        public PatientList()
        {
            InitializeComponent();
            IsPatientList = true;
            FillUnitList();
           
            dtpVisitFromDate.SelectedDate = DateTime.Now;
            dtpVisitToDate.SelectedDate = DateTime.Now;
           // this.DataContext = new Patient_SearchViewModel();
            this.Loaded += new RoutedEventHandler(PatientList_Loaded);
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //GetData();
        }
        void PatientList_Loaded(object sender, RoutedEventArgs e)
        {
            List<MasterListItem> RegTypeList = new List<MasterListItem>();
            RegTypeList.Add(new MasterListItem(10, "-Select-"));
            RegTypeList.Add(new MasterListItem(0, "OPD"));
            RegTypeList.Add(new MasterListItem(1, "IPD"));
            RegTypeList.Add(new MasterListItem(2, "Pharmacy"));
            RegTypeList.Add(new MasterListItem(5, "Pathology"));
            cmbRegType.ItemsSource = RegTypeList;
            cmbRegType.SelectedItem = RegTypeList[0];

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            DataList = new PagedSortableCollectionView<clsPatientGeneralVO>();//add by akshays
            DataList1 = new List<clsPatientGeneralVO>();//add by akshays
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);//add by akshays
            PageSize = 12;//add by akshays
            //IsPatientExist = true;
            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
            mElement1.Text = "Find Patient";
            BizActionObject = new clsGetPatientGeneralDetailsListBizActionVO();
            FillIdentity();
            FillCountryList();
            FillGender();
            FillSpecialRegistration();
            FillLoyaltyProgram();
            isLoaded = true;
            // By BHUSHAN
            FillRegistrationType();
            cmbClinic.Focus();
            cmbClinic.UpdateLayout();

            if (isReg == false)
                this.DataContext = new PatientSearchViewModel();
            else if (isReg == true)
                this.DataContext = new PatientSearchViewModel(true);
            

        }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        public PatientList(DateTime? FromDate,DateTime? ToDate)
        {
            InitializeComponent();
            FillIdentity();
            FillCountryList();
            FillGender();
            FillLoyaltyProgram();
            // By BHUSHAN
            FillRegistrationType();
            FillSpecialRegistration();
         
            this.DataContext = new PatientSearchViewModel(FromDate, ToDate);
        }
        private void FillIdentity()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IdentityMaster;
            BizAction.IsActive = true;
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
                    cmbIdentity.ItemsSource = null;
                    cmbIdentity.ItemsSource = objList;
                    cmbIdentity.SelectedItem = objList[0];
                  
                }
               
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void FillSpecialRegistration()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
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
                    cmbSpRegistration.ItemsSource = null;
                    cmbSpRegistration.ItemsSource = objList;
                    cmbSpRegistration.SelectedItem = objList[0];

                }         

               
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
        //private void FillGender()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbGender.ItemsSource = null;
        //            cmbGender.ItemsSource = objList;
        //            cmbGender.SelectedItem = objList[0];
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, new clsUserVO());
        //    Client.CloseAsync();
        //}

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
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

        private void FillCountryList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Country";
            BizAction.IsDecode = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();

        }

        private void FillStateList(string pCountry)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pCountry))
            {
                pCountry = pCountry.Trim();
                BizAction.Parent = new KeyValue { Key = "Country", Value = pCountry };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }

            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();


        }

        private void FillCityList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
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
                List<MasterListItem> objList = new List<MasterListItem>();
                if (ea.Error == null && ea.Result != null)
                {                    
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];
                } 
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                {
                    var res = from r in objList
                              where r.ID == User.UserLoginInfo.UnitId
                              select r;
                    cmbClinic.SelectedItem = ((MasterListItem)res.First());
                    cmbClinic.IsEnabled = true;
                }
                else
                {
                    GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                    cmbClinic.SelectedValue = User.UserLoginInfo.UnitId;
                    cmbClinic.IsEnabled = false;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            ////BizAction.IsActive = true;
            //BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    if (e.Error == null && e.Result != null)
            //    {
            //        List<MasterListItem> objList = new List<MasterListItem>();

            //        objList.Add(new MasterListItem(0, "- Select -"));
            //        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
            //        cmbClinic.ItemsSource = null;
            //        cmbClinic.ItemsSource = objList; 
            //        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
            //        {
            //            var res = from r in objList
            //                      where r.ID == User.UserLoginInfo.UnitId
            //                      select r;
            //            cmbClinic.SelectedItem = ((MasterListItem)res.First());
            //            cmbClinic.IsEnabled = true;
            //        }
            //        else
            //        {
            //            cmbClinic.SelectedValue = User.UserLoginInfo.UnitId;
            //            cmbClinic.IsEnabled = false;
            //        }

                   
            //    }
            //};

            //Client.ProcessAsync(BizAction, new clsUserVO());
            //Client.CloseAsync();
        }
                
        private void GetPinCodeList(string pArea)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Pincode";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pArea))
            {
                pArea = pArea.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pArea };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPinCode.ItemsSource = null;
                    txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchList();
         }

        public void SearchList()
        {
            bool res = true;

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.ToDate != null)
            {
                if (((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate > ((PatientSearchViewModel)this.DataContext).BizActionObject.ToDate)
                {
                    dtpFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate.Value.Date > DateTime.Now.Date)
            {
                dtpFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;

            }
            else
            {
                dtpFromDate.ClearValidationError();
            }            
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitToDate != null)
            {
                if (((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate > ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitToDate)
                {
                    dtpVisitFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpVisitFromDate.RaiseValidationError();
                    dtpVisitFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpVisitFromDate.ClearValidationError();
                }
            }
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpVisitFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpVisitFromDate.RaiseValidationError();
                dtpVisitFromDate.Focus();
                res = false;
            }
            else
            {
                dtpVisitFromDate.ClearValidationError();
            }
            //=====================================

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate != null)
            {
                if (((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate > ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate)
                {
                    dtpAdmissionFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpAdmissionFromDate.RaiseValidationError();
                    dtpAdmissionFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpAdmissionFromDate.ClearValidationError();
                }
            }

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpAdmissionFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpAdmissionFromDate.RaiseValidationError();
                dtpAdmissionFromDate.Focus();
                res = false;
            }
            else
            {
                dtpAdmissionFromDate.ClearValidationError();
            }


            if (cmbIdentity.SelectedItem != null )
            {
                ((PatientSearchViewModel)this.DataContext).BizActionObject.IdentityID = ((MasterListItem)cmbIdentity.SelectedItem).ID;
                if (txtIdentityNumber.Text.Trim() != string.Empty && ((MasterListItem)cmbIdentity.SelectedItem).ID > 0)
                {
                    ((PatientSearchViewModel)this.DataContext).BizActionObject.IdentityNumber = txtIdentityNumber.Text.Trim(); 
                }
            }

            if (cmbSpRegistration.SelectedItem != null)
            {
                ((PatientSearchViewModel)this.DataContext).BizActionObject.SpecialRegID = ((MasterListItem)cmbSpRegistration.SelectedItem).ID;
               
            }



            //========================================
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBToDate != null)
            {
                if (((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate > ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBToDate)
                {
                    dtpDOBFromDate.SetValidation("From Date Should Be Less Than To Date");
                    dtpDOBFromDate.RaiseValidationError();
                    dtpDOBFromDate.Focus();
                    res = false;
                }
                else
                {
                    dtpDOBFromDate.ClearValidationError();
                }
            }
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate.Value.Date > DateTime.Now.Date)
            {
                dtpDOBFromDate.SetValidation("From Date Should Not Be Greater Than Today's Date");
                dtpDOBFromDate.RaiseValidationError();
                dtpDOBFromDate.Focus();
                res = false;
            }
            else
            {
                dtpDOBFromDate.ClearValidationError();
            }
            //========================================

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.ToDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.ToDate = DateTime.Now;

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.ToDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.FromDate = Convert.ToDateTime("1/1/1900");


            if (((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitToDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitToDate = DateTime.Now;

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.VisitToDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitFromDate = Convert.ToDateTime("1/1/1900");

            //====================================
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate = DateTime.Now;

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionToDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionFromDate = Convert.ToDateTime("1/1/1900");            
            //=====================================
            if (((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBToDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBToDate = DateTime.Now;

            if (((PatientSearchViewModel)this.DataContext).BizActionObject.DOBToDate != null && ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate == null)
                ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBFromDate = Convert.ToDateTime("1/1/1900");
            //=====================================            
            if (res)
            {
                if (cmbGender.SelectedItem != null)
                    ((PatientSearchViewModel)this.DataContext).BizActionObject.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

                ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.SearchInAnotherClinic = false;

                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
                //{
                if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID > 0)
                {
                    //By Anjali....................
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((MasterListItem)cmbClinic.SelectedItem).ID && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    {
                        ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                    }
                    else
                    {
                        ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
                    }
                    //............................
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false && ((MasterListItem)cmbClinic.SelectedItem).ID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    {
                       // ((PatientSearchViewModel)this.DataContext).BizActionObject.SearchInAnotherClinic = true;
                        ((PatientSearchViewModel)this.DataContext).BizActionObject.SearchInAnotherClinic = false;
                    }
                }           
                else
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    {
                        ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                    }
                    else
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID != ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                            ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        else
                            ((PatientSearchViewModel)this.DataContext).BizActionObject.UnitID = 0;
                }            

                if (chkLoyaltyMember.IsChecked == true)
                {
                    if (cmbLoyaltyProgram.SelectedItem != null)
                        ((PatientSearchViewModel)this.DataContext).BizActionObject.LoyaltyProgramID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;
                }
                //By Anjali......................

                ((PatientSearchViewModel)this.DataContext).BizActionObject.IsFromSurrogacyModule = isSurrogacy;
                //................................
                // BY BHUSHAN
                if (cmbRegisttype.SelectedItem != null)
                {
                    ((PatientSearchViewModel)this.DataContext).BizActionObject.RegistrationTypeID = ((MasterListItem)cmbRegisttype.SelectedItem).ID;
                }
                //added by akshays 
                if (txtFirstName.Text.Trim() != "")
                    BizActionObject.FirstName = txtFirstName.Text.Trim();
                else
                    BizActionObject.FirstName = "";

                if (txtMiddleName.Text.Trim() != "")
                    BizActionObject.MiddleName = txtMiddleName.Text.Trim();
                else
                    BizActionObject.MiddleName = "";

                if (txtLastName.Text.Trim() != "")
                    BizActionObject.LastName = txtLastName.Text.Trim();
                else
                    BizActionObject.LastName = "";

                if (txtFamilyName.Text.Trim() != "")
                    BizActionObject.FamilyName = txtFamilyName.Text.Trim();
                else
                    BizActionObject.FamilyName = "";

                if (txtMrno.Text.Trim() != "")
                    BizActionObject.MRNo = txtMrno.Text.Trim();
                else
                    BizActionObject.MRNo = "";
           

                if (txtContactNo.Text.Trim() != "")
                    BizActionObject.ContactNo = txtContactNo.Text.Trim();
                else
                    BizActionObject.ContactNo = "";

                if (txtCivilID.Text.Trim() != "")
                    BizActionObject.CivilID = txtCivilID.Text.Trim();
                else
                    BizActionObject.CivilID = "";

                if (((MasterListItem)cmbRegType.SelectedItem) != null && ((MasterListItem)cmbRegType.SelectedItem).ID != 10)
                {
                    // if (((MasterListItem)cmbRegFrom.SelectedItem).ID != 10)
                    ((PatientSearchViewModel)this.DataContext).BizActionObject.RegistrationTypeID = ((MasterListItem)cmbRegType.SelectedItem).ID;
                }
                else
                {
                    ((PatientSearchViewModel)this.DataContext).BizActionObject.RegistrationTypeID = 10;
                }

              //  ((PatientSearchViewModel)this.DataContext).BizActionObject.SignificantIDList = SignificantIDList;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.FirstName = txtFirstName.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.MiddleName = txtMiddleName.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.LastName = txtLastName.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.MRNo = txtMrno.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.OPDNo = txtOPDNo.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.CivilID = txtCivilID.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.ContactNo = txtContactNo.Text;

                //added by neena
                ((PatientSearchViewModel)this.DataContext).BizActionObject.DonarCode = txtDonarCode.Text;
                ((PatientSearchViewModel)this.DataContext).BizActionObject.OldRegistrationNo = txtOldRegistrationNo.Text;
                //

                //closed by akshays
                //GetData();
                //this.DataContext = new PatientSearchViewModel();

                //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO(); // (clsPatientGeneralVO)dataGrid2.SelectedItem;
                //peopleDataPager.PageIndex = 0;
                ((PatientSearchViewModel)this.DataContext).GetData();
                peopleDataPager.PageIndex = 0;
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DataContext = new clsGetPatientGeneralDetailsListBizActionVO();
            this.DataContext = new PatientSearchViewModel(true);
           
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO(); // (clsPatientGeneralVO)dataGrid2.SelectedItem;
            cmbIdentity.SelectedValue = (long)0;
            cmbSpRegistration.SelectedValue = (long)0;
            txtIdentityNumber.Text = string.Empty;
            peopleDataPager.PageIndex = 0;
            //dataGrid2.ItemsSource = null;       

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID >0)
            {
                fillCoupleDetails();
                GetPatientBalanceAmount();
            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
           txtFirstName.Text = txtFirstName.Text.ToTitleCase();
          // txtMiddleName.Focus();
        }

        private void acc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                switch (acc.SelectedIndex)
                {
                    case 0:
                       // txtCountry.UpdateLayout();
                        txtCountry.Focus();
                        break;
                    default:
                        //SearchButton.UpdateLayout();
                        //SearchButton.Focus();
                        break;
                } 
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
           // txtLastName.Focus();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
         //   txtFamilyName.Focus();
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

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCountry.Text = txtCountry.Text.ToTitleCase();
            FillStateList(txtCountry.Text);
            //txtState.Focus();
        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();

            FillCityList(txtState.Text);         
            //txtCity.Focus();
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            GetPinCodeList(txtCity.Text);
           // txtPinCode.Focus();
        }

        private void txtPinCode_LostFocus(object sender, RoutedEventArgs e)
        {
            //txtContactNo.Focus();
        }
           
        private void cmbGender_LostFocus(object sender, RoutedEventArgs e)
        {
          // txtCivilID.Focus();
        }

        private void SearchButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PrintCardButton_Click(object sender, RoutedEventArgs e)
        {

            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && 
                (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                long PatientId = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                long UnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCard.aspx?ID=" + PatientId +"&UnitID=" + UnitID), "_blank");
    
            }
        }

        #region Fill Couple Details
        public void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                    BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = new clsCoupleVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient = new clsPatientGeneralVO();

                    ((IApplicationConfiguration)App.Current).SelectedCoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                    if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
                    {
                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    }
                    else
                    {
                        GetHeightAndWeight(((IApplicationConfiguration)App.Current).SelectedCoupleDetails);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.FemalePatient.BMI = BizAction.CoupleDetails.FemalePatient.BMI;

                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Height = BizAction.CoupleDetails.MalePatient.Height;
                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                         ((IApplicationConfiguration)App.Current).SelectedCoupleDetails.MalePatient.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                    }

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        #endregion

        #region Get Patient Bill Balance Amount
        public void GetPatientBalanceAmount()
        {
            clsGetPatientBillBalanceAmountBizActionVO BizAction = new clsGetPatientBillBalanceAmountBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if(((clsGetPatientBillBalanceAmountBizActionVO)args.Result).PatientGeneralDetails!=null)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient.BillBalanceAmountSelf = Convert.ToDouble(((clsGetPatientBillBalanceAmountBizActionVO)args.Result).PatientGeneralDetails.BillBalanceAmountSelf);
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        #endregion

        private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (chkRegistration.IsChecked == true)
            {
                dtpFromDate.SelectedDate = DateTime.Now;
                dtpToDate.SelectedDate = DateTime.Now;
                dtpVisitFromDate.Text = String.Empty;
                dtpVisitToDate.Text = String.Empty;
                dtpDOBFromDate.Text = String.Empty; //Added by AniketK on 11Feb2019
                dtpDOBToDate.Text = String.Empty;   //Added by AniketK on 11Feb2019
            }

            //Begin::Added by AniketK on 11Feb2019 to filter data with DOBwise
            if (chkDOB.IsChecked == true)
            {
                dtpFromDate.Text = String.Empty;
                dtpToDate.Text = String.Empty;
                dtpVisitFromDate.Text = String.Empty;
                dtpVisitToDate.Text = String.Empty;
                dtpDOBFromDate.SelectedDate = DateTime.Now;
                dtpDOBToDate.SelectedDate = DateTime.Now;
            }
            //End::Added by AniketK on 11Feb2019 to filter data with DOBwise

            if (chkVisit.IsChecked ==true)
            {
                dtpFromDate.Text = String.Empty;
                dtpToDate.Text = String.Empty;
                dtpVisitFromDate.SelectedDate = DateTime.Now;
                dtpVisitToDate.SelectedDate = DateTime.Now;
                dtpDOBFromDate.Text = String.Empty; //Added by AniketK on 11Feb2019
                dtpDOBToDate.Text = String.Empty;   //Added by AniketK on 11Feb2019
            }

           if(chkRegistration.IsChecked.HasValue) ((PatientSearchViewModel)this.DataContext).BizActionObject.RegistrationWise =  chkRegistration.IsChecked.Value;
           if (chkAdmission.IsChecked.HasValue) ((PatientSearchViewModel)this.DataContext).BizActionObject.AdmissionWise = chkAdmission.IsChecked.Value;
           if(chkVisit.IsChecked.HasValue) ((PatientSearchViewModel)this.DataContext).BizActionObject.VisitWise = chkVisit.IsChecked.Value;
           if (chkDOB.IsChecked.HasValue) ((PatientSearchViewModel)this.DataContext).BizActionObject.DOBWise = chkDOB.IsChecked.Value;
        }

        private void PatientFile_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null &&
               (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                long PatientId = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                long PatientUnitId = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/Patientfile.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/PatientFile2.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/PatientFile3.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/PatientFile/Patientfile4.aspx?PatientId=" + PatientId + "&PatientUnitId=" + PatientUnitId), "_blank");
            }
        }

        // BY BHUSHAN. . . . . 
        public void FillRegistrationType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
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
                    cmbRegisttype.ItemsSource = null;
                    cmbRegisttype.ItemsSource = objList;
                    cmbRegisttype.SelectedItem = objList[0];
                    if (isSurrogacy == true)
                    {
                        cmbRegisttype.SelectedValue = (long)10;
                        cmbRegisttype.IsEnabled = false;
                    }
                }
            };

            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }


        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
            if (e.Key == Key.Enter)
            {
                //txtFirstName.Text = txtFirstName.Text.ToTitleCase();
                //txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
                //txtLastName.Text = txtLastName.Text.ToTitleCase();
                //txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
                //txtMrno.Text = txtMrno.Text.ToTitleCase();
                //txtContactNo.Text = txtContactNo.Text.ToTitleCase();
                //txtCivilID.Text = txtCivilID.Text.ToTitleCase();
                //if (txtFirstName.Text != null && txtFirstName.Text != "")
                //{
                //    BizActionObject.FirstName = txtFirstName.Text;
                //}
                //if (txtMiddleName.Text != null && txtMiddleName.Text != "")
                //{
                //    BizActionObject.MiddleName = txtMiddleName.Text;
                //}
                //if (txtLastName.Text != null && txtLastName.Text != "")
                //{
                //    BizActionObject.LastName = txtLastName.Text;
                //}
                //if (txtFamilyName.Text != null && txtFamilyName.Text != "")
                //{
                //    BizActionObject.FamilyName = txtFamilyName.Text;
                //}
                //if (txtMrno.Text != null && txtMrno.Text != "")
                //{
                //    BizActionObject.MRNo = txtMrno.Text;
                //}
                //if (txtContactNo.Text != null && txtContactNo.Text != "")
                //{
                //    BizActionObject.ContactNo = txtContactNo.Text;
                //}
                //if (txtCivilID.Text != null && txtCivilID.Text != "")
                //{
                //    BizActionObject.CivilID = txtCivilID.Text;
                //}
                PatientSearchButton_Click(sender, e);

            }
           
        }

         private void txtDonorCode_KeyUp(object sender, KeyEventArgs e)
        {           
            if (e.Key == Key.Enter)
            {
                PatientSearchButton_Click(sender, e);

            }

        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void dtpVisitFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void dtpVisitToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           // SearchList();
        }

        private void txtContactNo_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
        }

        private void txtCivilID_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Tab)
            //{
            //    SearchList();
            //}
        }

        private void txtLastName_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter )
            //{
            //    SearchList();
            //}
        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void cmbRegisttype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchList();
        }

        private void SearchButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                SearchList();
            }
        }

        private void cmbUnitSelecetion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          // SearchList();
        }

        // By BHUSHAN . . . . 
        private void MRNoBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                BarcodeForm win = new BarcodeForm();
                string MRNo = Convert.ToString((((IApplicationConfiguration)App.Current).SelectedPatient).MRNo);              
                win.PrintData = "" + MRNo.ToString();
                win.OnCloseButton_Click += new RoutedEventHandler(Email_ClosedButton);
                win.Show();
            }
        }
        void Email_ClosedButton(object sender, RoutedEventArgs e)
        {
            int i = 0;
        }

        private void PatientBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID > 0)
            {
                BarcodeFormNew win = new BarcodeFormNew();
                win.Show();
            }

        }

        private void AttachDoc_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                frmPatientScanDocument win = new frmPatientScanDocument();
                win.Title = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                win.IsPatientList = true;
                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        //added by akshays
        //WaitIndicator indicator = new WaitIndicator();
        //void GetData()
        //{

        //    indicator.Show();
        //    BizActionObject.PatientDetailsList = new List<clsPatientGeneralVO>();
        //    BizActionObject.IsPagingEnabled = true;
        //    BizActionObject.MaximumRows = DataList.PageSize; ;
        //    BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, ea) =>
        //    {
        //        if (ea.Error == null)
        //        {
        //            if (ea.Result != null)
        //            {
        //                clsGetPatientGeneralDetailsListBizActionVO result = ea.Result as clsGetPatientGeneralDetailsListBizActionVO;
        //                DataList.TotalItemCount = result.TotalRows;
        //                DataList.Clear();
        //                DataList1.Clear();
        //                this.dataGrid2.ItemsSource = null;
        //                foreach (clsPatientGeneralVO person in result.PatientDetailsList)
        //                {

        //                    DataList.Add(person);
        //                    DataList1.Add(person);
        //                }
        //                this.dataGrid2.ItemsSource = DataList;
        //                peopleDataPager.Source = DataList;
        //                peopleDataPager.PageSize = DataList.PageSize;
        //                dataGrid2.UpdateLayout();
        //            }
        //            else
        //            {
        //                this.dataGrid2.ItemsSource = null;
        //                DataList.TotalItemCount = 0;
        //                DataList.Clear();
        //                DataList1.Clear();
        //                this.dataGrid2.ItemsSource = DataList;
        //                peopleDataPager.Source = DataList;
                        
        //                dataGrid2.UpdateLayout();
        //            }
        //        }
        //        indicator.Close();
        //    };
        //    client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}
        //public class Patient_SearchViewModel
        //{
        //    public bool IsPagingEnabled { get; set; }
        //    public bool RegistrationWise { get; set; }
        //    public bool VisitWise { get; set; }
        //    public int PageSize { get; set; }
        //    public DateTime? DOB { get; set; }
        //    public string FirstName { get; set; }
        //    public string MiddleName { get; set; }
        //    public string LastName { get; set; }
        //    public string FamilyName { get; set; }
        //    public string OPDNo { get; set; }
        //    public string ContactNo { get; set; }
        //    public string Country { get; set; }
        //    public string State { get; set; }
        //    public string City { get; set; }
        //    public string Pincode { get; set; }


        //    public Patient_SearchViewModel()
        //    {
        //        IsPagingEnabled = true;
        //        RegistrationWise = false;
        //        VisitWise = true;
        //        PageSize = 12;
        //        DOB = null;
        //        FirstName = string.Empty;
        //        MiddleName = string.Empty;
        //        LastName = string.Empty;
        //        FamilyName = string.Empty;
        //        OPDNo = string.Empty;
        //        ContactNo = string.Empty;
        //        Country = string.Empty;
        //        State = string.Empty;
        //        City = string.Empty;
        //        Pincode = string.Empty;
        //    }

        //    public Patient_SearchViewModel(bool ischeck)
        //    {
        //        IsPagingEnabled = true;
        //        RegistrationWise = false;
        //        VisitWise = true;
        //        PageSize = 12;
        //        DOB = null;
        //        FirstName = string.Empty;
        //        MiddleName = string.Empty;
        //        LastName = string.Empty;
        //        FamilyName = string.Empty;
        //        OPDNo = string.Empty;
        //        ContactNo = string.Empty;
        //        Country = string.Empty;
        //        State = string.Empty;
        //        City = string.Empty;
        //        Pincode = string.Empty;
        //    }
        //}

      
        //closed by akshays
        private void cmbIdentity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbIdentity.SelectedItem).ID > 0)
            {
                txtIdentityNumber.IsEnabled = true;
            }
            else
            {
                txtIdentityNumber.Text = string.Empty;
                txtIdentityNumber.IsEnabled = false;
            }
        }
        
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtContactNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
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

        private void txtContactNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void RegistrationReport_Click(object sender, RoutedEventArgs e)
        {
            

            if (dataGrid2.SelectedItem != null)
            {
                string msgText = "";
                msgText = "Are You Sure \n You Want To Print Patient Registration Report?";
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptPatientCase);
                msgW.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        void rptPatientCase(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                rptpatientReport();
        }
        private void rptpatientReport()
        {
            long ID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            long UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            if (ID > 0 && UnitID > 0)
            {

                string URL = "../Reports/Patient/rptPatientCaseReportNew.aspx?ID=" + ID + "&UnitId=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }

        FrmDonarLinking WinDonar = null;
       
        private void cmbDonorLinkingButton_Click(object sender, RoutedEventArgs e)
        {
            clsPatientGeneralVO ObjPatientDetails = ((IApplicationConfiguration)App.Current).SelectedPatient;
            WinDonar = new FrmDonarLinking();
            WinDonar.PatientCategoryID = 8;        
            WinDonar.OnSaveButton_Click += new RoutedEventHandler(WinDonar_OnSaveButton_Click);
            WinDonar.OnCloseButton_Click += new RoutedEventHandler(WinPatient_OnCloseButton_Click);
            WinDonar.Show();

           
        }        

        void WinDonar_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WinDonar.SelectedPatientObj != null)
            {          
                clsAddDonorCodeBizActionVO BizAction = new clsAddDonorCodeBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.PatientDetails.GeneralDetails = new clsPatientGeneralVO();                
                BizAction.PatientDetails.GeneralDetails.ID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID;
                BizAction.PatientDetails.GeneralDetails.UnitId = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientUnitID;
                BizAction.PatientDetails.GeneralDetails.MRNo = ((clsPatientGeneralVO)dataGrid2.SelectedItem).MRNo;
                BizAction.PatientDetails.GeneralDetails.DonarCode = WinDonar.SelectedPatientObj.MRNo;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        if (((clsAddDonorCodeBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Donor Code Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            SearchList();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }




        private void cmbDonorUnLinkingButton_Click(object sender, RoutedEventArgs e)
        {
            if (((clsPatientGeneralVO)dataGrid2.SelectedItem).DonarCode != "")
            {
             clsAddDonorCodeBizActionVO BizAction = new clsAddDonorCodeBizActionVO();
                BizAction.PatientDetails = new clsPatientVO();
                BizAction.PatientDetails.GeneralDetails = new clsPatientGeneralVO();                
                BizAction.PatientDetails.GeneralDetails.ID = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientID;
                BizAction.PatientDetails.GeneralDetails.UnitId = ((clsPatientGeneralVO)dataGrid2.SelectedItem).PatientUnitID;
                BizAction.PatientDetails.GeneralDetails.MRNo = ((clsPatientGeneralVO)dataGrid2.SelectedItem).MRNo;
                //BizAction.PatientDetails.GeneralDetails.DonarCode = WinDonar.SelectedPatientObj.MRNo;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        if (((clsAddDonorCodeBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Donor UnLink Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            SearchList();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Donor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    SearchList();
            }

        }

        void WinPatient_OnCloseButton_Click(object sender, RoutedEventArgs e)
        {            
                     

        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    clsUserRightsVO objUser;
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

                        if (objUser.IsCrossAppointment)
                        {
                            cmbClinic.IsEnabled = true;
                        }
                        else
                        {
                            cmbClinic.IsEnabled = false;

                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdPatientBillHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                //PatientBillTransactionHistoryReport frm = new PatientBillTransactionHistoryReport();
                //frm.MrNo = (dataGrid2.SelectedItem as clsPatientGeneralVO).MRNo;
                //frm.Show();

                long PatientID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
                long PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).PatientUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientBillsHistory.aspx?PatientID=" + PatientID + "&PatientUnitID=" + PatientUnitID), "_blank");


            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void cmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
                if (dataGrid2.SelectedItem != null)
            {
                DoctorList frm = new DoctorList();            
                frm.Title = "Patient Name: " + (dataGrid2.SelectedItem as clsPatientGeneralVO).PatientName + '/' + (dataGrid2.SelectedItem as clsPatientGeneralVO).MRNo;
                frm.FemaleName = (dataGrid2.SelectedItem as clsPatientGeneralVO).FemaleName;
                frm.MaleName = (dataGrid2.SelectedItem as clsPatientGeneralVO).MaleName;
                frm.MaleAge = (dataGrid2.SelectedItem as clsPatientGeneralVO).MaleAge;
                frm.FemaleAge = (dataGrid2.SelectedItem as clsPatientGeneralVO).FemaleAge;
                frm.GenderID = (dataGrid2.SelectedItem as clsPatientGeneralVO).GenderID;              
                frm.MRNO = (dataGrid2.SelectedItem as clsPatientGeneralVO).MRNo;
                frm.SelectedPatientName = (dataGrid2.SelectedItem as clsPatientGeneralVO).PatientName;
                frm.SelectedAge = (dataGrid2.SelectedItem as clsPatientGeneralVO).Age;
                frm.PatientTypeID = (dataGrid2.SelectedItem as clsPatientGeneralVO).PatientTypeID;
                frm.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }
        
        
    }
}
