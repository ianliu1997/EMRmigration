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
using PalashDynamics;
using CIMS;
using MessageBoxControl;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class PasswordWindow : ChildWindow
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnOkButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        public bool IsForReport { get; set; }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public long OrderID;      
        public long OrderUnitID;

        //public long OrderDetailID;
        //public long OrderDetailUnitID;
       
        public List<clsPathOrderBookingDetailVO> clsRemarkHistoryVOList;

        int Flag = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Flag += 1;
            if (Flag == 1)
            {
                try
                {
                    if (IsForReport != true)
                    {
                        if (CheckValidation())
                        {
                            string str = ((IApplicationConfiguration)App.Current).CurrentUser.PasswordDe;
                            if (txtPassword.Password == str)
                            {
                                SaveUserRemark();
                            }
                            else
                            {
                                Flag = 0;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Incorrect Password.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                            }
                        }
                        else
                        {
                            Flag = 0;
                        }
                    }
                    else
                    {
                        if (CheckValidation())
                        {
                            if (OnOkButton_Click != null)
                                OnOkButton_Click(this, new RoutedEventArgs());
                        }
                        Flag = 0;
                    }
                }
                catch (Exception ex)
                {
                    Flag = 0;
                }
            }

        }
        WaitIndicator indicator = new WaitIndicator();

        void SaveUserRemark()
        {
            try
            {
              
               
                indicator.Show();
                clsRemarkHistoryBizActionVO BizAction = new clsRemarkHistoryBizActionVO();
                BizAction.RemarkHistory = new List<clsPathOrderBookingDetailVO>();
                if (txtRemark.Text != string.Empty)
                {
                    BizAction.Remark = txtRemark.Text;
                }

                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.UserName = ((IApplicationConfiguration)App.Current).CurrentUser.UserNameNew;
                BizAction.OrderID = OrderID;
                BizAction.OrderUnitID = OrderUnitID;

                BizAction.RemarkHistory = clsRemarkHistoryVOList;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            long ID = 0;
                            ID = ((clsRemarkHistoryBizActionVO)arg.Result).ID;
                            if (ID > 0)
                            {
                                if (Flag == 1)
                                {
                                    if (OnOkButton_Click != null)
                                        OnOkButton_Click(this, new RoutedEventArgs());
                                    this.DialogResult = true;
                                    indicator.Close();
                                }

                            }
                            else
                            {
                                indicator.Close();
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Reason Not Save.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                            }
                        }
                        Flag = 0;
                    }
                    else
                    {
                        Flag = 0;
                        indicator.Close();
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Error Occured While Processing", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbx.Show();
                        

                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
                Flag = 0;
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
                result = false;
            }
            else if (txtPassword.Password == "")
            {
                txtPassword.SetValidation("Please Enter Password");
                txtPassword.RaiseValidationError();
                txtPassword.Focus();
                result = false;
            }
            else
                txtPassword.ClearValidationError();


            if (txtConfirmPassword.Password == null)
            {
                txtConfirmPassword.SetValidation("Please Enter Confirm Password");
                txtConfirmPassword.RaiseValidationError();
                txtConfirmPassword.Focus();
                result = false;
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

            if (txtRemark.Text.Trim() == string.Empty)
            {
                txtRemark.SetValidation("Please Enter Remark");
                txtRemark.RaiseValidationError();
                txtRemark.Focus();
                result = false;
            }
            else
                txtRemark.ClearValidationError();
            

            if (txtPassword.Password != "" && txtConfirmPassword.Password != "")
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

            if (txtPassword.Password != ((IApplicationConfiguration)App.Current).CurrentUser.PasswordDe)
            {
                txtPassword.SetValidation("Invalid Password.\nPlease Enter Correct Password");
                txtPassword.RaiseValidationError();
                txtPassword.Focus();
                result = false;

            }
            else
                txtPassword.ClearValidationError();



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

