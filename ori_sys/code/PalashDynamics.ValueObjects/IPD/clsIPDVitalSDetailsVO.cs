using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsIPDVitalSDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            return this.ToString();
        }

        #region Property Declaration
        //Added By Kiran For Fill TakenBy Combobox.
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private double _Collection;
        public double Collection 
        {
            get { return _Collection; }
            set
            {
                if (_Collection != value)
                {
                    _Collection = value;
                    OnPropertyChanged("Collection");
                }
            }
        }
      
        private long _VitalsID;
        public long VitalsID
        {
            get { return _VitalsID; }
            set
            {
                if (_VitalsID != value)
                {
                    _VitalsID = value;
                    OnPropertyChanged("VitalsID");
                }
            }
        }


        private long _VitalsUnitID;
        public long VitalsUnitID
        {
            get { return _VitalsUnitID; }
            set
            {
                if (_VitalsUnitID != value)
                {
                    _VitalsUnitID = value;
                    OnPropertyChanged("VitalsUnitID");
                }
            }
        }

        private long _VitalSignID;
        public long VitalSignID
        {
            get { return _VitalSignID; }
            set
            {
                if (_VitalSignID != value)
                {
                    _VitalSignID = value;
                    OnPropertyChanged("VitalSignID");
                }
            }
        }

        private long _VisitAdmID;
        public long VisitAdmID
        {
            get { return _VisitAdmID; }
            set
            {
                if(_VisitAdmID != value)
                {
                    _VisitAdmID = value;
                    OnPropertyChanged("VisitAdmID");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private double _DefaultValue;
        public double DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                if (_DefaultValue != value)
                {
                    _DefaultValue = value;
                    OnPropertyChanged("DefaultValue");
                }
            }
        }

        private double _MinValue;
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                    OnPropertyChanged("MinValue");
                }
            }
        }

        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    OnPropertyChanged("MaxValue");
                }
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string _IsVisible="Collapsed";
        public string IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }

        private long _VisitAdmUnitID;
        public long VisitAdmUnitID
        {
            get { return _VisitAdmUnitID; }
            set
            {
                if(_VisitAdmUnitID != value)
                {
                    _VisitAdmUnitID = value;
                    OnPropertyChanged("VisitAdmUnitID");
                }
            }
        }

        private short _Opd_Ipd;
        public short Opd_Ipd
        {
            get { return _Opd_Ipd; }
            set
            {
                if(_Opd_Ipd != value)
                {
                    _Opd_Ipd = value;
                    OnPropertyChanged("Opd_Ipd");
                }
            }
        }

        private long _IntakeOutputID;
        public long IntakeOutputID
        {
            get { return _IntakeOutputID; }
            set
            {
                if (_IntakeOutputID != value)
                {
                    _IntakeOutputID = value;
                    OnPropertyChanged("_IntakeOutputID");
                }
            }
        }

        private long _IntakeOutputIDUnitID;
        public long IntakeOutputIDUnitID
        {
            get { return _IntakeOutputIDUnitID; }
            set
            {
                if (_IntakeOutputIDUnitID != value)
                {
                    _IntakeOutputIDUnitID = value;
                    OnPropertyChanged("IntakeOutputIDUnitID");
                }
            }
        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        // Added by Ashutosh for display in diffrent colour...
        private int _NextapColor;
        public int NextapColor
        {
            get
            {
                return _NextapColor;
            }
            set
            {
                _NextapColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NextapColor"));
                }
            }
        }


        private bool _IsEnabled = true;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        //Added By kiran for Add TPR Details.

        private DateTime? _Date;
        public DateTime? Date
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



        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }
        
        private DateTime? _Time;
        public DateTime? Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private DateTime? _VitalDate;
        public DateTime? VitalDate
        {
            get { return _VitalDate; }
            set
            {
                if (_VitalDate != value)
                {
                    _VitalDate = value;
                    OnPropertyChanged("VitalDate");
                }
            }
        }

        private string _Time1;
        public string Time1
        {
            get { return _Time1; }
            set
            {
                if (_Time1 != value)
                {
                    _Time1 = value;
                    OnPropertyChanged("Time1");
                }
            }
        }

        private double _Temperature;
        public double Temperature
        {
            get { return _Temperature; }
            set
            {
                if (_Temperature != value)
                {
                    _Temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
        }


        private double _Pulse;
        public double Pulse
        {
            get { return _Pulse; }
            set
            {
                if (_Pulse != value)
                {
                    _Pulse = value;
                    OnPropertyChanged("Pulse");
                }
            }
        }

        private double _Respiration;
        public double Respiration
        {
            get { return _Respiration; }
            set
            {
                if (_Respiration != value)
                {
                    _Respiration = value;
                    OnPropertyChanged("Respiration");
                }
            }
        }
        private bool _IsEncounter;
        public bool IsEncounter
        {
            get { return _IsEncounter; }
            set
            {
                if (_IsEncounter != value)
                {
                    _IsEncounter = value;
                    OnPropertyChanged("IsEncounter");
                }
            }
        }

        private double _BP_Sys;
        public double BP_Sys
        {
            get { return _BP_Sys; }
            set
            {
                if (_BP_Sys != value)
                {
                    _BP_Sys = value;
                    OnPropertyChanged("BP_Sys");
                }
            }
        }

        private double _BP_Dia;
        public double BP_Dia
        {
            get { return _BP_Dia; }
            set
            {
                if (_BP_Dia != value)
                {
                    _BP_Dia = value;
                    OnPropertyChanged("BP_Dia");
                }
            }
        }
        private long _Height;
        public long Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        private float _Weight;
        public float Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        private long _TakenBy;
        public long TakenBy
        {
            get { return _TakenBy; }
            set
            {
                if (_TakenBy != value)
                {
                    _TakenBy = value;
                    OnPropertyChanged("TakenBy");
                }
            }
        }
        private string _TakenByName;
        public string TakenByName
        {
            get { return _TakenByName; }
            set
            {
                if (_TakenByName != value)
                {
                    _TakenByName = value;
                    OnPropertyChanged("TakenByName");
                }
            }
        }


        private bool _UnRegisterd;
        public bool UnRegistered
        {
            get { return _UnRegisterd; }
            set { _UnRegisterd = value; }
        }

        private bool _PagingEnabled;
        public bool InputPagingEnabled
        {
            get { return _PagingEnabled; }
            set { _PagingEnabled = value; }
        }

        private int _StartRowIndex = 0;

        public int InputStartRowIndex
        {
            get { return _StartRowIndex; }
            set { _StartRowIndex = value; }
        }

        private int _MaximumRows = 10;
        public int InputMaximumRows
        {
            get { return _MaximumRows; }
            set { _MaximumRows = value; }
        }

        public string SortExpression { get; set; }

        public bool IsBroughtDead { get; set; }

        #endregion

        #region Common Properties


        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    //OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
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
    }

}
