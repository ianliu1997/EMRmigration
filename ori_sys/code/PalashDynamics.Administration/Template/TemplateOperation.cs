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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PalashDynamics.ValueObjects;
using PalashDynamics.Administration.Template;
//using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.Service.DataTemplateServiceRef1;



namespace PalashDynamics.Administration
{
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
            //list.Add(new FieldType() { Id = 10, Title = "FollowUp Medication", DataType = "System.String" });
            //list.Add(new FieldType() { Id = 11, Title = "Other Investigation", DataType = "System.String" });
            list.Add(new FieldType() { Id = 12, Title = "List Of CheckBoxes", DataType = "System.String" });
            list.Add(new FieldType() { Id = 13, Title = "Databse List", DataType = "System.String" });
            list.Add(new FieldType() { Id = 14, Title = "Other Medication", DataType = "System.String" });
            list.Add(new FieldType() { Id = 15, Title = "Investigation & Services", DataType = "System.String" });
            list.Add(new FieldType() { Id = 16, Title = "Time Picker", DataType = "System.String" });
            list.Add(new FieldType() { Id = 17, Title = "File Upload", DataType = "System.String" });
            //list.Add(new FieldType() { Id = 18, Title = "BP Control", DataType = "System.String" });
            //list.Add(new FieldType() { Id = 19, Title = "Vision Control", DataType = "System.String" });
            //list.Add(new FieldType() { Id = 20, Title = "Glass Power Control", DataType = "System.String" });
            list.Add(new FieldType() { Id = 21, Title = "Image Control", DataType = "System.String" });
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



    //
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