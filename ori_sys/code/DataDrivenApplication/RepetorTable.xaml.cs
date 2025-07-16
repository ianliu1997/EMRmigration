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
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using PalashDynamics.ValueObjects;
using System.IO;

namespace DataDrivenApplication
{
    public partial class RepetorTable : UserControl
    {
        public RepetorTable()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(RepetorTable_Loaded);
        }

        List<Medication> lst = new List<Medication>();

        void RepetorTable_Loaded(object sender, RoutedEventArgs e)
        {
            Medication m = new Medication() { Command = "Add" };

            //m.Drug.Value = "Test";
            lst.Add(m);
            ExpBuilder.ItemsSource = null;
            ExpBuilder.ItemsSource = lst;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).TargetName.ToString() == "Remove")
            {
                //ExpBuilder.IsSynchronizedWithCurrentItem
                lst.RemoveAt(((Medication)((HyperlinkButton)sender).DataContext).Index);

                for (int i = 0; i < lst.Count; i++)
                {
                    if (i < lst.Count - 1)
                        lst[i].Command = "Remove";
                    lst[i].Index = i;
                }
            }

            if (((HyperlinkButton)sender).TargetName.ToString() == "Add")
            {
                lst.Add(new Medication() { Command = "Add", Index = lst.Count });
                for (int i = 0; i < lst.Count - 1; i++)
                {
                    lst[i].Command = "Remove";
                    lst[i].Index = i;
                }
            }






            ExpBuilder.ItemsSource = null;
            ExpBuilder.ItemsSource = lst;


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
