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
using System.Windows.Controls.Primitives;
using PalashDynamics.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.Administration.Template;
using CIMS;

namespace PalashDynamics.Administration
{
    public class MedicatioRepeterControlItem : ListBoxItem
    {
        public MedicatioRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(MedicatioRepeterControlItem);
        }

        // Fields
        private HyperlinkButton AddRemoveClick;
        //private ComboBox cmbDrugType;
        //private ComboBox cmbDrug;
        private AutoCompleteComboBox cmbDrug;
        //private ComboBox cmbDay;
        private TextBox cmbDay;
        //private ComboBox cmbQuantity;
        private TextBox cmbQuantity;
        //private ComboBox cmbDose;
        private TextBox cmbDose;
        private AutoCompleteComboBox cmbRoute;
        //private ComboBox cmbFrequency;
        private TextBox cmbFrequency;
        private ToggleButton btnContradiction;
        private ToggleButton btnSideEffects;
        public event RoutedEventHandler OnAddRemoveClick;

        public event RoutedEventHandler cmbSelectionChanged;
        public event TextChangedEventHandler txtFreqChanged;
        public event TextChangedEventHandler txtDayChanged;
        public event TextChangedEventHandler txtQtyChanged;
        public event KeyEventHandler txtKeyDown;


        public event RoutedEventHandler btnContradictionSideEffectClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;

            //this.cmbDrugType = base.GetTemplateChild("cmbDrugType") as ComboBox;
            this.cmbDrug = base.GetTemplateChild("cmbDrug") as AutoCompleteComboBox;
            this.cmbDay = base.GetTemplateChild("cmbDay") as TextBox;
            this.cmbQuantity = base.GetTemplateChild("cmbQuantity") as TextBox;
            this.cmbDose = base.GetTemplateChild("cmbDose") as TextBox;
            this.cmbRoute = base.GetTemplateChild("cmbRoute") as AutoCompleteComboBox;
            this.cmbFrequency = base.GetTemplateChild("cmbFrequency") as TextBox;
            this.btnContradiction = base.GetTemplateChild("btnContradiction") as ToggleButton;
            this.btnSideEffects = base.GetTemplateChild("btnSideEffect") as ToggleButton;

            //this.cmbDrugType.Tag = "DrugType";
            this.cmbDrug.Tag = "Drug";
            this.cmbDrug.Name = "Drug";
            this.cmbDay.Tag = "Day";
            this.cmbQuantity.Tag = "Quantity";
            this.cmbDose.Tag = "Dose";
            this.cmbRoute.Tag = "Route";
            this.cmbFrequency.Tag = "Frequency";
            this.btnContradiction.Tag = "Contradiction";
            this.btnSideEffects.Tag = "SideEffects";

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.cmbDrug.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            //this.cmbDay.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbDay.TextChanged += new TextChangedEventHandler(cmbDay_TextChanged);
            this.cmbDay.GotFocus += new RoutedEventHandler(txt_GotFocus);
            //this.cmbQuantity.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbQuantity.TextChanged += new TextChangedEventHandler(cmbQuantity_TextChanged);
            this.cmbQuantity.GotFocus += new RoutedEventHandler(txt_GotFocus);
            //this.cmbDose.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbRoute.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            //this.cmbFrequency.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbFrequency.TextChanged += new TextChangedEventHandler(cmbFrequency_TextChanged);
            this.cmbFrequency.GotFocus += new RoutedEventHandler(txt_GotFocus);
            this.btnContradiction.Click += new RoutedEventHandler(btnContradiction_Click);
            this.btnSideEffects.Click += new RoutedEventHandler(btnContradiction_Click);

            this.cmbDay.KeyDown += new KeyEventHandler(cmbDay_KeyDown);
            this.cmbQuantity.KeyDown += new KeyEventHandler(cmbDay_KeyDown);
            this.cmbFrequency.KeyDown += new KeyEventHandler(cmbDay_KeyDown);

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
            if (this.DataContext != null)
            {
                if (((Medication)this.DataContext).Drug != null)
                {
                    #region Code for Tool Tip
                    switch (((Medication)this.DataContext).Drug.Description)
                    {
                        case "Cotrimoxazole":
                        case "Trimethoprim":
                        case "Sulfamethoxazole":
                        case "Ciprofloxacin":
                            tbl.Text = "indicated for Shigella  or Enteroinvasive E.coli or V. Cholerae";
                            break;

                        case "Doxycycline":
                        case "Furazolidone":
                            tbl.Text = "indicated for  V. Cholerae, ";
                            break;

                        case "Metronidazole":
                            if (((Medication)this.DataContext).Dose == "15mg/kg/day in 3 divided doses for 5 days")
                                tbl.Text = "indicated for acute giardiasis";
                            else if (((Medication)this.DataContext).Dose == "35-50 mg/kg/day in 3 divided doses for 7-10days")
                                tbl.Text = "indicated for Entamoeba histolytica";
                            break;

                        case "Ampicillin":
                            tbl.Text = "indicated for non typhoid salmonella";
                            break;

                        case "Tinidazole":
                            tbl.Text = "indicated for acute giardiasis ";
                            break;

                        case "Dicylomine":
                            tbl.Text = "indicated for children above 6 months in presence of spasmodic diarrhoea\rContraindicated in jaundice and acute abdomen of unknown etiology";
                            break;
                        default:
                            tbl.Text = "";
                            break;
                    }
                    tt.Content = tbl;
                    if (tbl.Text != "")
                        ToolTipService.SetToolTip(this, tt);
                    #endregion
                    //IEnumerator<MasterListItem> mlst= (IEnumerator<MasterListItem>)this.cmbDrug.ItemsSource.GetEnumerator();
                    //while (mlst.MoveNext())
                    //{
                    //    if (((MasterListItem)mlst.Current).ID == ((Medication)this.DataContext).Drug.ID)
                    //    {
                    //        this.cmbDrug.SelectedItem = mlst.Current;
                    //    }
                    //}
                    this.cmbDrug.SelectedValue = ((Medication)this.DataContext).Drug.ID;
                    //this.cmbDrug.SelectedValue = ((Medication)this.DataContext).Drug.Description;
                }
                if (((Medication)this.DataContext).Day != null)
                {
                    //this.cmbDay.SelectedValue = ((Medication)this.DataContext).Day;
                    this.cmbDay.Text = ((Medication)this.DataContext).Day.ToString();
                }
                if (((Medication)this.DataContext).Quantity != null)
                {
                    //this.cmbQuantity.SelectedValue = ((Medication)this.DataContext).Quantity;
                    this.cmbQuantity.Text = ((Medication)this.DataContext).Quantity.ToString();
                }
                if (((Medication)this.DataContext).Dose != null)
                {
                    #region Code for tool Tip
                    switch (((Medication)this.DataContext).Dose)
                    {
                        case "15mg/kg/day in 3 divided doses for 5 days":
                            if (((Medication)this.DataContext).Drug.Description == "Metronidazole")
                                tbl.Text = "indicated for acute giardiasis";
                            break;

                        case "35-50 mg/kg/day in 3 divided doses for 7-10days":
                            if (((Medication)this.DataContext).Drug.Description == "Metronidazole")
                                tbl.Text = "indicated for Entamoeba histolytica";
                            else
                                tbl.Text = "";
                            break;

                    }
                    tt.Content = tbl;
                    if (tbl.Text != "")
                        ToolTipService.SetToolTip(this, tt);
                    #endregion
                    //this.cmbDose.SelectedValue = ((Medication)this.DataContext).Dose;
                    this.cmbDose.Text = ((Medication)this.DataContext).Dose.ToString();
                }
                if (((Medication)this.DataContext).Route != null)
                    this.cmbRoute.SelectedValue = ((Medication)this.DataContext).Route.ID;

                if (((Medication)this.DataContext).Frequency != null)
                {
                    //this.cmbFrequency.SelectedValue = ((Medication)this.DataContext).Frequency;
                    this.cmbFrequency.Text = ((Medication)this.DataContext).Frequency.ToString();
                }
            }
        }

        void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (((TextBox)sender).Text != null)
                ((TextBox)sender).SelectAll();

        }

        void cmbDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtKeyDown != null)
            {
                this.txtKeyDown(sender, e);
            }
        }

        void cmbFrequency_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtFreqChanged != null)
            {
                this.txtFreqChanged(sender, e);
                this.cmbQuantity.Text = ((Medication)this.DataContext).Quantity.ToString();
            }
        }

        void cmbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtQtyChanged != null)
            {
                this.txtQtyChanged(sender, e);
            }
        }

        void cmbDay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtDayChanged != null)
            {
                this.txtDayChanged(sender, e);
                this.cmbQuantity.Text = ((Medication)this.DataContext).Quantity.ToString();
            }
        }



        void btnContradiction_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnContradictionSideEffectClick != null)
            {
                this.btnContradictionSideEffectClick(sender, e);
            }
        }


        public void cmbDrug_SelectionChanged(object sender, RoutedEventArgs e)
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

    public class OtherMedicatioRepeterControlItem : ListBoxItem
    {
        public OtherMedicatioRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(OtherMedicatioRepeterControlItem);
        }

        // Fields
        private HyperlinkButton AddRemoveClick;
        //private ComboBox cmbDrugType;
        //private ComboBox cmbDrug;
        private TextBox cmbDrug;
        //private ComboBox cmbDay;
        private TextBox cmbDay;
        //private ComboBox cmbQuantity;
        private TextBox cmbQuantity;
        //private ComboBox cmbDose;
        private TextBox cmbDose;
        private TextBox txtReason;
        private AutoCompleteComboBox cmbRoute;
        //private ComboBox cmbFrequency;
        private TextBox cmbFrequency;
        //private ToggleButton btnContradiction;
        //private ToggleButton btnSideEffects;
        public event RoutedEventHandler OnAddRemoveClick;

        public event RoutedEventHandler cmbSelectionChanged;
        public event TextChangedEventHandler txtFreqChanged;
        public event TextChangedEventHandler txtDayChanged;
        public event TextChangedEventHandler txtQtyChanged;
        public event KeyEventHandler txtKeyDown;

        //public event RoutedEventHandler btnContradictionSideEffectClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;

            //this.cmbDrugType = base.GetTemplateChild("cmbDrugType") as ComboBox;
            this.cmbDrug = base.GetTemplateChild("cmbDrug") as TextBox;
            this.cmbDay = base.GetTemplateChild("cmbDay") as TextBox;
            this.cmbQuantity = base.GetTemplateChild("cmbQuantity") as TextBox;
            this.cmbDose = base.GetTemplateChild("cmbDose") as TextBox;
            this.txtReason = base.GetTemplateChild("txtReason") as TextBox;
            this.cmbRoute = base.GetTemplateChild("cmbRoute") as AutoCompleteComboBox;
            this.cmbFrequency = base.GetTemplateChild("cmbFrequency") as TextBox;
            //this.btnContradiction = base.GetTemplateChild("btnContradiction") as ToggleButton;
            //this.btnSideEffects = base.GetTemplateChild("btnSideEffect") as ToggleButton;

            //this.cmbDrugType.Tag = "DrugType";
            this.cmbDrug.Tag = "Drug";
            this.cmbDrug.Name = "Drug";
            this.cmbDay.Tag = "Day";
            this.cmbQuantity.Tag = "Quantity";
            this.cmbDose.Tag = "Dose";
            this.txtReason.Tag = "Reason";
            this.cmbRoute.Tag = "Route";
            this.cmbFrequency.Tag = "Frequency";
            //this.btnContradiction.Tag = "Contradiction";
            //this.btnSideEffects.Tag = "SideEffects";

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            //this.cmbDrug.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            //this.cmbDay.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbDay.TextChanged += new TextChangedEventHandler(cmbDay_TextChanged);
            this.cmbDay.GotFocus += new RoutedEventHandler(txt_GotFocus);
            //this.cmbQuantity.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbQuantity.TextChanged += new TextChangedEventHandler(cmbQuantity_TextChanged);
            this.cmbQuantity.GotFocus += new RoutedEventHandler(txt_GotFocus);
            //this.cmbDose.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbRoute.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            //this.cmbFrequency.SelectionChanged += new SelectionChangedEventHandler(cmbDrug_SelectionChanged);
            this.cmbFrequency.TextChanged += new TextChangedEventHandler(cmbFrequency_TextChanged);
            this.cmbFrequency.GotFocus += new RoutedEventHandler(txt_GotFocus);
            //this.btnContradiction.Click += new RoutedEventHandler(btnContradiction_Click);
            //this.btnSideEffects.Click += new RoutedEventHandler(btnContradiction_Click);

            this.cmbDay.KeyDown += new KeyEventHandler(cmbDay_KeyDown);
            this.cmbQuantity.KeyDown += new KeyEventHandler(cmbDay_KeyDown);
            this.cmbFrequency.KeyDown += new KeyEventHandler(cmbDay_KeyDown);

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
            if (this.DataContext != null)
            {
                if (((OtherMedication)this.DataContext).Drug != null)
                {
                    #region Code for Tool Tip
                    switch (((OtherMedication)this.DataContext).Drug)
                    {
                        case "Cotrimoxazole":
                        case "Trimethoprim":
                        case "Sulfamethoxazole":
                        case "Ciprofloxacin":
                            tbl.Text = "indicated for Shigella  or Enteroinvasive E.coli or V. Cholerae";
                            break;

                        case "Doxycycline":
                        case "Furazolidone":
                            tbl.Text = "indicated for  V. Cholerae, ";
                            break;

                        case "Metronidazole":
                            if (((OtherMedication)this.DataContext).Dose == "15mg/kg/day in 3 divided doses for 5 days")
                                tbl.Text = "indicated for acute giardiasis";
                            else if (((OtherMedication)this.DataContext).Dose == "35-50 mg/kg/day in 3 divided doses for 7-10days")
                                tbl.Text = "indicated for Entamoeba histolytica";
                            break;

                        case "Ampicillin":
                            tbl.Text = "indicated for non typhoid salmonella";
                            break;

                        case "Tinidazole":
                            tbl.Text = "indicated for acute giardiasis ";
                            break;

                        case "Dicylomine":
                            tbl.Text = "indicated for children above 6 months in presence of spasmodic diarrhoea\rContraindicated in jaundice and acute abdomen of unknown etiology";
                            break;
                        default:
                            tbl.Text = "";
                            break;
                    }
                    tt.Content = tbl;
                    if (tbl.Text != "")
                        ToolTipService.SetToolTip(this, tt);
                    #endregion
                    //IEnumerator<MasterListItem> mlst= (IEnumerator<MasterListItem>)this.cmbDrug.ItemsSource.GetEnumerator();
                    //while (mlst.MoveNext())
                    //{
                    //    if (((MasterListItem)mlst.Current).ID == ((Medication)this.DataContext).Drug.ID)
                    //    {
                    //        this.cmbDrug.SelectedItem = mlst.Current;
                    //    }
                    //}
                    this.cmbDrug.Text = ((OtherMedication)this.DataContext).Drug;
                    //this.cmbDrug.SelectedValue = ((Medication)this.DataContext).Drug.Description;
                }
                if (((OtherMedication)this.DataContext).Day != null)
                {
                    //this.cmbDay.SelectedValue = ((Medication)this.DataContext).Day;
                    this.cmbDay.Text = ((OtherMedication)this.DataContext).Day.ToString();
                }
                if (((OtherMedication)this.DataContext).Quantity != null)
                {
                    //this.cmbQuantity.SelectedValue = ((Medication)this.DataContext).Quantity;
                    this.cmbQuantity.Text = ((OtherMedication)this.DataContext).Quantity.ToString();
                }
                if (((OtherMedication)this.DataContext).Dose != null)
                {
                    #region Code for tool Tip
                    switch (((OtherMedication)this.DataContext).Dose)
                    {
                        case "15mg/kg/day in 3 divided doses for 5 days":
                            if (((OtherMedication)this.DataContext).Drug == "Metronidazole")
                                tbl.Text = "indicated for acute giardiasis";
                            break;

                        case "35-50 mg/kg/day in 3 divided doses for 7-10days":
                            if (((OtherMedication)this.DataContext).Drug == "Metronidazole")
                                tbl.Text = "indicated for Entamoeba histolytica";
                            else
                                tbl.Text = "";
                            break;

                    }
                    tt.Content = tbl;
                    if (tbl.Text != "")
                        ToolTipService.SetToolTip(this, tt);
                    #endregion
                    //this.cmbDose.SelectedValue = ((Medication)this.DataContext).Dose;
                    this.cmbDose.Text = ((OtherMedication)this.DataContext).Dose.ToString();
                }
                if (((OtherMedication)this.DataContext).Reason != null)
                {
                    this.txtReason.Text = ((OtherMedication)this.DataContext).Reason.ToString();
                }
                if (((OtherMedication)this.DataContext).Route != null)
                    this.cmbRoute.SelectedValue = ((OtherMedication)this.DataContext).Route.ID;

                if (((OtherMedication)this.DataContext).Frequency != null)
                {
                    //this.cmbFrequency.SelectedValue = ((Medication)this.DataContext).Frequency;
                    this.cmbFrequency.Text = ((OtherMedication)this.DataContext).Frequency.ToString();
                }
            }
        }

        void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (((TextBox)sender).Text != null)
                ((TextBox)sender).SelectAll();

        }

        void cmbDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtKeyDown != null)
            {
                this.txtKeyDown(sender, e);
            }
        }

        void cmbFrequency_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtFreqChanged != null)
            {
                this.txtFreqChanged(sender, e);
                this.cmbQuantity.Text = ((OtherMedication)this.DataContext).Quantity.ToString();
            }
        }

        void cmbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtQtyChanged != null)
            {
                this.txtQtyChanged(sender, e);
            }
        }

        void cmbDay_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtDayChanged != null)
            {
                this.txtDayChanged(sender, e);
                this.cmbQuantity.Text = ((OtherMedication)this.DataContext).Quantity.ToString();
            }
        }
        
        //void btnContradiction_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.btnContradictionSideEffectClick != null)
        //    {
        //        this.btnContradictionSideEffectClick(sender, e);
        //    }
        //}
        
        public void cmbDrug_SelectionChanged(object sender, RoutedEventArgs e)
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

    public class CheckListBoxControlItem : ListBoxItem
    {
        public CheckListBoxControlItem()
        {
            this.DefaultStyleKey = typeof(CheckListBoxControlItem);
        }

        private CheckBox chkItem;

        public event RoutedEventHandler chkItemClicked;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.chkItem = base.GetTemplateChild("chkItem") as CheckBox;

            this.chkItem.Click += new RoutedEventHandler(chkItem_Click);

            if (this.Tag != null)
            {
                this.chkItem.Tag = this.Tag;
            }
        }

        void chkItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.chkItemClicked != null)
            {
                this.chkItemClicked(sender, e);
            }
        }
    }

    //Added by Saily P on 29.11.13 Purpose - New Control
    public class BPControlItem : ListBoxItem
    {
        public BPControlItem()
        {
            this.DefaultStyleKey = typeof(BPControlItem);
        }
        
        private TextBox txtBP1;        
        private TextBox txtBP2;
        private TextBlock txtShlash;

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //public event TextChangedEventHandler txtBP1Changed;
        //public event TextChangedEventHandler txtBP2Changed;
        
        public event KeyEventHandler txtKeyDown;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtBP1 = base.GetTemplateChild("txtBP1") as TextBox;
            this.txtBP2 = base.GetTemplateChild("txtBP2") as TextBox;
            
            this.txtBP1.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtBP2.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);

            this.txtBP1.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtBP2.KeyDown += new KeyEventHandler(TextName_KeyDown);

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
            
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

    }

    public class VisionControlItem : ListBoxItem
    {
        public VisionControlItem()
        {
            this.DefaultStyleKey = typeof(VisionControlItem);
        }

        private TextBox txtSPH;
        private TextBox txtCYL;
        private TextBox txtAxis;

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //public event TextChangedEventHandler txtBP1Changed;
        //public event TextChangedEventHandler txtBP2Changed;

        public event KeyEventHandler txtKeyDown;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtAxis = base.GetTemplateChild("txtAxis") as TextBox;
            this.txtCYL = base.GetTemplateChild("txtCYL") as TextBox;
            this.txtSPH = base.GetTemplateChild("txtSPH") as TextBox;

            this.txtAxis.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtCYL.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtSPH.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);

            this.txtAxis.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtCYL.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtSPH.KeyDown += new KeyEventHandler(TextName_KeyDown);

            this.txtSPH.Tag = "SPH";
            this.txtAxis.Tag = "Axis";
            this.txtCYL.Tag = "CYL";

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

    }

    public class GlassPowerControlItem : ListBoxItem
    {
        public GlassPowerControlItem()
        {
            this.DefaultStyleKey = typeof(GlassPowerControlItem);
        }

        private TextBox txtSPH1;
        private TextBox txtCYL1;
        private TextBox txtAxis1;
        private TextBox txtVA1;
        private TextBox txtSPH2;
        private TextBox txtCYL2;
        private TextBox txtAxis2;
        private TextBox txtVA2;

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //public event TextChangedEventHandler txtBP1Changed;
        //public event TextChangedEventHandler txtBP2Changed;

        public event KeyEventHandler txtKeyDown;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.txtSPH1 = base.GetTemplateChild("txtSPH1") as TextBox;
            this.txtCYL1 = base.GetTemplateChild("txtCYL1") as TextBox;
            this.txtAxis1 = base.GetTemplateChild("txtAxis1") as TextBox;
            this.txtVA1 = base.GetTemplateChild("txtVA1") as TextBox;
            this.txtSPH2 = base.GetTemplateChild("txtSPH2") as TextBox;
            this.txtCYL2 = base.GetTemplateChild("txtCYL2") as TextBox;
            this.txtAxis2 = base.GetTemplateChild("txtAxis2") as TextBox;
            this.txtVA2 = base.GetTemplateChild("txtVA2") as TextBox;

            this.txtSPH1 = new TextBox();
            this.txtCYL1 = new TextBox();
            this.txtAxis1 = new TextBox();
            this.txtVA1 = new TextBox();

            this.txtSPH1.Tag = "SPH";
            this.txtAxis1.Tag = "Axis";
            this.txtCYL1.Tag = "CYL";
            this.txtVA1.Tag = "VA";

            this.txtSPH1.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtCYL1.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtAxis1.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtVA1.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtSPH2.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtCYL2.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtAxis2.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);
            this.txtVA2.TextChanged += new TextChangedEventHandler(txtNumber_TextChanged);

            this.txtSPH1.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtCYL1.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtAxis1.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtVA1.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtSPH2.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtCYL2.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtAxis2.KeyDown += new KeyEventHandler(TextName_KeyDown);
            this.txtCYL2.KeyDown += new KeyEventHandler(TextName_KeyDown);                    

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItDecimal() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

    }
    //
    
    public class FileUploadRepeterControlItem : ListBoxItem
    {
        public FileUploadRepeterControlItem()
        {
            this.DefaultStyleKey = typeof(FileUploadRepeterControlItem);
        }

        private HyperlinkButton View;
        private HyperlinkButton AddRemoveClick;

        private TextBox FileName;
        private Button Browse;

        public event RoutedEventHandler OnAddRemoveClick;
        public event RoutedEventHandler OnViewClick;
        public event RoutedEventHandler OnBrowseClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AddRemoveClick = base.GetTemplateChild("CmdAddRemove") as HyperlinkButton;
            this.View = base.GetTemplateChild("View") as HyperlinkButton;

            this.Browse = base.GetTemplateChild("Browse") as Button;
            this.FileName = base.GetTemplateChild("FileName") as TextBox;                        

            this.AddRemoveClick.Click += new RoutedEventHandler(this.CmdAddRemoveClick);
            this.View.Click += new RoutedEventHandler(View_Click);
            this.Browse.Click += new RoutedEventHandler(Browse_Click);

            ToolTip tt = new ToolTip();
            TextBlock tbl = new TextBlock();
            tbl.TextWrapping = TextWrapping.Wrap;
            tbl.Text = "";
            if (this.DataContext != null)
            {                
                if (((FileUpload)this.DataContext).FileName != null)
                {                    
                    this.FileName.Text = ((FileUpload)this.DataContext).FileName.ToString();
                }
            }
        }

        void Browse_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnBrowseClick != null)
            {
                this.OnBrowseClick(sender, e);
            }
        }

        void View_Click(object sender, RoutedEventArgs e)
        {
            if (this.OnViewClick != null)
            {
                this.OnViewClick(sender, e);
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
