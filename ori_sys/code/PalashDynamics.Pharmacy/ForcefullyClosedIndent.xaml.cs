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
using PalashDynamics.ValueObjects.Inventory;


namespace PalashDynamics.Pharmacy
{
    public partial class ForcefullyClosedIndent : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCanelButton_Click;
        public long? indentNo = 0;

        public bool isBulkIndentClosed = false;
        public List<clsIndentMasterVO> BulkCloseIndetList;

        public ForcefullyClosedIndent()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Cancel button click
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            BulkCloseIndetList = new List<clsIndentMasterVO>();
        }

        /// <summary>
        /// Purpose:Indent Close  reason  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButton_Click != null)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                if (txtAppReason.Text.Trim().Equals(String.Empty) || txtAppReason.Text == null)
                {
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter The Remarks", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();

                }
                else
                {

                    this.DialogResult = true;
                    OnSaveButton_Click(this, new RoutedEventArgs());


                    this.Close();
                }
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (OnCanelButton_Click != null)
            {
                this.DialogResult = false;
                BulkCloseIndetList = new List<clsIndentMasterVO>();
                OnCanelButton_Click(this, new RoutedEventArgs());
                
            }
            

        }
    }
}

