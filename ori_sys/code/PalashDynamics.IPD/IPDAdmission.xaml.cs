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
using CIMS.Forms;
using System.Reflection;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Windows.Media.Imaging;
using DataDrivenApplication.Forms;

namespace PalashDynamics.IPD
{
    public partial class IPDAdmission : UserControl,IInitiateCIMS
    {

        bool IsPatientExist = false;
        bool IsPageLoded = false;
        int ClickedFlag = 0;
        WaitIndicator wait = new WaitIndicator();

        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        public void Initiate(string Mode)
        {
            IsPatientExist = true;
            
            if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                IsPatientExist = false;

                return;
            }

            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                IsPatientExist = false;
                return;
            }

            switch (Mode)
            {

                case "NEW":
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                        mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                        this.DataContext = new clsIPDAdmissionVO()
                                        {
                                            PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                                            PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                                            Date  = DateTime.Now, Time = DateTime.Now
                                        };
                    break;
                    
                case "EDIT":
                    IsPatientExist = true;
                    

                    break;

            }
        }

        public IPDAdmission()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IPDAdmission_Loaded);
        }
   
        void IPDAdmission_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }

             if (!IsPageLoded && IsPatientExist)
             {              
                  txtUnit.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName;
                  FillDepartmentMaster(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                  FillDoctorList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,0);
                  FillAdmissionType();
                  FillBedCategory();
                  FillWard();
                  FillRelation();
                  fillCoupleDetails();

                 
             }
        }
             
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = CheckValidations();

                if (saveDtls == true)
                {
                    string msgText = "";
                    msgText = "Are you sure you want to save the Admission Details";
                   

                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

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
                SaveDetails();
            else
                ClickedFlag = 0;

        }

        private void SaveDetails()
        {

            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsSaveIPDAdmissionBizActionVO BizAction = new clsSaveIPDAdmissionBizActionVO();
                BizAction.Details = (clsIPDAdmissionVO)this.DataContext;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.Details.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                if (cmbDoctor.SelectedItem != null)
                    BizAction.Details.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;

                if (cmbWard.SelectedItem != null)
                    BizAction.Details.WardID = ((MasterListItem)cmbWard.SelectedItem).ID;

                if (cmbBedCategory.SelectedItem != null)
                    BizAction.Details.BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;

                if (cmbAdmissionType.SelectedItem != null)
                    BizAction.Details.AdmissionTypeID = ((MasterListItem)cmbAdmissionType.SelectedItem).ID;

                if (cmbRelation.SelectedItem != null)
                    BizAction.Details.KinRelationID = ((MasterListItem)cmbRelation.SelectedItem).ID;

                if (cmbDoctor1.SelectedItem != null)
                    BizAction.Details.Doctor1_ID = ((MasterListItem)cmbDoctor1.SelectedItem).ID;

                if (cmbDoctor2.SelectedItem != null)
                    BizAction.Details.Doctor2_ID = ((MasterListItem)cmbDoctor2.SelectedItem).ID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsSaveIPDAdmissionBizActionVO)arg.Result).Details != null)
                    {
                        CmdSave.IsEnabled = false;

                        Indicatior.Close();
                        txtIPDNumber.Text = ((clsSaveIPDAdmissionBizActionVO)arg.Result).Details.AdmissionNO;
                        
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Admission Details Saved Successfully. Do you want to Add Sponsor", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                       
                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            if (re == MessageBoxResult.Yes)
                            {
                                SponsorWindow sponsor = new SponsorWindow();
                                sponsor.Initiate("NEWR");
                                ((IApplicationConfiguration)App.Current).OpenMainContent(sponsor as UIElement);
                            }

                        };
                        msgW1.Show();
                    }
                    else
                    {
                        CmdSave.IsEnabled = true;
                        ClickedFlag = 0;
                        Indicatior.Close();

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while Saving Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {
                Indicatior.Close();
                CmdSave.IsEnabled = true;
                ClickedFlag = 0;
                throw;
            }

        }

        private bool CheckValidations()
        {
            bool isValid = true;



            return isValid;

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
        }

        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPageLoded && cmbDepartment.SelectedItem != null)
            {
                FillDoctorList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDepartment.SelectedItem).ID);
               // FillDoctor_1_List(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDepartment.SelectedItem).ID);
                //FillDoctor_2_List(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDepartment.SelectedItem).ID);

            }
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CmdBedSearch_Click(object sender, RoutedEventArgs e)
        {
            frmSearchBed win = new frmSearchBed();
            if (cmbBedCategory.SelectedItem != null)
                ((clsIPDAdmissionVO)this.DataContext).BedCategoryID = ((MasterListItem)cmbBedCategory.SelectedItem).ID;

            if (cmbWard.SelectedItem != null)
                ((clsIPDAdmissionVO)this.DataContext).WardID = ((MasterListItem)cmbWard.SelectedItem).ID;


            win.WardID = ((clsIPDAdmissionVO)this.DataContext).WardID;
            win.BedCategoryID = ((clsIPDAdmissionVO)this.DataContext).BedCategoryID;
            win.OnOKButton_Click += new RoutedEventHandler(win_OnOKButton_Click);
            win.Show();

        }

        void win_OnOKButton_Click(object sender, RoutedEventArgs e)
        {
            frmSearchBed win = (frmSearchBed)sender;

            if(win!=null)
            {
                ((clsIPDAdmissionVO)this.DataContext).BedCategoryID = win.BedCategoryID;
                ((clsIPDAdmissionVO)this.DataContext).WardID = win.WardID;
                ((clsIPDAdmissionVO)this.DataContext).BedID = win.BedID;
                ((clsIPDAdmissionVO)this.DataContext).BedNo = win.BedNO;
               
                txtBedNo.Text = ((clsIPDAdmissionVO)this.DataContext).BedNo;
                cmbWard.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).WardID;
                cmbBedCategory.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).BedCategoryID;

            }
        }

        private void FillAdmissionType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_AdmissionType;
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
                    cmbAdmissionType.ItemsSource = null;
                    cmbAdmissionType.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbAdmissionType.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).AdmissionTypeID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillBedCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ClassMaster;
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

                    cmbBedCategory.ItemsSource = null;
                    cmbBedCategory.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbBedCategory.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).BedCategoryID;
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillWard()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_WardMaster;
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



                    cmbWard.ItemsSource = null;
                    cmbWard.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbWard.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).WardID;


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillDepartmentMaster(long IUnitID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;
            BizAction.IsActive = true;

            if (IUnitID > 0)
                BizAction.Parent = new KeyValue { Key = IUnitID, Value = "UnitId" };
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



                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbDepartment.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DepartmentID;

                    if (((clsIPDAdmissionVO)this.DataContext).DepartmentID == 0)
                    {
                        cmbDepartment.TextBox.SetValidation("Please select the Department");

                        cmbDepartment.TextBox.RaiseValidationError();
                    }
                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        private void FillDoctorList(long iUnitID, long iDeptID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = iUnitID;

            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;//.DeepCopy();

                    cmbDoctor1.ItemsSource = null;
                    cmbDoctor1.ItemsSource = objList;//.DeepCopy();

                    cmbDoctor2.ItemsSource = null;
                    cmbDoctor2.ItemsSource = objList;

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);
                    }

                    if (iDeptID == 0)
                    {
                        txtReferenceDoctor.ItemsSource = null;
                        txtReferenceDoctor.ItemsSource = objList;//.DeepCopy();
                    }
                    else
                    {
                        cmbDoctor.ItemsSource = null;
                        cmbDoctor.ItemsSource = objList;//.DeepCopy();

                        cmbDoctor1.ItemsSource = null;
                        cmbDoctor1.ItemsSource = objList;//.DeepCopy();

                        cmbDoctor2.ItemsSource = null;
                        cmbDoctor2.ItemsSource = objList;//.DeepCopy();

                        if (this.DataContext != null)
                        {
                                //if (objList.Count > 0)
                                //{
                                    //if (objList.Count > 1)
                                    //    cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;
                                    //else
                                    cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;

                                    cmbDoctor1.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor1_ID;
                                    cmbDoctor2.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor2_ID;

                                    //if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                                    //{
                                    //    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                                    //    cmbDoctor.TextBox.RaiseValidationError();
                                    //}
                                //}
                        }
                    }
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDoctor_1_List(long iUnitID, long iDeptID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = iUnitID;

            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);
                    }

                    //if (iDeptID == 0)
                    //{
                    //    txtReferenceDoctor.ItemsSource = null;
                    //    txtReferenceDoctor.ItemsSource = objList;//.DeepCopy();
                    //}
                    //else
                    //{
                        //cmbDoctor.ItemsSource = null;
                        //cmbDoctor.ItemsSource = objList;//.DeepCopy();

                        cmbDoctor1.ItemsSource = null;
                        cmbDoctor1.ItemsSource = objList;//.DeepCopy();

                        //cmbDoctor2.ItemsSource = null;
                        //cmbDoctor2.ItemsSource = objList;//.DeepCopy();

                        if (this.DataContext != null)
                        {
                            //if (objList.Count > 0)
                            //{
                            //if (objList.Count > 1)
                            //    cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;
                            //else
                          //  cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;

                            cmbDoctor1.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor1_ID;
                         //   cmbDoctor2.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor2_ID;

                            //if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                            //{
                            //    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                            //    cmbDoctor.TextBox.RaiseValidationError();
                            //}
                            //}
                        }
                    //}
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillDoctor_2_List(long iUnitID, long iDeptID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = iUnitID;

            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);
                    }

                    //if (iDeptID == 0)
                    //{
                    //    txtReferenceDoctor.ItemsSource = null;
                    //    txtReferenceDoctor.ItemsSource = objList;//.DeepCopy();
                    //}
                    //else
                    //{
                    //cmbDoctor.ItemsSource = null;
                    //cmbDoctor.ItemsSource = objList;//.DeepCopy();

                    cmbDoctor2.ItemsSource = null;
                    cmbDoctor2.ItemsSource = objList;//.DeepCopy();

                    //cmbDoctor2.ItemsSource = null;
                    //cmbDoctor2.ItemsSource = objList;//.DeepCopy();

                    if (this.DataContext != null)
                    {
                        //if (objList.Count > 0)
                        //{
                        //if (objList.Count > 1)
                        //    cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;
                        //else
                        //  cmbDoctor.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).DoctorID;

                        cmbDoctor2.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor2_ID;
                        //   cmbDoctor2.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).Doctor2_ID;

                        //if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                        //{
                        //    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                        //    cmbDoctor.TextBox.RaiseValidationError();
                        //}
                        //}
                    }
                    //}
                }
            };

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        
        private void FillRelation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
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

                    cmbRelation.ItemsSource = null;
                    cmbRelation.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbRelation.SelectedValue = ((clsIPDAdmissionVO)this.DataContext).KinRelationID;
                }
                IsPageLoded = true;
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }

        #region Fill Couple Details

        private void fillCoupleDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails.CoupleId == 0)
            {
                wait.Close();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "IPD is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

            }
            else
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
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        if (CoupleDetails.CoupleId == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Cannot Create Therapy, Therapy is Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        }
                        else
                        {
                            //getEMRDetails(BizAction.CoupleDetails.FemalePatient, "F");
                            //getEMRDetails(BizAction.CoupleDetails.MalePatient, "M");
                            GetHeightAndWeight(BizAction.CoupleDetails);

                            //added by priti
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                                imgPhoto13.Source = bmp;
                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                imgP1.Visibility = System.Windows.Visibility.Visible;
                            }
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                                imgPhoto12.Source = bmp;
                                imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                imgP2.Visibility = System.Windows.Visibility.Visible;
                            }
                            //fillETDetails();
                        }
                    }
                    wait.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
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
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                        FemalePatientDetails = CoupleDetails.FemalePatient;
                        FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                        FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                        //FemalePatientDetails.BMI = BizAction.CoupleDetails.FemalePatient.BMI;
                        FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                        FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                        Female.DataContext = FemalePatientDetails;

                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        MalePatientDetails = CoupleDetails.MalePatient;
                        MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                        MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                        //MalePatientDetails.BMI = BizAction.CoupleDetails.MalePatient.BMI;
                        MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                        MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                        Male.DataContext = MalePatientDetails;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        #endregion
        #region Get Patient EMR Details(Height and Weight)

        private void getEMRDetails(clsPatientGeneralVO PatientDetails, string Gender)
        {
            clsGetEMRDetailsBizActionVO BizAction = new clsGetEMRDetailsBizActionVO();
            BizAction.PatientID = PatientDetails.PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.TemplateID = 8;//Using For Getting Height Wight Of Patient 
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Double height = 0;
            Double weight = 0;

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.EMRDetailsList = ((clsGetEMRDetailsBizActionVO)args.Result).EMRDetailsList;

                    if (BizAction.EMRDetailsList != null || BizAction.EMRDetailsList.Count > 0)
                    {
                        for (int i = 0; i < BizAction.EMRDetailsList.Count; i++)
                        {
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Height"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    height = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (BizAction.EMRDetailsList[i].ControlCaption.Equals("Weight"))
                            {
                                if (!string.IsNullOrEmpty(BizAction.EMRDetailsList[i].Value))
                                {
                                    weight = Convert.ToDouble(BizAction.EMRDetailsList[i].Value);
                                    if (height != 0 && weight != 0)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (height != 0 && weight != 0)
                        {
                            if (Gender.Equals("F"))
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                PatientDetails.Height = height;
                                PatientDetails.Weight = weight;
                                PatientDetails.BMI = Math.Round(CalculateBMI(height, weight), 2);
                                Male.DataContext = PatientDetails;
                            }
                        }
                        else
                        {
                            if (Gender.Equals("F"))
                            {
                                Female.DataContext = PatientDetails;
                            }
                            else
                            {
                                Male.DataContext = PatientDetails;
                            }
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion
        #region Calculate BMI
        private double CalculateBMI(Double Height, Double Weight)
        {
            try
            {
                if (Weight == 0)
                {
                    return 0.0;

                }
                else if (Height == 0)
                {

                    return 0.0;
                }
                else
                {
                    double weight = Convert.ToDouble(Weight);
                    double height = Convert.ToDouble(Height);
                    double TotalBMI = weight / height;
                    TotalBMI = TotalBMI / height;
                    TotalBMI = TotalBMI * 10000;
                    return TotalBMI;

                }
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }
        #endregion

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            if (FemaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            if (MaleAlert.Text.Trim().Length > 0)
            {
                frmAttention PatientAlert = new frmAttention();
                PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
                PatientAlert.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Attention not entered.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }

    }
}
