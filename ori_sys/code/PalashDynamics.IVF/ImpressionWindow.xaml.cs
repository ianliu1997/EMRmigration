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

namespace PalashDynamics.IVF
{
    public partial class ImpressionWindow : ChildWindow
    {
        public ImpressionWindow()
        {
            InitializeComponent();            
        }

       public bool Day = false;
       public bool Summary = false;

        #region Properties
        public string Impression { get; set; }
        #endregion

        public event RoutedEventHandler OnSaveClick;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (richTextEditor.Html == "")
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Impression.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                msgW1.Show();
            }
            else
            {
                Impression = richTextEditor.Html;

                if (Day == true)
                {
                    this.DialogResult = true;
                    if (OnSaveClick != null)
                        OnSaveClick(this, new RoutedEventArgs());
                }
                else
                {
                    if (this.OnSaveClick != null)
                    {
                        this.DialogResult = true;
                        this.OnSaveClick(Impression, e);
                    }
                }
            }
        }

        
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Impression != null && Impression != "")
            {
                richTextEditor.Html = Impression;
                if (Summary == true)
                {
                    cmdClose.Visibility = Visibility.Visible;
                    cmdSave.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

