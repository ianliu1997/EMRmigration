using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DataDrivenApplication
{
    public class InvestigationRepetorControlItem : ListBoxItem
    {
        public InvestigationRepetorControlItem()
        {
            this.DefaultStyleKey = typeof(InvestigationRepetorControlItem);
        }

        // Fields
        private HyperlinkButton AddRemoveClick;

        private ComboBox cmbInvestigation;
        private TextBox TxtRemarks;

        public event RoutedEventHandler OnAddRemoveClick;

        public event RoutedEventHandler cmbSelectionChanged;
        //public event RoutedEventHandler txtRemarksLostFocus;
        public event RoutedEventHandler txtRemarksTextChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;
            this.cmbInvestigation = base.GetTemplateChild("cmbInvestigation") as ComboBox;
            this.TxtRemarks = base.GetTemplateChild("TxtRemarks") as TextBox;


            this.cmbInvestigation.Tag = "Investigation";
            this.TxtRemarks.Tag = "Remarks";


            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.cmbInvestigation.SelectionChanged += new SelectionChangedEventHandler(cmbInvestigation_SelectionChanged);
            //this.TxtRemarks.LostFocus+=new RoutedEventHandler(TxtRemarks_LostFocus);
            this.TxtRemarks.TextChanged += new TextChangedEventHandler(TxtRemarks_TextChanged);
            // If dataContext !=null then do this
            if (this.DataContext != null)
            {
                this.cmbInvestigation.SelectedValue = ((OtherInvestigation)this.DataContext).Investigation;
                this.TxtRemarks.Text = ((OtherInvestigation)this.DataContext).Remarks;
            }
        }

        void TxtRemarks_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtRemarksTextChanged != null)
            {
                this.txtRemarksTextChanged(sender, e);
            }
        }

        //void  TxtRemarks_LostFocus(object sender, RoutedEventArgs e)
        //{            
        //    if (this.txtRemarksLostFocus != null)
        //    {
        //        this.txtRemarksLostFocus(sender, e);
        //    }
        //}

        void cmbInvestigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbSelectionChanged != null)
            {
                this.cmbSelectionChanged(sender, e);
            }
        }

        public void CmdAddRemoveClick(object sender, RoutedEventArgs e)
        {
            if (this.OnAddRemoveClick != null)
            {
                this.OnAddRemoveClick(sender, e);
            }
        }
    }
}


