using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using PalashDynamics.Administration;
using System.Xml.Serialization;
using PalashDynamics.ValueObjects;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
//using PalashDynamics.Administration.Template;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace PalashDynamics.IVF.DashBoard
{
    public abstract class BaseDetail
    {
        public abstract string Title { get; set; }
        public abstract string Name { get; set; }
    }
    public class FieldType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
    }
    public class ClassForEMRTemplate
    {

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
    public class FormDetail : BaseDetail
    {
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
    }
    public partial class IntraTemplateRelation
    {
        public BaseExpression ExpCondition { get; set; }
        public string RelationTitle { get; set; }
        public string Description { get; set; }
        [XmlIgnore]
        public FieldDetail SourceField { get; set; }
        [XmlIgnore]
        public FieldDetail TargetField { get; set; }
        [XmlIgnore]
        public string SourceSection { get; set; }
        //public string SourceField { get; set; }
        [XmlIgnore]
        public string TargetSection { get; set; }
        //public string TargetField { get; set; }





        private string ConditionField;

        private long RelationIdField;

        private string SourceFieldIdField;

        private string SourceSectionIdField;

        private string TargetFieldIdField;

        private string TargetSectionIdField;

        private long TemplateIdField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Condition
        {
            get
            {
                return this.ConditionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ConditionField, value) != true))
                {
                    this.ConditionField = value;
                    this.RaisePropertyChanged("Condition");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long RelationId
        {
            get
            {
                return this.RelationIdField;
            }
            set
            {
                if ((this.RelationIdField.Equals(value) != true))
                {
                    this.RelationIdField = value;
                    this.RaisePropertyChanged("RelationId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourceFieldId
        {
            get
            {
                return this.SourceFieldIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SourceFieldIdField, value) != true))
                {
                    this.SourceFieldIdField = value;
                    this.RaisePropertyChanged("SourceFieldId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourceSectionId
        {
            get
            {
                return this.SourceSectionIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.SourceSectionIdField, value) != true))
                {
                    this.SourceSectionIdField = value;
                    this.RaisePropertyChanged("SourceSectionId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TargetFieldId
        {
            get
            {
                return this.TargetFieldIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TargetFieldIdField, value) != true))
                {
                    this.TargetFieldIdField = value;
                    this.RaisePropertyChanged("TargetFieldId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TargetSectionId
        {
            get
            {
                return this.TargetSectionIdField;
            }
            set
            {
                if ((object.ReferenceEquals(this.TargetSectionIdField, value) != true))
                {
                    this.TargetSectionIdField = value;
                    this.RaisePropertyChanged("TargetSectionId");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long TemplateId
        {
            get
            {
                return this.TemplateIdField;
            }
            set
            {
                if ((this.TemplateIdField.Equals(value) != true))
                {
                    this.TemplateIdField = value;
                    this.RaisePropertyChanged("TemplateId");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class OtherInvestigation : INotifyPropertyChanged
    {
        public OtherInvestigation()
        {
            Investigation = "--Select--";
            Remarks = "";
        }
        public int Index { get; set; }

        public string Investigation { get; set; }

        public string Remarks { get; set; }

        public List<String> InvestigationSource { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public OtherInvestigationFieldSetting InvestigationSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class Medication : INotifyPropertyChanged
    {
        public Medication()
        {
        }
        public int Index { get; set; }
        //public string DrugType { get; set; }
        public MasterListItem Drug { get; set; }
        public int Day { get; set; }
        public int Quantity { get; set; }
        public string Dose { get; set; }
        public MasterListItem Route { get; set; }
        public int Frequency { get; set; }


        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<String> DrugTypeSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<MasterListItem> DrugSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<MasterListItem> DaySource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<MasterListItem> QuantitySource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<String> DosageSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<MasterListItem> RouteSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<string> FrequencySource { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public MedicationFieldSetting MedicationSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class FileUpload : INotifyPropertyChanged
    {
        public FileUpload()
        {
        }
        public int Index { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public Byte[] Data { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public FileInfo FileInfo { get; set; }

        public string FileName { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public FileUploadFieldSetting FileUploadSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public partial class PatientCaseRecordRelation
    {
        //public BaseExpression ExpCondition { get; set; }
        //public string RelationTitle { get; set; }
        public string Description { get; set; }

        public MasterListItem SourceField { get; set; } // Replace FieldDetail with appropriate Class                
        [XmlIgnore]
        public FieldDetail TargetField { get; set; }
        public string SourceSection { get; set; }
        [XmlIgnore]
        public string TargetSection { get; set; }
        public string TargetSectionId { get; set; }
        public string TargetFieldId { get; set; }
    }
    public partial class PatientCaseReferralRelation
    {
        //public BaseExpression ExpCondition { get; set; }
        //public string RelationTitle { get; set; }
        public string Description { get; set; }

        public MasterListItem SourceField { get; set; } // Replace FieldDetail with appropriate Class                
        [XmlIgnore]
        public FieldDetail TargetField { get; set; }
        public string SourceSection { get; set; }
        [XmlIgnore]
        public string TargetSection { get; set; }
        public string TargetSectionId { get; set; }
        public string TargetFieldId { get; set; }
    }
    public class BoolToYesNoConverter : IValueConverter
    {
        public object Convert(object value,
                           Type targetType,
                           object parameter,
                           CultureInfo culture)
        {
            try
            {
                bool? val = (bool?)value;
                if (val.HasValue)
                {
                    if (val.Value == bool.Parse(parameter.ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return val;
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return null;
        }
        public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            try
            {
                bool? val = (bool?)value;
                if (val.HasValue)
                {
                    if (val.Value == bool.Parse(parameter.ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return val;
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            return null;
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
    [XmlInclude(typeof(BPControlSetting))]
    [XmlInclude(typeof(VisionControlSetting))]
    [XmlInclude(typeof(GlassPowerControlSetting))]
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
    //Added by Saily P on 29.11.13 Purpose - three new controls - 
    public class BPControlSetting : Settings
    {
        public BPControlSetting()
        {
            ItemsSource = new List<BPControl>();
        }

        public List<BPControl> ItemsSource { get; set; }
        //public string lblSlash { get; set; }
        //public decimal? txtText1 { get; set; }
        //public decimal? txtText2 { get; set; }          
    }
    public class VisionControlSetting : Settings
    {
        public VisionControlSetting()
        {
            ItemsSource = new List<VisionControl>();
        }

        public List<VisionControl> ItemsSource { get; set; }
    }
    public class GlassPowerControlSetting : Settings
    {
        public GlassPowerControlSetting()
        {
            ItemsSource = new List<GlassPower>();
        }

        public List<GlassPower> ItemsSource { get; set; }
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
    // added by Saily P on 02.12.13 Purpose new controls
    public class BPControl : INotifyPropertyChanged
    {
        public BPControl()
        { }
        public int? BP1 { get; set; }
        public int? BP2 { get; set; }
        public int Index { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        //public BPControl Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public BPControlSetting BPSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class VisionControl : INotifyPropertyChanged
    {
        public VisionControl()
        { }

        public decimal? SPH { get; set; }
        public decimal? CYL { get; set; }
        public decimal? Axis { get; set; }
        public int Index { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public VisionControlSetting VisionSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class GlassPower : INotifyPropertyChanged
    {
        public GlassPower()
        { }

        public decimal? SPH1 { get; set; }
        public decimal? CYL1 { get; set; }
        public decimal? Axis1 { get; set; }
        public decimal? VA1 { get; set; }
        public decimal? SPH2 { get; set; }
        public decimal? CYL2 { get; set; }
        public decimal? Axis2 { get; set; }
        public decimal? VA2 { get; set; }
        public int Index { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public GlassPowerControlSetting GlassPowerSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class OtherMedication : INotifyPropertyChanged
    {
        public OtherMedication()
        {
        }
        public int Index { get; set; }
        //public string DrugType { get; set; }
        public string Drug { get; set; }
        public int Day { get; set; }
        public int Quantity { get; set; }
        public string Dose { get; set; }
        public MasterListItem Route { get; set; }
        public int Frequency { get; set; }
        public string Reason { get; set; }

        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<String> DrugTypeSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<MasterListItem> DrugSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<MasterListItem> DaySource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<MasterListItem> QuantitySource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<String> DosageSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        public List<MasterListItem> RouteSource { get; set; }
        //[XmlIgnore]
        //[IgnoreDataMember] 
        //public List<string> FrequencySource { get; set; }

        private string _Command;
        public string Command
        {
            get
            {
                return _Command;
            }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }

        [XmlIgnore]
        [IgnoreDataMember]
        public ListBox Parent { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public OtherMedicationFieldSetting MedicationSetting { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }

        #endregion
    }
    public class kekvalueItem
    {
        public int Id { get; set; }
        public string Value { get; set; }
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
    public class InvestigationRepetorControlItem : ListBoxItem
    {
        public InvestigationRepetorControlItem()
        {
            this.DefaultStyleKey = typeof(InvestigationRepetorControlItem);
        }

        private HyperlinkButton AddRemoveClick;

        private ComboBox cmbInvestigation;
        private TextBox TxtRemarks;

        public event RoutedEventHandler OnAddRemoveClick;

        public event RoutedEventHandler cmbSelectionChanged;
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
            this.TxtRemarks.TextChanged += new TextChangedEventHandler(TxtRemarks_TextChanged);
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
    public static class Helpers
    {
        //public static T DeepCopy<T>(this T oSource)
        //{

        //    T oClone;
        //    XmlSerializer dcs = new XmlSerializer(typeof(T));
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        dcs.Serialize(ms, oSource);
        //        ms.Position = 0;
        //        oClone = (T)dcs.Deserialize(ms);
        //    }
        //    return oClone;
        //}

        public static string XmlSerilze<T>(this T data)
        {
            System.Xml.Serialization.XmlSerializer MyXMLWriter = new System.Xml.Serialization.XmlSerializer(data.GetType());
            StringWriter MyStringWriter = new StringWriter();
            MyXMLWriter.Serialize(MyStringWriter, data);
            return MyStringWriter.ToString();

        }

        public static T XmlDeserialize<T>(this string xml)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                var serializer = new XmlSerializer(typeof(T));

                T theObject = (T)serializer.Deserialize(stream);

                return theObject;
            }
        }

        public static string Serialize<T>(this T data)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memoryStream, data);

                memoryStream.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(memoryStream);
                string content = reader.ReadToEnd();
                return content;
            }
        }

        public static T Deserialize<T>(this string xml)
        {
            using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                var serializer = new DataContractSerializer(typeof(T));
                T theObject = (T)serializer.ReadObject(stream);
                return theObject;
            }
        }

        public static List<DynamicListItem> GetList()
        {
            List<DynamicListItem> list = new List<DynamicListItem>();
            for (int i = 1; i <= 3; i++)
            {
                list.Add(new DynamicListItem() { Id = 1, Title = "Source" + i.ToString(), Value = "DataDrivenApplication.Source" + i.ToString() });
            }
            return list;
        }

        public static List<string> GetDrugTypeList()
        {
            //Old Version
            List<string> list = new List<string>();
            list.Add("Antibiotics");
            list.Add("Antiemetics");
            list.Add("Antipyretic");
            list.Add("Antispasmodic");
            return list;

            //New Version

        }

        public static List<string> GetListOfCheckBoxesTypeList()
        {
            List<string> list = new List<string>();
            list.Add("Nutrition List");
            list.Add("Other Alarms");
            return list;
        }


        public static List<string> GetAntibioticsList()
        {
            List<string> list = new List<string>();
            list.Add("Cotrimoxazole");
            //list.Add("Trimethoprim");
            //list.Add("Sulfamethoxazole");
            list.Add("Ciprofloxacin");
            list.Add("Doxycycline");
            list.Add("Furazolidone");
            //list.Add("Cotrimoxazole");
            list.Add("Metronidazole");
            //list.Add("Metronidazole");
            list.Add("Ampicillin");
            list.Add("Tinidazole");
            list.Add("Diloxanide Furoate");
            return list;
        }

        public static List<string> GetAntiemeticsList()
        {
            List<string> list = new List<string>();
            list.Add("metoclopramide");

            return list;
        }

        public static List<string> GetAntipyreticList()
        {
            List<string> list = new List<string>();
            list.Add("Paracetamol");

            return list;
        }

        public static List<string> GetAntispasmodicList()
        {
            List<string> list = new List<string>();
            list.Add("Dicylomine");

            return list;
        }

        //Older Version
        public static List<string> GetDrugList(long ID)
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 30; i++)
            //{
            //    list.Add("Drug" + i.ToString());
            //}
            list.Add("Cotrimoxazole");
            list.Add("Trimethoprim");
            list.Add("Sulfamethoxazole");
            list.Add("Ciprofloxacin");
            list.Add("Doxycycline");
            list.Add("Furazolidone");
            list.Add("Cotrimoxazole");
            list.Add("Metronidazole");
            list.Add("Metronidazole");
            list.Add("Ampicillin");
            list.Add("metoclopramide");
            list.Add("Paracetamol");
            list.Add("Dicylomine");
            return list;


            //Newer Version


        }

        public static List<MasterListItem> GetDayList()
        {
            //List<int> list = new List<int>();
            //for (int i = 1; i <= 100; i++)
            //{
            //    list.Add(i);
            //}

            List<MasterListItem> list = new List<MasterListItem>();
            for (int i = 1; i <= 100; i++)
            {
                list.Add(new MasterListItem() { ID = i, Description = i.ToString(), Status = true });
            }

            return list;
        }


        public static List<string> GetDosageList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Dosage" + i.ToString());
            //}
            list.Add("10 mg TMP and 50 mg SMX/ kg/day in 2 divided doses for 5 days");
            list.Add("20mg/kg/d in 2 divided doses for 5 days");
            list.Add("Single dose of 5mg/kg maximum 200mg)");
            list.Add("5-8mg/kg/day in 4 divided doses for 3 days");
            //list.Add("15mg/kg/day in 3 divided doses for 5 days");
            list.Add("15-20 mg/kg/day in 3 divided doses for 5days");
            list.Add("100 mg/kg/d for 10 divided doses");
            list.Add("10-15 mg/kg/day for 3 divided doses x 5 days");
            list.Add("20 mg /kg / day for 10 days");
            //list.Add("0.1 mg/kg/day 6-8 hours");
            //list.Add("15 mg/kg oral");
            list.Add("5-10mg orally upto 3-4 times daily");
            return list;
        }

        public static List<MasterListItem> GetRouteList()
        {
            //List<string> list = new List<string>();
            ////for (int i = 1; i <= 10; i++)
            ////{
            ////    list.Add("Dosage" + i.ToString());
            ////}

            //list.Add("Oral");
            //list.Add("IM");
            //list.Add("IV");

            List<MasterListItem> list = new List<MasterListItem>();
            list.Add(new MasterListItem() { ID = 1, Description = "Oral", Status = true });
            list.Add(new MasterListItem() { ID = 2, Description = "IM", Status = true });
            list.Add(new MasterListItem() { ID = 3, Description = "IV", Status = true });

            return list;
        }

        public static List<string> GetFrequencyList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            list.Add("Stat");
            list.Add("Every Hour");
            list.Add("Every 2 Hour");
            list.Add("Every 4 Hour");
            list.Add("Every 6 Hour");
            list.Add("Once Daily");
            list.Add("Twice Daily");
            list.Add("Thrice Daily");
            list.Add("as needed");
            list.Add("other");
            return list;
        }

        public static List<string> GetInvestigationList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            list.Add("--Select--");
            list.Add("Stool Exam");
            list.Add("Clinitest or Benedict’s test");
            list.Add("Endoscopy");
            list.Add("Sigmoidoscopy");
            list.Add("Small bowel biopsy");
            list.Add("Colonoscopy");
            list.Add("Barium studies");
            list.Add("Other");
            return list;
        }

        public static List<string> GetNutritionList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            //list.Add("--Select--");
            list.Add("encourage breast feeding");
            list.Add("ORS first 4 hours");
            list.Add("freshly prepared mashed / ground food every 3 hours");
            list.Add("energy rich food complements");
            list.Add("additional meals x 3 a day for breast fed children");
            list.Add("additional mealsx 5 a day for non breastfed infants");
            list.Add("meals x 6 times a day for older infants");
            list.Add("supplement additional leafy vegetables, legumes, carrot, banana, pumpkin etc");
            list.Add("Other");
            return list;
        }

        public static List<string> GetOtherAlertsList()
        {
            List<string> list = new List<string>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    list.Add("Frequency" + i.ToString());

            //}
            //list.Add("--Select--");
            list.Add("Blood in Stools");
            list.Add("Nausea Vomiting");
            list.Add("Urine Absent");
            list.Add("Lathergic Unconcious");
            list.Add("Eyes Sunken");
            list.Add("Skin color cyanosed");
            list.Add("Drinks poorly (Not able to drink)");
            list.Add("Skin/turgor retracts very slowly");
            list.Add("Weight loss >=9%");
            list.Add("Severe dehydration");
            list.Add("Nutritional status severely impaired");
            list.Add("Abdominal Examination Distended");
            list.Add("Eyes Sunken");
            list.Add("Skin color cyanosed");
            list.Add("Drinks poorly (Not able to drink)");
            list.Add("Skin/turgor retracts very slowly");
            list.Add("Other");
            return list;
        }

    }
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

    public class Patient
    {
        public Int64 PatientId { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        //public bool? Gender { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string Phone { get; set; }
        public string ClinicRegNo { get; set; }
        public long patientUnitID { get; set; }
        public string MRNo { get; set; }
    }
}
