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
using System.Windows.Data;
using PalashDynamics.Service.PalashTestServiceReference;
//using PalashDynamics.Administration.;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Administration;
using CIMS;

//using PalashDynamics.Administration;
//using PalashDynamics.Service.PalashTestServiceReference;
//using PalashDynamics.Administration;


//using PalashDynamics.ValueObjects.Administration.ChangePassword;


namespace PalashDynamics.Administration
{
    public partial class frmChangePassword : UserControl
    {
        #region Variables
        
        public string Password;
        public bool isValid = true;
        public string msgTitle = "Palash";
        public string msgText = "";
        public Int16 MinLength, MaxLength;
        public bool OneDigit, LowerCase, UpperCase, SpecialChar;
        bool chkMaxlength = true;
        bool chkMinLength = true;
        bool chkIsDigit, chkIsUpper, chkIsLower, chkIsSpecial;
        bool chkValid = true;
        string msgMinLen, msgMaxLen, msgIsDigit, msgIsUpper, msgIsLower, msgIsSpecial;
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        #endregion

        public frmChangePassword()
        {
            InitializeComponent();         
            Loaded += new RoutedEventHandler(frmChangePassword_Loaded);
        }    

        void frmChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            ValidateForm();
            GetUser();          
            txtOldPassword.Focus();
        }

        void GetUser()
        {                       
            clsGetUserBizActionVO objUser = new clsGetUserBizActionVO();
            objUser.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            
            objUser.UserType = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsUserVO objUserDetails = new clsUserVO();
                    objUserDetails = ((clsGetUserBizActionVO)ea.Result).Details;
                    txtLoginName.Text = objUserDetails.LoginName;
                    Password = objUserDetails.Password;
                    MinLength = objUserDetails.PassConfig.MinPasswordLength;
                    MaxLength = objUserDetails.PassConfig.MaxPasswordLength;
                    OneDigit = objUserDetails.PassConfig.AtLeastOneDigit;
                    LowerCase = objUserDetails.PassConfig.AtLeastOneLowerCaseChar;
                    UpperCase = objUserDetails.PassConfig.AtLeastOneUpperCaseChar;
                    SpecialChar = objUserDetails.PassConfig.AtLeastOneSpecialChar;
                }
            };
            client.ProcessAsync(objUser, User);
            client.CloseAsync();
            client = null;
        }
       
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            isValid = true;
            ValidateForm();
            
            msgText = "";
            ClearallFlags();

            if (isValid)
            {   
                if (txtOldPassword.Password.Trim() == Password)
                {
                    bool chkPassConfig = ValidatePassword();

                    if (!chkPassConfig)
                    {
                       // msgText = "New Password is not valid.";
                        MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();
                        txtNewPassword.Password = "";
                        txtConfirmNewPassword.Password = "";
                    }
                    else
                    {
                        msgText = "Are You Sure to Reset the Password?";

                        MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                        msgW.Show();
                    }    
                }
                else 
                {
                    msgText = "Exsiting Password is Incorrect.";
                    MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW.Show();
                    txtNewPassword.Password = "";
                    txtConfirmNewPassword.Password = "";
                }                
            }           
        }

        private void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {            
            if (result == MessageBoxResult.Yes)
            {
                Save();
                ((ChildWindow)(this.Parent)).Close();
            }
        }

        void Save()
        {
            clsUserVO objLogin = CreateFormData();
            clsChangePasswordBizActionVO objPasswordChange = new clsChangePasswordBizActionVO();
            objPasswordChange.Details = objLogin;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsUserVO objChangePassword = ((clsChangePasswordBizActionVO)ea.Result).Details;
                    this.DataContext = objChangePassword;

                    msgText = "Record is successfully submitted!";

                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();                    
                }
            };
            client.ProcessAsync(objPasswordChange, User);// new clsUserVO());
            client.CloseAsync();
            client = null;
           
        }

        private clsUserVO CreateFormData()
        {
            clsUserVO ObjUserVO = new clsUserVO();
            ObjUserVO.ID = User.ID;
            //ObjUserVO.ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            ObjUserVO.LoginName = txtLoginName.Text;
            ObjUserVO.Password = txtNewPassword.Password.Trim();
            
            MaxLength = User.PassConfig.MaxPasswordLength;
            MinLength = User.PassConfig.MinPasswordLength;
            //  AccountLockThreshold =  ((IApplicationConfiguration)App.Current).CurrentUser.AccountLockThreshold;
            //AccLockDuration =  ((IApplicationConfiguration)App.Current).CurrentUser.AccountLockDuration;
            OneDigit = User.PassConfig.AtLeastOneDigit;
            UpperCase = User.PassConfig.AtLeastOneUpperCaseChar;
            LowerCase = User.PassConfig.AtLeastOneLowerCaseChar;
            SpecialChar = User.PassConfig.AtLeastOneSpecialChar;
           
            return ObjUserVO;
        }
       
        private bool ValidatePassword()
        {
            //Check Password as per the Password Configuration.

            string temp = txtNewPassword.Password.Trim();
            long isPassValid;
            clsPassConfigurationVO ValidatePassword = new clsPassConfigurationVO();
            ValidatePassword = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig;
            isPassValid = ValidatePassword.IsPasswordValid(temp);
            if (isPassValid == 0)
            {
                chkValid = true;
            }

            if (isPassValid == 1)
            {
                chkValid = false;
                chkMinLength = false;
                msgMinLen = "Password Length Should Be Greater"; //give the value from the variable MinLength; 
            }
            if (isPassValid == 2)
            {
                chkValid = false;
                chkMaxlength = false;
                msgMaxLen = "Password Length Exceeds The Maximum Allowed Limit."; //give the value from the variable MaxLength;
            }
            if (isPassValid == 3)
            {
                chkValid = false;
                chkIsDigit = false;
                msgIsDigit = "Password Should Have Atleast One Digit. ";
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
                if (chkMinLength  == false)
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

            return chkValid;
            #region commented

            //string temp = txtNewPassword.Password;
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
            //    for (count = 0; count < length; count = count + 1)
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

            //if (UpperCase == true)
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

            //if (chkValid == false)
            //{
            //    if (chkMinLength == false)
            //        msgText = msgText + msgMinLen + MinLength;
            //    if (chkMaxlength == false)
            //        msgText = msgText + msgMaxLen + MaxLength;
            //    if (OneDigit == true)
            //    {
            //        if (chkIsDigit == false)
            //            msgText = msgText + msgIsDigit;
            //    }
            //    if (UpperCase == true)
            //    {
            //        if (chkIsUpper == false)
            //            msgText = msgText + msgIsUpper;
            //    }
            //    if (LowerCase == true)
            //    {
            //        if (chkIsLower == false)
            //            msgText = msgText + msgIsLower;
            //    }
            //    if (msgText.Trim() == null || msgText.Trim() == "")
            //    {
            //        chkValid = true;
            //    }
            //}
            //return chkValid;
#endregion

        }

        void ValidateForm()
        {
            if (string.IsNullOrEmpty(txtConfirmNewPassword.Password.Trim()))
            {
                txtConfirmNewPassword.SetValidation("Confirm Password cannot be blank");
                txtConfirmNewPassword.RaiseValidationError();
                txtConfirmNewPassword.Focus();
                isValid = false;
            }
            else if (string.Compare(txtNewPassword.Password.Trim(), txtConfirmNewPassword.Password.Trim()) == 0)
                {
                    txtConfirmNewPassword.ClearValidationError();
                }


                else
                {
                    txtConfirmNewPassword.SetValidation("Confirm Password should be same as New Password.");
                    txtConfirmNewPassword.RaiseValidationError();
                    txtConfirmNewPassword.Password = "";
                    txtConfirmNewPassword.Focus();
                    isValid = false;
                }
            
            if (string.IsNullOrEmpty(txtNewPassword.Password.Trim()))
            {
                txtNewPassword.SetValidation("New Password cannot be blank");
                txtNewPassword.RaiseValidationError();
                txtNewPassword.Focus();
                isValid = false;
            }
            else
                txtNewPassword.ClearValidationError();

            if (string.IsNullOrEmpty(txtOldPassword.Password.Trim()))
            {
                txtOldPassword.SetValidation("Existing Password cannot be blank");
                txtOldPassword.RaiseValidationError();
                txtOldPassword.Focus();
                isValid = false;
            }
            else
                txtOldPassword.ClearValidationError();
            //if (txtConfirmNewPassword.Password.Trim() == "" || txtConfirmNewPassword.Password.Trim() == null)
            //{
            //    txtConfirmNewPassword.SetValidation("Confirm Password cannot be blank");
            //    txtConfirmNewPassword.RaiseValidationError();
            //    isValid = false;
            //}

          
                //if (txtConfirmNewPassword.Password.Trim() == "" || txtConfirmNewPassword.Password.Trim() == null)
               
        }
        
        void ClearallFlags()
        {
            chkValid = true;
            chkMinLength = true;
            chkMaxlength = true;
            chkIsDigit = true;
            chkIsLower = true;
            chkIsUpper = true;
        }

        private void txtConfirmNewPassword_LostFocus(object sender, RoutedEventArgs e)
        {       
        //    if (string.Compare(txtNewPassword.Password.Trim(), txtConfirmNewPassword.Password.Trim()) == 0)
        //    {
        //        txtConfirmNewPassword.ClearValidationError();                  
        //    }
        //    else
        //    {
            //if (string.IsNullOrEmpty(txtConfirmNewPassword.Password.Trim()))    
            //{
            //    txtConfirmNewPassword.SetValidation("Confirm Password cannot be blank");
            //    txtConfirmNewPassword.RaiseValidationError();
            //    txtConfirmNewPassword.Focus();
            //    isValid = false;
            //}
        //        else
        //        {
        //            txtConfirmNewPassword.SetValidation("Confirm Password should be same as New Password.");
        //            txtConfirmNewPassword.RaiseValidationError();
        //            txtConfirmNewPassword.Password = "";
        //            txtConfirmNewPassword.Focus();
        //            isValid = false;
        //        }
        //    }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {         
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void CmdSave_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

