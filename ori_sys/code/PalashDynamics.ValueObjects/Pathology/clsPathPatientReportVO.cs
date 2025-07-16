using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathPatientReportVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

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

        private long? _PathOrderBookingDetailID;
        public long? PathOrderBookingDetailID
        {
            get { return _PathOrderBookingDetailID; }
            set
            {
                if (_PathOrderBookingDetailID != value)
                {
                    _PathOrderBookingDetailID = value;
                    OnPropertyChanged("PathOrderBookingDetailID");
                }
            }
        }

        
        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (_SourceURL != value)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }
            }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }


        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
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

        private long _OrderID;
        public long OrderID
        {
            get { return _OrderID; }
            set
            {
                if (_OrderID != value)
                {
                    _OrderID = value;
                    OnPropertyChanged("OrderID");
                }
            }
        }

        private long _PathPatientReportID;
        public long PathPatientReportID
        {
            get { return _PathPatientReportID; }
            set
            {
                if (_PathPatientReportID != value)
                {
                    _PathPatientReportID = value;
                    OnPropertyChanged("PathPatientReportID");
                }
            }
        }

        
        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }
        private long? _AgencyID;
        public long? AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (_AgencyID != value)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }
        
        private long _SubTestID;
        public long SubTestID
        {
            get { return _SubTestID; }
            set
            {
                if (_SubTestID != value)
                {
                    _SubTestID = value;
                    OnPropertyChanged("SubTestID");
                }
            }
        }


        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set
            {
                if (_SampleNo != value)
                {
                    _SampleNo = value;
                    OnPropertyChanged("SampleNo");
                }
            }
        }

        private DateTime? _SampleCollectionTime;
        public DateTime? SampleCollectionTime
        {
            get { return _SampleCollectionTime; }
            set
            {
                if (_SampleCollectionTime != value)
                {
                    _SampleCollectionTime = value;
                    OnPropertyChanged("SampleCollectionTime");
                }
            }
        }


        private long _PathologistID1;
        public long PathologistID1
        {
            get { return _PathologistID1; }
            set
            {
                if (_PathologistID1 != value)
                {
                    _PathologistID1 = value;
                    OnPropertyChanged("PathologistID1");
                }
            }
        }

        private long _PathologistID2;
        public long PathologistID2
        {
            get { return _PathologistID2; }
            set
            {
                if (_PathologistID2 != value)
                {
                    _PathologistID2 = value;
                    OnPropertyChanged("PathologistID2");
                }
            }
        }

        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
                }
            }
        }

        private long _PathologistID3;
        public long PathologistID3
        {
            get { return _PathologistID3; }
            set
            {
                if (_PathologistID3 != value)
                {
                    _PathologistID3 = value;
                    OnPropertyChanged("PathologistID3");
                }
            }
        }

        private string _ReferredBy;
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
                }
            }
        }

        private long _RefDoctorID;
        public long RefDoctorID
        {
            get { return _RefDoctorID; }
            set
            {
                if (_RefDoctorID != value)
                {
                    _RefDoctorID = value;
                    OnPropertyChanged("RefDoctorID");
                }
            }
        }

        private bool _IsFirstLevel;
        [DefaultValue(false)]
        public bool IsFirstLevel
        {
            get { return _IsFirstLevel; }
            set
            {
                if (_IsFirstLevel != value)
                {
                    _IsFirstLevel = value;
                    OnPropertyChanged("IsFirstLevel");
                }
            }
        }




        private bool _ISTEMplate;
        public bool ISTEMplate
        {
            get { return _ISTEMplate; }
            set
            {
                if (_ISTEMplate != value)
                {
                    _ISTEMplate = value;
                    OnPropertyChanged("ISTEMplate");
                }
            }
        }


        //by rohinee
        private string _ApproveByTemplate;
        public string ApproveByTemplate
        {
            get { return _ApproveByTemplate; }
            set
            {
                if (_ApproveByTemplate != value)
                {
                    _ApproveByTemplate = value;
                    OnPropertyChanged("ApproveByTemplate");
                }
            }
        }




        private List<clsPathoTestItemDetailsVO> _ItemList;
        public List<clsPathoTestItemDetailsVO> ItemList
        {
            get
            {
                if (_ItemList == null)
                    _ItemList = new List<clsPathoTestItemDetailsVO>();

                return _ItemList;
            }

            set
            {

                _ItemList = value;

            }
        }


        private List<clsPathoTestParameterVO> _List;
        public List<clsPathoTestParameterVO> TestList
        {
            get
            {
                if (_List == null)
                    _List = new List<clsPathoTestParameterVO>();

                return _List;
            }

            set
            {

                _List = value;

            }
        }
        #endregion

        #region NEwly Added

        private long? _CategoryID;
        public long? CategoryID
        {
            get { return _CategoryID; }
            set
            {
                if (_CategoryID != value)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }

        private DateTime? _ResultAddedDateTime;
        public DateTime? ResultAddedDateTime
        {
            get { return _ResultAddedDateTime; }
            set
            {
                if (_ResultAddedDateTime != value)
                {
                    _ResultAddedDateTime = value;
                    OnPropertyChanged("ResultAddedDateTime");
                }
            }
        }

        private DateTime? _SampleReceiveDateTime;
        public DateTime? SampleReceiveDateTime
        {
            get { return _SampleReceiveDateTime; }
            set
            {
                if (_SampleReceiveDateTime != value)
                {
                    _SampleReceiveDateTime = value;
                    OnPropertyChanged("SampleReceiveDateTime");
                }
            }
        }

        private clsPathoResultEntryTemplateVO _Template;
        public clsPathoResultEntryTemplateVO TemplateDetails
        {
            get
            {
                if (_Template == null)
                    _Template = new clsPathoResultEntryTemplateVO();

                return _Template;
            }

            set
            {

                _Template = value;

            }
        }

        private string _ReportType;
        public string ReportType
        {
            get { return _ReportType; }
            set
            {
                if (_ReportType != value)
                {
                    _ReportType = value;
                    OnPropertyChanged("ReportType");
                }
            }
        }

        private string _ApproveBy;
        public string ApproveBy
        {
            get { return _ApproveBy; }
            set
            {
                if (_ApproveBy != value)
                {
                    _ApproveBy = value;
                    OnPropertyChanged("ApproveBy");
                }
            }
        }

        private bool _IsSecondLevel;
        [DefaultValue(false)]
        public bool IsSecondLevel
        {
            get { return _IsSecondLevel; }
            set
            {
                if (_IsSecondLevel != value)
                {
                    _IsSecondLevel = value;
                    OnPropertyChanged("IsSecondLevel");
                }
            }
        }
        //by rohini to descide test is auto approved or not
        private bool _IsAutoApproved;
        public bool IsAutoApproved
        {
            get { return _IsAutoApproved; }
            set
            {
                if (_IsAutoApproved != value)
                {
                    _IsAutoApproved = value;
                    OnPropertyChanged("IsAutoApproved");
                }
            }
        }
        //

        private bool _IsThirdLevel;
        [DefaultValue(false)]
        public bool IsThirdLevel
        {
            get { return _IsThirdLevel; }
            set
            {
                if (_IsThirdLevel != value)
                {
                    _IsThirdLevel = value;
                    OnPropertyChanged("IsThirdLevel");
                }
            }
        }

        // Added on 26.04.2016 to get the AutoGeneration of Pathology Report
        private string _BillNo;
        public string  BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }


        private long _BillId;
        public long BillID
        {
            get { return _BillId; }
            set
            {
                if (_BillId != value)
                {
                    _BillId = value;
                    OnPropertyChanged("BillID");
                }
            }
        }




        #endregion


        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
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

        // added by Anumani

        private double _HighReffValue;
        public double HighReffValue
        {
            get { return _HighReffValue; }
            set
            {
                if (_HighReffValue != value)
                {
                    _HighReffValue = value;
                    OnPropertyChanged("HighReffValue");

                }
            }

        }

        private double _LowReffValue;
        public double LowReffValue
        {
            get { return _LowReffValue; }
            set
            {
                if (_LowReffValue != value)
                {
                    _LowReffValue = value;
                    OnPropertyChanged("LowReffValue");

                }
            }

        }

        private double _UpperPanicValue;
        public double UpperPanicValue
        {
            get { return _UpperPanicValue; }
            set
            {
                if (_UpperPanicValue != value)
                {
                    _UpperPanicValue = value;
                    OnPropertyChanged("UpperPanicValue");

                }
            }

        }

        private double _LowerPanicValue;
        public double LowerPanicValue
        {
            get { return _LowerPanicValue; }
            set
            {
                if (_LowerPanicValue != value)
                {
                    _LowerPanicValue = value;
                    OnPropertyChanged("LowerPanicValue");

                }
            }

        }



        private bool _ResultStatus;
        public bool ResultStatus
        {
            get { return _ResultStatus; }
            set
            {
                if (_ResultStatus != value)
                {
                    _ResultStatus = value;
                    OnPropertyChanged("ResultStatus");

                }
            }

        }

        //added by rohinee
        private int _IsAbnormal;
        public int IsAbnormal
        {
            get { return _IsAbnormal; }
            set
            {
                if (_IsAbnormal != value)
                {
                    _IsAbnormal = value;
                    OnPropertyChanged("IsAbnormal");

                }
            }

        }
        private int _IsBalence;
        public int IsBalence
        {
            get { return _IsBalence; }
            set
            {
                if (_IsBalence != value)
                {
                    _IsBalence = value;
                    OnPropertyChanged("IsBalence");

                }
            }

        }


        //
        public bool IsDoctorAuthorization { get; set; }

        public bool IsResultModified { get; set; }
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
