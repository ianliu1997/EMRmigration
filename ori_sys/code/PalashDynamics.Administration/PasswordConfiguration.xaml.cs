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
using System.Windows.Data;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.Diagnostics.CodeAnalysis;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Administration.RoleMaster;
using PalashDynamics.ValueObjects.Administration.Menu;
using MessageBoxControl;


namespace PalashDynamics.Administration
{
    public partial class PasswordConfiguration : UserControl, IInitiateCIMS
    {
        #region Variables
        
        bool isPageLoded = false;     
        public bool chkNullMinPassLen = true;
        public bool chkNullMaxPassLen = true;
        public bool chkNullMinPassAge = true;
        public bool chkNullMaxPassAge = true;
        public bool chkNullAccThreshold = true;
        public bool chkNullAccDuration = true;
        public bool chkNullPassToRemember = true;
        public bool chkPasswordMinLen = true;
        public bool chkPasswordMaxLen = true;
    //    public bool Updateconfirm = false;        
        public Int16 value;
        public string msgTitle;
        public string msgText = ""; // "PALASHDYNAMICS";
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser; //new clsUserVO();
        #endregion

        #region Variable Declaration

        clsUserVO user = null;
            WaitIndicator waiting = new WaitIndicator();
           // bool isPageLoded = false;

        #endregion

        public PasswordConfiguration()
        {
            InitializeComponent();
            this.DataContext = new clsPassConfigurationVO();
        }

        private void CmdSave_Clicked(object sender, RoutedEventArgs e)
        {
            bool Result = CheckValidations();
            if(Result==true)
                PasswordMinLengthCheck();

            if (Result == true && chkNullMinPassLen==true && chkNullMaxPassLen==true && chkNullMinPassAge==true && chkNullMaxPassAge==true && chkNullAccThreshold==true && chkNullAccDuration==true && chkNullPassToRemember == true && chkPasswordMinLen==true && chkPasswordMaxLen==true)
            {
               
                string msgText = "Are you sure you want to update the Password Configuration Details?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }
            else
            {           
                string msgText = "Please Check the Entered Values";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgWindow.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosedWarning);

                msgWindow.Show();
              //  MessageBox.Show("Please Check the Entered Values", "PalashDynamics", MessageBoxButton.OK);                             
            }

        }

        private void msgW_OnMessageBoxClosedWarning(MessageBoxResult result)
        {
            if (chkNullMinPassLen == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtPasswordMinLength.Focus();
            }
            else if (chkNullMaxPassLen == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtPasswordMaxLength.Focus();
            }
            else if (chkNullMinPassAge == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtMinPasswordAge.Focus();
            }
            else if (chkNullMaxPassAge == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtMaxPasswordAge.Focus();
            }
            else if (chkNullPassToRemember == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtPasswordRemember.Focus();
            }
            else if (chkNullAccDuration == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtAccountLockDuration.Focus();
            }
            else if (chkNullAccThreshold == false)
            {
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtAccountLockThreshold.Focus();
            }
            else if (chkPasswordMinLen == false)
            {
                //txtPasswordMinLength.Text = "";
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtPasswordMinLength.SetValidation("Minimum Password Length Not Set as per other Validations.");
                txtPasswordMinLength.RaiseValidationError();
                txtPasswordMinLength.Focus();
                txtPasswordMinLength.SelectedText = txtPasswordMinLength.Text;
            }
            else if (chkPasswordMaxLen == false)
            {
                txtPasswordMaxLength.Text = "";
                System.Windows.Browser.HtmlPage.Plugin.Focus();
                txtPasswordMaxLength.SetValidation("Maximum Password Length Not Set as per other Validations.");
                txtPasswordMaxLength.RaiseValidationError();
                txtPasswordMaxLength.Focus();
            }
        }
        
        private void PasswordMinLengthCheck()
        {
            int count = 0;
            if (chkAtleasrOneUpper.IsChecked==true)
            {
                count += 1;
            }
            if (chkAtleastOneDigit.IsChecked == true)
            {
                count += 1;
            }
            if (chkAtleastOneLower.IsChecked == true)
            {
                count += 1;
            }
            if (chkAtleastOneSpecial.IsChecked == true)
            {
                count += 1;
            }

            int MinLength = Int16.Parse(txtPasswordMinLength.Text.Trim());

            int MaxLength = Int16.Parse(txtPasswordMaxLength.Text.Trim());

            if (MinLength < count)
            {
                chkPasswordMinLen = false;
            }
            else
                chkPasswordMinLen = true;

            if (MaxLength < count)
            {
                chkPasswordMaxLen = false;
            }
            else
                chkPasswordMaxLen = true;

            
            
        }

        //private void Confirmation()
        //{
            //string msgText = "Are you sure to update the Details? ";
            //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
       
            //msgWindow.Show();

            //string msgTitle = "";
            ////string msgText = "Are you sure you want to save the Patient Details";

            ////MessageBoxControl.MessageBoxChildWindow msgW =
            ////    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            ////msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            ////msgW.Show();
        //}

        private void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //Updateconfirm = true;
                Save();
            }
            else
            {
                waiting.Show();
                FillPassConfig();
                isPageLoded = true;
                waiting.Close();
            }           
                //Updateconfirm = false;
        }
        private void Save()
        {
            //this.DataContext = null;
            waiting.Show();
            clsAddPasswordConfigBizActionVO obj = new clsAddPasswordConfigBizActionVO();
            obj.PasswordConfig = (clsPassConfigurationVO)this.DataContext;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    this.DataContext = null;
                    clsPassConfigurationVO objUpdatePassword = ((clsAddPasswordConfigBizActionVO)ea.Result).PasswordConfig;
                    this.DataContext = objUpdatePassword;
                   
                    string msgText = "Password Configuration Details Updated Successfully.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information );
                    msgWindow.Show();
//                    HtmlPage.Window.Alert("Password Configuration Details Updated Successfully");
                }
                else
                {
                    string msgText = "An Error Occured";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    //HtmlPage.Window.Alert("An Error Occured");
                }
                waiting.Close();
            };
            Client.ProcessAsync(obj, User); //new clsUserVO());
            Client.CloseAsync();
        }
     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isPageLoded)
            {
                bool Validation;
                waiting.Show();
                FillPassConfig();
                isPageLoded = true;
                waiting.Close();
               // Validation = CheckValidations();
                txtPasswordMinLength.Focus();
            }
            isPageLoded = true;            
         }

        private void FillPassConfig()
        {
            this.DataContext = null;
            waiting.Show();

            clsGetPassConfigBizActionVO obj = new clsGetPassConfigBizActionVO();
            if (obj.PassConfig == null) obj.PassConfig = new clsPassConfigurationVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        this.DataContext = null;

                        clsPassConfigurationVO objPwd = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig;
                        this.DataContext = objPwd;

                        //txtPasswordMinLength.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.MinPasswordLength.ToString();
                        //txtPasswordMaxLength.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.MaxPasswordLength.ToString();
                        //txtMinPasswordAge.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.MinPasswordAge.ToString();
                        //txtMaxPasswordAge.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.MaxPasswordAge.ToString();
                        //txtAccountLockThreshold.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AccountLockThreshold.ToString();
                        //txtAccountLockDuration.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AccountLockDuration.ToString();
                        //txtPasswordRemember.Text = ((clsGetPassConfigBizActionVO)ea.Result).PassConfig.NoOfPasswordsToRemember.ToString();

                        //if (((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AtLeastOneDigit == true)
                        //    chkAtleastOneDigit.IsChecked = true;
                        //else
                        //    chkAtleastOneDigit.IsChecked = false;

                        //if (((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AtLeastOneLowerCaseChar == true)
                        //    chkAtleastOneLower.IsChecked = true;
                        //else
                        //    chkAtleastOneLower.IsChecked = false;

                        //if (((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AtLeastOneUpperCaseChar == true)
                        //    chkAtleasrOneUpper.IsChecked = true;
                        //else
                        //    chkAtleasrOneUpper.IsChecked = false;

                        //if (((clsGetPassConfigBizActionVO)ea.Result).PassConfig.AtLeastOneSpecialChar == true)
                        //    chkAtleastOneSpecial.IsChecked = true;
                        //else
                        //    chkAtleastOneSpecial.IsChecked = false;

                    }
                    waiting.Close();

                };
            Client.ProcessAsync(obj, User);// new clsUserVO());
            Client.CloseAsync();
            Client = null;
            
        }

        public void Initiate(string Mode)
        {
            return;
        }

      
        #region TextBox Validation

        //private void txtPasswordMinLength_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    if (!txtPasswordMinLength.Text.IsItNumber())
        //    {
        //        txtPasswordMinLength.Text = "";
        //        txtPasswordMinLength.Focus();
        //        chkNullMinPassLen = false;

        //    }
        //    else if (txtPasswordMinLength.Text.Length == 0)
        //    {
        //        chkNullMinPassLen = false;
        //    }
        //    else
        //        chkNullMinPassLen = true;
        //}

        //private void txtPasswordMinLength_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtPasswordMinLength.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value is 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtPasswordMinLength.Text = "";
        //            txtPasswordMinLength.Focus();
        //            chkNullMinPassLen = false;
        //        }
        //    }
        
        //}

          
        
        //private void txtPasswordMaxLength_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtPasswordMaxLength.Text.IsItNumber())
        //    {
        //        txtPasswordMaxLength.Text = "";
        //        txtPasswordMaxLength.Focus();
        //        chkNullMaxPassLen = false;
        //    }
        //    else if (txtPasswordMaxLength.Text.Length == 0)
        //    {
        //        chkNullMaxPassLen = false;  
        //    }
        //    else
        //        chkNullMaxPassLen = true;
        //    if (txtPasswordMaxLength.Text.ToString() != null && txtPasswordMaxLength.Text.ToString() != "" && txtPasswordMaxLength.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtPasswordMaxLength.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value is32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtPasswordMaxLength.Text = "";
        //            txtPasswordMaxLength.Focus();
        //            chkNullMaxPassLen = false;
        //        }
        //        if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
        //        {
        //            int MinPasswordLengthValue = Convert.ToInt32(txtPasswordMinLength.Text);
        //            int MaxPasswordLengthValue = Convert.ToInt32(txtPasswordMaxLength.Text);

        //            if (MinPasswordLengthValue > MaxPasswordLengthValue)
        //            {
        //                MessageBox.Show("Maximum Password Length should not be less than Minimum Password Length", "PALASHDYNAMICS", MessageBoxButton.OK);
        //                txtPasswordMaxLength.Text = "";
        //                txtPasswordMaxLength.Focus();
        //                chkNullMaxPassLen = false;
        //            }
        //        }
                
        //    }
            
        //}

        //private void txtMinPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtMinPasswordAge.Text.IsItNumber())
        //    {
        //        txtMinPasswordAge.Text = "";
        //        txtMinPasswordAge.Focus();
        //        chkNullMinPassAge = false;
        //    }
        //    else if (txtMinPasswordAge.Text.Length == 0)
        //    {
        //        chkNullMinPassAge = false;
        //    }
        //    else
        //        chkNullMinPassAge = true;

        //    if (txtMinPasswordAge.Text.ToString() != null && txtMinPasswordAge.Text.ToString() != "" && txtMinPasswordAge.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtMinPasswordAge.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value is 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtMinPasswordAge.Text = "";
        //            txtMinPasswordAge.Focus();
        //            chkNullMinPassAge = false;
        //        }
        //    }
                      
        //}

        //private void txtMaxPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtMaxPasswordAge.Text.IsItNumber())
        //    {
        //        txtMaxPasswordAge.Text = "";
        //        txtMaxPasswordAge.Focus();
        //        chkNullMaxPassAge = false;
        //    }
        //    else if (txtMaxPasswordAge.Text.Length==0)
        //    {
        //        chkNullMaxPassAge = false;
        //    }
        //    else
        //        chkNullMaxPassAge = true;

        //    if (txtMaxPasswordAge.Text.ToString() != null && txtMaxPasswordAge.Text.ToString() != "" && txtMaxPasswordAge.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtMaxPasswordAge.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtMaxPasswordAge.Text = "";
        //            txtMaxPasswordAge.Focus();
        //            chkNullMaxPassAge = false;
        //        }
        //    }
        //}

        //private void txtAccountLockThreshold_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtAccountLockThreshold.Text.IsItNumber())
        //    {
        //        txtAccountLockThreshold.Text = "";
        //        txtAccountLockThreshold.Focus();
        //        chkNullAccThreshold = false;
        //    }
        //    else if (txtAccountLockThreshold.Text.Length == 0)
        //    {
        //        chkNullAccThreshold = false;
        //    }
        //    else
        //        chkNullAccThreshold = true;

        //    if (txtAccountLockThreshold.Text.ToString() != null && txtAccountLockThreshold.Text.ToString() != "" && txtAccountLockThreshold.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtAccountLockThreshold.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtAccountLockThreshold.Text = "";
        //            txtAccountLockThreshold.Focus();
        //            chkNullAccThreshold = false;
        //        }
        //    }
            
        //}

        //private void txtAccountLockDuration_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtAccountLockDuration.Text.IsItNumber())
        //    {
        //        txtAccountLockDuration.Text = "";
        //        txtAccountLockDuration.Focus();
        //        chkNullAccDuration = false;
        //    }
        //    else if (txtAccountLockDuration.Text.Length==0)
        //    {
        //        chkNullAccDuration = false;
        //    }
        //    else
        //        chkNullAccDuration = true;

        //    if (txtAccountLockDuration.Text.ToString() != null && txtAccountLockDuration.Text.ToString() != "" && txtAccountLockDuration.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtAccountLockDuration.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtAccountLockDuration.Text = "";
        //            txtAccountLockDuration.Focus();
        //            chkNullAccDuration = false;
        //        }
        //    }
            
        //}


        //private void txtPasswordRemember_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!txtPasswordRemember.Text.IsItNumber())
        //    {
        //        txtPasswordRemember.Text = "";
        //        txtPasswordRemember.Focus();
        //        chkNullPassToRemember = false;
        //    }
        //    else if (txtPasswordRemember.Text.Length == 0)
        //    {
        //        chkNullPassToRemember = false;
        //    }
        //    else
        //        chkNullPassToRemember = true;

        //    if (txtPasswordRemember.Text.ToString() != null && txtPasswordRemember.Text.ToString() != "" && txtPasswordRemember.Text.Length != 0)
        //    {
        //        value = Convert.ToInt16(txtPasswordRemember.Text);
        //        if (value > 32767)
        //        {
        //            MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
        //            txtPasswordRemember.Text = "";
        //            txtPasswordRemember.Focus();
        //            chkNullPassToRemember = false;

        //        }
        //    }
          
        //}
        #endregion

        #region Check Validation
        private bool CheckValidations()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtPasswordRemember.Text))
            //if (((clsPassConfigurationVO)this.DataContext).NoOfPasswordsToRemember == null || ((clsPassConfigurationVO)this.DataContext).NoOfPasswordsToRemember.ToString() == "")
            {
                txtPasswordRemember.SetValidation("No. Of Passwords To Remember Required");
                txtPasswordRemember.RaiseValidationError();
                txtPasswordRemember.Focus();
                result = false;
            }
            else
                txtPasswordRemember.ClearValidationError();

            if (string.IsNullOrEmpty(txtAccountLockDuration.Text))
            {
                txtAccountLockDuration.SetValidation("Account Lock Duration.");
                txtAccountLockDuration.RaiseValidationError();
                txtAccountLockDuration.Focus();
            }
            else
                txtAccountLockDuration.ClearValidationError();

            if (string.IsNullOrEmpty(txtAccountLockThreshold.Text))
            {
                txtAccountLockThreshold.SetValidation("Account Lock Threshold Required.");
                txtAccountLockThreshold.RaiseValidationError();
                txtAccountLockThreshold.Focus();
            }
            else
                txtAccountLockThreshold.ClearValidationError();

            if (string.IsNullOrEmpty(txtMaxPasswordAge.Text))
            //if (((clsPassConfigurationVO)this.DataContext).MaxPasswordAge == null || ((clsPassConfigurationVO)this.DataContext).MaxPasswordAge.ToString() == "")
            {
                txtMaxPasswordAge.SetValidation("Maximum Password Age Required");
                txtMaxPasswordAge.RaiseValidationError();
                txtMaxPasswordAge.Focus();
                result = false;
            }
            else
                txtMaxPasswordAge.ClearValidationError();

            if (string.IsNullOrEmpty(txtMinPasswordAge.Text))
            //if (((clsPassConfigurationVO)this.DataContext).MinPasswordAge == null || ((clsPassConfigurationVO)this.DataContext).MinPasswordAge.ToString() == "")
            {
                txtMinPasswordAge.SetValidation("Minimum Password Age Required");
                txtMinPasswordAge.RaiseValidationError();
                txtMinPasswordAge.Focus();
                result = false;
            }
            else
                txtMinPasswordAge.ClearValidationError();

            if (string.IsNullOrEmpty(txtPasswordMaxLength.Text))
            //if (((clsPassConfigurationVO)this.DataContext).MaxPasswordLength == null || ((clsPassConfigurationVO)this.DataContext).MaxPasswordLength.ToString() == "")
            {
                txtPasswordMaxLength.SetValidation("Maximum Password Length Required");
                txtPasswordMaxLength.RaiseValidationError();
                txtPasswordMaxLength.Focus();
                result = false;
            }
            else
                txtPasswordMaxLength.ClearValidationError();

            if( string.IsNullOrEmpty(txtPasswordMinLength.Text)) 
            //if (((clsPassConfigurationVO)this.DataContext).MinPasswordLength == null || ((clsPassConfigurationVO)this.DataContext).MinPasswordLength.ToString() == "")
            {
                txtPasswordMinLength.SetValidation("Minimum Password Length Required");
                txtPasswordMinLength.RaiseValidationError();
                txtPasswordMinLength.Focus();
                result = false;
            }
            else
                txtPasswordMinLength.ClearValidationError();
            return result;
        }
        #endregion 

        #region TextBox Validation for Lost Focus and TextChanged.
        private void txtPasswordMinLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPasswordMinLength.Text.IsItNumber())
            {
                txtPasswordMinLength.Text = "";
                txtPasswordMinLength.Focus();
                chkNullMinPassLen = false;

            }
            else if (txtPasswordMinLength.Text.Length == 0)
            {
                chkNullMinPassLen = false;
            }
            else
                chkNullMinPassLen = true;
        }

        private void txtPasswordMinLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPasswordMinLength.Text) && txtPasswordMinLength.Text.Length != 0)
            //if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
            {
                txtPasswordMinLength.ClearValidationError();
                value = Convert.ToInt16(txtPasswordMinLength.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value is 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtPasswordMinLength.Text = "";
                    txtPasswordMinLength.Focus();
                    chkNullMinPassLen = false;                    
                }
            }
            else
            {
                txtPasswordMinLength.SetValidation("Minimum Password Length cannot be Zero or Empty.");
                txtPasswordMinLength.RaiseValidationError();
                txtPasswordMinLength.Focus();
            }
        }
        
        private void txtPasswordMaxLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtPasswordMaxLength.Text.IsItNumber())
            {
                txtPasswordMaxLength.Text = "";
                txtPasswordMaxLength.Focus();
                chkNullMaxPassLen = false;
            }
            else if (txtPasswordMaxLength.Text.Length == 0)
            {
                chkNullMaxPassLen = false;
            }
            else
                chkNullMaxPassLen = true;
        }

        private void txtPasswordMaxLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPasswordMaxLength.Text) && txtPasswordMaxLength.Text.Length != 0)
            //if (txtPasswordMaxLength.Text.ToString() != null && txtPasswordMaxLength.Text.ToString() != "" && txtPasswordMaxLength.Text.Length != 0)
            {
                txtPasswordMaxLength.ClearValidationError();
                value = Convert.ToInt16(txtPasswordMaxLength.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value is32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtPasswordMaxLength.Text = "";
                    txtPasswordMaxLength.Focus();
                    chkNullMaxPassLen = false;
                }
                if (txtPasswordMinLength.Text.ToString() != null && txtPasswordMinLength.Text.ToString() != "" && txtPasswordMinLength.Text.Length != 0)
                {
                    int MinPasswordLengthValue = Convert.ToInt32(txtPasswordMinLength.Text);
                    int MaxPasswordLengthValue = Convert.ToInt32(txtPasswordMaxLength.Text);

                    if (MinPasswordLengthValue > MaxPasswordLengthValue)
                    {
                        string msgText = "Maximum Password Length should not be less than Minimum Password Length";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        //MessageBox.Show("Maximum Password Length should not be less than Minimum Password Length", "PALASHDYNAMICS", MessageBoxButton.OK);
                        txtPasswordMaxLength.Text = "";
                        txtPasswordMaxLength.Focus();
                        chkNullMaxPassLen = false;
                    }
                }

            }
            else
            {
                txtPasswordMaxLength.SetValidation("Maximum Password Length cannot be Zero or Empty.");
                txtPasswordMaxLength.RaiseValidationError();
                txtPasswordMaxLength.Focus();
            }
        }
     
        private void txtMinPasswordAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMinPasswordAge.Text.IsItNumber())
            {
                txtMinPasswordAge.Text = "";
                txtMinPasswordAge.Focus();
                chkNullMinPassAge = false;
            }
            else if (txtMinPasswordAge.Text.Length == 0)
            {
                chkNullMinPassAge = false;
            }
            else
                chkNullMinPassAge = true;
        }

        private void txtMinPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMinPasswordAge.Text) && txtMinPasswordAge.Text.Length != 0)
            //if (txtMinPasswordAge.Text.ToString() != null && txtMinPasswordAge.Text.ToString() != "" && txtMinPasswordAge.Text.Length != 0)
            {
                txtMinPasswordAge.ClearValidationError();
                value = Convert.ToInt16(txtMinPasswordAge.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value is 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtMinPasswordAge.Text = "";
                    txtMinPasswordAge.Focus();
                    chkNullMinPassAge = false;
                }
            }
            else
            {
                txtMinPasswordAge.SetValidation("Minimun Password Age cannot be Zero or Empty.");
                txtMinPasswordAge.RaiseValidationError();
                txtMinPasswordAge.Focus();
            }
        }

        private void txtMaxPasswordAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtMaxPasswordAge.Text.IsItNumber())
            {
                txtMaxPasswordAge.Text = "";
                txtMaxPasswordAge.Focus();
                chkNullMaxPassAge = false;
            }
            else if (txtMaxPasswordAge.Text.Length == 0)
            {
                chkNullMaxPassAge = false;
            }
            else
                chkNullMaxPassAge = true;

        }
        
        private void txtMaxPasswordAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaxPasswordAge.Text) && txtMaxPasswordAge.Text.Length != 0)
            //if (txtMaxPasswordAge.Text.ToString() != null && txtMaxPasswordAge.Text.ToString() != "" && txtMaxPasswordAge.Text.Length != 0)
            {
                txtMaxPasswordAge.ClearValidationError();
                value = Convert.ToInt16(txtMaxPasswordAge.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtMaxPasswordAge.Text = "";
                    txtMaxPasswordAge.Focus();
                    chkNullMaxPassAge = false;
                }
                if (txtMinPasswordAge.Text.ToString() != null && txtMinPasswordAge.Text.ToString() != "" && txtMinPasswordAge.Text.Length != 0)
                {
                    int MinPasswordAge = Convert.ToInt16(txtMinPasswordAge.Text);
                    int MaxPasswordAge = Convert.ToInt16(txtMaxPasswordAge.Text);
                    if (MinPasswordAge > MaxPasswordAge)
                    {
                        string msgText = "Maximum Password Age should not be less than Minimum Password Age";
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        //MessageBox.Show("Maximum Password Age should not be less than Minimum Password Age", "PALASHDYNAMICS", MessageBoxButton.OK);
                        txtMaxPasswordAge.Text = "";
                        txtMaxPasswordAge.Focus();
                        chkNullMaxPassAge = false;
                    }
                }
            }
            else
            {
                txtMaxPasswordAge.SetValidation("Maximum Password Age cannot be Zero or Empty.");
                txtMaxPasswordAge.RaiseValidationError();
                txtMaxPasswordAge.Focus();
            }
        }

        private void txtAccountLockThreshold_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtAccountLockThreshold.Text.IsItNumber())
            {
                txtAccountLockThreshold.Text = "";
                txtAccountLockThreshold.Focus();
                chkNullAccThreshold = false;
            }
            else if (txtAccountLockThreshold.Text.Length == 0)
            {
                chkNullAccThreshold = false;
            }
            else
                chkNullAccThreshold = true;
        }

        private void txtAccountLockThreshold_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAccountLockThreshold.Text) && txtAccountLockThreshold.Text.Length != 0)
            //if (txtAccountLockThreshold.Text.ToString() != null && txtAccountLockThreshold.Text.ToString() != "" && txtAccountLockThreshold.Text.Length != 0)
            {
                txtAccountLockThreshold.ClearValidationError();
                value = Convert.ToInt16(txtAccountLockThreshold.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtAccountLockThreshold.Text = "";
                    txtAccountLockThreshold.Focus();
                    chkNullAccThreshold = false;
                }
            }
            else
            {
                txtAccountLockThreshold.SetValidation("Account Lock Threshold cannot be Zero or Empty.");
                txtAccountLockThreshold.RaiseValidationError();
                txtAccountLockThreshold.Focus();
            }
        }

        private void txtAccountLockDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
             if (!txtAccountLockDuration.Text.IsItNumber())
             {
                 txtAccountLockDuration.Text = "";
                 txtAccountLockDuration.Focus();
                 chkNullAccDuration = false;
             }
             else if (txtAccountLockDuration.Text.Length == 0)
             {
                 chkNullAccDuration = false;
             }
             else
                 chkNullAccDuration = true;
        }

        private void txtAccountLockDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAccountLockDuration.Text) && txtAccountLockDuration.Text.Length != 0)
            //if (txtAccountLockDuration.Text.ToString() != null && txtAccountLockDuration.Text.ToString() != "" && txtAccountLockDuration.Text.Length != 0)
            {
                txtAccountLockDuration.ClearValidationError();
                value = Convert.ToInt16(txtAccountLockDuration.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtAccountLockDuration.Text = "";
                    txtAccountLockDuration.Focus();
                    chkNullAccDuration = false;
                }
            }
            else
            {
                txtAccountLockDuration.SetValidation("Account Lock Duration cannot be Zero or Empty.");
                txtAccountLockDuration.RaiseValidationError();
                txtAccountLockDuration.Focus();
            }
        }

        private void txtPasswordRemember_TextChanged(object sender, TextChangedEventArgs e)
        {
             if (!txtPasswordRemember.Text.IsItNumber())
             {
                 txtPasswordRemember.Text = "";
                 txtPasswordRemember.Focus();
                 chkNullPassToRemember = false;
             }
             else if (txtPasswordRemember.Text.Length == 0)
             {
                 chkNullPassToRemember = false;
             }
             else
                 chkNullPassToRemember = true;
        }

        private void txtPasswordRemember_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPasswordRemember.Text) && txtPasswordRemember.Text.Length != 0)
            //if (txtPasswordRemember.Text.ToString() != null && txtPasswordRemember.Text.ToString() != "" && txtPasswordRemember.Text.Length != 0)
            {
                txtPasswordRemember.ClearValidationError();
                value = Convert.ToInt16(txtPasswordRemember.Text);
                if (value > 32767)
                {
                    string msgText = "Maximum Allowed Value is 32767";
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                    //MessageBox.Show("Maximum Allowed Value 32767", "PALASHDYNAMICS", MessageBoxButton.OK);
                    txtPasswordRemember.Text = "";
                    txtPasswordRemember.Focus();
                    chkNullPassToRemember = false;
                }
            }
            else
            {
                txtPasswordRemember.SetValidation("No. Of Passwords to Remember cannot be Zero or Empty.");
                txtPasswordRemember.RaiseValidationError();
                txtPasswordRemember.Focus();
            }
        }

        #endregion



    }
}
