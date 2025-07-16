using System;
using System.Collections.Generic;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsOTDetailsVO : IValueObject, INotifyPropertyChanged
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

        public DateTime Date { get; set; }
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

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
                OnPropertyChanged("PatientID");
            }
        }

        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Remnark { get; set; }
        public string OTName { get; set; }
        public long OTID { get; set; }

        public long StaffID { get; set; }
        public string DocName { get; set; }
        public string StaffName { get; set; }

        public long detailsID { get; set; }
        public byte[] Photo { get; set; }

        //added by neena
        private string _ImageName;
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (value != _ImageName)
                {
                    _ImageName = value;
                }
            }
        }
        //

        public bool IsEmergency { get; set; }

        private long _ScheduleID;
        public long ScheduleID
        {
            get
            {
                return _ScheduleID;
            }
            set
            {
                _ScheduleID = value;
                OnPropertyChanged("ScheduleID");
            }
        }
        private long? _DoctorID;
        public long? DoctorID
        {
            get
            {
                return _DoctorID;
            }
            set
            {
                _DoctorID = value;
                OnPropertyChanged("DoctorID");
            }
        }

        private string _BedCategory;
        public string BedCategory
        {
            get
            {
                return _BedCategory;
            }
            set
            {
                _BedCategory = value;
                OnPropertyChanged("BedCategory");
            }
        }

        private string _Bed;
        public string Bed
        {
            get
            {
                return _Bed;
            }
            set
            {
                _Bed = value;
                OnPropertyChanged("Bed");
            }
        }

        private string _Ward;
        public string Ward
        {
            get
            {
                return _Ward;
            }
            set
            {
                _Ward = value;
                OnPropertyChanged("Ward");
            }
        }

        private DateTime? _AdmissionDate;
        public DateTime? AdmissionDate
        {
            get
            {
                return _AdmissionDate;
            }
            set
            {
                _AdmissionDate = value;
                OnPropertyChanged("AdmissionDate");
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

        private long? _AddedBy;
        public long? AddedBy
        {
            get
            {
                return _AddedBy;
            }
            set
            {
                _AddedBy = value;
                OnPropertyChanged("AddedBy");
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                _AddedOn = value;
                OnPropertyChanged("AddedOn");
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                _PatientName = value;
                OnPropertyChanged("PatientName");
            }
        }


        private string _Gender;
        public string Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                _Gender = value;
                OnPropertyChanged("Gender");
            }
        }


        private string _MRNo;
        public string MRNo
        {
            get
            {
                return _MRNo;
            }
            set
            {
                _MRNo = value;
                OnPropertyChanged("MRNo");
            }
        }


        private DateTime? _DOB;
        public DateTime? DOB
        {
            get
            {
                return _DOB;
            }
            set
            {
                _DOB = value;
                OnPropertyChanged("DOB");
            }
        }

        private DateTime? _AddedDateTime;
        public DateTime? AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                _AddedDateTime = value;
                OnPropertyChanged("AddedDateTime");
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
            set
            {
                _UpdatedBy = value;
                OnPropertyChanged("UpdatedBy");
            }
        }

        private string _UpdatedOn;
        public string UpdatedOn
        {
            get
            {
                return _UpdatedOn;
            }
            set
            {
                _UpdatedOn = value;
                OnPropertyChanged("UpdatedOn");
            }
        }
        private DateTime? _UpdatedDateTime;
        public DateTime? UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                _UpdatedDateTime = value;
                OnPropertyChanged("UpdatedDateTime");
            }
        }

        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                _AddedWindowsLoginName = value;
                OnPropertyChanged("AddedWindowsLoginName");
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

        private string _MaritalStatus;
        public string MaritalStatus
        {
            get
            {
                return _MaritalStatus;
            }
            set
            {
                _MaritalStatus = value;
                OnPropertyChanged("MaritalStatus");
            }
        }

        private string _Education;
        public string Education
        {
            get
            {
                return _Education;
            }
            set
            {
                _Education = value;
                OnPropertyChanged("Education");
            }
        }

        private string _Religion;
        public string Religion
        {
            get
            {
                return _Religion;
            }
            set
            {
                _Religion = value;
                OnPropertyChanged("Religion");
            }
        }

        private string _ContactNo1;
        public string ContactNo1
        {
            get
            {
                return _ContactNo1;
            }
            set
            {
                _ContactNo1 = value;
                OnPropertyChanged("ContactNo1");
            }
        }

        private clsOTDetailsOTSheetDetailsVO _objOtSheetDetails = new clsOTDetailsOTSheetDetailsVO();
        public clsOTDetailsOTSheetDetailsVO objOtSheetDetails
        {
            get
            {
                return _objOtSheetDetails;
            }
            set
            {
                _objOtSheetDetails = value;
            }
        }

        private List<clsPatientProcedureVO> _ProcedureList = new List<clsPatientProcedureVO>();
        public List<clsPatientProcedureVO> ProcedureList
        {
            get
            {
                return _ProcedureList;
            }
            set
            {
                _ProcedureList = value;
            }
        }

        private List<clsDoctorSuggestedServiceDetailVO> _OTServicesList = new List<clsDoctorSuggestedServiceDetailVO>();
        public List<clsDoctorSuggestedServiceDetailVO> OTServicesList
        {
            get
            {
                return _OTServicesList;
            }
            set
            {
                _OTServicesList = value;
            }
        }

        private clsOTDetailsPostInstructionDetailsVO _PostInstructionList = new clsOTDetailsPostInstructionDetailsVO();
        public clsOTDetailsPostInstructionDetailsVO PostInstructionList
        {
            get
            {
                return _PostInstructionList;
            }
            set
            {
                _PostInstructionList = value;
            }
        }

        private clsOTDetailsDoctorNotesDetailsVO _DoctorNotesList = new clsOTDetailsDoctorNotesDetailsVO();
        public clsOTDetailsDoctorNotesDetailsVO DoctorNotesList
        {
            get
            {
                return _DoctorNotesList;
            }
            set
            {
                _DoctorNotesList = value;
            }
        }

        //private List<clsOtDetailsProcedureDetailsVO> _ProcedureList = new List<clsOtDetailsProcedureDetailsVO>();
        //public List<clsOtDetailsProcedureDetailsVO> ProcedureList
        //{
        //    get
        //    {
        //        return _ProcedureList;
        //    }
        //    set
        //    {
        //        _ProcedureList = value;
        //    }
        //}

        private List<clsOTDetailsDocDetailsVO> _DocList = new List<clsOTDetailsDocDetailsVO>();
        public List<clsOTDetailsDocDetailsVO> DocList
        {
            get
            {
                return _DocList;
            }
            set
            {
                _DocList = value;
            }
        }

        private List<clsOTDetailsStaffDetailsVO> _StaffList = new List<clsOTDetailsStaffDetailsVO>();
        public List<clsOTDetailsStaffDetailsVO> StaffList
        {
            get
            {
                return _StaffList;
            }
            set
            {
                _StaffList = value;
            }
        }

        private List<clsOTDetailsItemDetailsVO> _ItemList = new List<clsOTDetailsItemDetailsVO>();
        public List<clsOTDetailsItemDetailsVO> ItemList
        {
            get
            {
                return _ItemList;
            }
            set
            {
                _ItemList = value;
            }
        }

        private List<clsProcedureItemDetailsVO> _ItemList1 = new List<clsProcedureItemDetailsVO>();
        public List<clsProcedureItemDetailsVO> ItemList1
        {
            get
            {
                return _ItemList1;
            }
            set
            {
                _ItemList1 = value;
            }
        }

        private List<clsOTDetailsInstrumentDetailsVO> _InstrumentList = new List<clsOTDetailsInstrumentDetailsVO>();
        public List<clsOTDetailsInstrumentDetailsVO> InstrumentList
        {
            get
            {
                return _InstrumentList;
            }
            set
            {
                _InstrumentList = value;
            }
        }

        private clsOtDetailsAnesthesiaNotesDetailsVO _objAnesthesiaNotes = new clsOtDetailsAnesthesiaNotesDetailsVO();
        public clsOtDetailsAnesthesiaNotesDetailsVO objAnesthesiaNotes
        {
            get
            {
                return _objAnesthesiaNotes;
            }
            set
            {
                _objAnesthesiaNotes = value;
            }
        }

        private clsOTDetailsSurgeryDetailsVO _objSurgeryNotes = new clsOTDetailsSurgeryDetailsVO();
        public clsOTDetailsSurgeryDetailsVO objSurgeryNotes
        {
            get
            {
                return _objSurgeryNotes;
            }
            set
            {
                _objSurgeryNotes = value;
            }
        }
    }

    public class clsOTDetailsDoctorNotesDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _DoctorNotesID;
        public long DoctorNotesID
        {
            get
            {
                return _DoctorNotesID;
            }
            set
            {
                _DoctorNotesID = value;
                OnPropertyChanged("PostInstructionID");
            }
        }

        private string _DoctorNotes;
        public string DoctorNotes
        {
            get
            {
                return _DoctorNotes;
            }
            set
            {
                _DoctorNotes = value;
                OnPropertyChanged("PostInstruction");
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
    }

    public class clsOTDetailsInstructionListDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private string _GroupName;
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
            }
        }

        private string _Instruction;
        public string Instruction
        {
            get
            {
                return _Instruction;
            }
            set
            {
                _Instruction = value;
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
    }


    public class clsOTDetailsPreInstructionDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _PreInstructionID;
        public long PreInstructionID
        {
            get
            {
                return _PreInstructionID;
            }
            set
            {
                _PreInstructionID = value;
                OnPropertyChanged("PreInstructionID");
            }
        }

        private string _PreInstruction;
        public string PreInstruction
        {
            get
            {
                return _PreInstruction;
            }
            set
            {
                _PreInstruction = value;
                OnPropertyChanged("PreInstruction");
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
    }


    public class clsOTDetailsIntraInstructionDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _IntraInstructionID;
        public long IntraInstructionID
        {
            get
            {
                return _IntraInstructionID;
            }
            set
            {
                _IntraInstructionID = value;
                OnPropertyChanged("IntraInstructionID");
            }
        }

        private string _IntraInstruction;
        public string IntraInstruction
        {
            get
            {
                return _IntraInstruction;
            }
            set
            {
                _IntraInstruction = value;
                OnPropertyChanged("IntraInstruction");
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
    }

   

    public class clsOTDetailsPostInstructionDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _PostInstructionID;
        public long PostInstructionID
        {
            get
            {
                return _PostInstructionID;
            }
            set
            {
                _PostInstructionID = value;
                OnPropertyChanged("PostInstructionID");
            }
        }

        private string _PostInstruction;
        public string PostInstruction
        {
            get
            {
                return _PostInstruction;
            }
            set
            {
                _PostInstruction = value;
                OnPropertyChanged("PostInstruction");
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
    }

    public class clsOTDetailsServiceDetailsVO : IValueObject, INotifyPropertyChanged
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

        private clsChargeVO _ChargesDetails = new clsChargeVO();
        public clsChargeVO ChargesDetails
        {
            get { return _ChargesDetails; }
            set
            {
                if (_ChargesDetails != value)
                {
                    _ChargesDetails = value;
                    OnPropertyChanged("ChargesDetails");
                }
            }
        }




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

        private long _ChargesID;
        public long ChargesID
        {
            get
            {
                return _ChargesID;
            }
            set
            {
                _ChargesID = value;
                OnPropertyChanged("ChargesID");
            }
        }


        private long _ChargesUnitID;
        public long ChargesUnitID
        {
            get
            {
                return _ChargesUnitID;
            }
            set
            {
                _ChargesUnitID = value;
                OnPropertyChanged("ChargesUnitID");
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

        private long _OTDetailsUnitID;
        public long OTDetailsUnitID
        {
            get
            {
                return _OTDetailsUnitID;
            }
            set
            {
                _OTDetailsUnitID = value;
                OnPropertyChanged("OTDetailsUnitID");
            }
        }

        private long _ProceduerID;
        public long ProceduerID
        {
            get
            {
                return _ProceduerID;
            }
            set
            {
                _ProceduerID = value;
                OnPropertyChanged("ProceduerID");
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

        public string ServiceName { get; set; }
        private long? _ServiceID;
        public long? ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
                OnPropertyChanged("ServiceID");
            }
        }

        private double? _Quantity;
        public double? Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
                OnPropertyChanged("Amount");
            }
        }

        private double? _Rate;
        public double? Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private double? _Amount;
        public double? Amount
        {
            get
            {
                return _Quantity * _Rate;
            }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
            }
        }


    }



    //public class clsOTDetailsVO : IValueObject, INotifyPropertyChanged
    //{
    //    #region IValueObject
    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }
    //    #endregion

    //    #region INotifyPropertyChanged Members

    //    /// <summary>
    //    /// Implemts the INotifyPropertyChanged interface.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;

    //        if (null != handler)
    //        {
    //            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion

    //    private long _ID;
    //    public long ID
    //    {
    //        get
    //        {
    //            return _ID;
    //        }
    //        set
    //        {
    //            _ID = value;
    //            OnPropertyChanged("ID");
    //        }
    //    }

    //    public DateTime Date { get; set; }
    //    private long _UnitID;
    //    public long UnitID
    //    {
    //        get
    //        {
    //            return _UnitID;
    //        }
    //        set
    //        {
    //            _UnitID = value;
    //            OnPropertyChanged("UnitID");
    //        }
    //    }

    //    private long _PatientID;
    //    public long PatientID
    //    {
    //        get
    //        {
    //            return _PatientID;
    //        }
    //        set
    //        {
    //            _PatientID = value;
    //            OnPropertyChanged("PatientID");
    //        }
    //    }

    //    public DateTime FromTime { get; set; }
    //    public DateTime ToTime { get; set; }
    //    public string Remnark { get; set; }
    //    public string OTName { get; set; }
    //    public long OTID { get; set; }
      
    //    public long StaffID { get; set; }
    //    public string DocName { get; set; }
    //    public string StaffName { get; set; }

    //    public long detailsID { get; set; }

    //    private long _ScheduleID;
    //    public long ScheduleID
    //    {
    //        get
    //        {
    //            return _ScheduleID;
    //        }
    //        set
    //        {
    //            _ScheduleID = value;
    //            OnPropertyChanged("ScheduleID");
    //        }
    //    }
    //    private long? _DoctorID;
    //    public long? DoctorID
    //    {
    //        get
    //        {
    //            return _DoctorID;
    //        }
    //        set
    //        {
    //            _DoctorID = value;
    //            OnPropertyChanged("DoctorID");
    //        }
    //    }

    //    private string _BedCategory;
    //    public string BedCategory
    //    {
    //        get
    //        {
    //            return _BedCategory;
    //        }
    //        set
    //        {
    //            _BedCategory = value;
    //            OnPropertyChanged("BedCategory");
    //        }
    //    }

    //    private string _Bed;
    //    public string Bed
    //    {
    //        get
    //        {
    //            return _Bed;
    //        }
    //        set
    //        {
    //            _Bed = value;
    //            OnPropertyChanged("Bed");
    //        }
    //    }

    //    private string _Ward;
    //    public string Ward
    //    {
    //        get
    //        {
    //            return _Ward;
    //        }
    //        set
    //        {
    //            _Ward = value;
    //            OnPropertyChanged("Ward");
    //        }
    //    }

    //    private DateTime? _AdmissionDate;
    //    public DateTime? AdmissionDate
    //    {
    //        get
    //        {
    //            return _AdmissionDate;
    //        }
    //        set
    //        {
    //            _AdmissionDate = value;
    //            OnPropertyChanged("AdmissionDate");
    //        }
    //    }

    //    private bool _Status;
    //    public bool Status
    //    {
    //        get
    //        {
    //            return _Status;
    //        }
    //        set
    //        {
    //            _Status = value;
    //            OnPropertyChanged("Status");
    //        }
    //    }

    //    private long? _CreatedUnitID;
    //    public long? CreatedUnitID
    //    {
    //        get
    //        {
    //            return _CreatedUnitID;
    //        }
    //        set
    //        {
    //            _CreatedUnitID = value;
    //            OnPropertyChanged("CreatedUnitID");
    //        }
    //    }

    //    private long? _UpdatedUnitID;
    //    public long? UpdatedUnitID
    //    {
    //        get
    //        {
    //            return _UpdatedUnitID;
    //        }
    //        set
    //        {
    //            _UpdatedUnitID = value;
    //            OnPropertyChanged("UpdatedUnitID");
    //        }
    //    }

    //    private long? _AddedBy;
    //    public long? AddedBy
    //    {
    //        get
    //        {
    //            return _AddedBy;
    //        }
    //        set
    //        {
    //            _AddedBy = value;
    //            OnPropertyChanged("AddedBy");
    //        }
    //    }

    //    private string _AddedOn;
    //    public string AddedOn
    //    {
    //        get
    //        {
    //            return _AddedOn;
    //        }
    //        set
    //        {
    //            _AddedOn = value;
    //            OnPropertyChanged("AddedOn");
    //        }
    //    }

    //    private DateTime? _AddedDateTime;
    //    public DateTime? AddedDateTime
    //    {
    //        get
    //        {
    //            return _AddedDateTime;
    //        }
    //        set
    //        {
    //            _AddedDateTime = value;
    //            OnPropertyChanged("AddedDateTime");
    //        }
    //    }

    //    private long? _UpdatedBy;
    //    public long? UpdatedBy
    //    {
    //        get
    //        {
    //            return _UpdatedBy;
    //        }
    //        set
    //        {
    //            _UpdatedBy = value;
    //            OnPropertyChanged("UpdatedBy");
    //        }
    //    }

    //    private string _UpdatedOn;
    //    public string UpdatedOn
    //    {
    //        get
    //        {
    //            return _UpdatedOn;
    //        }
    //        set
    //        {
    //            _UpdatedOn = value;
    //            OnPropertyChanged("UpdatedOn");
    //        }
    //    }
    //    private DateTime? _UpdatedDateTime;
    //    public DateTime? UpdatedDateTime
    //    {
    //        get
    //        {
    //            return _UpdatedDateTime;
    //        }
    //        set
    //        {
    //            _UpdatedDateTime = value;
    //            OnPropertyChanged("UpdatedDateTime");
    //        }
    //    }

    //    private string _AddedWindowsLoginName;
    //    public string AddedWindowsLoginName
    //    {
    //        get
    //        {
    //            return _AddedWindowsLoginName;
    //        }
    //        set
    //        {
    //            _AddedWindowsLoginName = value;
    //            OnPropertyChanged("AddedWindowsLoginName");
    //        }
    //    }

    //    private string _UpdateWindowsLoginName;
    //    public string UpdateWindowsLoginName
    //    {
    //        get
    //        {
    //            return _UpdateWindowsLoginName;
    //        }
    //        set
    //        {
    //            _UpdateWindowsLoginName = value;
    //            OnPropertyChanged("UpdateWindowsLoginName");
    //        }
    //    }

    //    private clsOTDetailsOTSheetDetailsVO _objOtSheetDetails = new clsOTDetailsOTSheetDetailsVO();
    //    public clsOTDetailsOTSheetDetailsVO objOtSheetDetails
    //    {
    //        get
    //        {
    //            return _objOtSheetDetails;
    //        }
    //        set
    //        {
    //            _objOtSheetDetails = value;
    //        }
    //    }

    //    private List<clsPatientProcedureVO> _ProcedureList = new List<clsPatientProcedureVO>();
    //    public List<clsPatientProcedureVO> ProcedureList
    //    {
    //        get
    //        {
    //            return _ProcedureList;
    //        }
    //        set
    //        {
    //            _ProcedureList = value;
    //        }
    //    }

    //    //private List<clsOtDetailsProcedureDetailsVO> _ProcedureList = new List<clsOtDetailsProcedureDetailsVO>();
    //    //public List<clsOtDetailsProcedureDetailsVO> ProcedureList
    //    //{
    //    //    get
    //    //    {
    //    //        return _ProcedureList;
    //    //    }
    //    //    set
    //    //    {
    //    //        _ProcedureList = value;
    //    //    }
    //    //}

    //    private List<clsOTDetailsDocDetailsVO> _DocList = new List<clsOTDetailsDocDetailsVO>();
    //    public List<clsOTDetailsDocDetailsVO> DocList
    //    {
    //        get
    //        {
    //            return _DocList;
    //        }
    //        set
    //        {
    //            _DocList = value;
    //        }
    //    }

    //    private List<clsOTDetailsStaffDetailsVO> _StaffList = new List<clsOTDetailsStaffDetailsVO>();
    //    public List<clsOTDetailsStaffDetailsVO> StaffList
    //    {
    //        get
    //        {
    //            return _StaffList;
    //        }
    //        set
    //        {
    //            _StaffList = value;
    //        }
    //    }

    //    private List<clsOTDetailsItemDetailsVO> _ItemList = new List<clsOTDetailsItemDetailsVO>();
    //    public List<clsOTDetailsItemDetailsVO> ItemList 
    //    {
    //        get
    //        {
    //            return _ItemList;
    //        }
    //        set
    //        {
    //            _ItemList = value;
    //        }
    //    }

    //    private List<clsOTDetailsInstrumentDetailsVO> _InstrumentList = new List<clsOTDetailsInstrumentDetailsVO>();
    //    public List<clsOTDetailsInstrumentDetailsVO> InstrumentList 
    //    {
    //        get
    //        {
    //            return _InstrumentList;
    //        }
    //        set
    //        {
    //            _InstrumentList = value;
    //        }
    //    }

    //    private clsOtDetailsAnesthesiaNotesDetailsVO _objAnesthesiaNotes = new clsOtDetailsAnesthesiaNotesDetailsVO();
    //    public clsOtDetailsAnesthesiaNotesDetailsVO objAnesthesiaNotes
    //    {
    //        get
    //        {
    //            return _objAnesthesiaNotes;
    //        }
    //        set
    //        {
    //            _objAnesthesiaNotes = value;
    //        }
    //    }

    //    private clsOTDetailsSurgeryDetailsVO _objSurgeryNotes = new clsOTDetailsSurgeryDetailsVO();
    //    public clsOTDetailsSurgeryDetailsVO objSurgeryNotes
    //    {
    //        get
    //        {
    //            return _objSurgeryNotes;
    //        }
    //        set
    //        {
    //            _objSurgeryNotes = value;
    //        }
    //    }
    //}

    //public class clsOTDetailsPostInstructionDetailsVO : IValueObject, INotifyPropertyChanged
    //{
    //    #region IValueObject
    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }
    //    #endregion

    //    #region INotifyPropertyChanged Members

    //    /// <summary>
    //    /// Implemts the INotifyPropertyChanged interface.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;

    //        if (null != handler)
    //        {
    //            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //    private long _ID;
    //    public long ID
    //    {
    //        get
    //        {
    //            return _ID;
    //        }
    //        set
    //        {
    //            _ID = value;
    //            OnPropertyChanged("ID");
    //        }
    //    }


    //    private long _UnitID;
    //    public long UnitID
    //    {
    //        get
    //        {
    //            return _UnitID;
    //        }
    //        set
    //        {
    //            _UnitID = value;
    //            OnPropertyChanged("UnitID");
    //        }
    //    }

    //    private long _OTDetailsID;
    //    public long OTDetailsID
    //    {
    //        get
    //        {
    //            return _OTDetailsID;
    //        }
    //        set
    //        {
    //            _OTDetailsID = value;
    //            OnPropertyChanged("OTDetailsID");
    //        }
    //    }

    //    private long _OTDetailsUnitID;
    //    public long OTDetailsUnitID
    //    {
    //        get
    //        {
    //            return _OTDetailsUnitID;
    //        }
    //        set
    //        {
    //            _OTDetailsUnitID = value;
    //            OnPropertyChanged("OTDetailsUnitID");
    //        }
    //    }

    //    private long _PostInstructionID;
    //    public long PostInstructionID
    //    {
    //        get
    //        {
    //            return _PostInstructionID;
    //        }
    //        set
    //        {
    //            _PostInstructionID = value;
    //            OnPropertyChanged("PostInstructionID");
    //        }
    //    }

    //    private string _PostInstruction;
    //    public string PostInstruction
    //    {
    //        get
    //        {
    //            return _PostInstruction;
    //        }
    //        set
    //        {
    //            _PostInstruction = value;
    //            OnPropertyChanged("PostInstruction");
    //        }
    //    }


    //    private bool _Status;
    //    public bool Status
    //    {
    //        get
    //        {
    //            return _Status;
    //        }
    //        set
    //        {
    //            _Status = value;
    //            OnPropertyChanged("Status");
    //        }
    //    }
    //}

    //public class clsOTDetailsServiceDetailsVO : IValueObject, INotifyPropertyChanged
    //{
    //    #region IValueObject
    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }
    //    #endregion

    //    #region INotifyPropertyChanged Members

    //    /// <summary>
    //    /// Implemts the INotifyPropertyChanged interface.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;

    //        if (null != handler)
    //        {
    //            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion

    //    private clsChargeVO _ChargesDetails = new clsChargeVO();
    //    public clsChargeVO ChargesDetails
    //    {
    //        get { return _ChargesDetails; }
    //        set
    //        {
    //            if (_ChargesDetails != value)
    //            {
    //                _ChargesDetails = value;
    //                OnPropertyChanged("ChargesDetails");
    //            }
    //        }
    //    }




    //    private long _ID;
    //    public long ID
    //    {
    //        get
    //        {
    //            return _ID;
    //        }
    //        set
    //        {
    //            _ID = value;
    //            OnPropertyChanged("ID");
    //        }
    //    }

    //    private long _UnitID;
    //    public long UnitID
    //    {
    //        get
    //        {
    //            return _UnitID;
    //        }
    //        set
    //        {
    //            _UnitID = value;
    //            OnPropertyChanged("UnitID");
    //        }
    //    }

    //    private long _ChargesID;
    //    public long ChargesID
    //    {
    //        get
    //        {
    //            return _ChargesID;
    //        }
    //        set
    //        {
    //            _ChargesID = value;
    //            OnPropertyChanged("ChargesID");
    //        }
    //    }


    //    private long _ChargesUnitID;
    //    public long ChargesUnitID
    //    {
    //        get
    //        {
    //            return _ChargesUnitID;
    //        }
    //        set
    //        {
    //            _ChargesUnitID = value;
    //            OnPropertyChanged("ChargesUnitID");
    //        }
    //    }

    //    private long _OTDetailsID;
    //    public long OTDetailsID
    //    {
    //        get
    //        {
    //            return _OTDetailsID;
    //        }
    //        set
    //        {
    //            _OTDetailsID = value;
    //            OnPropertyChanged("OTDetailsID");
    //        }
    //    }

    //    private long _OTDetailsUnitID;
    //    public long OTDetailsUnitID
    //    {
    //        get
    //        {
    //            return _OTDetailsUnitID;
    //        }
    //        set
    //        {
    //            _OTDetailsUnitID = value;
    //            OnPropertyChanged("OTDetailsUnitID");
    //        }
    //    }

    //    private long _ProceduerID;
    //    public long ProceduerID
    //    {
    //        get
    //        {
    //            return _ProceduerID;
    //        }
    //        set
    //        {
    //            _ProceduerID = value;
    //            OnPropertyChanged("ProceduerID");
    //        }
    //    }

    //    private bool _Status;
    //    public bool Status
    //    {
    //        get
    //        {
    //            return _Status;
    //        }
    //        set
    //        {
    //            _Status = value;
    //            OnPropertyChanged("Status");
    //        }
    //    }

    //    public string ServiceName { get; set; }
    //    private long? _ServiceID;
    //    public long? ServiceID
    //    {
    //        get
    //        {
    //            return _ServiceID;
    //        }
    //        set
    //        {
    //            _ServiceID = value;
    //            OnPropertyChanged("ServiceID");
    //        }
    //    }

    //    private double? _Quantity;
    //    public double? Quantity
    //    {
    //        get
    //        {
    //            return _Quantity;
    //        }
    //        set
    //        {
    //            _Quantity = value;
    //            OnPropertyChanged("Quantity");
    //            OnPropertyChanged("Amount");
    //        }
    //    }

    //    private double? _Rate;
    //    public double? Rate
    //    {
    //        get
    //        {
    //            return _Rate;
    //        }
    //        set
    //        {
    //            _Rate = value;
    //            OnPropertyChanged("Rate");
    //        }
    //    }

    //    private double? _Amount;
    //    public double? Amount
    //    {
    //        get
    //        {
    //            return _Quantity * _Rate;
    //        }
    //        set
    //        {
    //            _Amount = value;
    //            OnPropertyChanged("Amount");
    //        }
    //    }
    //}

    //public class clsOTDetailsInstructionListDetailsVO : IValueObject, INotifyPropertyChanged
    //{
    //    #region IValueObject
    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }
    //    #endregion

    //    #region INotifyPropertyChanged Members

    //    /// <summary>
    //    /// Implemts the INotifyPropertyChanged interface.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;

    //        if (null != handler)
    //        {
    //            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //    private long _ID;
    //    public long ID
    //    {
    //        get
    //        {
    //            return _ID;
    //        }
    //        set
    //        {
    //            _ID = value;
    //            OnPropertyChanged("ID");
    //        }
    //    }


    //    private long _UnitID;
    //    public long UnitID
    //    {
    //        get
    //        {
    //            return _UnitID;
    //        }
    //        set
    //        {
    //            _UnitID = value;
    //            OnPropertyChanged("UnitID");
    //        }
    //    }

    //    private long _OTDetailsID;
    //    public long OTDetailsID
    //    {
    //        get
    //        {
    //            return _OTDetailsID;
    //        }
    //        set
    //        {
    //            _OTDetailsID = value;
    //            OnPropertyChanged("OTDetailsID");
    //        }
    //    }

    //    private long _OTDetailsUnitID;
    //    public long OTDetailsUnitID
    //    {
    //        get
    //        {
    //            return _OTDetailsUnitID;
    //        }
    //        set
    //        {
    //            _OTDetailsUnitID = value;
    //            OnPropertyChanged("OTDetailsUnitID");
    //        }
    //    }

    //    private string _GroupName;
    //    public string GroupName
    //    {
    //        get
    //        {
    //            return _GroupName;
    //        }
    //        set
    //        {
    //            _GroupName = value;
    //        }
    //    }

    //    private string _Instruction;
    //    public string Instruction
    //    {
    //        get
    //        {
    //            return _Instruction;
    //        }
    //        set
    //        {
    //            _Instruction = value;
    //        }
    //    }

    //    private bool _Status;
    //    public bool Status
    //    {
    //        get
    //        {
    //            return _Status;
    //        }
    //        set
    //        {
    //            _Status = value;
    //            OnPropertyChanged("Status");
    //        }
    //    }
    //}

    //public class clsOTDetailsDoctorNotesDetailsVO : IValueObject, INotifyPropertyChanged
    //{
    //    #region IValueObject
    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }
    //    #endregion

    //    #region INotifyPropertyChanged Members

    //    /// <summary>
    //    /// Implemts the INotifyPropertyChanged interface.
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;

    //        if (null != handler)
    //        {
    //            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }

    //    #endregion
    //    private long _ID;
    //    public long ID
    //    {
    //        get
    //        {
    //            return _ID;
    //        }
    //        set
    //        {
    //            _ID = value;
    //            OnPropertyChanged("ID");
    //        }
    //    }


    //    private long _UnitID;
    //    public long UnitID
    //    {
    //        get
    //        {
    //            return _UnitID;
    //        }
    //        set
    //        {
    //            _UnitID = value;
    //            OnPropertyChanged("UnitID");
    //        }
    //    }

    //    private long _OTDetailsID;
    //    public long OTDetailsID
    //    {
    //        get
    //        {
    //            return _OTDetailsID;
    //        }
    //        set
    //        {
    //            _OTDetailsID = value;
    //            OnPropertyChanged("OTDetailsID");
    //        }
    //    }

    //    private long _OTDetailsUnitID;
    //    public long OTDetailsUnitID
    //    {
    //        get
    //        {
    //            return _OTDetailsUnitID;
    //        }
    //        set
    //        {
    //            _OTDetailsUnitID = value;
    //            OnPropertyChanged("OTDetailsUnitID");
    //        }
    //    }

    //    private long _DoctorNotesID;
    //    public long DoctorNotesID
    //    {
    //        get
    //        {
    //            return _DoctorNotesID;
    //        }
    //        set
    //        {
    //            _DoctorNotesID = value;
    //            OnPropertyChanged("PostInstructionID");
    //        }
    //    }

    //    private string _DoctorNotes;
    //    public string DoctorNotes
    //    {
    //        get
    //        {
    //            return _DoctorNotes;
    //        }
    //        set
    //        {
    //            _DoctorNotes = value;
    //            OnPropertyChanged("PostInstruction");
    //        }
    //    }


    //    private bool _Status;
    //    public bool Status
    //    {
    //        get
    //        {
    //            return _Status;
    //        }
    //        set
    //        {
    //            _Status = value;
    //            OnPropertyChanged("Status");
    //        }
    //    }
    //}

}
