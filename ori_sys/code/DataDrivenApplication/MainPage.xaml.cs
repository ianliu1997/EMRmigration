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
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
//using PalashDynamics.Service.DataTemplateHttpsServiceRef;
//using PalashDynamics.Service.DataTemplateServiceRef;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.DataTemplateServiceRef1;

namespace DataDrivenApplication
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //List<DateTime> Samples = new List<DateTime>();
            //for (int i = 0; i < 50; i++)
            //{
            //    Samples.Add(DateTime.Now.AddDays(-(i*5)));
            //}
            //int hours = MyClass.HowManyHoursInTheFirstYear(Samples, (d1, d2) => 
            //{
            //    return d1.Month == d2.Month;
            //});

            //MyClass.HowManyHoursInTheFirstYear(Samples, (d1, d2) = > { return d1.Month == d2.Month; }); 
            //MyClass.HowManyDaysInTheFirstPeriod(Samples, (d1, d2) = > { return d1.Year == d2.Year; }); 

        }


        public FormDetail FormView { get; set; }


        private void CreateForm_Click(object sender, RoutedEventArgs e)
        {
            FormView = new FormDetail();
            FormView.Title = fTitle.Text;
            FormView.Description = fDescription.Text;
            FormDetail.Visibility = Visibility.Collapsed;
            FieldDetail.Visibility = Visibility.Visible;
            fdType.ItemsSource = FieldTypeList.GetFieldTypesList();
            fdParent.ItemsSource = FormView.FieldList;
            trvParent.ItemsSource = FormView.FieldList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddField_Click(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(fdTitle.Text))
            //{
            //    if (FormView == null)
            //        throw new ArgumentNullException("FormDetail");
            //    if (FormView.FieldList == null)
            //    {
            //        FormView.FieldList = new List<FieldDetail>();
            //    }

            //    FieldDetail fd = new FieldDetail();
            //    fd.Title = fdTitle.Text;
            //    fd.DataType = (FieldType)fdType.SelectedItem;
            //    if (fd.DataType.Id == 1)
            //    {
            //        fd.Settings = new TextFieldSetting() { Mode = rdbSingleLine.IsChecked.Value };
            //    }
            //    if (fd.DataType.Id == 2)
            //    {
            //        fd.Settings = new BooleanFieldSetting() { Mode = rdbCheckBox.IsChecked.Value };
            //    }
            //    if (fd.DataType.Id == 4)
            //    {
            //        List<string> list = new List<string>();
            //        foreach (var item in fdOptionList.Text.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries))
            //        {
            //            list.Add(item);
            //        }
            //        fd.Settings = new ListFieldSetting() { ChoiceMode = SelectionMode.Single };
            //        fd.DefaultValve = list;
            //    }

            //    if (trvParent.SelectedItem != null)
            //    {
            //        if (((FieldDetail)trvParent.SelectedItem).DataType.Id == 1)
            //        {
            //            //fd.Condition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
            //        }
            //        if (((FieldDetail)trvParent.SelectedItem).DataType.Id == 2)
            //        {
            //            StackPanel panel = (StackPanel)cValueContainer.Content;

            //            fd.Condition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = true };
            //        }
            //        if (((FieldDetail)trvParent.SelectedItem).DataType.Id == 3)
            //        {
            //           // fd.Condition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
            //        }
            //        if (((FieldDetail)trvParent.SelectedItem).DataType.Id == 4)
            //        {
            //           // fd.Condition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
            //        }
            //        fd.Parent = (FieldDetail)trvParent.SelectedItem;
            //        ((FieldDetail)trvParent.SelectedItem).DependentFieldDetail.Add(fd);
            //    }
            //    else
            //    {
            //        FormView.FieldList.Add(fd);
            //    }
            //    fdTitle.Text = "";
            //    fdType.SelectedIndex = 0;
            //    fdParent.ItemsSource = null;
            //    fdParent.ItemsSource = FormView.FieldList;
            //    fdParent.SelectedItem = null;
            //    trvParent.ItemsSource = null;
            //    trvParent.ItemsSource = FormView.FieldList;
            //}
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            RowDefinition Title = new RowDefinition();
            Title.Height = new GridLength(23, GridUnitType.Auto);
            RowDefinition Desc = new RowDefinition();
            Desc.Height = new GridLength(50, GridUnitType.Auto);
            RowDefinition Description = new RowDefinition();
            Form.RowDefinitions.Add(Title);
            Form.RowDefinitions.Add(Desc);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(200, GridUnitType.Auto);
            ColumnDefinition column2 = new ColumnDefinition();
            //column1.Width = new GridLength(200, GridUnitType.Auto);
            Form.ColumnDefinitions.Add(column1);
            Form.ColumnDefinitions.Add(column2);

            TextBlock T = new TextBlock();
            T.Text = FormView.Title;
            Grid.SetColumnSpan(T, 2);
            TextBlock T2 = new TextBlock();
            T2.Text = FormView.Description;
            Grid.SetRow(T2, 1);
            Form.Children.Add(T);
            Form.Children.Add(T2);
            if (FormView != null)
            {
                foreach (var item in FormView.FieldList)
                {
                    AddNodeItems(item);
                }

                Form.Visibility = Visibility.Visible;
                FormDetail.Visibility = Visibility.Collapsed;
                FieldDetail.Visibility = Visibility.Collapsed;
            }
        }

        public void AddNodeItems(FieldDetail PItem)
        {
            //RowDefinition Row = new RowDefinition();
            //Row.Height = new GridLength(23, GridUnitType.Auto);
            //Form.RowDefinitions.Add(Row);

            //TextBlock FTitle = new TextBlock();
            //FTitle.Text = PItem.Title;
            //Grid.SetRow(FTitle, Form.RowDefinitions.Count - 1);
            //Form.Children.Add(FTitle);

            //switch (PItem.DataType.Id)
            //{
            //    case 1:
            //        TextBox Field = new TextBox();
            //        Grid.SetRow(Field, Form.RowDefinitions.Count - 1);
            //        Grid.SetColumn(Field, 1);
            //        if (PItem.Settings != null && ((TextFieldSetting)PItem.Settings).Mode )
            //        {
            //            Field.AcceptsReturn = true;
            //            Field.Height = 60;
            //            Field.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //        }
            //        if (PItem.Parent != null)
            //            Field.IsEnabled = false;
            //        PItem.Control = Field;
            //        Form.Children.Add(Field);
            //        break;
            //    case 2:
            //        if (PItem.Settings != null && ((BooleanFieldSetting)PItem.Settings).Mode)

            //        {
            //            CheckBox chk = new CheckBox();
            //            chk.Click += new RoutedEventHandler(chk_Click);
            //            chk.DataContext = PItem;
            //            Grid.SetRow(chk, Form.RowDefinitions.Count - 1);
            //            Grid.SetColumn(chk, 1);
            //            //chk.ItemsSource = (List<String>)item.DefaultValve;
            //            PItem.Control = chk;
            //            if (PItem.Parent != null)
            //                chk.IsEnabled = false;
            //            Form.Children.Add(chk);
            //        }
            //        else
            //        {
            //            StackPanel panel = new StackPanel();
            //            panel.DataContext = PItem;
            //            panel.Orientation = Orientation.Horizontal;
            //            RadioButton yes = new RadioButton();
            //            if (PItem.Parent != null)
            //                yes.IsEnabled = false;
            //            yes.Click += new RoutedEventHandler(chk_Click);
            //            yes.Content = "Yes";
            //            RadioButton No = new RadioButton();
            //            if (PItem.Parent != null)
            //                No.IsEnabled = false;
            //            No.Click += new RoutedEventHandler(chk_Click);
            //            No.Content = "No";
            //            panel.Children.Add(yes);
            //            panel.Children.Add(No);
            //            Grid.SetRow(panel, Form.RowDefinitions.Count - 1);
            //            Grid.SetColumn(panel, 1);
            //            PItem.Control = panel;
            //            Form.Children.Add(panel);
            //        }
            //        break;
            //    case 3:
            //        DatePicker dtp = new DatePicker();
            //        Grid.SetRow(dtp, Form.RowDefinitions.Count - 1);
            //        Grid.SetColumn(dtp, 1);
            //        PItem.Control = dtp;
            //        if (PItem.Parent != null)
            //            dtp.IsEnabled = false;
            //        Form.Children.Add(dtp);
            //        break;
            //    case 4:
            //        if (PItem.Settings != null && ((ListFieldSetting)PItem.Settings).ChoiceMode == SelectionMode.Single)
            //        {
            //            ComboBox cmbList = new ComboBox();
            //            Grid.SetRow(cmbList, Form.RowDefinitions.Count - 1);
            //            Grid.SetColumn(cmbList, 1);
            //            cmbList.ItemsSource = (List<String>)PItem.DefaultValve;
            //            PItem.Control = cmbList;
            //            if (PItem.Parent != null)
            //                cmbList.IsEnabled = false;
            //            Form.Children.Add(cmbList);
            //        }
            //        else
            //        {
            //            ListBox cmbList = new ListBox();
            //            Grid.SetRow(cmbList, Form.RowDefinitions.Count - 1);
            //            Grid.SetColumn(cmbList, 1);
            //            cmbList.ItemsSource = (List<String>)PItem.DefaultValve;
            //            PItem.Control = cmbList;
            //            if (PItem.Parent != null)
            //                cmbList.IsEnabled = false;
            //            Form.Children.Add(cmbList);
            //        }
            //        break;
            //}
            //if (PItem.DependentFieldDetail != null && PItem.DependentFieldDetail.Count > 0)
            //    foreach (var item in PItem.DependentFieldDetail)
            //    {
            //        AddNodeItems(item);
            //    }
        }


        void chk_Click(object sender, RoutedEventArgs e)
        {

            if (sender is CheckBox)
            {
                foreach (var item in ((FieldDetail)(((CheckBox)sender).DataContext)).DependentFieldDetail)
                {
                    if (item.Control is TextBox)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((CheckBox)sender).IsChecked)
                                    ((TextBox)item.Control).IsEnabled = true;
                                else
                                    ((TextBox)item.Control).IsEnabled = false;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((CheckBox)sender).IsChecked)
                                    ((TextBox)item.Control).IsEnabled = true;
                                else
                                    ((TextBox)item.Control).IsEnabled = false;
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }


                    if (item.Control is CheckBox)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((CheckBox)sender).IsChecked)
                                    ((CheckBox)item.Control).IsEnabled = true;
                                else
                                    ((CheckBox)item.Control).IsEnabled = false;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((CheckBox)sender).IsChecked)
                                    ((CheckBox)item.Control).IsEnabled = true;
                                else
                                    ((CheckBox)item.Control).IsEnabled = false;
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }

                    if (item.Control is Panel)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((CheckBox)sender).IsChecked)
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = true;
                                        }
                                    }

                                else
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = false;
                                        }
                                    }
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((CheckBox)sender).IsChecked)
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = true;
                                        }
                                    }
                                else
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = false;
                                        }
                                    }
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }

                }
            }

            if (sender is RadioButton)
            {
                foreach (var item in ((FieldDetail)(((RadioButton)sender).DataContext)).DependentFieldDetail)
                {
                    if (item.Control is TextBox)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    ((TextBox)item.Control).IsEnabled = true;
                                else
                                    ((TextBox)item.Control).IsEnabled = false;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    ((TextBox)item.Control).IsEnabled = true;
                                else
                                    ((TextBox)item.Control).IsEnabled = false;
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }


                    if (item.Control is CheckBox)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    ((CheckBox)item.Control).IsEnabled = true;
                                else
                                    ((CheckBox)item.Control).IsEnabled = false;
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    ((CheckBox)item.Control).IsEnabled = true;
                                else
                                    ((CheckBox)item.Control).IsEnabled = false;
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }

                    if (item.Control is Panel)
                    {
                        switch (((BooleanExpression<bool>)item.Condition).Operation)
                        {
                            case BooleanOperations.EqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue == ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = true;
                                        }
                                    }

                                else
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = false;
                                        }
                                    }
                                break;
                            case BooleanOperations.NotEqualTo:
                                if ((bool)((BooleanExpression<bool>)item.Condition).ReferenceValue != ((RadioButton)((StackPanel)((RadioButton)sender).Parent).Children[0]).IsChecked)
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = true;
                                        }
                                    }
                                else
                                    foreach (var child in ((Panel)item.Control).Children)
                                    {
                                        if (child is Control)
                                        {
                                            ((Control)child).IsEnabled = false;
                                        }
                                    }
                                break;
                        }
                        //((TextBox)item.Control).IsEnabled = 
                    }

                }
            }

        }

        private void fdType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblOptions.Visibility = Visibility.Collapsed;
            fdOptions.Visibility = Visibility.Collapsed;
            TextDataTypeSetting.Visibility = Visibility.Collapsed;
            BooleanDataTypeSetting.Visibility = Visibility.Collapsed;
            ListDataTypeSetting.Visibility = Visibility.Collapsed;
            if (fdType.SelectedItem == null || fdType.SelectedIndex == 0)
                return;

            switch (((FieldType)fdType.SelectedItem).Id)
            {
                case 1:
                    lblOptions.Visibility = Visibility.Visible;
                    fdOptions.Visibility = Visibility.Visible;
                    TextDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 2:
                    lblOptions.Visibility = Visibility.Visible;
                    fdOptions.Visibility = Visibility.Visible;
                    BooleanDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 3:
                    break;
                case 4:
                    lblOptions.Visibility = Visibility.Visible;
                    fdOptions.Visibility = Visibility.Visible;
                    ListDataTypeSetting.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fdParent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fdParent.SelectedItem = null;
        }

        private void trvParent_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (trvParent.SelectedItem != null)
            {
                fdCondition.Visibility = Visibility.Visible;
                switch (((FieldDetail)trvParent.SelectedItem).DataType.Id)
                {
                    case 1:
                        break;
                    case 2:
                        List<Operation<BooleanOperations>> list = new List<Operation<BooleanOperations>>();
                        list.Add(new Operation<BooleanOperations>() { Title = BooleanOperations.EqualTo.ToString(), OperationType = BooleanOperations.EqualTo });
                        list.Add(new Operation<BooleanOperations>() { Title = BooleanOperations.NotEqualTo.ToString(), OperationType = BooleanOperations.NotEqualTo });
                        StackPanel panel = new StackPanel();
                        panel.Orientation = Orientation.Horizontal;
                        RadioButton yes = new RadioButton();
                        yes.Content = "Yes";
                        RadioButton No = new RadioButton();
                        No.Content = "No";
                        panel.Children.Add(yes);
                        panel.Children.Add(No);
                        cValueContainer.Content = panel;
                        //list.Add(new Operation<BooleanOperations>>() { Id = 0, Title = "-- Select -- ", DataType = "System.String" });
                        //list.Add(new FieldType() { Id = 1, Title = "Text", DataType = "System.String" });
                        //list.Add(new FieldType() { Id = 2, Title = "Boolen", DataType = "System.Boolen" });
                        //list.Add(new FieldType() { Id = 3, Title = "Date", DataType = "System.DateTime" });
                        //list.Add(new FieldType() { Id = 4, Title = "List", DataType = "System.String" });
                        lbcCondition.ItemsSource = list;
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            else
            {
                fdCondition.Visibility = Visibility.Collapsed;
            }
        }

        Calculator cal = new Calculator();

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Result.Text = cal.Calculate(Operations.Add, 45, 6).ToString();
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            Result.Text = cal.Calculate(Operations.Subtract, 45, 6).ToString();
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            Result.Text = cal.Calculate(Operations.Multiply, 45, 6).ToString();
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            Result.Text = cal.Calculate(Operations.Divide, 45, 6).ToString();
        }
    }



    [XmlInclude(typeof(PatientCaseRecordSetting))]
    public abstract class CaseRecordSettings
    {

    }

    public class PatientCaseRecordSetting : CaseRecordSettings
    {
        #region Set Particulars
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Add { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string ClinicRefNo { get; set; }
        #endregion

        #region Set Illness Summary
        public string ComplaintReported { get; set; }
        public string ChiefComplaint { get; set; }
        public string PastHistory { get; set; }
        public string DrugHistory { get; set; }
        public string Allergies { get; set; }
        #endregion

        #region Set Observation
        public string Investigations { get; set; }
        public string ProvisionalDiagnosis { get; set; }
        public string FinalDiagnosis { get; set; }
        #endregion

        #region Set Therapy

        public string HydrationStatusManagement { get; set; }
        public string Hydration4StatusManagement { get; set; }
        public string ZincSupplementManagement { get; set; }
        public string NutritionAdvise { get; set; }

        public List<Medication> ItemsSource1 { get; set; }
        public List<Medication> ItemsSource2 { get; set; }
        public List<Medication> ItemsSource3 { get; set; }
        public List<Medication> ItemsSource4 { get; set; }

        #endregion

        #region Set Education
        public string AdvisoryAttached { get; set; }
        public string WhenToVisit { get; set; }
        public string SpecificInstructions { get; set; }
        #endregion

        #region Set Others
        public string FollowUpDate { get; set; }
        public string FollowUpAt { get; set; }
        public string ReferralTo { get; set; }
        #endregion

    }


    public class PatientCaseReferralSetting : CaseRecordSettings
    {
        #region Set Particulars
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Add { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string ClinicRefNo { get; set; }
        #endregion

        #region Set Referral Details
        public string ReferredByDoctor { get; set; }
        public string ReferredToDoctor { get; set; }
        public string ClinicNo1 { get; set; }
        public string ClinicNo2 { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        #endregion

        #region Set Set Provisonal Diagnosis && Chief Complaint
        public string ProDiag { get; set; }
        public string ChiefComplaint { get; set; }
        #endregion

        #region Set SetPatientDetails
        public string Summary { get; set; }
        #endregion

        #region Set Set Referral Remarks
        public string Remarks { get; set; }
        #endregion
    }

    [XmlInclude(typeof(FormDetail))]
    [XmlInclude(typeof(SectionDetail))]
    [XmlInclude(typeof(FieldDetail))]
    public abstract class BaseDetail
    {
        public abstract string Title { get; set; }
        public abstract string Name { get; set; }
    }

    public class FormDetail : BaseDetail
    {
        public bool IsPhysicalExam { get; set; }
        public bool IsForOT { get; set; }
        public int ID { get; set; }
        public override string Title { get; set; }
        public override string Name { get; set; }
        public string Description { get; set; }
        public string ProtocolUrl { get; set; }
        public string FlowChartUrl { get; set; }
        public List<SectionDetail> SectionList { get; set; }
        public List<FieldDetail> FieldList { get; set; }
        public List<IntraTemplateRelation> Relations { get; set; }
        public List<PatientCaseRecordRelation> PCRRelations { get; set; }
        public List<PatientCaseReferralRelation> CaseReferralRelations { get; set; }
        public int ApplicableTo { get; set; }
        //BY ROHINI DATED 15.1.2016
        public long TemplateTypeID { get; set; }
        public string TemplateType { get; set; }
        public long TemplateSubtypeID { get; set; }
        public string TemplateSubtype { get; set; }
        //
    }

    public class SectionDetail : BaseDetail
    {
        public SectionDetail()
        {
        }
        public int ID { get; set; }
        public override string Title { get; set; }
        public override string Name { get; set; }
        public string Description { get; set; }
        #region Added by Harish
        public string Tab { get; set; }
        public bool IsToolTip { get; set; }
        public string ToolTipText { get; set; }
        #endregion
        public List<FieldDetail> FieldList { get; set; }
        public string UniqueId { get; set; }
        #region Added by Harish
        public List<string> ReadPermission { get; set; }
        public List<string> ReadWritePermission { get; set; }
        #endregion
    }

    public class FieldDetail : BaseDetail
    {
        public FieldDetail()
        {
            DependentFieldDetail = new List<FieldDetail>();
        }
        public string UniqueId { get; set; }
        public int ID { get; set; }
        public override string Title { get; set; }
        public override string Name { get; set; }
        public FieldType DataType { get; set; }
        public bool IsRequired { get; set; }
        #region Added by Harish
        public bool IsToolTip { get; set; }
        public string ToolTipText { get; set; }
        #endregion
        //public object DefaultValve { get; set; }
        public Settings Settings { get; set; }
        //public object Valve { get; set; }
        public List<FieldDetail> DependentFieldDetail { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public List<FieldDetail> RelationalFieldList { get; set; }
        public BaseExpression Condition { get; set; }
        public BaseExpression RelationCondition { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public BaseDetail Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public Object Control { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public Object LabelControl { get; set; }
    }


    public class FieldType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
    }

    public class FieldTypeList
    {
        public static List<FieldType> GetFieldTypesList()
        {
            List<FieldType> list = new List<FieldType>();
            list.Add(new FieldType() { Id = 0, Title = "-- Select -- ", DataType = "System.String" });
            list.Add(new FieldType() { Id = 1, Title = "Text", DataType = "System.String" });
            list.Add(new FieldType() { Id = 2, Title = "Boolean", DataType = "System.Boolean" });
            list.Add(new FieldType() { Id = 3, Title = "Date", DataType = "System.DateTime" });
            list.Add(new FieldType() { Id = 4, Title = "List", DataType = "System.String" });
            list.Add(new FieldType() { Id = 5, Title = "Decimal", DataType = "System.Double" });
            list.Add(new FieldType() { Id = 6, Title = "Hyperlink", DataType = "System.string" });
            list.Add(new FieldType() { Id = 7, Title = "Header", DataType = "System.String" });
            list.Add(new FieldType() { Id = 8, Title = "Lookup", DataType = "System.String" });
            list.Add(new FieldType() { Id = 9, Title = "Medication", DataType = "System.String" });
            list.Add(new FieldType() { Id = 10, Title = "FollowUp Medication", DataType = "System.String" });
            list.Add(new FieldType() { Id = 11, Title = "Other Investigation", DataType = "System.String" });
            list.Add(new FieldType() { Id = 12, Title = "List Of CheckBoxes", DataType = "System.String" });
            list.Add(new FieldType() { Id = 13, Title = "Databse List", DataType = "System.String" });
            list.Add(new FieldType() { Id = 14, Title = "Other Medication", DataType = "System.String" });
            list.Add(new FieldType() { Id = 15, Title = "Investigation & Services", DataType = "System.String" });
            list.Add(new FieldType() { Id = 16, Title = "Time Picker", DataType = "System.String" });
            list.Add(new FieldType() { Id = 17, Title = "File Upload", DataType = "System.String" });
            return list;
        }



        public static List<Operation<BooleanOperations>> GetBoolenOperationList()
        {
            return new List<Operation<BooleanOperations>> { new Operation<BooleanOperations>() { OperationType = BooleanOperations.EqualTo, Title = BooleanOperations.EqualTo.ToString() }, new Operation<BooleanOperations>() { OperationType = BooleanOperations.NotEqualTo, Title = BooleanOperations.NotEqualTo.ToString() } };
        }

        public static List<Operation<DoubleOperations>> GetNumberOperationList()
        {
            return new List<Operation<DoubleOperations>>();
        }
    }

    [XmlInclude(typeof(InvestigationFieldSetting))]
    [XmlInclude(typeof(AutomatedListFieldSetting))]
    [XmlInclude(typeof(ListOfCheckBoxesFieldSetting))]
    [XmlInclude(typeof(OtherInvestigationFieldSetting))]
    [XmlInclude(typeof(HyperlinkFieldSetting))]
    [XmlInclude(typeof(DecimalFieldSetting))]
    [XmlInclude(typeof(ListFieldSetting))]
    [XmlInclude(typeof(LookUpFieldSetting))]
    [XmlInclude(typeof(MedicationFieldSetting))]
    [XmlInclude(typeof(OtherMedicationFieldSetting))]
    [XmlInclude(typeof(BooleanFieldSetting))]
    [XmlInclude(typeof(TextFieldSetting))]
    [XmlInclude(typeof(DateFieldSetting))]
    [XmlInclude(typeof(TimeFieldSetting))]
    [XmlInclude(typeof(FileUploadFieldSetting))]
    public abstract class Settings
    {
    }

    public class AutomatedListFieldSetting : Settings
    {
        public AutomatedListFieldSetting()
        {
            ItemSource = new List<MasterListItem>();
            SelectedItems = new List<MasterListItem>();
        }
        public bool IsDynamic { get; set; }
        public MasterListItem SelectedTable { get; set; }
        public MasterListItem SelectedColumn { get; set; }
        public List<MasterListItem> ItemSource { get; set; }
        public List<MasterListItem> SelectedItems { get; set; }
        public MasterListItem SelectedItem { get; set; }
        public int DefaultSelectedItemIndex { get; set; }
        public SelectionMode ChoiceMode { get; set; }
        public AutoListControlType ControlType { get; set; }
    }

    public class InvestigationFieldSetting : Settings
    {
        public InvestigationFieldSetting()
        {
            ItemSource = new List<MasterListItem>();
            SelectedItems = new List<MasterListItem>();
        }
        public MasterListItem SelectedSpecialization { get; set; }
        public List<MasterListItem> ItemSource { get; set; }
        public List<MasterListItem> SelectedItems { get; set; }
        public MasterListItem SelectedItem { get; set; }
        public int DefaultSelectedItemIndex { get; set; }
        public SelectionMode ChoiceMode { get; set; }
        public AutoListControlType ControlType { get; set; }
    }

    public class ListOfCheckBoxesFieldSetting : Settings
    {
        public ListOfCheckBoxesFieldSetting()
        {
            ItemsSource = new List<string>();
            SelectedItems = new List<bool>();
            IsOtherText = false;
        }
        public string ListType { get; set; }
        public List<string> ItemsSource { get; set; }
        public List<bool> SelectedItems { get; set; }
        public bool IsOtherText { get; set; }
        public string OtherText { get; set; }
    }

    public class OtherInvestigationFieldSetting : Settings
    {
        public OtherInvestigationFieldSetting()
        {
            ItemsSource = new List<OtherInvestigation>();
        }
        public List<OtherInvestigation> ItemsSource { get; set; }
    }

    public class DateFieldSetting : Settings
    {
        public DateTime? Date { get; set; }
        public bool IsDefaultDate { get; set; }
        public bool? Mode { get; set; }
        public int? Days { get; set; }
    }
    public class TimeFieldSetting : Settings
    {
        public DateTime? Time { get; set; }
    }
    public class HyperlinkFieldSetting : Settings
    {
        public string Url { get; set; }
        public string ImagePath { get; set; }
    }
    public class FileUploadFieldSetting : Settings
    {
        public FileUploadFieldSetting()
        {
            ItemsSource = new List<FileUpload>();
        }
        public List<FileUpload> ItemsSource { get; set; }
    }
    
    public class DecimalFieldSetting : Settings
    {
        public string Unit { get; set; }
        public decimal? DefaultValue { get; set; }
        public decimal? Value { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
    }
    public class ListFieldSetting : Settings
    {
        public ListFieldSetting()
        {
            ItemSource = new List<DynamicListItem>();
            SelectedItems = new List<DynamicListItem>();
        }
        public List<DynamicListItem> ItemSource { get; set; }
        public List<DynamicListItem> SelectedItems { get; set; }
        public DynamicListItem SelectedItem { get; set; }
        public int DefaultSelectedItemIndex { get; set; }
        public SelectionMode ChoiceMode { get; set; }
        public ListControlType ControlType { get; set; }
    }

    public class DynamicListItem
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }

    public class LookUpFieldSetting : Settings
    {
        public DynamicListItem SelectedSource { get; set; }
        #region added by harish
        public List<DynamicListItem> ItemSource { get; set; }
        public DynamicListItem SelectedItem { get; set; }
        public string AlternateText { get; set; }
        #endregion
        public int DefaultSelectedItemIndex { get; set; }
        public SelectionMode ChoiceMode { get; set; }
        public bool IsAlternateText { get; set; }
    }

    public class BooleanFieldSetting : Settings
    {
        public bool Mode { get; set; }
        public bool Value { get; set; }
        public bool DefaultValue { get; set; }
    }

    public class TextFieldSetting : Settings
    {
        public bool Mode { get; set; }
        public string Value { get; set; }
        public string DefaultText { get; set; }
        public string Unit { get; set; }
    }

    public class MedicationFieldSetting : Settings
    {
        public MedicationFieldSetting()
        {
            ItemsSource = new List<Medication>();
            //MedSetting.ItemsSource.Add(mrci);
        }
        public MasterListItem MedicationDrugType { get; set; } // Theraputic class
        public MasterListItem MoleculeType { get; set; } // Molecule
        public MasterListItem GroupType { get; set; } // Group
        public MasterListItem CategoryType { get; set; } // Category
        public MasterListItem PregnancyClass { get; set; } // Pregnancy class

        public List<Medication> ItemsSource { get; set; }
    }

    

    public class OtherMedicationFieldSetting : Settings
    {
        public OtherMedicationFieldSetting()
        {
            ItemsSource = new List<OtherMedication>();
            //MedSetting.ItemsSource.Add(mrci);
        }
        //public MasterListItem MedicationDrugType { get; set; } // Theraputic class
        //public MasterListItem MoleculeType { get; set; } // Molecule
        //public MasterListItem GroupType { get; set; } // Group
        //public MasterListItem CategoryType { get; set; } // Category
        //public MasterListItem PregnancyClass { get; set; } // Pregnancy class

        public List<OtherMedication> ItemsSource { get; set; }
    }


    [XmlInclude(typeof(BooleanExpression<bool>))]
    [XmlInclude(typeof(DecimalExpression<decimal>))]
    [XmlInclude(typeof(ComboExpression<bool>))]
    [XmlInclude(typeof(CheckListExpression<bool>))]
    public abstract class BaseExpression
    {

    }

    public class BooleanExpression<T> : BaseExpression
    {
        public BooleanOperations Operation { get; set; }
        public T ReferenceValue { get; set; }
    }

    public class DecimalExpression<T> : BaseExpression
    {
        public DoubleOperations Operation { get; set; }
        public T ReferenceValue { get; set; }
    }

    public class ComboExpression<T> : BaseExpression
    {
        public ComboOperations Operation { get; set; }
        public T ReferenceValue { get; set; }
        public String SelectedItem { get; set; }
    }

    public class CheckListExpression<T> : BaseExpression
    {
        public ComboOperations Operation { get; set; }
        public T ReferenceValue { get; set; }
        public MasterListItem SelectedItem { get; set; }
    }


    public enum ListControlType
    {
        ComboBox = 1,
        RadioButton = 2
    }

    public enum AutoListControlType
    {
        ComboBox = 1,
        ListBox = 2,
        CheckListBox = 3
    }

    public enum SelectionMode
    {
        Single = 1,
        Multiples = 2
    }


    public class ConditionExpression<T>
    {
        public BooleanOperations Operation { get; set; }
        public object ReferenceValue { get; set; }
        //public object SelectedItem { get; set; }
    }

    public class Operation<T>
    {
        public T OperationType { get; set; }
        public string Title { get; set; }
    }

    public enum BooleanOperations
    {
        EqualTo = 1,
        NotEqualTo = 2
    }

    public enum DoubleOperations
    {
        EqualTo = 1,
        NotEqualTo = 2,
        LessThan = 3,
        LessThanEqualTo = 4,
        GreterThan = 5,
        GreterThanEqualTo = 6
    }

    public enum ComboOperations
    {
        EqualTo = 1,
        NotEqualTo = 2
    }

    public class Sample
    {
        public DateTime Date { get; set; }
    }
}
