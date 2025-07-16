using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using PalashDynamics.Controls;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IVF.TherpyExecution
{
    public class RepeterControlSurrogacy:ListBoxItem
    {
        public RepeterControlSurrogacy()
        {
            this.DefaultStyleKey = typeof(RepeterControlSurrogacy);
        }

        private TextBox txtPOG;
        private TextBox txtFindings;
        private TextBox txtUSGReproductive;
        private TextBox txtAnyInvestigation;
        private TextBox txtRemarks;
        private DatePicker dtpDate;
        private HyperlinkButton AddRemoveClick;

        public event RoutedEventHandler OnAddRemoveClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.txtPOG = base.GetTemplateChild("txtPOG") as TextBox;
            this.txtFindings = base.GetTemplateChild("txtFindings") as TextBox;
            this.txtAnyInvestigation = base.GetTemplateChild("txtAnyInvestigation") as TextBox;
            this.txtRemarks = base.GetTemplateChild("txtRemarks") as TextBox;
            this.txtUSGReproductive = base.GetTemplateChild("txtUSGReproductive") as TextBox;
            this.dtpDate = base.GetTemplateChild("dtpDate") as DatePicker;

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;

            this.txtPOG.Tag = "POG";
            this.txtFindings.Tag = "Findings";
            this.txtAnyInvestigation.Tag = "Any Investigations";
            this.txtUSGReproductive.Tag = "USG Reproductive";
            this.txtRemarks.Tag = "Remark";

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
        
        
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
