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
using System.Windows.Browser;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.Service.PalashTestServiceReference;
//using RAZIClinic.UserInforServiceRef;
using PalashDynamics;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.Controls;



namespace CIMS.Forms
{
    public partial class frmFirstChangePassword : ChildWindow   
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public bool isValid = true;
        public bool okClick = false;
        public string msgTitle ="";
        public string msgText="";
        public Int16 MaxAge, MinAge, MinLength, MaxLength, NoOfPassword, AccountLockThreshold;
        public float AccLockDuration;
        public bool OneDigit, LowerCase, UpperCase, SpecialChar;
        bool  chkMaxlength=true;
        bool chkMinLength=true;
        bool chkIsDigit, chkIsUpper, chkIsLower, chkIsSpecial;
        bool chkValid = true;
        string msgMinLen, msgMaxLen, msgIsDigit, msgIsUpper, msgIsLower, msgIsSpecial;

        public frmFirstChangePassword()
        {
            InitializeComponent();
        }

        private void FillSecretQtnList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //   BizAction.MasterTable = MasterTableNameList.
            BizAction.MasterTable = MasterTableNameList.Secret_Qtn;

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

                    cmbSecretQtn.ItemsSource = null;
                    cmbSecretQtn.ItemsSource = objList;
                }
                if (this.DataContext != null)
                {
                    cmbSecretQtn.SelectedValue = ((clsSecretQtnVO)this.DataContext).Id;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Validate();
            txtLoginName.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            txtNewPassword.Focus();
            GetUserDashBoardDetails();
            FillSecretQtnList();
        }

        void ClearallFlags()
        {
            chkValid = true;
            chkMinLength =true;
            chkMaxlength = true;
            chkIsDigit = true;
            chkIsLower = true;
            chkIsUpper = true;
            msgMinLen = msgMaxLen= msgIsDigit= msgIsUpper= msgIsLower= msgIsSpecial="";
        }
       
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            isValid = true;
            okClick = true;
            Validate();
            msgText = "";
            ClearallFlags();
            
            if (isValid)
            {
                clsUserVO objLogin = CreateFormData();
              
                bool chkPassConfig = ValidatePassword();
                if (!chkPassConfig)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    txtNewPassword.Password = "";
                    txtConfirmNewPassword.Password = "";               
                }
                else
                {                   
                    msgText = "Are You Sure to Reset the Password ?";

                    MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    
                    msgW.Show();
                }                
            }
        }

        private void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }

        void Save()
        {
            this.DialogResult = true;
            clsUserVO objLogin = CreateFormData();
            clsUserRoleVO objRole = CreateRoleObjectFromFormData();
            
            clsChangePasswordFirstTimeBizActionVO objPasswordChange = new clsChangePasswordFirstTimeBizActionVO();
            objPasswordChange.Details = objLogin;
            objPasswordChange.DashBoardList = objRole.DashBoardList;

            if (cmbSecretQtn.SelectedItem != null)
                objPasswordChange.Details.UserGeneralDetailVO.SecreteQtn = ((MasterListItem)cmbSecretQtn.SelectedItem).ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsUserVO objChangePassword = ((clsChangePasswordFirstTimeBizActionVO)ea.Result).Details;
                    this.DataContext = objChangePassword;

                    msgText = "Record is successfully submitted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                    Uri address1 = new Uri(Application.Current.Host.Source, "../login.aspx");
                    HtmlPage.Window.Navigate(address1);
                    //UpdateAudtiTrail();

                    okClick = false;
                }
            };
            client.ProcessAsync(objPasswordChange, new clsUserVO());
            client.CloseAsync();
            client = null;             
        }

        private void UpdateAudtiTrail()
        {
            clsUserVO User = null;
            User = ((IApplicationConfiguration)App.Current).CurrentUser;
            clsUpdateAuditTrailBizActionVO BizAction = new clsUpdateAuditTrailBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                //((IApplicationConfiguration)App.Current).CurrentUser = null;
                Uri address1 = new Uri(Application.Current.Host.Source, "../login.aspx");
                HtmlPage.Window.Navigate(address1);
            };
            client.ProcessAsync(BizAction, User);// new clsUserVO());
            client.CloseAsync();
            client = null;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

     
        void GetUserDashBoardDetails()
        {
            clsGetLoginNamePasswordBizActionVO objBizVO = new clsGetLoginNamePasswordBizActionVO();
          
            objBizVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
           
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
              client.ProcessCompleted += (s, ea) =>
              {
                   if (ea.Result != null && ea.Error == null)
                   {
                       clsUserVO objRoleDetails=((clsGetLoginNamePasswordBizActionVO)ea.Result).LoginDetails;                 
                       lstItems.ItemsSource = ((clsGetLoginNamePasswordBizActionVO)ea.Result).DashBoardList;
                   }
              };
              client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
              client.CloseAsync();
           
        }

        #region Commented
        
        void client_GetUserDashBoardDetails(object sender, ProcessCompletedEventArgs e)
        {
            // clsGetUserOtherDetailsByIdBizActionVO objUserOtherDetails = e.Result as clsGetUserOtherDetailsByIdBizActionVO;
            if (e.Error == null && e.Result != null)
            {


              clsGetLoginNamePasswordBizActionVO objRoleMenuDetails = e.Result as clsGetLoginNamePasswordBizActionVO;

                ////if (objRoleMenuDetails.LoginDetails != null)
                ////{
                ////  //  List<clsDashBoardVO> lList = new List<clsDashBoardVO> (); //lstItems.ItemsSource;
                ////  //  lList=(List<clsDashBoardVO>)lstItems.ItemsSource;
                ////    List<clsDashBoardVO> lList = (List<clsDashBoardVO>)lstItems.ItemsSource; 
                ////    //foreach (var smitem in objRoleMenuDetails.DashBoardList)
                ////    foreach(var smitem in objRoleMenuDetails.LoginDetails.UserGeneralDetailVO.RoleDetails.DashBoardList)
                ////    {
                ////        foreach (var item in lList)
                ////        {
                            
                ////            if (item.ID == smitem.ID)
                ////            {
                ////                item.Status = smitem.Status;                                                              
                ////                break;
                ////            }
                ////            //if (item.Status == false)
                ////            //{
                ////            //    // Set Visible False if status is False.
                ////            //    this.lstItems.Visibility = Visibility.Collapsed;
                ////            //}
                ////        }
                ////    }


                    ////lstItems.ItemsSource = null;
                    ////lstItems.ItemsSource = lList;
              ////  }

            }
        }
        #endregion

        void Validate()
       {
           if (string.IsNullOrEmpty(txtNewPassword.Password.Trim()))
           {
               txtNewPassword.SetValidation("Password cannot be blank");
               txtNewPassword.RaiseValidationError();
               txtNewPassword.Focus();
               isValid = false;
           }
           else
               txtNewPassword.ClearValidationError();
          
           if (string.IsNullOrEmpty(txtConfirmNewPassword.Password.Trim()))
           {
               txtConfirmNewPassword.SetValidation("Confirm Password cannot be blank");
               txtConfirmNewPassword.RaiseValidationError();
               txtConfirmNewPassword.Focus();
               isValid = false;
           }
           else
               txtConfirmNewPassword.ClearValidationError();
           
           //if (txtSecreteQuestion.Text.Trim() == "")
           //{
           //    txtSecreteQuestion.SetValidation("Secret Question cannot be blank");
           //    txtSecreteQuestion.RaiseValidationError();
           //    txtSecreteQuestion.Focus();
           //    isValid = false;
           //}
           //else
           //    txtSecreteQuestion.ClearValidationError();

           if (string.IsNullOrEmpty(txtSecreteAnswer.Password.Trim()))
           {
               txtSecreteAnswer.SetValidation("Secret Answer cannot be blank");
               txtSecreteAnswer.RaiseValidationError();
               txtSecreteAnswer.Focus();
               isValid = false;
           }
           else
               txtSecreteAnswer.ClearValidationError();

           if (okClick == true)
           {
               //Check Confirm Password
               if (string.Compare(txtNewPassword.Password.Trim(), txtConfirmNewPassword.Password.Trim()) == 0)
               {
                   txtConfirmNewPassword.ClearValidationError();
               }
               else
               {
                   txtConfirmNewPassword.SetValidation("Confirm Password must be same as Password");
                   txtConfirmNewPassword.RaiseValidationError();
                   txtConfirmNewPassword.Focus();
                   isValid = false;
               }
           }
       }

       private bool ValidatePassword()
       {  
     
           //Check Password as per the Password Configuration.
           string temp = txtNewPassword.Password.Trim();
           long isPassValid;
           clsPassConfigurationVO ValidatePassword = new clsPassConfigurationVO();
           ValidatePassword =  ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig;
           isPassValid = ValidatePassword.IsPasswordValid(temp);
           if (isPassValid == 0)
           {
               chkValid = true;
           }

           if (isPassValid == 1)
           {
               chkValid = false;
               chkMinLength = false;
               msgMinLen = "Minimum Length of the Password should be greater than " + MinLength + " characters "; //give the value from the variable MinLength; 
           }
                if (isPassValid == 2)
                {
                    chkValid = false;
                    chkMaxlength = false;
                    msgMaxLen = "Maximum Length of the Password should less than " + MaxLength + " characters "; //give the value from the variable MaxLength;
                }
                if (isPassValid == 3)
                {
                    chkValid = false;
                    chkIsDigit = false;
                    msgIsDigit = "Password should have atleast one digit. ";
                }
                if (isPassValid == 4)
                {
                    chkValid = false;
                    chkIsLower = false;
                    msgIsLower = "Password should have atleast One Lower Case Character. ";
                }
                if (isPassValid == 5)
                {
                    chkValid = false;
                    chkIsUpper = false;
                    msgIsUpper = "Password should have atleast One Upper Case Character. ";
                }
                if (isPassValid == 6)
                {
                    chkValid = false;
                    chkIsSpecial = false;
                    msgIsSpecial = "Password should have atleast One Special Character among '@', '#', '$', '%', '^', '&', '+', '='";
                }

                if (chkValid == false)
                {
                    if (chkMinLength == false)
                    {
                        msgText = msgText + msgMinLen;
                    }
                    if (chkMaxlength == false)
                    {
                        msgText = msgText + msgMaxLen;
                    }
                    if (OneDigit == true)
                    {
                        if (chkIsDigit == false)
                            msgText = msgText + msgIsDigit;
                    }
                    if (UpperCase == true)
                    {
                        if (chkIsUpper == false)
                            msgText = msgText + msgIsUpper;
                    }
                    if (LowerCase == true)
                    {
                        if (chkIsLower == false)
                            msgText = msgText + msgIsLower;
                    }
                    if (SpecialChar == true)
                    {
                        if (chkIsSpecial == false)
                            msgText = msgText + msgIsSpecial;
                    }
                    if (msgText.Trim() == null || msgText.Trim() == "")
                    {
                        chkValid = true;
                    }
                }
                            
           return chkValid ;
           #region Commented

           //int length = txtNewPassword.Password.Length;
           //string tempchk;
           //int count;

           //if (txtNewPassword.Password.Length < MinLength)
           //{
           //    chkValid = false;
           //    chkMinLength = false;
           //    msgMinLen = "Minimum Length of the Password should be "; //give the value from the variable MinLength; 
           //}

           //if (txtNewPassword.Password.Length > MaxLength)
           //{
           //    chkValid = false;
           //    chkMaxlength = false;
           //    msgMaxLen = "Maximum Length of the Password should be "; //give the value from the variable MaxLength;
           //}

           //if (OneDigit == true)
           //{              
           //    for (count = 0; count < length ; count = count + 1)
           //    {
           //        tempchk = temp[count].ToString();
           //        if (tempchk.IsItNumber())
           //        {
           //            chkIsDigit = true;
           //            msgIsDigit = "";
           //            count = length - 1;
           //        }
           //        else
           //        {
           //            chkValid = false;
           //            chkIsDigit = false;
           //            msgIsDigit = "Password should have atleast one digit. ";
           //        }                      
           //    }
           //}

           //if (UpperCase==true)
           //{
           //    for (count = 0; count < length; count = count + 1)
           //    {
           //        tempchk = temp[count].ToString();
           //        if (tempchk.IsItUpperCase())
           //        {
           //            chkIsUpper = true;
           //            msgIsUpper = "";
           //            count = length - 1;
           //        }
           //        else
           //        {
           //            chkValid = false;
           //            chkIsUpper = false;
           //            msgIsUpper = "Password should have atleast One Upper Case Character. ";
           //        }
           //    }
           //}

           //if (LowerCase == true)
           //{
           //    for (count = 0; count < length; count = count + 1)
           //    {
           //        tempchk = temp[count].ToString();
           //        if (tempchk.IsItLowerCase())
           //        {
           //            chkIsLower = true;
           //            msgIsLower = "";
           //            count = length - 1;
           //        }
           //        else
           //        {
           //            chkValid = false;
           //            chkIsLower = false;
           //            msgIsLower = "Password should have atleast One Lower Case Character. ";
           //        }
           //    }
           //}
           //if (SpecialChar == true)
           //{
           //    for (count = 0; count < length; count = count + 1)
           //    {
           //        tempchk = temp[count].ToString();
           //        if (tempchk.IsItSpecialChar())
           //        {
           //            chkIsSpecial = true;
           //            msgIsSpecial = "";
           //            count = length - 1;
           //        }
           //        else
           //        {
           //            chkValid = false;
           //            chkIsSpecial = false;
           //            msgIsSpecial = "Password should have atleast One Special Character among '@', '#', '$', '%', '^', '&', '+', '='"; 
           //        }
           //    }
           //}

           //if (chkValid==false)
           //{
           //    if (chkMinLength == false)
           //        msgText = msgText +  msgMinLen + MinLength;
           //    if (chkMaxlength == false)
           //        msgText = msgText +  msgMaxLen + MaxLength;
           //    if (OneDigit == true)
           //    {
           //        if (chkIsDigit== false)
           //            msgText = msgText +  msgIsDigit;
           //    }
           //    if (UpperCase == true)
           //    {
           //        if (chkIsUpper == false)
           //            msgText = msgText +  msgIsUpper;
           //    }
           //    if (LowerCase == true)
           //    {
           //        if (chkIsLower == false)
           //            msgText = msgText +  msgIsLower;
           //    }
           //    if (msgText.Trim() == null || msgText.Trim() == "")
           //    {
           //        chkValid = true;
           //    }
           //}           
           //return chkValid;
#endregion
           
       }
              
       private void txtConfirmNewPassword_LostFocus(object sender, RoutedEventArgs e)
       {
           //if (string.Compare(txtNewPassword.Password, txtConfirmNewPassword.Password) == 0)
           //{
           //    txtConfirmNewPassword.ClearValidationError();             
           //}
           //else
           //{
           //    txtConfirmNewPassword.SetValidation("Confirm Password must be same as Password");
           //    txtConfirmNewPassword.RaiseValidationError();
           //    txtConfirmNewPassword.Focus();
           //    isValid = false;
           //}
       }

       private clsUserVO CreateFormData()
       {
           clsUserVO objUserVO = new clsUserVO();
                      
           objUserVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
           objUserVO.LoginName = txtLoginName.Text.Trim();
           objUserVO.Password = txtNewPassword.Password.Trim();
          // objUserVO.UserGeneralDetailVO.SecreteQtn = txtSecreteQuestion.Text.Trim();
          // objUserVO.UserGeneralDetailVO.SecreteQtn = txtSecreteQuestion.Text.Trim();

           objUserVO.UserGeneralDetailVO.SecreteAns = txtSecreteAnswer.Password.Trim();
           MaxLength = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.MaxPasswordLength;
           MinLength = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.MinPasswordLength;
           OneDigit = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.AtLeastOneDigit;
           UpperCase = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.AtLeastOneUpperCaseChar;
           LowerCase = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.AtLeastOneLowerCaseChar;
           SpecialChar = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.AtLeastOneSpecialChar;
           NoOfPassword = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig.NoOfPasswordsToRemember;   
                      
           return objUserVO;
       }

       void ClearForm()
       {
           txtNewPassword.Password = "";
           txtConfirmNewPassword.Password = "";
           txtSecreteAnswer.Password = "";
           //txtSecreteQuestion.Text = "";
       }
     
       public object objChangePassword { get; set; }

       private clsUserRoleVO CreateRoleObjectFromFormData()
       {
           clsUserRoleVO objRoleVO = new clsUserRoleVO();
                      
           objRoleVO.Status = true;
           objRoleVO.DashBoardList = (List<clsDashBoardVO>)lstItems.ItemsSource;
                     
           return objRoleVO;
       }   

    }
}

