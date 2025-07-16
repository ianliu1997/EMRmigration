using System;
using System.Xml.Serialization;
using System.Windows.Data;
using System.Globalization;
using PalashDynamics.ValueObjects;
using EMR;

namespace PalashDynamics.Service.DataTemplateServiceRef1
{
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
}
