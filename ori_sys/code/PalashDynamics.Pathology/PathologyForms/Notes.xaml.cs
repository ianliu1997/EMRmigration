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
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class Notes : ChildWindow
    {
        #region Variable Declaration
        public event RoutedEventHandler OnAddButton_Click;
        public long TestID;
        public String FootNote = String.Empty;
        public String SuggestionNote = String.Empty;
        public bool IsSecondLevel;
        public bool IsThirdLevel;
        public clsPathoTestParameterVO TestDetails;
        #endregion

        #region Constructor and Loaded Event
        public Notes()
        {
            InitializeComponent();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (TestDetails != null)
            {
                if (IsThirdLevel || IsSecondLevel)
                {
                    txtFootNote.IsEnabled = true;
                    txtSuggestionNote.IsEnabled = true;
                }
                //else
                //{
                //    txtFootNote.IsEnabled = false;
                //    txtSuggestionNote.IsEnabled = false;
                //}
                lblTestName.Text = TestDetails.PathoTestName;
                if (!String.IsNullOrEmpty(TestDetails.FootNote))
                    txtFootNote.Text = TestDetails.FootNote;
                if(!String.IsNullOrEmpty(TestDetails.Note))
                    txtSuggestionNote.Text = TestDetails.Note;
            }
        }
        #endregion

        #region Button Click Events
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtFootNote.Text.Trim()))
                FootNote = txtFootNote.Text.Trim();
            else
                FootNote = null;
            if (!String.IsNullOrEmpty(txtSuggestionNote.Text.Trim()))
                SuggestionNote = txtSuggestionNote.Text.Trim();
            else
                SuggestionNote = null;
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

