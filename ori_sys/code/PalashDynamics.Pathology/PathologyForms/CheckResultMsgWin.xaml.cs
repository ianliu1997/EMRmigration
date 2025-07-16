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

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class CheckResultMsgWin : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public String checkResultMessage = String.Empty;
        #endregion

        #region Constructor and Loaded Event
        public CheckResultMsgWin()
        {
            InitializeComponent();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblUserName.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
        }
        #endregion

        #region Button Click Events
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCheckResultMsg.Text))
            {
                checkResultMessage = txtCheckResultMsg.Text;
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "PLEASE ENTER CHECK RESULT VALUE MESSAGE.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

