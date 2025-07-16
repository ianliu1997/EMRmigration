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
using System.Reflection;
using CIMS;

namespace DataDrivenApplication.Forms
{
    public partial class EMRWindow : ChildWindow ,IInitiateCIMS
    {
        public long TemplateID = 0;
        public string EMRName="";
        public EMRWindow()
        {
            InitializeComponent();
        }
        #region Initiate
        public void Initiate(string Mode)
        {
            TemplateID = Convert.ToInt64( Mode);
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string strTemplateID = Convert.ToString(TemplateID);                     
                    // PatientEMR Win = new PatientEMR();
                    PatientEMRNew Win = new PatientEMRNew();
                    ((IInitiateCIMS)Win).Initiate(strTemplateID);
                    EMRContent.Content = Win;             
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}

