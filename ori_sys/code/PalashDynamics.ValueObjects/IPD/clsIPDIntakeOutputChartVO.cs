using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

//Added By kiran for IPDIntakeOutputChart form.
namespace PalashDynamics.ValueObjects.IPD
{
   public  class clsIPDIntakeOutputChartVO: IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            return this.ToString();
        }

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
      
       #region Property Declaration
       
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

         private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }
        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("AdmUnitID");
                }
            }
        }


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

        private string _strTime;
        public string strTime
        {
            get { return _strTime; }
            set
            {
                if (_strTime != value)
                {
                    _strTime = value;
                    OnPropertyChanged("strTime");
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
                    OnPropertyChanged("IntakeOutputID");
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

        private bool? _IsFreezed;
        public bool? IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

        private double _Oral;
        public double Oral
        {
            get { return _Oral; }
            set
            {
                if (_Oral != value)
                {
                    _Oral = value;
                    OnPropertyChanged("Oral");
                }
            }
        }
      
        private double _Total_Parenteral;
        public double Total_Parenteral
        {
            get { return _Total_Parenteral; }
            set
            {
                if (_Total_Parenteral != value)
                {
                    _Total_Parenteral = value;
                    OnPropertyChanged("Total_Parenteral");
                }
            }
        }


        private double _OtherIntake;
        public double OtherIntake
        {
            get { return _OtherIntake; }
            set
            {
                if (_OtherIntake != value)
                {
                    _OtherIntake = value;
                    OnPropertyChanged("OtherIntake");
                }
            }
        }


        private double _IntakeTotal;
        public double IntakeTotal
        {
            get { return _IntakeTotal; }
            set
            {
                if (_IntakeTotal != value)
                {
                    _IntakeTotal = value;
                    OnPropertyChanged("IntakeTotal");
                }
            }
        }

        private double _OutputTotal;
        public double OutputTotal
        {
            get { return _OutputTotal; }
            set
            {
                if (_OutputTotal != value)
                {
                    _OutputTotal = value;
                    OnPropertyChanged("OutputTotal");
                }
            }
        }
        private double _Urine;
        public double Urine
        {
            get { return _Urine; }
            set
            {
                if (_Urine != value)
                {
                    _Urine = value;
                    OnPropertyChanged("Urine");
                }
            }
        }

        private double _Ng;
        public double Ng
        {
            get { return _Ng; }
            set
            {
                if (_Ng != value)
                {
                    _Ng = value;
                    OnPropertyChanged("Ng");
                }
            }
        }

        private double _Drain;
        public double Drain
        {
            get { return _Drain; }
            set
            {
                if (_Drain != value)
                {
                    _Drain = value;
                    OnPropertyChanged("Drain");
                }
            }
        }

        private bool _IsEnable;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                if (_IsEnable != value)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }


        private double _OtherOutput;
        public double OtherOutput
        {
            get { return _OtherOutput; }
            set
            {
                if (_OtherOutput != value)
                {
                    _OtherOutput = value;
                    OnPropertyChanged("OtherOutput");
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
        private long _SuccessStatus;
        public long SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
        #endregion

        #region Common Properties


        private long? _CreatedUnitID;
        public long? CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
            set
            {
                _CreatedUnitID = value;
                OnPropertyChanged("CreatedUnitID");
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get
            {
                return _UpdatedUnitID;
            }
            set
            {
                _UpdatedUnitID = value;
                OnPropertyChanged("UpdatedUnitID");
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

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                _UpdateWindowsLoginName = value;
                OnPropertyChanged("UpdateWindowsLoginName");
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

       
   
    }
}
