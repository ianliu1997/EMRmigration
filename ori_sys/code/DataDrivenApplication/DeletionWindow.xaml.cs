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

namespace DataDrivenApplication
{
    public partial class DeletionWindow : ChildWindow
    {

        public string Message { get; set; }
        public int ID { get; set; }

        public DeletionWindow()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler OnOkButtonClick;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(this, e);
            }
            this.DialogResult = true;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = Message;
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

