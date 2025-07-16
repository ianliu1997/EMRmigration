using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsPatientCompoundPrescriptionVO : INotifyPropertyChanged, IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        #region Properties
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        public string Frequency { get; set; }
        public string Instruction { get; set; }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _sCompoundQuantity;
        public string sCompoundQuantity
        {
            get { return _sCompoundQuantity; }
            set
            {
                if (_sCompoundQuantity != value)
                {
                    _sCompoundQuantity = value;
                    OnPropertyChanged("sCompoundQuantity");
                }
            }
        }

        private string _sComponentQuantity;
        public string sComponentQuantity
        {
            get { return _sComponentQuantity; }
            set
            {
                if (_sComponentQuantity != value)
                {
                    _sComponentQuantity = value;
                    OnPropertyChanged("sComponentQuantity");
                }
            }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private long _CompoundDrugID;
        public long CompoundDrugID
        {
            get
            {
                return _CompoundDrugID;
            }
            set
            {
                if (_CompoundDrugID != value)
                {
                    _CompoundDrugID = value;
                    OnPropertyChanged("CompoundDrugID");
                }
            }
        }

        private long _CompoundDrugUnitID;
        public long CompoundDrugUnitID
        {
            get
            {
                return _CompoundDrugUnitID;
            }
            set
            {
                if (_CompoundDrugUnitID != value)
                {
                    _CompoundDrugUnitID = value;
                    OnPropertyChanged("CompoundDrugUnitID");
                }
            }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get
            {
                return _PrescriptionID;
            }
            set
            {
                if (_PrescriptionID != value)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private String _Reason;
        public String Reason
        {
            get { return _Reason; }
            set
            {
                if (_Reason != value)
                {
                    _Reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }

        private string _CompoundDrug;
        public string CompoundDrug
        {
            get { return _CompoundDrug; }
            set
            {
                if (_CompoundDrug != value)
                {
                    _CompoundDrug = value;
                    OnPropertyChanged("CompoundDrug");
                }
            }
        }



        private double _ComponentQuantity;
        public double ComponentQuantity
        {
            get { return _ComponentQuantity; }
            set
            {
                if (_ComponentQuantity != value)
                {
                    _ComponentQuantity = value;
                    OnPropertyChanged("ComponentQuantity");
                }
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private float _Rate;
        public float Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        FrequencyMaster _SelectedFrequency = new FrequencyMaster { ID = 0, Description = "--Select--" };
        public FrequencyMaster SelectedFrequency
        {
            get
            {
                return _SelectedFrequency;
            }
            set
            {
                if (value != _SelectedFrequency)
                {
                    _SelectedFrequency = value;
                    OnPropertyChanged("SelectedFrequency");
                }
            }


        }


        List<MasterListItem> _RouteName = new List<MasterListItem>();
        public List<MasterListItem> RouteName
        {
            get
            {
                return _RouteName;
            }
            set
            {
                if (value != _RouteName)
                {
                    _RouteName = value;
                }
            }

        }


        MasterListItem _SelectedRoute = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRoute
        {
            get
            {
                return _SelectedRoute;
            }
            set
            {
                if (value != _SelectedRoute)
                {
                    _SelectedRoute = value;
                    OnPropertyChanged("SelectedRoute");
                }
            }


        }


        List<MasterListItem> _InstructionName = new List<MasterListItem>();
        public List<MasterListItem> InstructionName
        {
            get
            {
                return _InstructionName;
            }
            set
            {
                if (value != _InstructionName)
                {
                    _InstructionName = value;
                }
            }

        }


        MasterListItem _SelectedInstruction = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedInstruction
        {
            get
            {
                return _SelectedInstruction;
            }
            set
            {
                if (value != _SelectedInstruction)
                {
                    _SelectedInstruction = value;
                    OnPropertyChanged("SelectedInstruction");
                }
            }

        }

        private int _Days;
        public int Days
        {
            get { return _Days; }
            set
            {
                if (_Days != value)
                {
                    _Days = value;
                    OnPropertyChanged("Days");
                }
            }
        }

        private string _Dose;
        public string Dose
        {
            get
            {
                return _Dose;
            }
            set
            {
                if (_Dose != value)
                {
                    _Dose = value;
                    OnPropertyChanged("Dose");
                }
            }
        }

       
        #endregion

    }
}
