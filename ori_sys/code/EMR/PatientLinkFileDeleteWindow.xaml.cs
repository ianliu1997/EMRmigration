//Created Date:11/Dec/2013
//Created By: Nilesh Raut
//Specification: Delete The Patient Attached Documents

//Review By:
//Review Date:

//Modified By: 
//Modified Date: 
using System;
using System.Windows;
using System.Windows.Controls;

namespace EMR
{
    public partial class PatientLinkFileDeleteWindow : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;

        public PatientLinkFileDeleteWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());
       
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
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

