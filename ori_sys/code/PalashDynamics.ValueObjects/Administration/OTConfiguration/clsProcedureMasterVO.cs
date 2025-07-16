using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.OTConfiguration
{
    public class clsProcedureMasterVO : IValueObject, INotifyPropertyChanged
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

        private List<clsProcedureTemplateDetailsVO> _ProcedureTempalateList = new List<clsProcedureTemplateDetailsVO>();
        public List<clsProcedureTemplateDetailsVO> ProcedureTempalateList
        {
            get
            {
                return _ProcedureTempalateList;
            }
            set
            {
                _ProcedureTempalateList = value;
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

        private long _DepartmentID;
        public long DepartmentID
        {
            get
            {
                return _DepartmentID;
            }
            set
            {
                _DepartmentID = value;
                OnPropertyChanged("DepartmentID");
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
        private string _Code;
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _Specialization;
        public string Specialization
        {
            get
            {
                return _Specialization;
            }
            set
            {
                _Specialization = value;
                OnPropertyChanged("Specialization");
            }
        }

        private string _SubSpecialization;
        public string SubSpecialization
        {
            get
            {
                return _SubSpecialization;
            }
            set
            {
                _SubSpecialization = value;
                OnPropertyChanged("SubSpecialization");
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

        private string _Duration;
        public string Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
                OnPropertyChanged("Duration");
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

        private long? _RecommandedAnesthesiaTypeID;
        public long? RecommandedAnesthesiaTypeID
        {
            get
            {
                return _RecommandedAnesthesiaTypeID;
            }
            set
            {
                _RecommandedAnesthesiaTypeID = value;
                OnPropertyChanged("RecommandedAnesthesiaTypeID");
            }
        }

        private long? _OperationTheatreID;
        public long? OperationTheatreID
        {
            get
            {
                return _OperationTheatreID;
            }
            set
            {
                _OperationTheatreID = value;
                OnPropertyChanged("OperationTheatreID");
            }
        }

        private long? _CategoryID;
        public long? CategoryID
        {
            get
            {
                return _CategoryID;
            }
            set
            {
                _CategoryID = value;
                OnPropertyChanged("CategoryID");
            }
        }
        private long? _SubCategoryID;
        public long? SubCategoryID
        {
            get
            {
                return _SubCategoryID;
            }
            set
            {
                _SubCategoryID = value;
                OnPropertyChanged("SubCategoryID");
            }
        }
        private long? _CheckListID;
        public long? CheckListID
        {
            get
            {
                return _CheckListID;
            }
            set
            {
                _CheckListID = value;
                OnPropertyChanged("CheckListID");
            }
        }
        private long? _OTTableID;
        public long? OTTableID
        {
            get
            {
                return _OTTableID;
            }
            set
            {
                _OTTableID = value;
                OnPropertyChanged("OTTableID");
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

        private List<clsProcedureConsentDetailsVO> _ConsentList = new List<clsProcedureConsentDetailsVO>();
        public List<clsProcedureConsentDetailsVO> ConsentList
        {
            get
            {
                return _ConsentList;
            }
            set
            {
                _ConsentList = value;
            }
        }

        private List<clsProcedureInstructionDetailsVO> _InstructionList = new List<clsProcedureInstructionDetailsVO>();
        public List<clsProcedureInstructionDetailsVO> InstructionList
        {
            get
            {
                return _InstructionList;
            }
            set
            {
                _InstructionList = value;
            }
        }
        private List<clsProcedureInstructionDetailsVO> _PreInstructionList = new List<clsProcedureInstructionDetailsVO>();
        public List<clsProcedureInstructionDetailsVO> PreInstructionList
        {
            get
            {
                return _PreInstructionList;
            }
            set
            {
                _PreInstructionList = value;
            }
        }
        private List<clsProcedureInstructionDetailsVO> _PostInstructionList = new List<clsProcedureInstructionDetailsVO>();
        public List<clsProcedureInstructionDetailsVO> PostInstructionList
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
        private List<clsProcedureItemDetailsVO> _ItemList = new List<clsProcedureItemDetailsVO>();
        public List<clsProcedureItemDetailsVO> ItemList
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

        private List<clsDoctorSuggestedServiceDetailVO> _ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
        public List<clsDoctorSuggestedServiceDetailVO> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                _ServiceList = value;
            }
        }

        private List<clsProcedureStaffDetailsVO> _StaffList = new List<clsProcedureStaffDetailsVO>();
        public List<clsProcedureStaffDetailsVO> StaffList
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

        private List<clsProcedureChecklistDetailsVO> _CheckList = new List<clsProcedureChecklistDetailsVO>();
        public List<clsProcedureChecklistDetailsVO> CheckList
        {
            get
            {
                return _CheckList;
            }
            set
            {
                _CheckList = value;
            }
        }

        private List<clsProcedureEquipmentDetailsVO> _EquipList = new List<clsProcedureEquipmentDetailsVO>();
        public List<clsProcedureEquipmentDetailsVO> EquipList
        {
            get
            {
                return _EquipList;
            }
            set
            {
                _EquipList = value;
            }
        }

       

        
    }
}
