using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsOTSheetDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject
        public string ToXml()
        {
            return this.ToString();
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

        public string OTName { get; set; }
        public string AnesthesiaType { get; set; }
        public string ProcedureType { get; set; }
        public string OperationResult { get; set; }
        public string OperationStatus { get; set; }

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;
                OnPropertyChanged("UnitID");
            }
        }

        private long _OTDetailsID;
        public long OTDetailsID
        {
            get
            {
                return _OTDetailsID;
            }
            set
            {
                _OTDetailsID = value;
                OnPropertyChanged("OTDetailsID");
            }
        }

        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }

        private DateTime? _FromTime;
        public DateTime? FromTime
        {
            get
            {
                return _FromTime;
            }
            set
            {
                _FromTime = value;
                OnPropertyChanged("FromTime");
            }
        }

        private DateTime? _ToTime;
        public DateTime? ToTime
        {
            get
            {
                return _ToTime;
            }
            set
            {
                _ToTime = value;
                OnPropertyChanged("ToTime");
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }

        private long? _OTID;
        public long? OTID
        {
            get
            {
                return _OTID;
            }
            set
            {
                _OTID = value;
                OnPropertyChanged("OTID");
            }
        }

        //private long? _TotalHours;
        //public long? TotalHours
        //{
        //    get
        //    {
        //        return _TotalHours;
        //    }
        //    set
        //    {
        //        _TotalHours = value;
        //        OnPropertyChanged("TotalHours");
        //    }
        //}

        private string _TotalHours;
        public string TotalHours
        {
            get
            {
                return _TotalHours;
            }
            set
            {
                _TotalHours = value;
            }
        }

        private long? _AnesthesiaTypeID;
        public long? AnesthesiaTypeID
        {
            get
            {
                return _AnesthesiaTypeID;
            }
            set
            {
                _AnesthesiaTypeID = value;
                OnPropertyChanged("AnesthesiaTypeID");
            }
        }

        private long? _ProcedureTypeID;
        public long? ProcedureTypeID
        {
            get
            {
                return _ProcedureTypeID;
            }
            set
            {
                _ProcedureTypeID = value;
                OnPropertyChanged("ProcedureTypeID");
            }
        }

        private long? _OTResultID;
        public long? OTResultID
        {
            get
            {
                return _OTResultID;
            }
            set
            {
                _OTResultID = value;
                OnPropertyChanged("OTResultID");
            }
        }

        private long? _OTStatusID;
        public long? OTStatusID
        {
            get
            {
                return _OTStatusID;
            }
            set
            {
                _OTStatusID = value;
                OnPropertyChanged("OTStatusID");
            }
        }

        private long? _ManPower;
        public long? ManPower
        {
            get
            {
                return _ManPower;
            }
            set
            {
                _ManPower = value;
                OnPropertyChanged("ManPower");
            }
        }


        private bool _IsEmergency;
        public bool IsEmergency
        {
            get
            {
                return _IsEmergency;
            }
            set
            {
                _IsEmergency = value;
                OnPropertyChanged("IsEmergency");
            }
        }

        private string _Remark;
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                _Remark = value;
                OnPropertyChanged("Remark");
            }
        }


        private string _SpecialRequirement;
        public string SpecialRequirement
        {
            get
            {
                return _SpecialRequirement;
            }
            set
            {
                _SpecialRequirement = value;
                OnPropertyChanged("SpecialRequirement");
            }
        }

        private DateTime? _AnesthesiaStartTime;
        public DateTime? AnesthesiaStartTime
        {
            get
            {
                return _AnesthesiaStartTime;
            }
            set
            {
                _AnesthesiaStartTime = value;
                OnPropertyChanged("AnesthesiaStartTime");
            }
        }

        private DateTime? _AnesthesiaEndTime;
        public DateTime? AnesthesiaEndTime
        {
            get
            {
                return _AnesthesiaEndTime;
            }
            set
            {
                _AnesthesiaEndTime = value;
                OnPropertyChanged("AnesthesiaEndTime");
            }
        }

        private DateTime? _WheelInTime;
        public DateTime? WheelInTime
        {
            get
            {
                return _WheelInTime;
            }
            set
            {
                _WheelInTime = value;
                OnPropertyChanged("WheelInTime");
            }
        }

        private DateTime? _WheelOutTime;
        public DateTime? WheelOutTime
        {
            get
            {
                return _WheelOutTime;
            }
            set
            {
                _WheelOutTime = value;
                OnPropertyChanged("WheelOutTime");
            }
        }

        private DateTime? _SurgeryStartTime;
        public DateTime? SurgeryStartTime
        {
            get
            {
                return _SurgeryStartTime;
            }
            set
            {
                _SurgeryStartTime = value;
                OnPropertyChanged("SurgeryStartTime");
            }
        }

        private DateTime? _SurgeryEndTime;
        public DateTime? SurgeryEndTime
        {
            get
            {
                return _SurgeryEndTime;
            }
            set
            {
                _SurgeryEndTime = value;
                OnPropertyChanged("SurgeryEndTime");
            }
        }


    }
}
