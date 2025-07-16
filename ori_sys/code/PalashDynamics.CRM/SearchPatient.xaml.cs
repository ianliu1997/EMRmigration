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
using PalashDynamics.ValueObjects.EMR;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.Service.EmailServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;

namespace PalashDynamics.CRM
{
    public partial class SearchPatient : UserControl
    {
        #region Variable Declaration
        bool isLoaded = false;
        string textBefore = null;
       public string[] PatientEmailId;
       List<clsPatientVO> EmailIDList = new List<clsPatientVO>();
       public string PatEmailId;
       public List<clsPatientVO> patientItemSource { get; set; }
       public int count;
       public string msgTitle = " ";
       public string EmailText;
       public string PatientName;
       public string EmailSubject;
       public bool EmailFormat;
       public long EmailSentCount = 0;
       public string EmailAttachment;
       public long TempId;
       public string SMSEnglishText;
       public string SMSLocalText;
       public bool Email_SmsFlag = false;
       clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
       WaitIndicator waiting = new WaitIndicator();
       public string ClinicEmailId;
       public ObservableCollection<string> AttachmentFiles { get; set; }
       int countParam = 0;
       //string[] Parameters;
       List<string> Parameters;
       public DateTime DateTimeNow;
        #endregion

       #region Paging

       public PagedSortableCollectionView<clsPatientVO> DataList { get; private set; }

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
       public SearchPatient()
        {
            InitializeComponent();
            dgPatientList.LoadingRow += new EventHandler<DataGridRowEventArgs>(dgPatientList_LoadingRow);
           
            DataList = new PagedSortableCollectionView<clsPatientVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            Parameters = new List<string>();
            countParam = 0;
        }
       void DataList_OnRefresh(object sender, RefreshEventArgs e)
       {
           FetchData();

       }

        void dgPatientList_LoadingRow(object sender, DataGridRowEventArgs e)
        {           
        }

        private void SearchPatient_Loaded(object sender, RoutedEventArgs e)
        {
            FetchData();
            FillUnitList();
            FillStateList();
            FillGender();
            FillMaritalStatus();
            FillLoyaltyProgram();
            FillComplaint();
            cmbAge.ItemsSource = FillAge();
            cmbAge.SelectedValue = (long)0;
            isLoaded = true;
            dtpFromDate.Focus();
            GetEmailfromConfig();
            Parameters = new List<string>();

            fillProtocolType();
            fillPlannedTreatment();

        }
       
        #region FillCombobox
        private void FillStateList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = true;

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

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        
        private void FillCityList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
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

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        
        private void FillAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();

            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = false;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                    cmbGender.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
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
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList;
                    cmbMaritalStatus.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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

        private void FillComplaint()
        {
            clsGetEMRTemplateListBizActionVO BizAction = new clsGetEMRTemplateListBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    BizAction.objEMRTemplateList = ((clsGetEMRTemplateListBizActionVO)arg.Result).objEMRTemplateList;

                    BizAction.objEMRTemplateList.Insert(0, new clsEMRTemplateVO()
                    {
                        Title = "--Select--"
                    }
                    );
                    cmbComplaint.ItemsSource = BizAction.objEMRTemplateList;
                    cmbComplaint.SelectedItem = BizAction.objEMRTemplateList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public static List<MasterListItem> FillAge()
        {
            List<MasterListItem> objAge = new List<MasterListItem>();
            objAge.Add(new MasterListItem(0, "Select"));
            objAge.Add(new MasterListItem(1, "="));
            objAge.Add(new MasterListItem(2, "<"));
            objAge.Add(new MasterListItem(3, ">"));
          
            return objAge;
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

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
       
        private void FillDepartmentList(long iUnitId)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;

            if (iUnitId > 0)
                BizAction.Parent = new KeyValue { Key = iUnitId, Value = "UnitId" };
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


                    if (((MasterListItem)cmbClinic.SelectedItem).ID == 0)
                    {

                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                    cmbDepartment.SelectedItem = objList[0];
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        //private void FillDoctorList(long iDeptId)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_DoctorDepartmentView;
        //    if (iDeptId > 0)
        //        BizAction.Parent = new KeyValue { Key = iDeptId, Value = "DepartmentId" };
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

        //            cmbDoctor.ItemsSource = null;
        //            cmbDoctor.ItemsSource = objList;
        //            cmbDoctor.SelectedItem = objList[0];
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        private void FillDoctor(long IUnitId, long iDeptId)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            if ((MasterListItem)cmbClinic.SelectedItem != null)
            {
                BizAction.UnitId = IUnitId;
            }
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptId;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    cmbDoctor.SelectedItem = objList[0];
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillProtocolType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFProtocolType;
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

                    cmbProtocol.ItemsSource = null;
                    cmbProtocol.ItemsSource = objList;
                    cmbProtocol.SelectedValue = (long)0;
                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        private void fillPlannedTreatment()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IVFPlannedTreatment;
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
                    cmbSrcTreatmentPlan.ItemsSource = null;
                    cmbSrcTreatmentPlan.ItemsSource = objList;
                    cmbSrcTreatmentPlan.SelectedValue = 0;
                    if (this.DataContext != null)
                    {
                        cmbSrcTreatmentPlan.SelectedValue = (long)0;  //((clsFemaleLabDay0VO)this.DataContext).TreatmentTypeID;
                    }
                }

                //fillPlan();

            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        #endregion

        #region  Validation
        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();
            FillCityList(txtState.Text);
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            FillAreaList(txtState.Text);
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

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();
        }

        #endregion

        private void GetEmailfromConfig()
        {
            clsAppConfigVO objAppcongif = new clsAppConfigVO();
            objAppcongif.ID = 1;

            clsGetAppConfigBizActionVO objApp = new clsGetAppConfigBizActionVO();
            objApp.AppConfig = objAppcongif;
            objApp.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsAppConfigVO objAppEmail = new clsAppConfigVO();
                    objAppEmail = ((clsGetAppConfigBizActionVO)ea.Result).AppConfig;
                    ClinicEmailId = "nileshi@seedhealthcare.com"; //objAppEmail.Email;
                }
            };
            client.ProcessAsync(objApp, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbClinic.SelectedItem != null) ;
            FillDepartmentList(((MasterListItem)cmbClinic.SelectedItem).ID);
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null) ;
            //FillDoctorList(((MasterListItem)cmbDepartment.SelectedItem).ID);
            FillDoctor(((MasterListItem)cmbClinic.SelectedItem).ID, ((MasterListItem)cmbDepartment.SelectedItem).ID);
        }

        private void FetchData()
        {
            waiting.Show();
            
            clsGetPatientDetailsForCRMBizActionVO BizAction = new clsGetPatientDetailsForCRMBizActionVO();
            BizAction.PatientDetails = new List<clsPatientVO>();

            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate;
            if (dtpToDate.SelectedDate != null)
            {
                BizAction.ToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            }
            if (txtMrno.Text != null)
                BizAction.MRNo = txtMrno.Text;
            if (txtOPDNo.Text != null)
                BizAction.OPDNo = txtOPDNo.Text;
            if (txtFirstName.Text != null)
                BizAction.FirstName = txtFirstName.Text;
            if (txtMiddleName.Text != null)
                BizAction.MiddleName = txtMiddleName.Text;
            if (txtLastName.Text != null)
                BizAction.LastName = txtLastName.Text;


            if (cmbClinic.SelectedItem != null && ((MasterListItem)cmbClinic.SelectedItem).ID !=0)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
           


            if (cmbDepartment.SelectedItem != null)
                BizAction.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            if (cmbDoctor.SelectedItem != null)
                BizAction.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
            if (txtState.Text != null)
                BizAction.State = txtState.Text;
            if (txtCity.Text != null)
                BizAction.City = txtCity.Text;
            if (txtArea.Text != null)
                BizAction.Area = txtArea.Text;
            if (cmbGender.SelectedItem != null)
                BizAction.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            if (txtContactNo.Text != null)
                BizAction.ContactNo = txtContactNo.Text;
            if (cmbLoyaltyProgram.SelectedItem != null)
                BizAction.LoyaltyCardID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;

            if (cmbComplaint.SelectedItem != null)
                BizAction.ComplaintID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;

            if (cmbAge.SelectedItem != null)
                BizAction.AgeFilter = ((MasterListItem)cmbAge.SelectedItem).Description;

            if (cmbProtocol.SelectedItem != null)
                BizAction.ProtocolID = ((MasterListItem)cmbProtocol.SelectedItem).ID;

            if (cmbSrcTreatmentPlan.SelectedItem != null)
                BizAction.TreatmentID = ((MasterListItem)cmbSrcTreatmentPlan.SelectedItem).ID;

            if (txtAge.Text.Trim() != "")
            {
                BizAction.Age = int.Parse(txtAge.Text);                
            }


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            //dgPatientList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Result != null && arg.Error == null)
                {
                    if (((clsGetPatientDetailsForCRMBizActionVO)arg.Result).PatientDetails != null)
                    {
                        waiting.Close();
                        //dgPatientList.ItemsSource = ((clsGetPatientDetailsForCRMBizActionVO)arg.Result).PatientDetails;

                        clsGetPatientDetailsForCRMBizActionVO result = arg.Result as clsGetPatientDetailsForCRMBizActionVO;
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
                  

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                           
                            dgPatientList.SelectedIndex = -1;

                        }

                        patientItemSource = ((clsGetPatientDetailsForCRMBizActionVO)arg.Result).PatientDetails;
                    }
                }
                else
                {
                    waiting.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
       
        /// <summary>
        /// Purpose:Send sms to selected patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      
        private void CmdSendSms_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            for (int counter = 0; counter < dgPatientList.SelectedItems.Count; counter++)
            {
                if (((clsPatientVO)dgPatientList.SelectedItem).SelectPatient == true)
                {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
            if (flag==true)
            {                
                ChildWindow chform = new ChildWindow();
                chform.Title = "SMS Template";
                ViewSMSTemplate win = new ViewSMSTemplate();
                
                win.SMSOk_Click += new RoutedEventHandler(win_SMSOk_Click);
                chform.Content = win;
                chform.Show();                              
                //win.Show();
            }
            else
            { 
                 MessageBoxControl.MessageBoxChildWindow msgW1 =
                 new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select atleast 1 patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                
                CmdSendEmail.IsEnabled = false;
                CmdSendSms.IsEnabled = false;
            }            
        }

        void win_SMSOk_Click(object sender, RoutedEventArgs e)
        {
            waiting.Show();
            if (((ViewSMSTemplate)sender).DialogResult == true)
            {
                Email_SmsFlag = true;
                PagedSortableCollectionView<clsPatientVO> TempList = new PagedSortableCollectionView<clsPatientVO>();
                TempList = (PagedSortableCollectionView<clsPatientVO>)dgPatientList.ItemsSource;

                TempId = (((ViewSMSTemplate)sender).TemplateId);
                SMSEnglishText = (((ViewSMSTemplate)sender).SMSEnglishText);
                SMSLocalText = (((ViewSMSTemplate)sender).SMSLocalText);

                clsEmailSMSSentListVO objTemp = CreateEmailFormData();
                clsAddEmailSMSSentListVO objSent = new clsAddEmailSMSSentListVO();
                objSent.EmailTemplate = objTemp;

                for (int i = 0; i < TempList.Count; i++)
                {
                    if (TempList[i].SelectPatient == true)
                    {
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Result != null && arg.Error == null)
                            {
                                this.DataContext = null;
                                objSent.EmailTemplate.SuccessStatus = false;
                                clsEmailSMSSentListVO objAddSent = ((clsAddEmailSMSSentListVO)arg.Result).EmailTemplate;
                            }
                            waiting.Close();
                        };
                        Client.ProcessAsync(objSent, User);
                        Client.CloseAsync();
                    }
                }
            }
        }

        List<CheckBox> checkboxes = new List<CheckBox>();
       
        
        /// <summary>
        /// Purpose:Send email to selected patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void CmdSendEmail_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsPatientVO> objList = (PagedSortableCollectionView<clsPatientVO>)dgPatientList.ItemsSource;

            bool flag = false;
            for (int counter = 0; counter < objList.Count; counter++)
            {
                if (objList[counter].SelectPatient == true)
                {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
            if (flag == true)
            {
                
                ChildWindow chform = new ChildWindow();
                chform.Title = "Email Template";
                EmailTemplate win = new EmailTemplate();
                
                win.Ok_Click += new RoutedEventHandler(win_Ok_Click);
                chform.Content = win;
                chform.Show();
                //win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select atleast 1 patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

                CmdSendEmail.IsEnabled = false;
                CmdSendSms.IsEnabled = false;
            }
        }

        #region Count the no of parameters in the Email text

        private void CountParameters()
        {            
            foreach (char c in EmailText)
            {
                if (c == '{')
                    countParam=countParam+1;
            }   
        }

        #endregion
        void win_Ok_Click(object sender, RoutedEventArgs e)
        {
            long EmailSentCount = 0;
            long EmailNotSentCount = 0;
            string [] EmailSentTo = new string [1000];
            string [] EmailNotSentTo = new string [1000];

            DateTimeNow = DateTime.Now;
            //long j = 0;
            waiting.Show();
            if (((EmailTemplate)sender).DialogResult == true)
            {
                bool EmailNotSent = false;
                Email_SmsFlag = false;
                PagedSortableCollectionView<clsPatientVO> TempList = new PagedSortableCollectionView<clsPatientVO>();
                TempList = (PagedSortableCollectionView<clsPatientVO>)dgPatientList.ItemsSource;

                EmailText = (((EmailTemplate)sender).EmailText);
                EmailSubject = (((EmailTemplate)sender).EmailSubject);
                EmailAttachment = (((EmailTemplate)sender).EmailFilePath);
                TempId = (((EmailTemplate)sender).EmailTemplateId);

                if (!string.IsNullOrEmpty(EmailText) && !string.IsNullOrEmpty(EmailSubject))
                {

                    //EmailText = (((EmailTemplate)sender).
                    // Email_SmsFl
                    string[] SplitText1;

                    Parameters = new List<string>();
                    CountParameters();
                    string[] SplitText = EmailText.Split('{');
                    if (countParam > 0)
                    {
                        for (int i = 1; i <= countParam; i++)
                        {
                            string SplitText3 = SplitText[i];
                            string[] SplitT = SplitText3.Split('}');
                            Parameters.Add(SplitT[0]);
                        }
                    }


                    clsEmailSMSSentListVO objEmailTemp = CreateEmailFormData();
                    clsAddEmailSMSSentListVO objEmailSent = new clsAddEmailSMSSentListVO();
                    objEmailSent.EmailTemplate = objEmailTemp;
                    for (int i = 0; i < TempList.Count; i++)
                    {
                        if (TempList[i].SelectPatient == true)
                        {
                            if (TempList[i].Email != null && TempList[i].Email != "")
                            {
                                objEmailSent.EmailTemplate.PatientEmailId = TempList[i].Email;
                                objEmailSent.EmailTemplate.PatientId = TempList[i].GeneralDetails.PatientID;
                                objEmailSent.EmailTemplate.SuccessStatus = true;

                                if (!string.IsNullOrEmpty(PatientName))
                                {
                                    EmailText = EmailText.Replace(PatientName, TempList[i].GeneralDetails.PatientName);
                                    PatientName = TempList[i].GeneralDetails.PatientName;
                                }
                                else
                                {
                                    for (int j = 0; j < Parameters.Count; j++)
                                    {
                                        //if (Parameters[j] == "")
                                        // {
                                        //EmailText = EmailText.Replace("{", "");
                                        switch (Parameters[j])
                                        {
                                            case "MRNo":
                                                EmailText = EmailText.Replace(Parameters[j], TempList[i].GeneralDetails.MRNo);
                                                break;
                                            case "Patient Name":
                                                EmailText = EmailText.Replace("{Patient Name}", TempList[i].GeneralDetails.PatientName);
                                                PatientName = TempList[i].GeneralDetails.PatientName;
                                                break;
                                            case "":
                                                EmailText = EmailText.Replace(Parameters[j], TempList[i].GeneralDetails.OPDNO);
                                                break;
                                            case "a":
                                                EmailText = EmailText.Replace(Parameters[j], Convert.ToString(TempList[i].GeneralDetails.PatientID));
                                                break;
                                            case "b":
                                                EmailText = EmailText.Replace(Parameters[j], Convert.ToString(TempList[i].GeneralDetails.BMI));
                                                break;

                                        }

                                        //EmailText = EmailText.Replace("}", "");
                                        //}
                                    }
                                }
                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, arg) =>
                                {
                                    if (arg.Result != null && arg.Error == null)
                                    {
                                        this.DataContext = null;
                                        clsEmailSMSSentListVO objAddSent = ((clsAddEmailSMSSentListVO)arg.Result).EmailTemplate;
                                        EmailSentCount += 1;
                                        EmailNotSent = true;
                                        waiting.Close();
                                    }
                                };
                                Client.ProcessAsync(objEmailSent, User);
                                Client.CloseAsync();

                                if (((EmailTemplate)sender).AttachmentNo > 0)
                                {
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
                                    EmailClient.SendEmailwithAttachmentCompleted += (ea, args) =>
                                    {
                                        waiting.Close();
                                        if (args.Error == null)
                                        {

                                            CmdSendSms.IsEnabled = false;
                                            //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            //new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Request Sent To The Server Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            //msgW1.Show();

                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW1.Show();
                                        }                                        
                                    };
                                    EmailClient.SendEmailwithAttachmentAsync(ClinicEmailId, TempList[i].Email, (((EmailTemplate)sender).EmailSubject), EmailText, ((EmailTemplate)sender).AttachmentNo, ((EmailTemplate)sender).AttachmentFiles); //txtSubject.Text, txtTemplateText.Text, txtFilePath.Text);
                                    EmailClient.CloseAsync();

                                }
                                else
                                {
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
                                    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
                                    EmailClient.SendEmailCompleted += (ea, args) =>
                                    {
                                        waiting.Close();
                                        if (args.Error == null)
                                        {
                                            EmailSentCount=EmailSentCount+1;
                                            CmdSendSms.IsEnabled = false;
                                            //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            //new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Sent Successfully .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            //msgW1.Show();
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                                            msgW1.Show();
                                        }                                        
                                    };
                                    EmailClient.SendEmailAsync(ClinicEmailId, TempList[i].Email, (((EmailTemplate)sender).EmailSubject), EmailText);
                                    EmailClient.CloseAsync();
                                }
                            }
                            else
                            {
                                objEmailSent.EmailTemplate.PatientId = TempList[i].GeneralDetails.PatientID;
                                objEmailSent.EmailTemplate.SuccessStatus = false;
                                objEmailSent.EmailTemplate.FailureReason = "EmailId doesnot Exists";

                                EmailNotSentTo[EmailNotSentCount] = TempList[i].GeneralDetails.PatientName;
                                EmailNotSentCount += 1;

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, arg) =>
                                {
                                    waiting.Close();
                                    if (arg.Result != null && arg.Error == null)
                                    {
                                        this.DataContext = null;
                                        clsEmailSMSSentListVO objAddSent = ((clsAddEmailSMSSentListVO)arg.Result).EmailTemplate;
                                    }                                    
                                };
                                Client.ProcessAsync(objEmailSent, User);
                                Client.CloseAsync();
                            }
                        }
                    }
                    if (EmailNotSentCount > 0)
                    {
                        string PatEmailId = "";
                        for (int j = 0; j < EmailNotSentCount; j++)
                        {
                            if (j > 0)
                                PatEmailId = PatEmailId + ", ";
                            PatEmailId = PatEmailId + EmailNotSentTo[j];
                        }
                        waiting.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Not Sent To " + PatEmailId, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                    //else
                    //{
                    //    waiting.Close();
                    //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Sent To " + EmailSentCount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information); 
                    //}

                }
                else
                {
                    waiting.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Subject And Text required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();

                }
                //    throw new NotImplementedException();
            }
            
        }
        
        /// <summary>
        /// Purpose:Getting list of patient details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsPatientVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;


            if (dtpToDate.SelectedDate < dtpFromDate.SelectedDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "To date can not be less than From date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();

            }
            else
            {
                FetchData();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //clsPatientVO Obj= new clsPatientVO();
            //txtFirstName.Text = string.Empty;
            //txtLastName.Text = string.Empty;
            //txtMiddleName.Text = string.Empty;
            //dtpFromDate.SelectedDate = null;
            //dtpToDate.SelectedDate = null;
            //txtOPDNo.Text = string.Empty;
            //txtMrno.Text = string.Empty;
            
            //cmbClinic.SelectedValue = (long)0;
            //cmbDepartment.SelectedValue = (long)0;
            //cmbDoctor.SelectedValue = (long)0;



            //dgDataPager.PageIndex = 0;
            //FetchData();
        }

        private void dgPatientList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPatientList.SelectedItem != null)
            {
                if (((clsPatientVO)dgPatientList.SelectedItem).SelectPatient == false)
                {
                    ((clsPatientVO)dgPatientList.SelectedItem).SelectPatient = true;
                }
                else
                    ((clsPatientVO)dgPatientList.SelectedItem).SelectPatient = false;
            }
        }
        
        private void accordian_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                switch (acc.SelectedIndex)
                {
                    case 0:
                        cmbClinic.Focus();
                        break;
                    default:
                       break;
                } 

        }
       
        private void chkPAtient_Click(object sender, RoutedEventArgs e)
        {
            if (CmdSendEmail.IsEnabled == false)
            {
                CmdSendEmail.IsEnabled = true;
                CmdSendSms.IsEnabled = true;
            }

            //if (((clsPatientVO)dgPatientList.SelectedItem).SelectPatient == false)
            //{
            //    ((clsPatientVO)dgPatientList.SelectedItem).SelectPatient = true;
            //}
            //else            
            //    ((clsPatientVO)dgPatientList.SelectedItem).SelectPatient = false;                         
        }

        private clsEmailSMSSentListVO CreateEmailFormData()
        {
            clsEmailSMSSentListVO objTemp = new clsEmailSMSSentListVO();

            if (Email_SmsFlag == true)
            {
                objTemp.TemplateID = TempId;
                objTemp.Email_SMS = false;
                objTemp.EnglishText = SMSEnglishText;
                objTemp.LocalText = SMSLocalText;
            }
            else
            {
                objTemp.EmailSubject = EmailSubject;
                objTemp.EmailText = EmailText;
                objTemp.EmailAttachment = EmailAttachment;
                objTemp.TemplateID = TempId;
                //objTemp.e
                objTemp.Email_SMS = true;      
      
            }
                       
            return objTemp;
        }
        private void PatientCount()
        {           
            clsGetPatientDetailsForCRMBizActionVO BizAction = new clsGetPatientDetailsForCRMBizActionVO();
            BizAction.PatientDetails = new List<clsPatientVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPatientList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Result != null && arg.Error == null)
                {
                    if (((clsGetPatientDetailsForCRMBizActionVO)arg.Result).PatientDetails != null)
                    {
                        dgPatientList.ItemsSource = ((clsGetPatientDetailsForCRMBizActionVO)arg.Result).PatientDetails;
                        for (count = 0; count < dgPatientList.SelectedItems.Count; count++)
                        {
                            //if (  BizAction.PatientDetails[dgPatientList.SelectedItems.Count].Email != null)
                            //{
                            //    PatientEmailId[count] = BizAction.PatientDetails[count].Email;
                            //}
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
        /// Purpose:Select all patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        PagedSortableCollectionView<clsPatientVO> lst = new PagedSortableCollectionView<clsPatientVO>();

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkSelectAll.IsChecked == true)
                {                    
                    //lst = (List<clsPatientVO>)dgPatientList.ItemsSource;
                    lst=(PagedSortableCollectionView<clsPatientVO>)dgPatientList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {                          
                            item.SelectPatient = true;                           
                        }
                        dgPatientList.ItemsSource = null;
                        dgPatientList.ItemsSource = lst;
                        dgPatientList.SelectedIndex = -1;
                        CmdSendEmail.IsEnabled = true;
                        CmdSendSms.IsEnabled = true;
                    }
                }
            }
            catch(Exception)
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
                    lst = (PagedSortableCollectionView<clsPatientVO>)dgPatientList.ItemsSource;
                    if (lst != null)
                    {
                        foreach (var item in lst)
                        {
                            item.SelectPatient = false;
                        }
                        dgPatientList.ItemsSource = null;
                        dgPatientList.ItemsSource = lst;
                        dgPatientList.SelectedIndex = -1;
                        
                        CmdSendEmail.IsEnabled = false;
                        CmdSendSms.IsEnabled = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dtFT = dtpFromDate.SelectedDate;
            DateTime? dtTT = dtpToDate.SelectedDate;
            string MRNO = null;
            string OPDNo = null;
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            string AgeFilter = null;
            string State = null;
            string City = null;
            string Area = null;
            string ContactNo = null;
            long UnitID = 0;
            long DepartmentID = 0;
            long DoctorID = 0;
            long GenderID = 0;
            long MaritalStatusID = 0;
            long LoyaltyCardID = 0;
            long ComplaintID = 0;
            int Age = 0;

            if (txtMrno.Text != "")
            {
                MRNO = txtMrno.Text;
            }
            if (txtOPDNo.Text != "")
            {
                OPDNo = txtOPDNo.Text;
            }
            if (txtFirstName.Text != null)
            {
                FirstName = txtFirstName.Text;
            }
            if (txtMiddleName.Text != null)
            {

                MiddleName = txtMiddleName.Text;
            }
            if (txtLastName.Text != null)
            {
                LastName = txtLastName.Text;
            }
            
            if (cmbClinic.SelectedItem != null)
                UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbDepartment.SelectedItem != null)
                DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            if (cmbDoctor.SelectedItem != null)
                DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
           
            if (txtState.Text != null)
                 State = txtState.Text;
            if (txtCity.Text != null)
                City = txtCity.Text;
            if (txtArea.Text != null)
                Area = txtArea.Text;
            if (cmbGender.SelectedItem != null)
                GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbMaritalStatus.SelectedItem != null)
                MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            if (txtContactNo.Text != null)
                ContactNo = txtContactNo.Text;
            if (cmbLoyaltyProgram.SelectedItem != null)
                LoyaltyCardID = ((MasterListItem)cmbLoyaltyProgram.SelectedItem).ID;

            if (cmbComplaint.SelectedItem != null)
                ComplaintID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;

            if (cmbAge.SelectedItem != null)
                AgeFilter = ((MasterListItem)cmbAge.SelectedItem).Description;

            if (txtAge.Text.Trim() != "")
            {
                Age = int.Parse(txtAge.Text);
            }
            UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            string URL = "../Reports/CRM/SearchPatientReport.aspx?FromDate=" + dtFT + "&ToDate=" + dtTT + "&ClinicID=" + UnitID + "&DepartmentID=" + DepartmentID + "&DoctorID=" + DoctorID + "&MRNO=" + MRNO + "&OPDNO=" + OPDNo + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&LoyaltyCardID=" + LoyaltyCardID + "&State =" + State + "&City=" + City + "&Area=" + Area + "&ContactNo=" + ContactNo + "&AgeFilter=" + AgeFilter + "&Age=" + Age + "&GenderID=" + GenderID + "&MaritalStatusID="+ MaritalStatusID + "&Complaint=" + ComplaintID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


        }

        private void cmbAge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (((MasterListItem)cmbAge.SelectedItem).ID==0)
            //{
            //    txtAge.Text = string.Empty;

            //}



            if (cmbAge.SelectedItem != null && ((MasterListItem)cmbAge.SelectedItem).ID != 0)
            {
                txtAge.IsReadOnly = false;
            }
            else
            {
                txtAge.IsReadOnly = true;
                txtAge.Text = string.Empty;
            }

        
        }

       
        
        }
       
    

  }
