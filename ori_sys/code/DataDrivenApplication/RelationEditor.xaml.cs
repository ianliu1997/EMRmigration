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
using DataDrivenApplication;
//using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Service.DataTemplateServiceRef1;

namespace DataDrivenApplication
{
    public partial class RelationEditor : ChildWindow
    {
        public FormDetail Form { get; set; }
        public IntraTemplateRelation ITR { get; set; }
        public RelationEditor(FormDetail Form)
        {
            if (Form == null)
            {
                throw new ArgumentNullException();
            }
            InitializeComponent();

            this.Form = Form;
            this.ITR = null;
            this.Loaded += new RoutedEventHandler(FormEditor_Loaded);
        }

        void FormEditor_Loaded(object sender, RoutedEventArgs e)
        {
            dgTemplateList.ItemsSource = Form.Relations;
            //cmbSourceSection.ItemsSource = Form.SectionList;
            //cmbTargetSection.ItemsSource = Form.SectionList;
        }

        public event RoutedEventHandler OnOkButtonClick;
        public event RoutedEventHandler OnDeleteRelClick;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IntraTemplateRelation relation = new IntraTemplateRelation();
                relation.TemplateId = Form.ID;
                relation.SourceSectionId = ((SectionDetail)cmbSourceSection.SelectedItem).UniqueId.ToString();
                relation.SourceFieldId = ((FieldDetail)cmbSourceField.SelectedItem).UniqueId.ToString();
                relation.TargetSectionId = ((SectionDetail)cmbTargetSection.SelectedItem).UniqueId.ToString();
                relation.TargetFieldId = ((FieldDetail)cmbTargetField.SelectedItem).UniqueId.ToString();
                if (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id == 2)
                {
                    relation.ExpCondition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = coptYes.IsChecked.Value };
                }
                if (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id == 5)
                {
                    relation.ExpCondition = new DecimalExpression<decimal>() { Operation = ((Operation<DoubleOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = decimal.Parse(cdecREf.Text) };
                }
                if (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id == 4)
                {
                    relation.ExpCondition = new ComboExpression<bool>() { Operation = ((Operation<ComboOperations>)lbcCondition.SelectedItem).OperationType, SelectedItem = (String)((DynamicListItem)lbcItem.SelectedItem).Title };
                }
                if (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id == 13)
                {
                    relation.ExpCondition = new CheckListExpression<bool>() { Operation = ((Operation<ComboOperations>)lbcCondition.SelectedItem).OperationType, SelectedItem = (PalashDynamics.ValueObjects.MasterListItem)CLBCItem.SelectedItem };
                }
                if (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id == 15)
                {
                    relation.ExpCondition = new CheckListExpression<bool>() { Operation = ((Operation<ComboOperations>)lbcCondition.SelectedItem).OperationType, SelectedItem = (PalashDynamics.ValueObjects.MasterListItem)CLBCItem.SelectedItem };
                }

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(relation, e);
                }
                this.DialogResult = true;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void cmbSourceSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSourceField.ItemsSource = null;
            if (cmbSourceSection.SelectedItem != null)
            {
                cmbSourceField.ItemsSource = ((SectionDetail)cmbSourceSection.SelectedItem).FieldList;
            }
        }

        private void cmbTargetSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTargetField.ItemsSource = null;
            if (cmbTargetSection.SelectedItem != null)
            {
                cmbTargetField.ItemsSource = ((SectionDetail)cmbTargetSection.SelectedItem).FieldList;
            }
        }

        private void cmbSourceField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cmbTargetField.ItemsSource = null;
            if (cmbSourceField.SelectedItem != null)
            {
                if ((cmbSourceField.SelectedItem is FieldDetail))
                {
                    fdCondition.Visibility = Visibility.Visible;
                    fdCondition.DataContext = cmbSourceField.SelectedItem;
                    switch (((FieldDetail)cmbSourceField.SelectedItem).DataType.Id)
                    {
                        case 1:
                            break;
                        case 2:
                            List<Operation<BooleanOperations>> list = new List<Operation<BooleanOperations>>();
                            list.Add(new Operation<BooleanOperations>() { Title = BooleanOperations.EqualTo.ToString(), OperationType = BooleanOperations.EqualTo });
                            list.Add(new Operation<BooleanOperations>() { Title = BooleanOperations.NotEqualTo.ToString(), OperationType = BooleanOperations.NotEqualTo });
                            lbcCondition.ItemsSource = list;
                            cboolREf.Visibility = Visibility.Visible;
                            break;
                        case 3:
                            break;
                        case 4:
                            List<Operation<ComboOperations>> clist = new List<Operation<ComboOperations>>();
                            clist.Add(new Operation<ComboOperations>() { Title = ComboOperations.EqualTo.ToString(), OperationType = ComboOperations.EqualTo });
                            clist.Add(new Operation<ComboOperations>() { Title = ComboOperations.NotEqualTo.ToString(), OperationType = ComboOperations.NotEqualTo });
                            lbcCondition.ItemsSource = clist;
                            FieldDetail fd = (FieldDetail)cmbSourceField.SelectedItem;
                            lbcItem.ItemsSource = ((ListFieldSetting)fd.Settings).ItemSource;
                            lbcItem.Visibility = Visibility.Visible;
                            //cboolREf.Visibility = Visibility.Visible;
                            break;
                        case 5:
                            List<Operation<DoubleOperations>> dlist = new List<Operation<DoubleOperations>>();
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.EqualTo.ToString(), OperationType = DoubleOperations.EqualTo });
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.NotEqualTo.ToString(), OperationType = DoubleOperations.NotEqualTo });
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.GreterThan.ToString(), OperationType = DoubleOperations.GreterThan });
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.GreterThanEqualTo.ToString(), OperationType = DoubleOperations.GreterThanEqualTo });
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.LessThan.ToString(), OperationType = DoubleOperations.LessThan });
                            dlist.Add(new Operation<DoubleOperations>() { Title = DoubleOperations.LessThanEqualTo.ToString(), OperationType = DoubleOperations.LessThanEqualTo });
                            lbcCondition.ItemsSource = dlist;
                            cdecREf.Visibility = Visibility.Visible;
                            break;
                        case 6:
                            break;
                        case 7:
                            break;
                        case 13:
                            List<Operation<ComboOperations>> CBLClist = new List<Operation<ComboOperations>>();
                            CBLClist.Add(new Operation<ComboOperations>() { Title = ComboOperations.EqualTo.ToString(), OperationType = ComboOperations.EqualTo });
                            CBLClist.Add(new Operation<ComboOperations>() { Title = ComboOperations.NotEqualTo.ToString(), OperationType = ComboOperations.NotEqualTo });
                            lbcCondition.ItemsSource = CBLClist;
                            FieldDetail CBLCfd = (FieldDetail)cmbSourceField.SelectedItem;
                            CLBCItem.ItemsSource = ((AutomatedListFieldSetting)CBLCfd.Settings).ItemSource;
                            CLBCItem.Visibility = Visibility.Visible;
                            //cboolREf.Visibility = Visibility.Visible;
                            break;
                        case 15:
                            List<Operation<ComboOperations>> CBLClist1 = new List<Operation<ComboOperations>>();
                            CBLClist1.Add(new Operation<ComboOperations>() { Title = ComboOperations.EqualTo.ToString(), OperationType = ComboOperations.EqualTo });
                            CBLClist1.Add(new Operation<ComboOperations>() { Title = ComboOperations.NotEqualTo.ToString(), OperationType = ComboOperations.NotEqualTo });
                            lbcCondition.ItemsSource = CBLClist1;
                            FieldDetail CBLCfd1 = (FieldDetail)cmbSourceField.SelectedItem;
                            CLBCItem.ItemsSource = ((InvestigationFieldSetting)CBLCfd1.Settings).ItemSource;
                            CLBCItem.Visibility = Visibility.Visible;
                            //cboolREf.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    fdCondition.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void PreviewHyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteItemButton1_Click(object sender, RoutedEventArgs e)
        {
            if (dgTemplateList.SelectedItem != null)
            {
                if (OnDeleteRelClick != null)
                {
                    OnDeleteRelClick(this.ITR, e);
                }
                this.DialogResult = true;
            }
        }

        private void AddItemButton1_Click(object sender, RoutedEventArgs e)
        {
            ListView.Visibility = Visibility.Collapsed;
            FormView.Visibility = Visibility.Visible;
            cmbSourceSection.ItemsSource = Form.SectionList;
            cmbTargetSection.ItemsSource = Form.SectionList;
        }



        private void CancelItemButton_Click(object sender, RoutedEventArgs e)
        {
            ListView.Visibility = Visibility.Visible;
            FormView.Visibility = Visibility.Collapsed;
        }

        private void lbcItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //lbcItem.ItemsSource = null;
            //if (cmbSourceField.SelectedItem != null)
            //{
            //    FieldDetail fd = (FieldDetail)cmbSourceField.SelectedItem;
            //    lbcItem.ItemsSource=((ListFieldSetting)fd.Settings).ItemSource;                                
            //}
        }

        private void dgTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.ITR = (IntraTemplateRelation)dgTemplateList.SelectedItem;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

