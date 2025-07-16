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

namespace PalashDynamics.Radiology
{
    public partial class PasswordWindow : ChildWindow
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnOkButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtPassword.Focus();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidation())
            {
                string msgTitle = "Palash";
                string msgText = "Are you sure you want to update the Result Entry?";

                MessageBoxControl.MessageBoxChildWindow msgW =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                msgW.Show();
            }

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                this.DialogResult = true;
                if (OnOkButton_Click != null)
                    OnOkButton_Click(this, new RoutedEventArgs());
            }
        }
        private bool CheckValidation()
        {
            bool result = true;

            if (txtPassword.Password == null)
            {
                txtPassword.SetValidation("Please Enter Password");
                txtPassword.RaiseValidationError();
                txtPassword.Focus();
                result= false;
            }
            else if (txtPassword.Password == "")
            {
                txtPassword.SetValidation("Please Enter Password");
                txtPassword.RaiseValidationError();
                txtPassword.Focus();
                result =false;
            }
            else 
                txtPassword.ClearValidationError();


            if (txtConfirmPassword.Password == null)
            {
                txtConfirmPassword.SetValidation("Please Enter Confirm Password");
                txtConfirmPassword.RaiseValidationError();
                txtConfirmPassword.Focus();
                result =false;
            }
            else if (txtConfirmPassword.Password == "")
            {
                txtConfirmPassword.SetValidation("Please Enter Confirm Password");
                txtConfirmPassword.RaiseValidationError();
                txtConfirmPassword.Focus();
                result = false;
            }
            else 
                txtConfirmPassword.ClearValidationError();


            if (txtPassword.Password !="" && txtConfirmPassword.Password != "")
            {
                if (txtPassword.Password != txtConfirmPassword.Password)
                {
                    txtConfirmPassword.SetValidation("Confirm Password should be same as entered Password");
                    txtConfirmPassword.RaiseValidationError();
                    txtConfirmPassword.Focus();
                    result = false;
                }
                else 
                    txtConfirmPassword.ClearValidationError();

            }

            //if (txtPassword.Password != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PasswordToModifyRadiologyReport)
            //{
            //    txtPassword.SetValidation("Invalid Password.\nPlease Enter correct password");
            //    txtPassword.RaiseValidationError();
            //    txtPassword.Focus();
            //    result = false;

            //}
            //else
            //    txtPassword.ClearValidationError();



            return result;
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());



        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

