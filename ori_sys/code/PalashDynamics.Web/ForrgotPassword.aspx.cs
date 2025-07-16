using System;
using System.Collections.Generic;
using PalashDynamics.ValueObjects;

using PalashDynamics.BusinessLayer;
using PalashDynamics.BusinessLayer.User;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;


namespace PalashDynamics.Web
{
    public partial class ForrgotPassword : System.Web.UI.Page
    {
        public bool chkisValid = true;
        //public string msgTitle = "PALASHDYNAMICS";
        //public string msgText = "";
        public bool isFormValid = true;
        public Int16 MaxAge, MinAge, MinLength1, MaxLength1, NoOfPassword, AccountLockThreshold;
        public float AccLockDuration;
        //public bool OneDigit, LowerCase, UpperCase, SpecialChar;
        bool chkMaxlength = true;
        bool chkMinLength = true;
        bool chkIsDigit = true ;
        bool chkIsUpper = true;
        bool chkIsLower = true;
        bool chkIsSpecial = true;
        bool chkValid = false;
        string msgMinLen, msgMaxLen, msgIsDigit, msgIsUpper, msgIsLower, msgIsSpecial, msgText;

        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        clsUserVO NewPassword = null;
        clsUserVO user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtLogin.Focus();
                FillSecretQtn();
            }
            
        }

        void ClearAllMessages()
        {
            //lblConfirmMsg.Visible = false;
            //lblLoginMsg.Visible = false;
            //lblMessage.Visible = false;
            //lblSecretAMsg.Visible = false;
            //lblSecretQMsg.Visible = false;
            Response.Clear();
           
        }

        public void FillSecretQtn()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //   BizAction.MasterTable = MasterTableNameList.
            BizAction.MasterTable = MasterTableNameList.Secret_Qtn;

            BizAction.MasterList = new List<MasterListItem>();
            //clsSecretQtnBizActionVO BizAction = new clsSecretQtnBizActionVO();
            BizAction.MasterList.Add(new MasterListItem(0, "---SELECT---"));
                      
            PalashDynamicsWeb service = new PalashDynamicsWeb();
            BizAction = (clsGetMasterListBizActionVO)service.Process(BizAction, new clsUserVO());
                       //objList.Add(new MasterListItem(0, "-- Select --"));

            ddllSecretQtn.DataSource = BizAction.MasterList;
            ddllSecretQtn.DataBind();
            //List<MasterListItem> objList = new List<MasterListItem>();
            //objList.Add(new MasterListItem(0, "-- Select --"));
           
            //objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

            //cmbSecretQtn.ItemsSource = null;
            //cmbSecretQtn.ItemsSource = objList;

            //clsSecretQtnBizActionVO BizActionSecret = new clsSecretQtnBizActionVO();
            //PalashDynamicsWeb service1 = new PalashDynamicsWeb();
            //BizActionSecret = (clsSecretQtnBizActionVO)service1.Process(BizActionSecret, new clsUserVO());
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            clsUserVO UserLoginName=null; 
            clsUserVO LockedUser=null;

            ClearAllMessages();
            //if (lblNewPassword.Visible == true)
            //{                
            //    bool chkPassConfig = ValidatePassword();
            //    if (!chkPassConfig)
            //    {
            //        lblMessage.Text = "Invalid Password. Password not set as per the Configuration.";
            //        lblMessage.Visible = true;

            //        txtNewPassword.Text = "";
            //        txtConfirmPassword.Text = "";
                  
            //    }
            //    else
            //    {
            //        long PassUserId = Convert.ToInt32(ViewState["ID"]);
            //        NewPassword = UpdateForgotPasswordBizAction.GetInstance().UpdateForgotPassword(PassUserId, txtNewPassword.Text.Trim());

            //        lblMessage.Text = "New Password Set Successfully";
            //        lblMessage.Visible = true;                
            //    }   
            //}
            //else
            //{
               bool chkPassword= ValidateForm();
                //lblMessage.Visible = true;
               if (chkPassword)
               {
                   string LoginName = txtLogin.Text.Trim();
                   string Password = " ";
                   //string SecretQtn = txtSecreteQuestion.Text.Trim();

                   string SecretQtn = ddllSecretQtn.SelectedValue;
                   //long SecretQtnId = (ddllSecretQtn.SelectedIndex)+1; 
                   long SecretQtnId = (ddllSecretQtn.SelectedIndex);
                   long UnitId = 0;
                   string SecretAns = txtSecreteAnswer.Text.Trim();

                   //if (LoginName != null)
                   //    lblLoginMsg.Visible = false;
                   //if (SecretQtn != null)
                   //    lblSecretQMsg.Visible = false;
                   //if (SecretAns != null)
                   //    lblSecretAMsg.Visible = false;
                   UserLoginName = LoginAuthenticationBizAction.GetInstance().LoginAuthenticate(LoginName,Password, UnitId);

                   if (UserLoginName != null)
                   {
                       //string SecretQtn = "aa";
                       //LockedUser = LockUserBizAction.GetInstance().LockUser(UserLoginName.ID);
                       if (UserLoginName.UserGeneralDetailVO.Locked == false)
                       {
                           user = ForgotPasswordBizAction.GetInstance().ForgotPassword(LoginName, SecretQtnId, SecretAns);

                           if (user != null)
                           {
                               //lblNewPassword.Visible = true;
                               //txtNewPassword.Visible = true;
                               //lblConfirmPassword.Visible = true;
                               //txtConfirmPassword.Visible = true;


                               ViewState["MaxPasswordLength"] = user.PassConfig.MaxPasswordLength;
                               ViewState["MinPasswordLength"] = user.PassConfig.MinPasswordLength;
                               ViewState["ID"] = user.ID;
                               ViewState["AtleastOneDigit"] = user.PassConfig.AtLeastOneDigit;
                               ViewState["AtleastOneLowerCaseChar"] = user.PassConfig.AtLeastOneLowerCaseChar;
                               ViewState["AtleastOneUpperCaseChar"] = user.PassConfig.AtLeastOneUpperCaseChar;
                               ViewState["AtleastOneSpecialChar"] = user.PassConfig.AtLeastOneSpecialChar;
                               //MinLength = user.MinPasswordLength;
                               //MaxLength = user.MaxPasswordLength;


                              //// lblSecretQuestion.Visible = false;
                              ////// txtSecreteQuestion.Visible = false;
                              //// ddllSecretQtn.Visible = false;
                              //// lblSecretAnswer.Visible = false;
                              //// txtSecreteAnswer.Visible = false;

                               //check the new password for validation

                               bool chkPassConfig = ValidatePassword();
                               if (!chkPassConfig)
                               {
                                  // lblMessage.Text = msgText; //"Invalid Password. Password not set as per the Configuration.";
                                  // Response.Clear();
                                   Response.Write ( msgText);
                                   //if(lblConfirmMsg.Visible == true)
                                   //     lblMessage.Visible=false;
                                   //else
                                   //    lblMessage.Visible = true;

                                   txtNewPassword.Text = "";
                                   txtConfirmPassword.Text = "";

                               }
                               else
                               {
                                   long PassUserId = Convert.ToInt32(ViewState["ID"]);
                                   NewPassword = UpdateForgotPasswordBizAction.GetInstance().UpdateForgotPassword(PassUserId, txtNewPassword.Text.Trim());

                                   //lblMessage.Text = "New Password Set Successfully";
                                   //lblMessage.Visible = true;
                                   Response.Clear();
                                   Response.Write("New Password Set Successfully");
                               }   
                           }
                           else
                           {
                               //lblMessage.Text = "Invalid User Input";
                               //lblMessage.Visible = true;
                               Response.Clear();
                               Response.Write("Invalid User Input");

                               txtLogin.Text = "";
                              // txtSecreteQuestion.Text = "";
                               txtSecreteAnswer.Text = "";
                               txtLogin.Focus();
                           }
                       }
                       else
                       {
                           //lblMessage.Text = "Your Login is Locked. Please contact your System Adminstrator.";
                           //lblMessage.Visible = true;
                           Response.Clear();
                           Response.Write("Your Login is Locked. Please contact your System Adminstrator.");
                       }
                       
                   }
                   else
                   {
                       //lblMessage.Text = "Invalid UserName";
                       //lblMessage.Visible = true;
                       Response.Clear();
                       Response.Write("Invalid UserName");
                   }
                   
               }
               else
               { }
           // }
            
        }

        private bool ValidateForm()
        {
            if (txtLogin.Text.Trim() == null || txtLogin.Text.Trim() == "")
            {
                //lblLoginMsg.Text = "Please enter LoginName";
                //lblLoginMsg.Visible = true;
                Response.Clear();
                Response.Write("Please enter LoginName");
                isFormValid = false;
            }
                       
            if (txtSecreteAnswer.Text.Trim() == null || txtSecreteAnswer.Text.Trim() == "")
            {
                //lblSecretAMsg.Text = "Please Enter Secret Answer";
                //lblSecretAMsg.Visible = true;
                  Response.Clear();
                               Response.Write("Please Enter Secret Answer");
                isFormValid = false;
            }

            return isFormValid;
        }

        private bool ValidatePassword()
        {
            if (txtConfirmPassword.Text.Trim() == null || txtConfirmPassword.Text.Trim() == "")
            {
                //lblSecretAMsg.Text = "Please Confirm Password";
                //lblSecretAMsg.Visible = true;
                Response.Clear();
                Response.Write("Please Enter Confirm Password");
                chkisValid = false;
            }

          

            if (txtNewPassword.Text.Trim() == null || txtNewPassword.Text.Trim() == "")
            {
                //lblSecretQMsg.Text = "Please Enter New Password";
                //lblSecretQMsg.Visible = true;
                Response.Clear();
                Response.Write("Please Enter New Password");
                chkisValid = false;
            }

            if (txtLogin.Text.Trim() == null || txtLogin.Text.Trim() == "")
            {
                //lblLoginMsg.Text = "Please enter LoginName";
                //lblLoginMsg.Visible = true;
                Response.Clear();
                Response.Write("Please enter LoginName");
                chkisValid = false;
            }

            //if (lblSecretAMsg.Visible == true || lblSecretQMsg.Visible == true)
            if (txtConfirmPassword.Text.Trim() != null && txtNewPassword.Text.Trim() != null)
            {
                if (string.Compare(txtNewPassword.Text.Trim(), txtConfirmPassword.Text.Trim()) == 0)
                {
                }
                else
                {
                   // lblConfirmMsg.Text = "Confirm Password must be same as Password";
                    chkisValid = false;
                   // lblConfirmMsg.Visible = true;   
                    Response.Clear();
                    Response.Write("Confirm Password must be same as Password");
                }
            }

            if (chkisValid == true)
            {
                string temp = txtNewPassword.Text.Trim();
                long isPassValid;
                clsPassConfigurationVO ValidatePassword = new clsPassConfigurationVO();
                ValidatePassword.MinPasswordLength = Convert.ToInt16(ViewState["MinPasswordLength"]);  // = ((IApplicationConfiguration)App.Current).CurrentUser.PassConfig;
                ValidatePassword.MaxPasswordLength = Convert.ToInt16(ViewState["MaxPasswordLength"]);
                ValidatePassword.AtLeastOneDigit = Convert.ToBoolean(ViewState["AtleastOneDigit"]);
                ValidatePassword.AtLeastOneLowerCaseChar = Convert.ToBoolean(ViewState["AtleastOneLowerCaseChar"]);
                ValidatePassword.AtLeastOneUpperCaseChar = Convert.ToBoolean(ViewState["AtleastOneUpperCaseChar"]);
                ValidatePassword.AtLeastOneSpecialChar = Convert.ToBoolean(ViewState["AtleastOneSpecialChar"]);
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
                    if (chkMinLength == false)
                    {
                        msgText = msgText + msgMinLen;
                    }
                    if (chkMaxlength == false)
                    {
                        msgText = msgText + msgMaxLen;
                    }
                    if (ValidatePassword.AtLeastOneDigit == true)
                    {
                        if (chkIsDigit == false)
                            msgText = msgText + msgIsDigit;
                    }
                    if (ValidatePassword.AtLeastOneUpperCaseChar == true)
                    {
                        if (chkIsUpper == false)
                            msgText = msgText + msgIsUpper;
                    }
                    if (ValidatePassword.AtLeastOneLowerCaseChar == true)
                    {
                        if (chkIsLower == false)
                            msgText = msgText + msgIsLower;
                    }
                    if (ValidatePassword.AtLeastOneSpecialChar == true)
                    {
                        if (chkIsSpecial == false)
                            msgText = msgText + msgIsSpecial;
                    }
                    if (msgText.Trim() == null || msgText.Trim() == "" )
                    {                        
                             chkValid = true;                       
                    }
                }
            }
            return chkValid;
            
            #region Commented

            //Check Password as per the Password Configuration.
            
            
            //if (chkisValid == true)
            //{
            //    string temp = txtNewPassword.Text;
            //    int length = txtNewPassword.Text.Length;
            //    string tempchk;
            //    int count;

            //    if (ViewState["MaxPasswordLength"]!=null)
            //    {
                    
            //        if (txtNewPassword.Text.Length > maxLength)
            //        {
            //            chkMaxlength = false;
            //          chkisValid = false;
            //          msgMaxLen = "Password Length should be Less.";

            //        }
            //    }
            //    if (ViewState["MinPasswordLength"] != null)
            //    {
                    
            //        if (txtNewPassword.Text.Length<minLength)
            //        {
            //            chkMinLength = false;
            //           chkisValid = false;
            //           msgMinLen = "Password Length should be Greater.";
            //        }
            //    }
            //    if (ViewState["AtleastOneDigit"] != null)
            //    {
                    
            //        if (OneDigit == true)
            //        {
            //            for (count = 0; count < length; count = count + 1)
            //            {
            //                tempchk = temp[count].ToString();
            //                if (tempchk.IsItNumber())
            //                {
            //                    chkIsDigit = true;
            //                    msgIsDigit = "";
            //                    count = length - 1;
            //                }
            //                else
            //                {
            //                    chkisValid = false;
            //                    chkIsDigit = false;
            //                    msgIsDigit = "Password should have atleast one digit. ";
            //                }
            //            }
            //        }
            //    }
            //    if (ViewState["AtleastOneUpperCaseChar"] != null)
            //    {
                    
            //        if (UpperCase == true)
            //        {
            //            for (count = 0; count < length; count = count + 1)
            //            {
            //                tempchk = temp[count].ToString();
            //                if (tempchk.IsItUpperCase())
            //                {
            //                    chkIsUpper = true;
            //                    msgIsUpper = "";
            //                    count = length - 1;
            //                }
            //                else
            //                {
            //                    chkisValid = false;
            //                    chkIsUpper = false;
            //                    msgIsUpper = "Password should have atleast One Upper Case Character. ";
            //                }
            //            }
            //        }
            //    }

            //    if (ViewState["AtleastOneLowerCaseChar"] != null)
            //    {
                    
            //        if (LowerCase == true)
            //        {
            //            for (count = 0; count < length; count = count + 1)
            //            {
            //                tempchk = temp[count].ToString();
            //                if (tempchk.IsItLowerCase())
            //                {
            //                    chkIsLower = true;
            //                    msgIsLower = "";
            //                    count = length - 1;
            //                }
            //                else
            //                {
            //                    chkisValid = false;
            //                    chkIsLower = false;
            //                    msgIsLower = "Password should have atleast One Lower Case Character. ";
            //                }
            //            }
            //        }
            //    }
            //    if (ViewState["AtleastOneSpecialChar"] != null)
            //    {                   
            //        if (SpecialChar == true)
            //        {
            //            for (count = 0; count < length; count = count + 1)
            //            {
            //                tempchk = temp[count].ToString();
            //                if (tempchk.IsItSpecialChar())
            //                {
            //                    chkIsSpecial  = true;
            //                    msgIsSpecial = "";
            //                    count = length - 1;
            //                }
            //                else
            //                {
            //                   chkisValid = false;
            //                    chkIsSpecial = false;
            //                    msgIsSpecial = "Password should have atleast One Special Character. ";
            //                }
            //            }
            //        }
            //    }
            //}

            //if (chkisValid == true)
            //{
            //    if (chkIsDigit == false)
            //        chkisValid = false;
            //    if (chkIsLower == false)
            //        chkisValid = false;
            //    if (chkIsUpper == false)
            //        chkisValid = false;
            //    if (chkIsSpecial == false)
            //        chkisValid = false;
            //}

            //if (chkisValid == false)
            //{
            //    if (chkMinLength == false)
            //        msgText = msgText + msgMinLen;
            //    if (chkMaxlength == false)
            //        msgText = msgText + msgMaxLen;
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
            //    if (SpecialChar == true)
            //    {
            //        if (chkIsSpecial == false)
            //            msgText = msgText + msgIsSpecial;
            //    }
            //    if (msgText.Trim() == null || msgText.Trim() == "")
            //    {
            //        chkisValid = true;
            //    }
            //}
            //return chkisValid;
#endregion
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}