using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
    public class clsDiischargeApprovedByDepartmentVO : IValueObject, INotifyPropertyChanged
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

        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
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

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }
        private string _IPDNO;
        public string IPDNO
        {
            get { return _IPDNO; }
            set
            {
                if (_IPDNO != value)
                {
                    _IPDNO = value;
                    OnPropertyChanged("IPDNO");
                }
            }
        }
        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (_DepartmentName != value)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }
        private string _AdviseAuthorityName;
        public string AdviseAuthorityName
        {
            get { return _AdviseAuthorityName; }
            set
            {
                if (_AdviseAuthorityName != value)
                {
                    _AdviseAuthorityName = value;
                    OnPropertyChanged("AdviseAuthorityName");
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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _PatientId;
        public long PatientId
        {
            get { return _PatientId; }
            set
            {
                if (_PatientId != value)
                {
                    _PatientId = value;
                    OnPropertyChanged("PatientId");
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
        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }
        private long _AdmissionId;
        public long AdmissionId
        {
            get { return _AdmissionId; }
            set
            {
                if (_AdmissionId != value)
                {
                    _AdmissionId = value;
                    OnPropertyChanged("AdmissionId");
                }
            }
        }

        private long _AdmissionUnitID;
        public long AdmissionUnitID
        {
            get { return _AdmissionUnitID; }
            set
            {
                if (_AdmissionUnitID != value)
                {
                    _AdmissionUnitID = value;
                    OnPropertyChanged("AdmissionUnitID");
                }
            }
        }
        private long _StaffID;
        public long StaffID
        {
            get { return _StaffID; }
            set
            {
                if (_StaffID != value)
                {
                    _StaffID = value;
                    OnPropertyChanged("StaffID");
                }
            }
        }
        private long _LoginUserID;
        public long LoginUserID
        {
            get { return _LoginUserID; }
            set
            {
                if (_LoginUserID != value)
                {
                    _LoginUserID = value;
                    OnPropertyChanged("LoginUserID");
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
        private MasterListItem _SelectedStaff;
        public MasterListItem SelectedStaff
        {
            get { return _SelectedStaff; }
            set
            {
                if (_SelectedStaff != value)
                {
                    _SelectedStaff = value;
                    OnPropertyChanged("SelectedStaff");
                }
            }
        }

        private bool _ApprovalStatus;

        public bool ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set { _ApprovalStatus = value; }
        }

        private string _ApprovalRemark;

        public string ApprovalRemark
        {
            get { return _ApprovalRemark; }
            set { _ApprovalRemark = value; }
        }

        private bool _IsEnable;

        public bool IsEnable
        {
            get { return _IsEnable; }
            set { _IsEnable = value; }
        }
    }
    public class clsAddDiischargeApprovedByDepartmentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsAddDiischargeApprovedByDepartmentBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private clsDiischargeApprovedByDepartmentVO _AddAdviseDetails = null;
        public clsDiischargeApprovedByDepartmentVO AddAdviseDetails
        {
            get { return _AddAdviseDetails; }
            set { _AddAdviseDetails = value; }
        }

        private List<clsDiischargeApprovedByDepartmentVO> _AddAdviseList = null;
        public List<clsDiischargeApprovedByDepartmentVO> AddAdviseList
        {
            get { return _AddAdviseList; }
            set { _AddAdviseList = value; }
        }

        private bool _IsUpdateApproval;

        public bool IsUpdateApproval
        {
            get { return _IsUpdateApproval; }
            set { _IsUpdateApproval = value; }
        }
    }

    public class clsGetListOfAdviseDischargeForApprovalBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetListOfAdviseDischargeForApprovalBizAction";
        }

        public string ToXml()
        {
            //throw new NotImplementedException();
            return this.ToString();
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private clsDiischargeApprovedByDepartmentVO _AddAdviseDetails = null;
        public clsDiischargeApprovedByDepartmentVO AddAdviseDetails
        {
            get { return _AddAdviseDetails; }
            set { _AddAdviseDetails = value; }
        }

        private List<clsDiischargeApprovedByDepartmentVO> _AddAdviseList = null;
        public List<clsDiischargeApprovedByDepartmentVO> AddAdviseList
        {
            get { return _AddAdviseList; }
            set { _AddAdviseList = value; }
        }
        public string LinkServer { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
    }
}
