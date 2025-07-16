using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel;
using PalashDynamics.ValueObjects;
using System.IO;
using System;
using System.Collections.Generic;

namespace EMR
{
    public class DesignControl
    {

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
    //

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
}
