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

namespace DataDrivenApplication
{
    public partial class DrugOtherDetails : ChildWindow
    {
        public string Message { get; set; }
        public string URL { get; set; }
        public DrugOtherDetails()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DrugOtherDetails_Loaded);
        }

        void DrugOtherDetails_Loaded(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = Message;            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void DrugLink_Click(object sender, RoutedEventArgs e)
        {
            if (URL != null && URL != "")
            {
                try
                {
                    HtmlPage.Window.Navigate(new Uri(URL), "_blank");
                }
                catch (Exception ex)
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Wrong URL is Defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "URL not Defined", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbox.Show();
            }
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

