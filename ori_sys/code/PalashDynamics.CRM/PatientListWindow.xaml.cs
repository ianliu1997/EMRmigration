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

namespace PalashDynamics.CRM
{
    public partial class PatientListWindow : ChildWindow
    {
        public PatientListWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string MRNO { get; set; }
        public string RegistrationNO { get; set; }
        public string PatientName { get; set; }


        private List<PatientListWindow> LoadDataGrid()
        {
            List<PatientListWindow> lst = new List<PatientListWindow>();
            lst.Add(new PatientListWindow()
            {
                MRNO = "0000001",
                RegistrationNO = "1",
                PatientName = "ABC"
            }
            );
            return lst;
        }

     

        private void PatientList_Loaded(object sender, RoutedEventArgs e)
        {
            //dgPatientList.ItemsSource = LoadDataGrid();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SmsTemplate win = new SmsTemplate();
            win.Show();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

