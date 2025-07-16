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
using System.IO;
using DataDrivenApplication;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using CIMS;
using PalashDynamics.ValueObjects.Administration;

namespace DataDrivenApplication
{
    public partial class FieldEditor : ChildWindow
    {
        private bool IsSaved = false;
        private string[] files;
        public FieldEditor(BaseDetail Parent)
        {
            InitializeComponent();
            this.Parent = Parent;
            this.Loaded += new RoutedEventHandler(FieldEditor_Loaded);
        }

        void FieldEditor_Loaded(object sender, RoutedEventArgs e)
        {
            #region Added for Date Mode Combo box
            cmbDateMode.Items.Add("+");
            cmbDateMode.Items.Add("-");
            cmbDateMode.SelectedItem = "+";
            #endregion
            List<FieldType> lst = FieldTypeList.GetFieldTypesList();
            fdType.ItemsSource = lst;
            if ((this.Parent is FieldDetail) && this.Parent != null)
            {
                fdCondition.Visibility = Visibility.Visible;
                fdCondition.DataContext = this.Parent;
                switch (((FieldDetail)this.Parent).DataType.Id)
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
                }
            }
            else
            {
                fdCondition.Visibility = Visibility.Collapsed;
            }

            if (this.DataContext != null)
            {
                tbControlName.Text = string.IsNullOrEmpty(((FieldDetail)this.DataContext).Name) ? "" : ((FieldDetail)this.DataContext).Name;
                tbTitle.Text = string.IsNullOrEmpty(((FieldDetail)this.DataContext).Title) ? "" : ((FieldDetail)this.DataContext).Title;
                var selectedItem = lst.First(f => (f.Id == ((FieldDetail)this.DataContext).DataType.Id));
                fdType.SelectedItem = selectedItem;
                IsRequired.IsChecked = ((FieldDetail)this.DataContext).IsRequired;
                #region Added by Harish
                IsToolTip.IsChecked = ((FieldDetail)this.DataContext).IsToolTip;
                tbToolTip.Text = string.IsNullOrEmpty(((FieldDetail)this.DataContext).ToolTipText) ? "" : ((FieldDetail)this.DataContext).ToolTipText;
                #endregion

                if (((FieldDetail)this.DataContext).DataType.Id == 1)
                {
                    rdbSingleLine.IsChecked = ((TextFieldSetting)((FieldDetail)this.DataContext).Settings).Mode;
                    rdbMultiMultiline.IsChecked = !rdbSingleLine.IsChecked;
                    if ((bool)rdbSingleLine.IsChecked)
                    {
                        tbDefaultText.Text = string.IsNullOrEmpty(((TextFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultText) ? "" : ((TextFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultText;
                        rdbSingleLine_Click(rdbSingleLine, new RoutedEventArgs());
                    }
                    else
                    {
                        tbDefaultMultiText.Text = string.IsNullOrEmpty(((TextFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultText) ? "" : ((TextFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultText;
                        rdbMultiMultiline_Click(rdbMultiMultiline, new RoutedEventArgs());
                    }
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 2)
                {
                    rdbCheckBox.IsChecked = ((BooleanFieldSetting)((FieldDetail)this.DataContext).Settings).Mode;
                    rdbYesNo.IsChecked = !rdbCheckBox.IsChecked;

                    rdbTrue.IsChecked = ((BooleanFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultValue;
                    rdbFalse.IsChecked = !rdbTrue.IsChecked;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 3)
                {
                    if (((DateFieldSetting)((FieldDetail)this.DataContext).Settings).IsDefaultDate == true)
                    {
                        IsDefaultDate.IsChecked = true;
                        IsDefaultDate_Checked(IsDefaultDate, new RoutedEventArgs());
                        cmbDateMode.SelectedItem = ((DateFieldSetting)((FieldDetail)this.DataContext).Settings).Mode == true ? "+" : "-";
                        tbDateDays.Text = ((DateFieldSetting)((FieldDetail)this.DataContext).Settings).Days.ToString();
                    }
                    else
                    {
                        IsDefaultDate.IsChecked = false;
                        IsDefaultDate_Unchecked(IsDefaultDate, new RoutedEventArgs());
                    }
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 4)
                {
                    rdbSingleChoice.IsChecked = (((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ChoiceMode).Equals(SelectionMode.Single) ? true : false;
                    rdbMultiChoice.IsChecked = !rdbSingleChoice.IsChecked;

                    if (rdbMultiChoice.IsChecked == true)
                    {
                        rtbCT.Visibility = Visibility.Collapsed;
                        rdbSingleChoiceSettings.Visibility = Visibility.Collapsed;
                        rdbSingleChoiceSettings2.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        //rdbSC1.IsChecked = (((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType).Equals(ListControlType.RadioButton) ? true : false;
                        rdbSC2.IsChecked = (((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType).Equals(ListControlType.ComboBox) ? true : false;

                        rtbCT.Visibility = Visibility.Visible;
                        rdbSingleChoiceSettings.Visibility = Visibility.Visible;
                        rdbSingleChoiceSettings2.Visibility = Visibility.Visible;
                    }

                    foreach (var item in ((List<DynamicListItem>)((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ItemSource))
                    {
                        fdOptionList.Text += ((DynamicListItem)item).Title + "\r";
                    }

                    listdefaultValue.ItemsSource = ((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ItemSource;

                    if (((ListFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultSelectedItemIndex != -1)
                    listdefaultValue.SelectedItem = ((ListFieldSetting)((FieldDetail)this.DataContext).Settings).ItemSource[((ListFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultSelectedItemIndex];

                    fdOptionList.Focus();
                    listdefaultValue.Focus();
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 5)
                {

                    tbDecUnit.Text = ((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).Unit;

                    if (((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultValue == null)
                        tbDecDefaultValue.Text = "";
                    else
                        tbDecDefaultValue.Text = ((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).DefaultValue.ToString();

                    if (((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).MinValue == null)
                        tbDecMinVal.Text = "";
                    else
                        tbDecMinVal.Text = ((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).MinValue.ToString();

                    if (((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).MaxValue == null)
                        tbDecMaxVal.Text = "";
                    else
                        tbDecMaxVal.Text = ((DecimalFieldSetting)((FieldDetail)this.DataContext).Settings).MaxValue.ToString();


                }
                if (((FieldDetail)this.DataContext).DataType.Id == 6)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GetFileListCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            lstFiles.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args.Result;
                            string s1 = string.IsNullOrEmpty(((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url) ? "" : ((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url;
                            if (s1 != "")
                            {
                                lstFiles.SelectedItem = s1;
                            }
                        }
                        //App.MainWindow.MainRegion.Content = new TemplateList();
                    };
                    client.GetFileListAsync();

                    //tbURL.Text = string.IsNullOrEmpty(((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url) ? "" : ((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url;
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 8)
                {
                    //LookUpFieldSetting listLuStg = new LookUpFieldSetting();
                    //listLuStg.SelectedSource = (DynamicListItem)listSelectedSource.SelectedItem;
                    //listLuStg.ChoiceMode = rdbLuSingleChoice.IsChecked.Value ? SelectionMode.Single : SelectionMode.Multiples;
                    //listLuStg.IsAlternateText = chkAltText.IsChecked.Value;
                    //((FieldDetail)this.DataContext).Settings = listLuStg;


                    List<DynamicListItem> lst1 = Helpers.GetList();
                    listSelectedSource.ItemsSource = lst1;

                    var selectedItem1 = lst1.First(f => (f.Id == ((DynamicListItem)((LookUpFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedSource).Id));
                    listSelectedSource.SelectedItem = selectedItem1;

                    //listSelectedSource.SelectedItem=((DynamicListItem)((LookUpFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedSource);

                    rdbLuSingleChoice.IsChecked = (((LookUpFieldSetting)((FieldDetail)this.DataContext).Settings).ChoiceMode).Equals(SelectionMode.Single) ? true : false;
                    rdbLuMultiChoice.IsChecked = !rdbLuSingleChoice.IsChecked;

                    if (rdbLuMultiChoice.IsChecked == true)
                    {
                        rtbAltText.Visibility = Visibility.Collapsed;
                        chkAltText.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        rtbAltText.Visibility = Visibility.Visible;
                        chkAltText.Visibility = Visibility.Visible;
                        chkAltText.IsChecked = ((LookUpFieldSetting)((FieldDetail)this.DataContext).Settings).IsAlternateText;
                    }
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 9)
                {
                    //listSelectedDrugTypeSource.SelectedItem = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MedicationDrugType;
                    //listSelectedMoleculeTypeSource.SelectedItem = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MoleculeType;
                    //listSelectedGroupTypeSource.SelectedItem = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).GroupType;
                    //listSelectedCategoryTypeSource.SelectedItem = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).CategoryType;
                    //listSelectedPregnancyTypeSource.SelectedItem = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).PregnancyClass;

                    //Mithilesh sir
                    //MedicationFieldSetting medSet = new MedicationFieldSetting();
                    //medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //((FieldDetail)this.DataContext).Settings = medSet;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 12)
                {
                    listOfCheckBoxesTypeSource.SelectedItem = ((ListOfCheckBoxesFieldSetting)((FieldDetail)this.DataContext).Settings).ListType;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 13)
                {
                    if (((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.ComboBox)
                    {
                        rdbCombo.IsChecked = true;
                        rdbCombo_Click(rdbCombo, new RoutedEventArgs());
                        rdbSingle.IsChecked = true;
                        rdbMultiple.IsChecked = false;
                    }
                    else if (((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.ListBox)
                    {
                        rdbList.IsChecked = true;
                        rdbList_Click(rdbList, new RoutedEventArgs());
                        rdbSingle.IsChecked = ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).ChoiceMode == SelectionMode.Single ? true : false;
                        rdbMultiple.IsChecked = !rdbSingle.IsChecked;
                    }
                    else if (((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.CheckListBox)
                    {
                        rdbCombo.IsChecked = true;
                        rdbCombo_Click(rdbCombo, new RoutedEventArgs());
                        rdbMultiple.IsChecked = true;
                        rdbSingle.IsChecked = false;
                    }
                    chkIsDynamic.IsChecked = ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).IsDynamic;
                    //listSelectedTableSource.SelectedItem =((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedTable;                    
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 15)
                {
                    if (((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.ComboBox)
                    {
                        rdbInvestCombo.IsChecked = true;
                        rdbInvestCombo_Click(rdbInvestCombo, new RoutedEventArgs());
                        rdbInvestSingle.IsChecked = true;
                        rdbInvestMultiple.IsChecked = false;
                    }
                    else if (((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.ListBox)
                    {
                        rdbInvestList.IsChecked = true;
                        rdbInvestList_Click(rdbInvestList, new RoutedEventArgs());
                        rdbInvestSingle.IsChecked = ((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).ChoiceMode == SelectionMode.Single ? true : false;
                        rdbInvestMultiple.IsChecked = !rdbSingle.IsChecked;
                    }
                    else if (((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).ControlType == AutoListControlType.CheckListBox)
                    {
                        rdbInvestCombo.IsChecked = true;
                        rdbInvestCombo_Click(rdbInvestCombo, new RoutedEventArgs());
                        rdbInvestMultiple.IsChecked = true;
                        rdbInvestSingle.IsChecked = false;
                    }
                }
                IsSaved = true;
            }
            else
            {
                this.DataContext = new FieldDetail();
                IsSaved = false;
            }
        }

        public BaseDetail Parent { get; set; }

        public event RoutedEventHandler OnSaveButtonClick;

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
            //if (!string.IsNullOrEmpty(tbTitle.Text))
            //{
            //FieldDetail fd = new FieldDetail();
            bool IsValidName = true;
            try
            {
                //tbControlName.Name = tbTitle.Text;
            }
            catch (Exception ex)
            {
                IsValidName = false;
            }

            if (IsValidName == true)
            {
                ((FieldDetail)this.DataContext).Name = tbControlName.Text;
                ((FieldDetail)this.DataContext).Title = tbTitle.Text;
                ((FieldDetail)this.DataContext).DataType = (FieldType)fdType.SelectedItem;
                ((FieldDetail)this.DataContext).IsRequired = IsRequired.IsChecked.Value;
                ((FieldDetail)this.DataContext).IsToolTip = IsToolTip.IsChecked.Value;
                ((FieldDetail)this.DataContext).ToolTipText = tbToolTip.Text;
                if (((FieldDetail)this.DataContext).DataType.Id == 1)
                {
                    if (rdbSingleLine.IsChecked.Value)
                        ((FieldDetail)this.DataContext).Settings = new TextFieldSetting() { Mode = rdbSingleLine.IsChecked.Value, DefaultText = tbDefaultText.Text, Value = tbDefaultText.Text };
                    else
                        ((FieldDetail)this.DataContext).Settings = new TextFieldSetting() { Mode = rdbSingleLine.IsChecked.Value, DefaultText = tbDefaultMultiText.Text, Value = tbDefaultMultiText.Text };
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 2)
                {
                    ((FieldDetail)this.DataContext).Settings = new BooleanFieldSetting() { Mode = rdbCheckBox.IsChecked.Value, DefaultValue = rdbTrue.IsChecked.Value, Value = rdbTrue.IsChecked.Value };
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 3)
                {
                    if ((bool)IsDefaultDate.IsChecked)
                    {
                        ((FieldDetail)this.DataContext).Settings = new DateFieldSetting() { IsDefaultDate = true, Mode = (string)cmbDateMode.SelectedValue == "+" ? true : false, Days = Convert.ToInt32(tbDateDays.Text) };
                    }
                    else
                    {
                        ((FieldDetail)this.DataContext).Settings = new DateFieldSetting() { IsDefaultDate = false };
                    }
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 4)
                {
                    List<DynamicListItem> list = new List<DynamicListItem>();
                    foreach (var item in fdOptionList.Text.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        list.Add(new DynamicListItem() { Title = item });
                    }
                    ListFieldSetting listStg = new ListFieldSetting();
                    listStg.ItemSource = list;
                    listStg.ChoiceMode = rdbSingleChoice.IsChecked.Value ? SelectionMode.Single : SelectionMode.Multiples;
                    //listStg.ControlType = rdbSC1.IsChecked.Value ? ListControlType.RadioButton : ListControlType.ComboBox;
                    listStg.ControlType = ListControlType.ComboBox;
                    listStg.DefaultSelectedItemIndex = listdefaultValue.SelectedIndex;
                    //default value selected
                    if (rdbMultiChoice.IsChecked == false)
                    {
                        if (listStg.DefaultSelectedItemIndex != -1)
                        listStg.SelectedItem = list[listStg.DefaultSelectedItemIndex];
                    }
                    ((FieldDetail)this.DataContext).Settings = listStg;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 5)
                {
                    DecimalFieldSetting setting = new DecimalFieldSetting();
                    setting.Unit = (tbDecUnit.Text);
                    setting.DefaultValue = string.IsNullOrEmpty(tbDecDefaultValue.Text) ? null : (decimal?)decimal.Parse(tbDecDefaultValue.Text);
                    setting.MinValue = string.IsNullOrEmpty(tbDecMinVal.Text) ? null : (decimal?)decimal.Parse(tbDecMinVal.Text);
                    setting.MaxValue = string.IsNullOrEmpty(tbDecMaxVal.Text) ? null : (decimal?)decimal.Parse(tbDecMaxVal.Text);
                    ((FieldDetail)this.DataContext).Settings = setting;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 6)
                {
                    if (lstFiles.SelectedItem != null)
                        ((FieldDetail)this.DataContext).Settings = new HyperlinkFieldSetting() { Url = (string)lstFiles.SelectedItem };
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 8)
                {
                    LookUpFieldSetting listLuStg = new LookUpFieldSetting();
                    listLuStg.SelectedSource = (DynamicListItem)listSelectedSource.SelectedItem;
                    listLuStg.ChoiceMode = rdbLuSingleChoice.IsChecked.Value ? SelectionMode.Single : SelectionMode.Multiples;
                    listLuStg.IsAlternateText = chkAltText.IsChecked.Value;
                    ((FieldDetail)this.DataContext).Settings = listLuStg;
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 9)
                {
                    #region Comment by Harish
                    //MedicationFieldSetting medSet = new MedicationFieldSetting();
                    //medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetDrugList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //((FieldDetail)this.DataContext).Settings = medSet;
                    #endregion

                    #region Added by Harish
                    // Old Version
                    //MedicationFieldSetting medSet = new MedicationFieldSetting();
                    //switch ((string)listSelectedDrugTypeSource.SelectedItem)
                    //{
                    //    case "Antibiotics":
                    //        medSet.MedicationDrugType = "Antibiotics";
                    //        medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntibioticsList(),DaySource=Helpers.GetDayList(),QuantitySource=Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //        break;

                    //    case "Antiemetics":
                    //        medSet.MedicationDrugType = "Antiemetics";
                    //        medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntiemeticsList(), DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //        break;

                    //    case "Antipyretic":
                    //        medSet.MedicationDrugType = "Antipyretic";
                    //        medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntipyreticList(), DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //        break;

                    //    case "Antispasmodic":
                    //        medSet.MedicationDrugType = "Antispasmodic";
                    //        medSet.ItemsSource.Add(new Medication() { DrugSource = Helpers.GetAntispasmodicList(), DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    //        break;
                    //}

                    //((FieldDetail)this.DataContext).Settings = medSet;
                    #endregion

                    List<MasterListItem> lstDrug = new List<MasterListItem>();

                    // Commented by harish becoz it is not dynamically changes drug list after once defined. (Target drug list)
                    //for (int i = 0; i < lstTargetDrug.Items.Count; i++)
                    //{
                    //    lstDrug.Add(new MasterListItem() { ID = ((clsDrugVO)lstTargetDrug.Items[i]).ID, Description = ((clsDrugVO)lstTargetDrug.Items[i]).DrugName});
                    //}

                    // Dynamically changes drug list (Source drug list)
                    for (int i = 0; i < lstSourceDrug.Items.Count; i++)
                    {
                        lstDrug.Add(new MasterListItem() { ID = ((clsDrugVO)lstSourceDrug.Items[i]).ID, Description = ((clsDrugVO)lstSourceDrug.Items[i]).DrugName });
                    }

                    // Sort Drug List                
                    lstDrug = (List<MasterListItem>)(lstDrug.OrderBy(i => i.Description).ToList<MasterListItem>());
                    //IEnumerator<MasterListItem> ob= (IEnumerator<MasterListItem>)(lstDrug.OrderBy(i=>i.Description).GetEnumerator());

                    //lstDrug=null;
                    //lstDrug=new List<MasterListItem>();

                    //while (ob.MoveNext())
                    //{
                    //    lstDrug.Add(ob.Current);
                    //}


                    MedicationFieldSetting medSet = new MedicationFieldSetting();

                    if (listSelectedDrugTypeSource.SelectedItem != null)
                    {
                        medSet.MedicationDrugType = (MasterListItem)listSelectedDrugTypeSource.SelectedItem;
                    }
                    if (listSelectedMoleculeTypeSource.SelectedItem != null)
                    {
                        medSet.MoleculeType = (MasterListItem)listSelectedMoleculeTypeSource.SelectedItem;
                    }
                    if (listSelectedGroupTypeSource.SelectedItem != null)
                    {
                        medSet.GroupType = (MasterListItem)listSelectedGroupTypeSource.SelectedItem;
                    }
                    if (listSelectedCategoryTypeSource.SelectedItem != null)
                    {
                        medSet.CategoryType = (MasterListItem)listSelectedCategoryTypeSource.SelectedItem;
                    }
                    if (listSelectedPregnancyTypeSource.SelectedItem != null)
                    {
                        medSet.PregnancyClass = (MasterListItem)listSelectedPregnancyTypeSource.SelectedItem;
                    }
                    //medSet.ItemsSource.Add(new Medication() { DrugSource = lstDrug, DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    medSet.ItemsSource.Add(new Medication() { DrugSource = lstDrug, DaySource = Helpers.GetDayList(), QuantitySource = Helpers.GetDayList(), DosageSource = Helpers.GetDosageList(), RouteSource = Helpers.GetRouteList(), FrequencySource = Helpers.GetFrequencyList() });
                    ((FieldDetail)this.DataContext).Settings = medSet;
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 11)
                {

                    #region Added by Harish
                    OtherInvestigationFieldSetting InvestSet = new OtherInvestigationFieldSetting();
                    InvestSet.ItemsSource.Add(new OtherInvestigation() { InvestigationSource = Helpers.GetInvestigationList() });
                    ((FieldDetail)this.DataContext).Settings = InvestSet;
                    #endregion
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 12)
                {

                    #region Added by Harish
                    ListOfCheckBoxesFieldSetting ListOfCheck = new ListOfCheckBoxesFieldSetting();

                    if (listOfCheckBoxesTypeSource != null && listOfCheckBoxesTypeSource.SelectedValue == "Nutrition List")
                    {
                        ListOfCheck.ListType = "Nutrition List";
                        ListOfCheck.ItemsSource = Helpers.GetNutritionList();
                    }
                    if (listOfCheckBoxesTypeSource != null && listOfCheckBoxesTypeSource.SelectedValue == "Other Alarms")
                    {
                        ListOfCheck.ListType = "Other Alarms";
                        ListOfCheck.ItemsSource = Helpers.GetOtherAlertsList();
                    }

                    int i = 0;
                    while (i < ListOfCheck.ItemsSource.Count)
                    {
                        ListOfCheck.SelectedItems.Add(false);
                        i++;
                    }
                    ((FieldDetail)this.DataContext).Settings = ListOfCheck;
                    #endregion
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 13)
                {
                    AutomatedListFieldSetting AutoListSet = new AutomatedListFieldSetting();
                    if ((bool)rdbCombo.IsChecked)
                    {
                        AutoListSet.ControlType = AutoListControlType.ComboBox;
                        rdbSingle.IsChecked = true;
                        AutoListSet.ChoiceMode = SelectionMode.Single;
                    }
                    else if ((bool)rdbList.IsChecked)
                    {
                        AutoListSet.ControlType = AutoListControlType.ListBox;
                        AutoListSet.ChoiceMode = (bool)rdbSingle.IsChecked ? SelectionMode.Single : SelectionMode.Multiples;
                    }
                    else if ((bool)rdbCheckList.IsChecked)
                    {
                        AutoListSet.ControlType = AutoListControlType.CheckListBox;
                        rdbMultiple.IsChecked = true;
                        AutoListSet.ChoiceMode = SelectionMode.Multiples;
                    }
                    AutoListSet.IsDynamic = (bool)chkIsDynamic.IsChecked;
                    AutoListSet.SelectedTable = (MasterListItem)listSelectedTableSource.SelectedItem;
                    AutoListSet.SelectedColumn = (MasterListItem)listTableColumnSource.SelectedItem;
                    //(List<MasterListItem>)(lstDrug.OrderBy(i => i.Description).ToList<MasterListItem>());

                    List<MasterListItem> lst = new List<MasterListItem>();
                    for (int i = 0; i < lstTargetAuto.Items.Count; i++)
                    {
                        lst.Add((MasterListItem)lstTargetAuto.Items[i]);
                    }
                    lst = lst.OrderBy(i => i.Description).ToList<MasterListItem>();
                    AutoListSet.ItemSource = lst;
                    ((FieldDetail)this.DataContext).Settings = AutoListSet;
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 14)
                {
                    OtherMedicationFieldSetting medSet = new OtherMedicationFieldSetting();
                    medSet.ItemsSource.Add(new OtherMedication() { RouteSource = Helpers.GetRouteList() });
                    ((FieldDetail)this.DataContext).Settings = medSet;
                }

                if (((FieldDetail)this.DataContext).DataType.Id == 15)
                {
                    InvestigationFieldSetting InvestAutoListSet = new InvestigationFieldSetting();
                    if ((bool)rdbInvestCombo.IsChecked)
                    {
                        InvestAutoListSet.ControlType = AutoListControlType.ComboBox;
                        rdbInvestSingle.IsChecked = true;
                        InvestAutoListSet.ChoiceMode = SelectionMode.Single;
                    }
                    else if ((bool)rdbInvestList.IsChecked)
                    {
                        InvestAutoListSet.ControlType = AutoListControlType.ListBox;
                        InvestAutoListSet.ChoiceMode = (bool)rdbInvestSingle.IsChecked ? SelectionMode.Single : SelectionMode.Multiples;
                    }
                    else if ((bool)rdbInvestCheckList.IsChecked)
                    {
                        InvestAutoListSet.ControlType = AutoListControlType.CheckListBox;
                        rdbInvestMultiple.IsChecked = true;
                        InvestAutoListSet.ChoiceMode = SelectionMode.Multiples;
                    }

                    InvestAutoListSet.SelectedSpecialization = (MasterListItem)listInvestSelectedTableSource.SelectedItem;
                    //(List<MasterListItem>)(lstDrug.OrderBy(i => i.Description).ToList<MasterListItem>());

                    List<MasterListItem> lst = new List<MasterListItem>();
                    for (int i = 0; i < lstInvestTargetAuto.Items.Count; i++)
                    {
                        //lst.Add((MasterListItem)lstInvestTargetAuto.Items[i]);
                        lst.Add(new MasterListItem() { ID = ((clsServiceMasterVO)lstInvestTargetAuto.Items[i]).ID, Description = ((clsServiceMasterVO)lstInvestTargetAuto.Items[i]).ServiceName });//(MasterListItem)lstInvestTargetAuto.Items[i]);
                    }
                    lst = lst.OrderBy(i => i.Description).ToList<MasterListItem>();
                    InvestAutoListSet.ItemSource = lst;
                    ((FieldDetail)this.DataContext).Settings = InvestAutoListSet;
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 16)
                {
                    ((FieldDetail)this.DataContext).Settings = new TimeFieldSetting();
                }
                if (((FieldDetail)this.DataContext).DataType.Id == 17)
                {
                    FileUploadFieldSetting FUSet = new FileUploadFieldSetting();
                    FUSet.ItemsSource.Add(new FileUpload());
                    ((FieldDetail)this.DataContext).Settings = FUSet;
                }
                if (Parent != null)
                {
                    if (Parent is FieldDetail)
                    {
                        if (((FieldDetail)Parent).DataType.Id == 1)
                        {
                            //  fd.Condition = new ConditionExpression<BooleanOperations>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
                        }
                        if (((FieldDetail)Parent).DataType.Id == 2)
                        {
                            ((FieldDetail)this.DataContext).Condition = new BooleanExpression<bool>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = coptYes.IsChecked.Value };
                        }
                        if (((FieldDetail)Parent).DataType.Id == 3)
                        {
                            //fd.Condition = new ConditionExpression<BooleanOperations>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
                        }
                        if (((FieldDetail)Parent).DataType.Id == 4)
                        {
                            //fd.Condition = new ConditionExpression<BooleanOperations>() { Operation = ((Operation<BooleanOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = 1 };
                        }
                        if (((FieldDetail)Parent).DataType.Id == 5)
                        {
                            ((FieldDetail)this.DataContext).Condition = new DecimalExpression<decimal>() { Operation = ((Operation<DoubleOperations>)lbcCondition.SelectedItem).OperationType, ReferenceValue = decimal.Parse(cdecREf.Text) };
                        }
                    }
                    if (Parent is SectionDetail)
                    {
                        ((FieldDetail)this.DataContext).Parent = (SectionDetail)Parent;
                    }


                    //((FieldDetail)Parent).DependentFieldDetail.Add(fd);
                }
                else
                {
                    //Parent.FieldList.Add(fd);
                }
                //fdTitle.Text = "";
                //fdType.SelectedIndex = 0;
                //fdParent.ItemsSource = null;
                //fdParent.ItemsSource = FormView.FieldList;
                //fdParent.SelectedItem = null;
                //trvParent.ItemsSource = null;
                //trvParent.ItemsSource = FormView.FieldList;
                //}

                if (IsSaved == false)
                    ((FieldDetail)this.DataContext).UniqueId = Guid.NewGuid().ToString();

                if (OnSaveButtonClick != null)
                {
                    OnSaveButtonClick(((FieldDetail)this.DataContext), e);
                }
                this.DialogResult = true;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgbx = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Name is not alid.\nPlease enter a valid name", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgbx.Show();
            }
        }

        private void fdType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fdOptions.Visibility = Visibility.Collapsed;
            TextDataTypeSetting.Visibility = Visibility.Collapsed;
            BooleanDataTypeSetting.Visibility = Visibility.Collapsed;
            DateDataTypeSetting.Visibility = Visibility.Collapsed;
            ListDataTypeSetting.Visibility = Visibility.Collapsed;
            DecimalDataTypeSetting.Visibility = Visibility.Collapsed;
            LookupDataTypeSetting.Visibility = Visibility.Collapsed;
            MedicationDataTypeSetting.Visibility = Visibility.Collapsed;
            HyperlinkDataTypeSetting.Visibility = Visibility.Collapsed;
            ListOfCheckBoxesDataTypeSetting.Visibility = Visibility.Collapsed;
            AutomatedListDataTypeSetting.Visibility = Visibility.Collapsed;
            InvestigationListDataTypeSetting.Visibility = Visibility.Collapsed;
            if (fdType.SelectedItem == null || fdType.SelectedIndex == 0)
                return;
            switch (((FieldType)fdType.SelectedItem).Id)
            {
                case 1:
                    fdOptions.Visibility = Visibility.Visible;
                    TextDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 2:
                    fdOptions.Visibility = Visibility.Visible;
                    BooleanDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 3:
                    fdOptions.Visibility = Visibility.Visible;
                    DateDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 4:
                    fdOptions.Visibility = Visibility.Visible;
                    ListDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 5:
                    fdOptions.Visibility = Visibility.Visible;
                    DecimalDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 6:
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GetFileListCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            lstFiles.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args.Result;
                            //string s1 = string.IsNullOrEmpty(((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url) ? "" : ((HyperlinkFieldSetting)((FieldDetail)this.DataContext).Settings).Url;
                            //if (s1 != "")
                            //{
                            //    lstFiles.SelectedItem = s1;
                            //}
                        }
                        //App.MainWindow.MainRegion.Content = new TemplateList();
                    };
                    client.GetFileListAsync();
                    fdOptions.Visibility = Visibility.Visible;
                    HyperlinkDataTypeSetting.Visibility = Visibility.Visible;
                    break;
                case 8:
                    fdOptions.Visibility = Visibility.Visible;
                    LookupDataTypeSetting.Visibility = Visibility.Visible;
                    listSelectedSource.ItemsSource = Helpers.GetList();
                    break;
                case 9:
                    fdOptions.Visibility = Visibility.Visible;
                    MedicationDataTypeSetting.Visibility = Visibility.Visible;
                    //listSelectedDrugTypeSource.ItemsSource = Helpers.GetDrugTypeList();
                    try
                    {
                        #region Get TherapeuticClass List

                        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                        BizAction.MasterTable = MasterTableNameList.M_TherapeuticClass;
                        BizAction.Parent = new KeyValue();
                        BizAction.Parent.Key = "1";
                        BizAction.Parent.Value = "Status";

                        BizAction.MasterList = new List<MasterListItem>();
                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client1.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                listSelectedDrugTypeSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is MedicationFieldSetting && ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MedicationDrugType != null)
                                {
                                    listSelectedDrugTypeSource.SelectedValue = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MedicationDrugType.ID;
                                }
                                else
                                {
                                    listSelectedDrugTypeSource.SelectedValue = 0L;
                                }
                                grdDrugList.Visibility = Visibility.Collapsed;
                                lstSourceDrug.ItemsSource = null;
                                lstTargetDrug.ItemsSource = null;
                            }
                        };
                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client1.CloseAsync();

                        #endregion

                        #region Get Molecule List

                        clsGetMasterListBizActionVO BizActionMolecule = new clsGetMasterListBizActionVO();
                        BizActionMolecule.MasterTable = MasterTableNameList.M_Molecule;
                        BizActionMolecule.Parent = new KeyValue();
                        BizActionMolecule.Parent.Key = "1";
                        BizActionMolecule.Parent.Value = "Status";

                        BizActionMolecule.MasterList = new List<MasterListItem>();
                        //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client2 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client2.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                listSelectedMoleculeTypeSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is MedicationFieldSetting && ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MoleculeType != null)
                                {
                                    listSelectedMoleculeTypeSource.SelectedValue = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).MoleculeType.ID;
                                }
                                else
                                {
                                    listSelectedMoleculeTypeSource.SelectedValue = 0L;
                                }
                                grdDrugList.Visibility = Visibility.Collapsed;
                                lstSourceDrug.ItemsSource = null;
                                lstTargetDrug.ItemsSource = null;
                            }
                        };
                        client2.ProcessAsync(BizActionMolecule, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client2.CloseAsync();

                        #endregion

                        #region Get Group List

                        clsGetMasterListBizActionVO BizActionGroup = new clsGetMasterListBizActionVO();
                        BizActionGroup.MasterTable = MasterTableNameList.M_ItemGroup;
                        BizActionGroup.Parent = new KeyValue();
                        BizActionGroup.Parent.Key = "1";
                        BizActionGroup.Parent.Value = "Status";

                        BizActionGroup.MasterList = new List<MasterListItem>();
                        //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client3 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client3.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                listSelectedGroupTypeSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is MedicationFieldSetting && ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).GroupType != null)
                                {
                                    listSelectedGroupTypeSource.SelectedValue = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).GroupType.ID;
                                }
                                else
                                {
                                    listSelectedGroupTypeSource.SelectedValue = 0L;
                                }
                                grdDrugList.Visibility = Visibility.Collapsed;
                                lstSourceDrug.ItemsSource = null;
                                lstTargetDrug.ItemsSource = null;
                            }
                        };
                        client3.ProcessAsync(BizActionGroup, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client3.CloseAsync();

                        #endregion

                        #region Get Category List

                        clsGetMasterListBizActionVO BizActionCategory = new clsGetMasterListBizActionVO();
                        BizActionCategory.MasterTable = MasterTableNameList.M_ItemCategory;
                        BizActionCategory.Parent = new KeyValue();
                        BizActionCategory.Parent.Key = "1";
                        BizActionCategory.Parent.Value = "Status";

                        BizActionCategory.MasterList = new List<MasterListItem>();
                        //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client4 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client4.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                //listSelectedDrugTypeSource.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                listSelectedCategoryTypeSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is MedicationFieldSetting && ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).CategoryType != null)
                                {
                                    listSelectedCategoryTypeSource.SelectedValue = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).CategoryType.ID;
                                }
                                else
                                {
                                    listSelectedCategoryTypeSource.SelectedValue = 0L;
                                }
                                grdDrugList.Visibility = Visibility.Collapsed;
                                lstSourceDrug.ItemsSource = null;
                                lstTargetDrug.ItemsSource = null;
                            }
                        };
                        client4.ProcessAsync(BizActionCategory, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client4.CloseAsync();

                        #endregion

                        #region Get Pregnancy Class List

                        clsGetMasterListBizActionVO BizActionPregnancy = new clsGetMasterListBizActionVO();
                        BizActionPregnancy.MasterTable = MasterTableNameList.M_PreganancyClass;
                        BizActionPregnancy.Parent = new KeyValue();
                        BizActionPregnancy.Parent.Key = "1";
                        BizActionPregnancy.Parent.Value = "Status";

                        BizActionPregnancy.MasterList = new List<MasterListItem>();
                        //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client5 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client5.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                listSelectedPregnancyTypeSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is MedicationFieldSetting && ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).PregnancyClass != null)
                                {
                                    listSelectedPregnancyTypeSource.SelectedValue = ((MedicationFieldSetting)((FieldDetail)this.DataContext).Settings).PregnancyClass.ID;
                                }
                                else
                                {
                                    listSelectedPregnancyTypeSource.SelectedValue = 0L;
                                }
                                grdDrugList.Visibility = Visibility.Collapsed;
                                lstSourceDrug.ItemsSource = null;
                                lstTargetDrug.ItemsSource = null;
                            }
                        };
                        client5.ProcessAsync(BizActionPregnancy, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client5.CloseAsync();

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //fdOptions.Visibility = Visibility.Visible;
                    //MedicationDataTypeSetting.Visibility = Visibility.Collapsed;
                    break;
                case 12:
                    fdOptions.Visibility = Visibility.Visible;
                    ListOfCheckBoxesDataTypeSetting.Visibility = Visibility.Visible;
                    listOfCheckBoxesTypeSource.ItemsSource = Helpers.GetListOfCheckBoxesTypeList();
                    break;
                case 13:
                    fdOptions.Visibility = Visibility.Visible;
                    AutomatedListDataTypeSetting.Visibility = Visibility.Visible;
                    try
                    {
                        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                        BizAction.MasterTable = MasterTableNameList.M_EMRTemplateConfigurationMaster;
                        BizAction.Parent = new KeyValue();
                        BizAction.Parent.Key = "1";
                        BizAction.Parent.Value = "Status";

                        BizAction.MasterList = new List<MasterListItem>();
                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client1.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                lstSourceAuto.ItemsSource = null;
                                lstTargetAuto.ItemsSource = null;
                                listSelectedTableSource.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is AutomatedListFieldSetting && ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedTable != null)
                                {
                                    listSelectedTableSource.SelectedValue = ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedTable.ID;
                                }
                                grdAutoList.Visibility = Visibility.Collapsed;
                            }
                        };
                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client1.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
                case 15:
                    fdOptions.Visibility = Visibility.Visible;
                    InvestigationListDataTypeSetting.Visibility = Visibility.Visible;
                    try
                    {
                        clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                        BizAction.MasterTable = MasterTableNameList.M_Specialization;
                        BizAction.Parent = new KeyValue();
                        BizAction.Parent.Key = "1";
                        BizAction.Parent.Value = "Status";

                        BizAction.MasterList = new List<MasterListItem>();
                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        client1.ProcessCompleted += (s, args) =>
                        {
                            if (args.Error == null && args.Result != null)
                            {
                                lstInvestSourceAuto.ItemsSource = null;
                                lstInvestTargetAuto.ItemsSource = null;
                                List<MasterListItem> list = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                list.Insert(0, new MasterListItem() { ID = 0, Description = "All" });
                                //listInvestSelectedTableSource.ItemsSource = ((clsGetMasterListBizActionVO)args.Result).MasterList;
                                listInvestSelectedTableSource.ItemsSource = list;
                                if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is InvestigationFieldSetting && ((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedSpecialization != null)
                                {
                                    listInvestSelectedTableSource.SelectedValue = ((InvestigationFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedSpecialization.ID;
                                }
                                grdInvestAutoList.Visibility = Visibility.Collapsed;
                            }
                        };
                        client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client1.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
            }
        }

        private void rdbSC1_Click(object sender, RoutedEventArgs e)
        {
            rtbCT.Visibility = Visibility.Visible;
            rdbSingleChoiceSettings.Visibility = Visibility.Visible;
            rdbSingleChoiceSettings2.Visibility = Visibility.Visible;
        }

        private void rdbSC2_Click(object sender, RoutedEventArgs e)
        {
            rtbCT.Visibility = Visibility.Collapsed;
            rdbSingleChoiceSettings.Visibility = Visibility.Collapsed;
            rdbSingleChoiceSettings2.Visibility = Visibility.Collapsed;
        }

        private void fdOptionList_LostFocus(object sender, RoutedEventArgs e)
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            foreach (var item in fdOptionList.Text.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries))
            {
                list.Add(new DynamicListItem() { Title = item });
            }
            listdefaultValue.ItemsSource = list;
        }

        private void clrDefListVal_Click(object sender, RoutedEventArgs e)
        {
            listdefaultValue.SelectedItem = null;
        }

        private void clrDefLuListVal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdbLuSingleChoice_Click(object sender, RoutedEventArgs e)
        {
            rtbAltText.Visibility = Visibility.Visible;
            chkAltText.Visibility = Visibility.Visible;
        }

        private void rdbLuMultiChoice_Click(object sender, RoutedEventArgs e)
        {
            rtbAltText.Visibility = Visibility.Collapsed;
            chkAltText.Visibility = Visibility.Collapsed;
        }

        private void IsToolTip_Checked(object sender, RoutedEventArgs e)
        {
            checked
            {
                txtBlockToolTip.Visibility = Visibility.Visible;
                tbToolTip.Visibility = Visibility.Visible;
            }
        }

        private void IsToolTip_Unchecked(object sender, RoutedEventArgs e)
        {
            unchecked
            {
                txtBlockToolTip.Visibility = Visibility.Collapsed;
                tbToolTip.Visibility = Visibility.Collapsed;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    bool exist = lstFiles.Items.Any(i => i.ToString() == openDialog.File.Name);
                    bool flag = true;
                    if (exist == true)
                    {
                        if (MessageBox.Show("File is already exist. Do you want to overwrite it?", "File Alreadt Exist", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            flag = true;
                        else
                            flag = false;
                    }
                    if (flag == true)
                    {
                        using (Stream stream = openDialog.File.OpenRead())
                        {
                            // Don't allow really big files (more than 5 MB).
                            if (stream.Length < 5120000)
                            {
                                byte[] data = new byte[stream.Length];
                                stream.Read(data, 0, (int)stream.Length);

                                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                                client.UploadFileCompleted += (s, args) =>
                                {
                                    if (args.Error == null)
                                    {
                                        //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                                        DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("CustomBinding_DataTemplateService", address.AbsoluteUri);
                                        client1.GetFileListCompleted += (s1, args1) =>
                                        {
                                            if (args1.Error == null)
                                            {
                                                lstFiles.ItemsSource = (System.Collections.ObjectModel.ObservableCollection<string>)args1.Result;
                                            }
                                        };

                                        client1.GetFileListAsync();
                                    }
                                };

                                client.UploadFileAsync(openDialog.File.Name, data);
                            }
                            else
                            {
                                MessageBox.Show("File must be less than 5 MB");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while reading file.");
                }

            }
        }

        private void listSelectedDrugTypeSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstSourceDrug.ItemsSource = null;
            while (lstTargetDrug.Items.Count != 0)
            {
                lstTargetDrug.Items.RemoveAt(0);
            }

            long TherID, MoleculeID, GroupID, CategoryID, PregID;
            TherID = MoleculeID = GroupID = CategoryID = PregID = 0;

            if (listSelectedDrugTypeSource.SelectedItem != null && ((MasterListItem)listSelectedDrugTypeSource.SelectedItem).ID != 0)
            {
                TherID = ((MasterListItem)listSelectedDrugTypeSource.SelectedItem).ID;
            }
            if (listSelectedMoleculeTypeSource.SelectedItem != null && ((MasterListItem)listSelectedMoleculeTypeSource.SelectedItem).ID != 0)
            {
                MoleculeID = ((MasterListItem)listSelectedMoleculeTypeSource.SelectedItem).ID;
            }
            if (listSelectedGroupTypeSource.SelectedItem != null && ((MasterListItem)listSelectedGroupTypeSource.SelectedItem).ID != 0)
            {
                GroupID = ((MasterListItem)listSelectedGroupTypeSource.SelectedItem).ID;
            }
            if (listSelectedCategoryTypeSource.SelectedItem != null && ((MasterListItem)listSelectedCategoryTypeSource.SelectedItem).ID != 0)
            {
                CategoryID = ((MasterListItem)listSelectedCategoryTypeSource.SelectedItem).ID;
            }
            if (listSelectedPregnancyTypeSource.SelectedItem != null && ((MasterListItem)listSelectedPregnancyTypeSource.SelectedItem).ID != 0)
            {
                PregID = ((MasterListItem)listSelectedPregnancyTypeSource.SelectedItem).ID;
            }


            clsGetDrugListBizActionVO BizAction = new clsGetDrugListBizActionVO();
            BizAction.TheraputicID = TherID;
            BizAction.MoleculeID = MoleculeID;
            BizAction.GroupID = GroupID;
            BizAction.CategoryID = CategoryID;
            BizAction.PregnancyID = PregID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    lstSourceDrug.ItemsSource = ((clsGetDrugListBizActionVO)args.Result).objDrugList;
                    grdDrugList.Visibility = Visibility.Visible;

                    // This code section is Added to invisible List boxes which are earlier used to move drugs to target list
                    lstTargetDrug.Visibility = Visibility.Collapsed;
                    btnMove.Visibility = Visibility.Collapsed;
                    btnMoveAll.Visibility = Visibility.Collapsed;
                    btnRemove.Visibility = Visibility.Collapsed;
                    btnRemoveAll.Visibility = Visibility.Collapsed;
                    lblTargetDrug.Visibility = Visibility.Collapsed;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            if (lstSourceDrug.SelectedItems.Count != 0)
            {
                IEnumerator<object> list = ((IEnumerator<object>)lstSourceDrug.SelectedItems.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstTargetDrug.Items.Contains((clsDrugVO)list.Current))
                    {
                        lstTargetDrug.Items.Add((clsDrugVO)list.Current);


                        //List<ListItem> list = new List<ListItem>(ListBoxToSort.Items.Cast<ListItem>());
                        //// use LINQ to Objects to order the items as required 
                        //list = list.OrderBy(li => li.Text).ToList<ListItem>(); 
                        //// remove the unordered items from the listbox, so we don't get duplicates 
                        //ListBoxToSort.Items.Clear(); 
                        //// now add back our sorted items 
                        //ListBoxToSort.Items.AddRange(list.ToArray<ListItem>()); 
                    }
                }
            }
        }

        private void btnMoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (lstSourceDrug.ItemsSource != null)
            {
                IEnumerator<clsDrugVO> list = ((IEnumerator<clsDrugVO>)lstSourceDrug.ItemsSource.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstTargetDrug.Items.Contains(list.Current))
                    {
                        lstTargetDrug.Items.Add(list.Current);
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator<object> list = (IEnumerator<object>)lstTargetDrug.SelectedItems.GetEnumerator();

            while (list.MoveNext())
            {
                lstTargetDrug.Items.Remove((clsDrugVO)list.Current);
            }
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            while (lstTargetDrug.Items.Count != 0)
            {
                lstTargetDrug.Items.RemoveAt(0);
            }
        }

        private void rdbCombo_Click(object sender, RoutedEventArgs e)
        {
            rdbListSettings.Visibility = Visibility.Collapsed;
        }

        private void rdbList_Click(object sender, RoutedEventArgs e)
        {
            rdbListSettings.Visibility = Visibility.Visible;
        }

        private void rdbCheckList_Click(object sender, RoutedEventArgs e)
        {
            rdbListSettings.Visibility = Visibility.Collapsed;
        }

        private void listSelectedTableSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listSelectedTableSource.SelectedItem != null)
                {
                    lstSourceAuto.ItemsSource = null;
                    listTableColumnSource.ItemsSource = null;
                    if (lstTargetAuto.Items.Count != 0)
                    {
                        lstTargetAuto.Items.Clear();
                    }

                    clsGetColumnListByTableNameBizActionVO BizAction = new clsGetColumnListByTableNameBizActionVO();
                    BizAction.MasterTable = ((MasterListItem)listSelectedTableSource.SelectedItem).Description;
                    
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {                            
                            listTableColumnSource.ItemsSource = ((clsGetColumnListByTableNameBizActionVO)args.Result).ColumnList;
                            if (this.DataContext != null && ((FieldDetail)this.DataContext).Settings is AutomatedListFieldSetting && ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedColumn != null)
                            {
                                listTableColumnSource.SelectedValue = ((AutomatedListFieldSetting)((FieldDetail)this.DataContext).Settings).SelectedColumn.ID;
                            }
                            grdAutoList.Visibility = Visibility.Collapsed;
                        }
                    };
                    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAutoMove_Click(object sender, RoutedEventArgs e)
        {
            if (lstSourceAuto.SelectedItems.Count != 0)
            {
                IEnumerator<object> list = ((IEnumerator<object>)lstSourceAuto.SelectedItems.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstTargetAuto.Items.Contains((MasterListItem)list.Current))
                    {
                        lstTargetAuto.Items.Add((MasterListItem)list.Current);
                    }
                }
            }
        }

        private void btnAutoMoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (lstSourceAuto.ItemsSource != null)
            {
                IEnumerator<MasterListItem> list = ((IEnumerator<MasterListItem>)lstSourceAuto.ItemsSource.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstTargetAuto.Items.Contains(list.Current))
                    {
                        lstTargetAuto.Items.Add(list.Current);
                    }
                }
            }
        }

        private void btnAutoRemove_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator<object> list = (IEnumerator<object>)lstTargetAuto.SelectedItems.GetEnumerator();

            while (list.MoveNext())
            {
                lstTargetAuto.Items.Remove((MasterListItem)list.Current);
            }
        }

        private void btnAutoRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (lstTargetAuto.Items.Count != 0)
            {
                lstTargetAuto.Items.Clear();
            }
        }

        private void rdbSingleLine_Click(object sender, RoutedEventArgs e)
        {
            tbDefaultText.Visibility = Visibility.Visible;
            tbDefaultMultiText.Visibility = Visibility.Collapsed;
        }

        private void rdbMultiMultiline_Click(object sender, RoutedEventArgs e)
        {
            tbDefaultText.Visibility = Visibility.Collapsed;
            tbDefaultMultiText.Visibility = Visibility.Visible;
        }

        private void IsDefaultDate_Checked(object sender, RoutedEventArgs e)
        {
            checked
            {
                optDateSettings.Visibility = Visibility.Visible;
            }
        }

        private void IsDefaultDate_Unchecked(object sender, RoutedEventArgs e)
        {
            unchecked
            {
                optDateSettings.Visibility = Visibility.Collapsed;
            }
        }


        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void tbDateDays_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void tbDateDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != "" && !((TextBox)sender).Text.IsNumberValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
            else if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "0";
        }

        private void rdbInvestCombo_Click(object sender, RoutedEventArgs e)
        {
            rdbInvestListSettings.Visibility = Visibility.Collapsed;
        }

        private void rdbInvestList_Click(object sender, RoutedEventArgs e)
        {
            rdbInvestListSettings.Visibility = Visibility.Visible;
        }

        private void rdbInvestCheckList_Click(object sender, RoutedEventArgs e)
        {
            rdbInvestListSettings.Visibility = Visibility.Collapsed;
        }

        private void listInvestSelectedTableSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listInvestSelectedTableSource.SelectedItem != null)
                {
                    lstInvestSourceAuto.ItemsSource = null;
                    if (lstInvestTargetAuto.Items.Count != 0)
                    {
                        lstInvestTargetAuto.Items.Clear();
                    }

                    clsGetServiceBySpecializationBizActionVO BizAction = new clsGetServiceBySpecializationBizActionVO();
                    BizAction.ServiceList = new List<clsServiceMasterVO>();
                    BizAction.ServiceMaster = new clsServiceMasterVO();
                    BizAction.ServiceMaster.Specialization = ((MasterListItem)listInvestSelectedTableSource.SelectedItem).ID;
                    BizAction.ServiceMaster.Description = "";

                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            List<clsServiceMasterVO> lst = new List<clsServiceMasterVO>();
                            lst = ((clsGetServiceBySpecializationBizActionVO)args.Result).ServiceList;
                            //lstInvestSourceAuto.ItemsSource = ((clsGetServiceBySpecializationBizActionVO)args.Result).ServiceList;
                            lst = lst.OrderBy(i => i.ServiceName).ToList<clsServiceMasterVO>();
                            lstInvestSourceAuto.ItemsSource = lst;
                            grdInvestAutoList.Visibility = Visibility.Visible;

                        }
                    };
                    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnInvestAutoMove_Click(object sender, RoutedEventArgs e)
        {
            if (lstInvestSourceAuto.SelectedItems.Count != 0)
            {
                IEnumerator<object> list = ((IEnumerator<object>)lstInvestSourceAuto.SelectedItems.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstInvestTargetAuto.Items.Contains((clsServiceMasterVO)list.Current))
                    {
                        lstInvestTargetAuto.Items.Add((clsServiceMasterVO)list.Current);

                    }
                }
            }
        }

        private void btnInvestAutoMoveAll_Click(object sender, RoutedEventArgs e)
        {
            if (lstInvestSourceAuto.ItemsSource != null)
            {
                IEnumerator<clsServiceMasterVO> list = ((IEnumerator<clsServiceMasterVO>)lstInvestSourceAuto.ItemsSource.GetEnumerator());

                while (list.MoveNext())
                {
                    if (!lstInvestTargetAuto.Items.Contains(list.Current))
                    {
                        lstInvestTargetAuto.Items.Add(list.Current);
                    }
                }
            }
        }

        private void btnInvestAutoRemove_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator<object> list = (IEnumerator<object>)lstInvestTargetAuto.SelectedItems.GetEnumerator();

            while (list.MoveNext())
            {
                lstInvestTargetAuto.Items.Remove((clsServiceMasterVO)list.Current);
            }
        }

        private void btnInvestAutoRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            while (lstInvestTargetAuto.Items.Count != 0)
            {
                lstInvestTargetAuto.Items.RemoveAt(0);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void listTableColumnSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listTableColumnSource.SelectedItem != null)
                {
                    clsGetMasterListByTableNameAndColumnNameBizActionVO BizAction = new clsGetMasterListByTableNameAndColumnNameBizActionVO();
                    BizAction.MasterTable = ((MasterListItem)listSelectedTableSource.SelectedItem).Description;
                    BizAction.ColumnName = ((MasterListItem)listTableColumnSource.SelectedItem).Description;

                    BizAction.MasterList = new List<MasterListItem>();
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            lstSourceAuto.ItemsSource = ((clsGetMasterListByTableNameAndColumnNameBizActionVO)args.Result).MasterList;
                            grdAutoList.Visibility = Visibility.Visible;
                        }
                    };
                    client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

