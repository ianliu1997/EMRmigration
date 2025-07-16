using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Billing
{
    public class GetCancellationAndConcessionApprovalBizactionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.GetCancellationAndConcessionApprovalBizaction";
        }

        #endregion

        #region IValueObject Members

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



        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsRefundVO objDetails = null;
        public clsRefundVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region Properties

        private bool _IsForApproval = false;
        public bool IsForApproval
        {
            get { return _IsForApproval; }
            set
            {
                if (_IsForApproval != value)
                {
                    _IsForApproval = value;
                    OnPropertyChanged("IsForApproval");
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

        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        private bool _IsSetApprovalReq = true;
        public bool IsSetApprovalReq
        {
            get { return _IsSetApprovalReq; }
            set
            {
                if (_IsSetApprovalReq != value)
                {
                    _IsSetApprovalReq = value;
                    OnPropertyChanged("IsSetApprovalReq");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
                }
            }
        }

        private string _RequestedOn = "";
        public string RequestedOn
        {
            get { return _RequestedOn; }
            set
            {
                if (_RequestedOn != value)
                {
                    _RequestedOn = value;
                    OnPropertyChanged("RequestedOn");
                }
            }
        }

        private DateTime? _RequestedDateTime;
        public DateTime? RequestedDateTime
        {
            get { return _RequestedDateTime; }
            set
            {
                if (_RequestedDateTime != value)
                {
                    _RequestedDateTime = value;
                    OnPropertyChanged("RequestedDateTime");
                }
            }
        }

        private bool _IsFirstApproval = true;
        public bool IsFirstApproval
        {
            get { return _IsFirstApproval; }
            set
            {
                if (_IsFirstApproval != value)
                {
                    _IsFirstApproval = value;
                    OnPropertyChanged("IsFirstApproval");
                }
            }
        }

        private long _FirstApprovalyBy;
        public long FirstApprovalyBy
        {
            get { return _FirstApprovalyBy; }
            set
            {
                if (_FirstApprovalyBy != value)
                {
                    _FirstApprovalyBy = value;
                    OnPropertyChanged("FirstApprovalyBy");
                }
            }
        }

        private string _FirstApprovalOn = "";
        public string FirstApprovalOn
        {
            get { return _FirstApprovalOn; }
            set
            {
                if (_FirstApprovalOn != value)
                {
                    _FirstApprovalOn = value;
                    OnPropertyChanged("FirstApprovalOn");
                }
            }
        }

        private DateTime? _FirstApprovalDateTime;
        public DateTime? FirstApprovalDateTime
        {
            get { return _FirstApprovalDateTime; }
            set
            {
                if (_FirstApprovalDateTime != value)
                {
                    _FirstApprovalDateTime = value;
                    OnPropertyChanged("FirstApprovalDateTime");
                }
            }
        }

        private bool _IsSecondApproval = true;
        public bool IsSecondApproval
        {
            get { return _IsSecondApproval; }
            set
            {
                if (_IsSecondApproval != value)
                {
                    _IsSecondApproval = value;
                    OnPropertyChanged("IsSecondApproval");
                }
            }
        }

        private long _SecondApprovalyBy;
        public long SecondApprovalyBy
        {
            get { return _SecondApprovalyBy; }
            set
            {
                if (_SecondApprovalyBy != value)
                {
                    _SecondApprovalyBy = value;
                    OnPropertyChanged("SecondApprovalyBy");
                }
            }
        }

        private string _SecondApprovalOn = "";
        public string SecondApprovalOn
        {
            get { return _SecondApprovalOn; }
            set
            {
                if (_SecondApprovalOn != value)
                {
                    _SecondApprovalOn = value;
                    OnPropertyChanged("SecondApprovalOn");
                }
            }
        }

        private DateTime? _SecondApprovalDateTime;
        public DateTime? SecondApprovalDateTime
        {
            get { return _SecondApprovalDateTime; }
            set
            {
                if (_SecondApprovalDateTime != value)
                {
                    _SecondApprovalDateTime = value;
                    OnPropertyChanged("SecondApprovalDateTime");
                }
            }
        }



        private long _AddedBy;
        public long AddedBy
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

        private DateTime? _AddedDateTime;
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #endregion


        private List<clsChargeVO> ObjList = null;
        public List<clsChargeVO> ChargeList
        {
            get { return ObjList; }
            set { ObjList = value; }
        }

        private List<clsChargeVO> ObjApprovalList = null;
        public List<clsChargeVO> ApprovalChargeList
        {
            get { return ObjApprovalList; }
            set { ObjApprovalList = value; }
        }

    }
}
