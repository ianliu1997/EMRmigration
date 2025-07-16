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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Billing;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.CRM.LoyaltyProgram;


namespace PalashDynamics.CRM
{
    public partial class LoyaltyCardIssueTemplate : ChildWindow
    {
        public LoyaltyCardIssueTemplate()
        {
            InitializeComponent();
        }
        int ClickedFlag = 0;
        #region Variable Region
        public clsPaymentVO Payment { get; set; }

         bool UseAppDoctorID = true;

        public long LTariffID { get; set; }
        public long FMTariffID { get; set; }
        public long SelfTariffID { get; set;}
        public string SelfTariff { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        public List<clsPatientFamilyDetailsVO> FamilyList { get; set; }
        public List<clsPatientServiceDetails> ServiceList { get; set; }
        clsPatientFamilyDetailsVO FamilyVO { get; set; }
        public long PatientUnitID { get; set; }
        bool IsPageLoded = false;
       
        
       

        public event RoutedEventHandler OnSaveButton_Click;
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LoyaltyCardIssueTemplate_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                this.DataContext = new clsPatientVO();
                FamilyList = new List<clsPatientFamilyDetailsVO>();
                ServiceList = new List<clsPatientServiceDetails>();


                //FillLoyaltyProgram();
                FillCardType();
                FillGender();
                FillUnitList();
                FillRelation();
                FillFamilyTariff();
                GetPatientInfo();

                dtpIssueDate.SelectedDate = DateTime.Today;
                txtLoyaltyCardNumber.Focus();
            }
            IsPageLoded = true;
        }

        #region FillCombobox
        private void FillLoyaltyProgram()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
            
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

                    cmbCardType.ItemsSource = null;

                    cmbCardType.ItemsSource = objList;
                    cmbCardType.SelectedItem = objList[0];
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillCardType()
        {
            clsFillCardTypeComboBizActionVO BizAction = new clsFillCardTypeComboBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsFillCardTypeComboBizActionVO)arg.Result).MasterList);

                    cmbCardType.ItemsSource = null;

                    cmbCardType.ItemsSource = objList;
                    cmbCardType.SelectedItem = objList[0];
                    if (this.DataContext != null)
                    {
                        cmbCardType.SelectedValue = ((clsPatientVO)this.DataContext).LoyaltyCardID;
                            
                    }
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        

        private void FillTariff(long iProgramID)
        {
            clsGetLoyaltyProgramTariffByIDBizActionVO BizAction = new clsGetLoyaltyProgramTariffByIDBizActionVO();
            BizAction.Details = new clsLoyaltyProgramVO();
            BizAction.ID = iProgramID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    cmbTariff.SelectedItem = ((clsGetLoyaltyProgramTariffByIDBizActionVO)arg.Result).Details.Tariff;
                    LTariffID = ((clsGetLoyaltyProgramTariffByIDBizActionVO)arg.Result).Details.TariffID;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

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
                }
                if (this.DataContext != null)
                {
                    cmbClinic.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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

                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        private void FillRelation()
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

                    cmbRelation.ItemsSource = null;
                    cmbRelation.ItemsSource = objList;

                    cmbRelation.SelectedItem = objList[0];
                }
                if (this.DataContext != null)
                {
                    cmbRelation.SelectedValue = ((clsPatientVO)this.DataContext).RelationID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillFamilyTariff()
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
                    cmbApplicableTariff.SelectedValue = ((clsPatientVO)this.DataContext).TariffID;

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();


        }

        //private void FillFamilyTariff(long iRelationID, long iProgramID)
        //{
        //    clsFillFamilyTariffUsingRelationIDBizActionVO BizAction = new clsFillFamilyTariffUsingRelationIDBizActionVO();
        //    BizAction.Details = new clsLoyaltyProgramFamilyDetails();
        //    BizAction.RelationID = iRelationID;
        //    BizAction.LoyaltyProgramID = iProgramID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            cmbApplicableTariff.SelectedItem = ((clsFillFamilyTariffUsingRelationIDBizActionVO)arg.Result).Details.Tariff;
        //            FMTariffID = ((clsFillFamilyTariffUsingRelationIDBizActionVO)arg.Result).Details.TariffID;
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //} 

        #endregion

        #region Patient Details
        private void GetPatientInfo()
        {
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if ((clsGetPatientBizActionVO)arg.Result != null)
                        {
                            clsGetPatientBizActionVO objPatient = new clsGetPatientBizActionVO();
                            objPatient = (clsGetPatientBizActionVO)arg.Result;
                            txtMRNo.Text = objPatient.PatientDetails.GeneralDetails.MRNo;
                            txtFirstName.Text = objPatient.PatientDetails.GeneralDetails.FirstName;
                            if (objPatient.PatientDetails.GeneralDetails.MiddleName != null && objPatient.PatientDetails.GeneralDetails.MiddleName != "")
                            {
                                txtMiddleName.Text = objPatient.PatientDetails.GeneralDetails.MiddleName;
                            }
                            txtLastName.Text = objPatient.PatientDetails.GeneralDetails.LastName;
                            dtpRegistrationDate.SelectedDate = objPatient.PatientDetails.GeneralDetails.RegistrationDate;
                            cmbClinic.SelectedValue = objPatient.PatientDetails.GeneralDetails.UnitId;
                            ((clsPatientVO)this.DataContext).GeneralDetails.UnitId = objPatient.PatientDetails.GeneralDetails.UnitId;
                            FillUnitList();
                            
                            txtAddress1.Text = objPatient.PatientDetails.AddressLine1;
                            txtAddress2.Text = objPatient.PatientDetails.AddressLine2;
                            txtAddress3.Text = objPatient.PatientDetails.AddressLine3;
                            txtEmail.Text = objPatient.PatientDetails.Email;
                            
                            cmbGender.SelectedValue = objPatient.PatientDetails.GenderID;
                            ((clsPatientVO)this.DataContext).GenderID = objPatient.PatientDetails.GenderID;
                            FillGender();

                            txtContactNo1.Text = objPatient.PatientDetails.ContactNo1;
                            txtContactNo2.Text = objPatient.PatientDetails.ContactNo2;
                            txtMobileCountryCode.Text = objPatient.PatientDetails.MobileCountryCode.ToString();

                            txtResiCountryCode.Text = objPatient.PatientDetails.ResiNoCountryCode.ToString();
                            txtResiSTD.Text = objPatient.PatientDetails.ResiSTDCode.ToString();
                            txtMemberLastName.Text = objPatient.PatientDetails.GeneralDetails.LastName;
                            PatientUnitID = objPatient.PatientDetails.GeneralDetails.UnitId;

                        }
                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        
        private void cmbCardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbCardType.SelectedItem != null)
            {
                FillTariff(((MasterListItem)cmbCardType.SelectedItem).ID);
            }
            if ((MasterListItem)cmbCardType.SelectedItem != null && ((MasterListItem)cmbCardType.SelectedItem).ID != 0)
            {
                clsGetLoyaltyProgramByIDBizActionVO BizAction = new clsGetLoyaltyProgramByIDBizActionVO();
                BizAction.LoyaltyProgramDetails = new clsLoyaltyProgramVO();
                BizAction.ID = ((MasterListItem)cmbCardType.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsGetLoyaltyProgramByIDBizActionVO)arg.Result).LoyaltyProgramDetails != null)
                            {
                                clsGetLoyaltyProgramByIDBizActionVO ObjLoyalty = new clsGetLoyaltyProgramByIDBizActionVO();
                                ObjLoyalty = (clsGetLoyaltyProgramByIDBizActionVO)arg.Result;

                                FromDate = Convert.ToDateTime((ObjLoyalty.LoyaltyProgramDetails.EffectiveDate).Value.ToShortDateString());
                                ToDate = Convert.ToDateTime((ObjLoyalty.LoyaltyProgramDetails.ExpiryDate).Value.ToShortDateString());
                                SelfTariffID = ObjLoyalty.LoyaltyProgramDetails.TariffID;
                                SelfTariff = ObjLoyalty.LoyaltyProgramDetails.Tariff;
                                FillFamilyGridWithSelf();

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }



        }


       
        private void hlCharges_Click(object sender, RoutedEventArgs e)
        {
            PaymentDetails paymentWin = new PaymentDetails();
            paymentWin.OnPaymentSaveButton_Click += new RoutedEventHandler(paymentWin_OnPaymentSaveButton_Click);
            paymentWin.OnPaymentCancelButton_Click += new RoutedEventHandler(paymentWin_OnPaymentCancelButton_Click);
            paymentWin.Show();

        }
        long PaymodeID {get;set;}
        double PaidAmount { get; set; }
        string Number { get; set; }
        DateTime? Date { get; set; }
        long BankID { get; set; }

        void paymentWin_OnPaymentSaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            txtCharges.Text = Convert.ToString(((PaymentDetails)sender).LPatientCharge);
            PaymodeID = ((MasterListItem)(((PaymentDetails)sender).cmbPayMode.SelectedItem)).ID;
            PaidAmount = Convert.ToDouble(((PaymentDetails)sender).txtPaidAmount.Text);
            Number = ((PaymentDetails)sender).txtNumber.Text;
            Date = ((PaymentDetails)sender).dtpDate.SelectedDate;
            BankID = ((MasterListItem)(((PaymentDetails)sender).cmbBank.SelectedItem)).ID;

        }
        void paymentWin_OnPaymentCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }

        WaitIndicator Indicatior = null;
        private void cmdIssue_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
                
            try
            {
                if (ClickedFlag == 1)
                {
                    bool LoyaltyCardIssue = true;
                    LoyaltyCardIssue = CheckValidations();
                    if (LoyaltyCardIssue == true)
                    {


                             Payment = new clsPaymentVO();
                            Payment.CreditAuthorizedBy = 0;
                            Payment.ChequeAuthorizedBy = 0;

                            clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();

                            //objPay.PaymentModeID = ((MasterListItem)(paymentWin.cmbPayMode.SelectedItem)).ID;
                            //objPay.PaidAmount = Convert.ToDouble(paymentWin.txtPaidAmount.Text);
                            //objPay.Number = paymentWin.txtNumber.Text;
                            //objPay.Date = paymentWin.dtpDate.SelectedDate;
                            //if (paymentWin.cmbBank.SelectedItem != null)
                            //    objPay.BankID = ((MasterListItem)(paymentWin.cmbBank.SelectedItem)).ID;
                            
                            objPay.PaymentModeID = PaymodeID;
                           
                            objPay.PaidAmount = PaidAmount;
                          
                            objPay.Number = Number;
                           
                            objPay.Date = Date;
                            
                            if (BankID != null && BankID != 0)
                                objPay.BankID = BankID; 

                        Payment.PaymentDetails.Add(objPay);
                        
                        clsBillVO objBill = new clsBillVO();
                        objBill.Date = DateTime.Now;

                        objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        objBill.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        objBill.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        objBill.Opd_Ipd_External = 1;


                        objBill.TotalBillAmount = PaidAmount;
                        objBill.NetBillAmount = PaidAmount;
                        objBill.IsFreezed = true;

                        if (Payment != null)
                        {
                            objBill.PaymentDetails = Payment;
                        }

                        clsChargeVO objCharge = new clsChargeVO();
                        objCharge.Date = DateTime.Now;
                        objCharge.Rate = PaidAmount;
                        objCharge.Quantity = 1;
                        objCharge.IsBilled = true;
                        objBill.ChargeDetails.Add(objCharge);


                        clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                        BizAction.Details = new clsBillVO();
                        BizAction.Details = objBill;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            ClickedFlag = 0;
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {
                                    
                                    if (((clsAddBillBizActionVO)arg.Result).Details != null)
                                    {
                                        if (OnSaveButton_Click != null)
                                        {
                                            this.DialogResult = true;
                                            OnSaveButton_Click(this, new RoutedEventArgs());
                                            
                                        }

                                    }
                                }
                            }
                            else
                            {
                               
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }

                        };

                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            

        #region Validation
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;


        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);

                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);

                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            }
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);

                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);

                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            }

            txtDD.SelectAll();

        }

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            {
                int val = int.Parse(txtYY.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);

                }
            }

            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            {
                int val = int.Parse(txtMM.Text.Trim());
                if (val > 0)
                {
                    dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);

                }
            }

            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            {
                int val = int.Parse(txtDD.Text.Trim());
                if (val > 0)
                    dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            }

        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {

            }
            else if (!((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void dtpEffectiveDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpEffectiveDate.SelectedDate == null)
            {
                dtpEffectiveDate.SetValidation("Please select Effective Date");
                dtpEffectiveDate.RaiseValidationError();
            }
            else if (dtpEffectiveDate.SelectedDate < FromDate )
            {

                dtpEffectiveDate.SetValidation("Effective Date can not be less than" + " " + FromDate);
                dtpEffectiveDate.RaiseValidationError();
            }
            else if (dtpEffectiveDate.SelectedDate > ToDate)
            {
                dtpEffectiveDate.SetValidation("Effective Date can not be greater than" + " " + ToDate);
                dtpEffectiveDate.RaiseValidationError();
            }

            else
                dtpEffectiveDate.ClearValidationError();

        }

        private void dtpExpiryDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpExpiryDate.SelectedDate == null)
            {
                dtpExpiryDate.SetValidation("Please select Expiry Date");
                dtpExpiryDate.RaiseValidationError();
            }
            else
                dtpExpiryDate.ClearValidationError();
        }


        private bool CheckValidations()
        {
            bool result = true;
            try
            {


                if (cmbTariff.SelectedItem == null)
                {
                    cmbTariff.TextBox.SetValidation("Please Select Tariff");
                    cmbTariff.TextBox.RaiseValidationError();
                    cmbTariff.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    cmbTariff.TextBox.ClearValidationError();



                if (txtCharges.Text == "")
                {

                    txtCharges.SetValidation("Please Enter Charges");
                    txtCharges.RaiseValidationError();
                    txtCharges.Focus();
                    TabControlMain.SelectedIndex = 0;

                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtCharges.ClearValidationError();


                if ((MasterListItem)cmbCardType.SelectedItem == null)
                {
                    cmbCardType.TextBox.SetValidation("Please Select Card Type");
                    cmbCardType.TextBox.RaiseValidationError();
                    TabControlMain.SelectedIndex = 0;
                    cmbCardType.Focus();
                    result = false;
                    ClickedFlag = 0;
                }
                else if (((MasterListItem)cmbCardType.SelectedItem).ID == 0)
                {
                    cmbCardType.TextBox.SetValidation("Please Select Card Type");
                    cmbCardType.TextBox.RaiseValidationError();
                    cmbCardType.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    cmbCardType.TextBox.ClearValidationError();



                if (txtLoyaltyCardNumber.Text.Length < 12)
                {
                    txtLoyaltyCardNumber.SetValidation("Please Enter Valid Loyalty Card Number");
                    txtLoyaltyCardNumber.RaiseValidationError();
                    txtLoyaltyCardNumber.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    ClickedFlag = 0;
                }
                else if (txtLoyaltyCardNumber.Text == "")
                {

                    txtLoyaltyCardNumber.SetValidation("Please Enter Loyalty Card Number");
                    txtLoyaltyCardNumber.RaiseValidationError();
                    txtLoyaltyCardNumber.Focus();
                    TabControlMain.SelectedIndex = 0;
                    result = false;
                    ClickedFlag = 0;
                }
                else
                    txtLoyaltyCardNumber.ClearValidationError();
                
                
                if (IsPageLoded)
                {
                    if (dtpEffectiveDate.SelectedDate == null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW11 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Effective Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW11.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW11.Show();
                        //dtpEffectiveDate.SetValidation("Please select Effective Date");
                        //dtpEffectiveDate.RaiseValidationError();
                        //dtpEffectiveDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;

                    }
                    else if (dtpEffectiveDate.SelectedDate < DateTime.Today)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW15 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Effective Date can not be less than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW15.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW15.Show();
                        //dtpEffectiveDate.SetValidation("Effective Date can not be less than Today");
                        //dtpEffectiveDate.RaiseValidationError();
                        //dtpEffectiveDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;
                    }
                    else if (dtpEffectiveDate.SelectedDate < FromDate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW16 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Effective Date can not be less than From Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW16.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW16.Show();

                        //dtpEffectiveDate.SetValidation("Effective Date can not be less than" + " " + FromDate);
                        //dtpEffectiveDate.RaiseValidationError();
                        //dtpEffectiveDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;

                    }
                    else if (dtpEffectiveDate.SelectedDate > ToDate)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW17 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Effective Date can not be greater than To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW17.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW17.Show();
                        //dtpEffectiveDate.SetValidation("Effective Date can not be greater than" + " " + ToDate);
                        //dtpEffectiveDate.RaiseValidationError();
                        //dtpEffectiveDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;
                    }
                    else
                        dtpEffectiveDate.ClearValidationError();

                    if (dtpExpiryDate.SelectedDate == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW18 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Expiry Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW18.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW18.Show();
                        //dtpExpiryDate.SetValidation("Please Select Expiry Date ");
                        //dtpExpiryDate.RaiseValidationError();
                        //dtpExpiryDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;

                    }
                    else if (dtpExpiryDate.SelectedDate < DateTime.Today)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW19 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry Date can not be less than Today.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW19.OnMessageBoxClosed += (MessageBoxResult res1) =>
                        {
                            if (res1 == MessageBoxResult.OK)
                            {
                                dtpEffectiveDate.Focus();

                            }
                        };

                        msgW19.Show();
                        //dtpExpiryDate.SetValidation("Expiry Date can not be less than Today");
                        //dtpExpiryDate.RaiseValidationError();
                        //dtpExpiryDate.Focus();
                        TabControlMain.SelectedIndex = 0;
                        ClickedFlag = 0;
                        result = false;
                        return result;
                    }

                    else
                        dtpExpiryDate.ClearValidationError();
                }

            }
            catch (Exception ex)
            {
                throw;
                
            }
            return result;
        }

        #endregion
        #endregion

        #region Family Details
        private void cmbRelation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCardType.SelectedItem != null && cmbRelation.SelectedItem != null)
            {
                if (((MasterListItem)cmbCardType.SelectedItem).ID != 0 && ((MasterListItem)cmbRelation.SelectedItem).ID != 0)
                {
                    clsFillFamilyTariffUsingRelationIDBizActionVO BizAction = new clsFillFamilyTariffUsingRelationIDBizActionVO();
                    BizAction.Details = new clsLoyaltyProgramFamilyDetails();
                    BizAction.RelationID = ((MasterListItem)cmbRelation.SelectedItem).ID;
                    BizAction.LoyaltyProgramID = ((MasterListItem)cmbCardType.SelectedItem).ID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            cmbApplicableTariff.SelectedValue = ((clsFillFamilyTariffUsingRelationIDBizActionVO)arg.Result).Details.TariffID;
                            
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
            }
        }

        private void dgFamilyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem != null)
            {
                if (dgFamilyList.SelectedIndex != 0)
                {
                    FamilyVO = new clsPatientFamilyDetailsVO();
                    FamilyVO = (clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem;
                }
            }
        }

        private void hlbEditMember_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamilyList.SelectedIndex != 0)
            {
                txtMemberFirstName.Text = FamilyVO.FirstName;
                txtMemberMiddleName.Text = FamilyVO.MiddleName;
                txtMemberLastName.Text = FamilyVO.LastName;
                dtpDOB.SelectedDate = FamilyVO.DateOfBirth;
                cmbRelation.SelectedValue = FamilyVO.RelationID;
                cmbTariff.SelectedValue = FamilyVO.TariffID;
                    
                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                txtYY.SelectAll();

                
            }
        }

        private void cmdDeleteMember_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamilyList.SelectedItem != null)
            {
                if (dgFamilyList.SelectedIndex != 0)
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

        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            bool IsExists = false;
            bool AddFamilyList = true;

            AddFamilyList = ChkFamilyValidation();

            if (AddFamilyList == true)
            {
                if (FamilyList.Count > 0)
                {
                    for (int i = 0; i < FamilyList.Count; i++)
                    {
                        if (FamilyList[i].RelationID.Equals(((MasterListItem)cmbRelation.SelectedItem).ID) || FamilyList[i].FirstName == (txtMemberFirstName.Text))
                        {
                            IsExists = true;
                            break;
                        }
                    }
                    if (IsExists == false)
                    {

                        FamilyList.Add(new clsPatientFamilyDetailsVO
                        {
                            PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                            FirstName = txtMemberFirstName.Text,
                            MiddleName = txtMemberMiddleName.Text,
                            LastName = txtMemberLastName.Text,
                            DateOfBirth = dtpDOB.SelectedDate,
                            RelationID = ((MasterListItem)cmbRelation.SelectedItem).ID,
                            Relation = ((MasterListItem)cmbRelation.SelectedItem).Description,
                            TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID,
                            Tariff = ((MasterListItem)cmbApplicableTariff.SelectedItem).Description,
                            PatientUnitID=PatientUnitID,
                            Status = true
                        }
                        );
                        dgFamilyList.ItemsSource = FamilyList;
                        dgFamilyList.Focus();
                        dgFamilyList.UpdateLayout();
                        dgFamilyList.SelectedIndex = FamilyList.Count - 1;

                        txtMemberFirstName.Text = "";
                        txtMemberMiddleName.Text = "";
                        dtpDOB.SelectedDate = null;
                        txtYY.Text = "";
                        txtMM.Text = "";
                        txtDD.Text = "";

                        GetServiceByTariffID(((MasterListItem)cmbApplicableTariff.SelectedItem).ID, ((MasterListItem)cmbRelation.SelectedItem).ID);
                    }

                    else
                    {
                        string msgTitle = "";
                        string msgText = "Family member is already exists";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWD.Show();

                    }
                    

                }
            }





        }
        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            bool IsExists = false;
            bool EditFamilyList = true;

            EditFamilyList = ChkFamilyValidation();

            if (EditFamilyList == true)
            {
                if (FamilyList.Count > 0)
                {
                    int var = dgFamilyList.SelectedIndex;
                    FamilyList.RemoveAt(dgFamilyList.SelectedIndex);

                    if (FamilyList[0].RelationID.Equals(((MasterListItem)cmbRelation.SelectedItem).ID))
                    {
                        IsExists = true;
                    }
                    if (IsExists == false)
                    {
                        FamilyList.Insert(var, new clsPatientFamilyDetailsVO
                        {
                            PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                            FirstName = txtMemberFirstName.Text,
                            MiddleName = txtMemberMiddleName.Text,
                            LastName = txtMemberLastName.Text,
                            DateOfBirth = dtpDOB.SelectedDate,
                            RelationID = ((MasterListItem)cmbRelation.SelectedItem).ID,
                            Relation = ((MasterListItem)cmbRelation.SelectedItem).Description,
                            TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID,
                            Tariff = ((MasterListItem)cmbApplicableTariff.SelectedItem).Description,
                            PatientUnitID=PatientUnitID,
                            Status = true
                        }
                        );
                        dgFamilyList.ItemsSource = null;
                        dgFamilyList.ItemsSource = FamilyList;
                        dgFamilyList.Focus();
                        dgFamilyList.UpdateLayout();
                        //dgFamilyList.SelectedIndex = FamilyList.Count - 1;


                        txtMemberFirstName.Text = "";
                        txtMemberMiddleName.Text = "";
                        txtMemberLastName.Text = "";
                        dtpDOB.SelectedDate = null;
                        txtYY.Text = "";
                        txtMM.Text = "";
                        txtDD.Text = "";
                        cmbRelation.SelectedValue = 0;
                        cmbTariff.SelectedValue = 0;
                    }
                    else
                    {
                        string msgTitle = "";
                        string msgText = "Family member is already exists";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWD.Show();

                    }
                }


            }

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

        #region Validation
        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDOB.SelectedDate > DateTime.Now)
            {
                dtpDOB.SetValidation("Date of birth can not be greater than Today");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else if (dtpDOB.SelectedDate == null)
            {
                dtpDOB.SetValidation("Please select the Date of birth");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else
            {
                dtpDOB.ClearValidationError();
                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            }

            txtYY.SelectAll();
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtMemberFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMemberFirstName.Text = txtMemberFirstName.Text.ToTitleCase();

        }

        private void txtMemberMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMemberMiddleName.Text = txtMemberMiddleName.Text.ToTitleCase();
        }

        private void txtMemberLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMemberLastName.Text = txtMemberLastName.Text.ToTitleCase();
        }

        private bool ChkFamilyValidation()
        {
            bool Familyresult = true;


            if ((MasterListItem)cmbApplicableTariff.SelectedItem == null)
            {
                cmbApplicableTariff.TextBox.SetValidation("Please select Tariff");
                cmbApplicableTariff.TextBox.RaiseValidationError();
                cmbApplicableTariff.Focus();
                Familyresult = false;
            }
            else if (((MasterListItem)cmbApplicableTariff.SelectedItem).ID == 0)
            {
                cmbApplicableTariff.TextBox.SetValidation("Please select Tariff");
                cmbApplicableTariff.TextBox.RaiseValidationError();
                cmbApplicableTariff.Focus();
                Familyresult = false;

            }
            else
                cmbApplicableTariff.TextBox.ClearValidationError();            
           

            if ((MasterListItem)cmbRelation.SelectedItem == null)
            {
                cmbRelation.TextBox.SetValidation("Please select Relation");
                cmbRelation.TextBox.RaiseValidationError();
                cmbRelation.Focus();
                Familyresult = false;
            }
            else if (((MasterListItem)cmbRelation.SelectedItem).ID == 0)
            {
                cmbRelation.TextBox.SetValidation("Please select Relation");
                cmbRelation.TextBox.RaiseValidationError();
                cmbRelation.Focus();
                Familyresult = false;

            }
            else
                cmbRelation.TextBox.ClearValidationError();


            if (dtpDOB.SelectedDate > DateTime.Now)
            {

                dtpDOB.SetValidation("Date of birth can not be greater than Today");
                dtpDOB.RaiseValidationError();
                dtpDOB.Focus();
                Familyresult = false;
            }
            else if (dtpDOB.SelectedDate == null)
            {
                dtpDOB.SetValidation("Please select the Date of birth");
                dtpDOB.RaiseValidationError();
                dtpDOB.Focus();
                Familyresult = false;
            }
            else
                dtpDOB.ClearValidationError();


            if (txtMemberLastName.Text == "")
            {

                txtMemberLastName.SetValidation("Please Enter Last Name");
                txtMemberLastName.RaiseValidationError();
                txtMemberLastName.Focus();
                Familyresult = false;
            }
            else
                txtMemberLastName.ClearValidationError();


            if (txtMemberFirstName.Text == "")
            {

                txtMemberFirstName.SetValidation("Please Enter First Name");
                txtMemberFirstName.RaiseValidationError();
                txtMemberFirstName.Focus();
                Familyresult = false;
            }
            else
                txtMemberFirstName.ClearValidationError();

            if (txtYY.Text != "")
            {
                if (Convert.ToInt16(txtYY.Text) > 120)
                {
                    txtYY.SetValidation("Age can not be greater than 121");
                    txtYY.RaiseValidationError();
                    txtYY.Focus();
                    Familyresult = false;
            
                }
                else
                    txtYY.ClearValidationError();
            }



            return Familyresult;

        }
        #endregion

       

       
        private void FillFamilyGridWithSelf()
        {
                clsGetTariffAndRelationFromApplicationConfigurationBizActionVO BizAction = new clsGetTariffAndRelationFromApplicationConfigurationBizActionVO();
                BizAction.FamilyDetails = new List<clsPatientFamilyDetailsVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                FamilyList = new List<clsPatientFamilyDetailsVO>();
                ServiceList = new List<clsPatientServiceDetails>();
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsGetTariffAndRelationFromApplicationConfigurationBizActionVO)arg.Result).FamilyDetails != null)
                            {
                                BizAction.FamilyDetails = new List<clsPatientFamilyDetailsVO>();

                                long AppRelationID = ((clsGetTariffAndRelationFromApplicationConfigurationBizActionVO)arg.Result).FamilyDetails[0].RelationID;
                                for (int i = 0; i < ((clsGetTariffAndRelationFromApplicationConfigurationBizActionVO)arg.Result).FamilyDetails.Count; i++)
                                {
                                    clsPatientFamilyDetailsVO ObjVO = new clsPatientFamilyDetailsVO();
                                    ObjVO = ((clsGetTariffAndRelationFromApplicationConfigurationBizActionVO)arg.Result).FamilyDetails[i];
                                    ObjVO.TariffID = SelfTariffID;
                                    ObjVO.Tariff = SelfTariff;
                                    ObjVO.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                                    ObjVO.FirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                                    ObjVO.MiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                                    ObjVO.LastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                                    ObjVO.DateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                                    ObjVO.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                                    FamilyList.Add(ObjVO);

                                }

                                dgFamilyList.ItemsSource = FamilyList;
                                dgFamilyList.Focus();
                                dgFamilyList.UpdateLayout();

                                GetServiceByTariffID(SelfTariffID, AppRelationID);

                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            

        }

        private DateTime? ConvertDateBack(string parameter, int value, DateTime? DateTobeConvert)
        {
            try
            {
                DateTime BirthDate;
                if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                    BirthDate = DateTobeConvert.Value;
                else
                    BirthDate = DateTime.Now;


                int mValue = Int32.Parse(value.ToString());

                switch (parameter.ToString().ToUpper())
                {
                    case "YY":
                        BirthDate = BirthDate.AddYears(-mValue);

                        break;
                    case "MM":
                        BirthDate = BirthDate.AddMonths(-mValue);
                        // result = (age.Month - 1).ToString();
                        break;
                    case "DD":
                        //result = (age.Day - 1).ToString();
                        BirthDate = BirthDate.AddDays(-mValue);
                        break;
                    default:
                        BirthDate = BirthDate.AddYears(-mValue);
                        break;
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }

        }

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

        #region Service Details
        private void cmdEditTariff_Click(object sender, RoutedEventArgs e)
        {
            if (dgFamilyList.SelectedIndex != 0)
            {
                EditFamilyTariff ObjEditTariff = new EditFamilyTariff();
                if (((MasterListItem)cmbApplicableTariff.SelectedItem).ID != 0)
                {
                    List<clsPatientServiceDetails> ObjServiceList = new List<clsPatientServiceDetails>();
                    long DataRelationID = ((clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem).RelationID;
                    long DataTariffID = ((clsPatientFamilyDetailsVO)dgFamilyList.SelectedItem).TariffID;
                   
                    if (ServiceList != null && ServiceList.Count > 0)
                    {
                      var  ObjService = from r in ServiceList
                                       where r.RelationID == DataRelationID

                                     select new clsPatientServiceDetails
                                       {
                                           ServiceID=r.ServiceID,
                                           ServiceName=r.ServiceName,
                                           Rate=r.Rate,
                                           ConcessionAmount=r.ConcessionAmount,
                                           ConcessionPercentage=r.ConcessionPercentage,
                                           NetAmount=r.NetAmount,
                                           SelectService=r.SelectService,
                                           //LoyaltyID=r.LoyaltyID,
                                           PatientID=r.PatientID,
                                           RelationID=r.RelationID,
                                           TariffID=r.TariffID,
                                           PatientUnitID=PatientUnitID


                                       };

                      if (ObjService.ToList().Count > 0)
                        {
                          ObjServiceList=ObjService.ToList();
                          ObjEditTariff.dgTariffServiceList.ItemsSource = ObjServiceList;
                        }
                    }


                    ObjEditTariff.OnSaveButton_Click += new RoutedEventHandler(ObjEditTariff_OnSaveButton_Click);
                    ObjEditTariff.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "Please select tariff.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            }
        }

        void ObjEditTariff_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((PalashDynamics.CRM.EditFamilyTariff)sender).DialogResult == true)
            {
                List<clsPatientServiceDetails> EditedService = new List<clsPatientServiceDetails>();
                EditedService= ((EditFamilyTariff)sender).lstService;
                if(EditedService!=null && EditedService.Count !=0)
                {
                    for (int i = 0; i < EditedService.Count; i++)
                    {
                        for (int j = 0; j < ServiceList.Count; j++)
                        {
                            if (EditedService[i].RelationID == ServiceList[j].RelationID && EditedService[i].ServiceID == ServiceList[j].ServiceID)
                            {
                                ServiceList[j].ConcessionPercentage = EditedService[i].ConcessionPercentage;
                                ServiceList[j].ConcessionAmount = EditedService[i].ConcessionAmount;
                                ServiceList[j].NetAmount = EditedService[i].NetAmount;
                                ServiceList[j].SelectService = EditedService[i].SelectService;
                                break;
                            }
                        }
                    }
                }
                
              
            }
        }


        private void GetServiceByTariffID(long iTariffID,long iRelationID)
        {
            try
            {
                clsGetServiceForIssueBizActionVO BizAction = new clsGetServiceForIssueBizActionVO();
                BizAction.ServiceList = new List<clsPatientServiceDetails>();
                //BizAction.TariffID = ((MasterListItem)cmbApplicableTariff.SelectedItem).ID;
                BizAction.TariffID = iTariffID;
                

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
             
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsGetServiceForIssueBizActionVO)arg.Result).ServiceList != null)
                        {
                            BizAction.ServiceList = ((clsGetServiceForIssueBizActionVO)arg.Result).ServiceList;


                            foreach (var item in BizAction.ServiceList)
                            {
                                ServiceList.Add(new clsPatientServiceDetails
                                {
                                    ServiceID = item.ServiceID,
                                    ServiceCode = item.ServiceCode,
                                    ServiceName = item.ServiceName,
                                    Rate = item.Rate,
                                    SelectService = item.SelectService,
                                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                                    RelationID = iRelationID,
                                    //LoyaltyID = ((MasterListItem)cmbCardType.SelectedItem).ID,
                                    TariffID = item.TariffID,
                                    PatientUnitID=PatientUnitID


                                });
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
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
       

      

    }
}

