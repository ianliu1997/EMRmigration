using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFSpermDefectTestVO: IValueObject,INotifyPropertyChanged
    {
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
        private DateTime? _Date = System.DateTime.Now;
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

        private bool _IsFreezed;
        public bool IsFreezed
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
        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }
        private long _VisitUnitID;
        public long VisitUnitID
        {
            get { return _VisitUnitID; }
            set
            {
                if (_VisitUnitID != value)
                {
                    _VisitUnitID = value;
                    OnPropertyChanged("VisitUnitID");
                }
            }
        }

        private double _AnnexinBinding;
        public double AnnexinBinding
        {
            get { return _AnnexinBinding; }
            set
            {
                if (_AnnexinBinding != value)
                {
                    _AnnexinBinding = value;
                    OnPropertyChanged("AnnexinBinding");
                }
            }
        }
        private double _CaspaseActivity;
        public double CaspaseActivity
        {
            get { return _CaspaseActivity; }
            set
            {
                if (_CaspaseActivity != value)
                {
                    _CaspaseActivity = value;
                    OnPropertyChanged("CaspaseActivity");
                }
            }
        }
        private double _AcrosinActivity;
        public double AcrosinActivity
        {
            get { return _AcrosinActivity; }
            set
            {
                if (_AcrosinActivity != value)
                {
                    _AcrosinActivity= value;
                    OnPropertyChanged("AcrosinActivity");
                }
            }
        }
        private double _GlucosidaseActivity;
        public double GlucosidaseActivity
        {
            get { return _GlucosidaseActivity; }
            set
            {
                if (_GlucosidaseActivity != value)
                {
                    _GlucosidaseActivity= value;
                    OnPropertyChanged("GlucosidaseActivity");
                }
            }
        }

        private long _AndrologistID;
        public long AndrologistID
        {
            get { return _AndrologistID; }
            set
            {
                if (_AndrologistID != value)
                {
                    _AndrologistID = value;
                    OnPropertyChanged("AndrologistID");
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
        private string _OPDNo;
        public string OPDNo
        {
            get { return _OPDNo; }
            set
            {
                if (_OPDNo != value)
                {
                    _OPDNo = value;
                    OnPropertyChanged("OPDNo");
                }
            }
        }
        #endregion
    }


    public class clsAddUpdateIVFSpermDefectTestBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsAddUpdateIVFSpermDefectTestBizAction";
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

        private clsIVFSpermDefectTestVO _clsIVFSpermDefectTest;

        public clsIVFSpermDefectTestVO ClsIVFSpermDefectTest
        {
            get { return _clsIVFSpermDefectTest; }
            set { _clsIVFSpermDefectTest = value; }
        }
    
       

        public string LinkServer { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
    }

    public class clsGetIVFSpermDefectTestBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.DashBoardBizAction.clsGetIVFSpermDefectTestBizAction";
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

        private clsIVFSpermDefectTestVO _clsIVFSpermDefectTest;

        public clsIVFSpermDefectTestVO ClsIVFSpermDefectTest
        {
            get { return _clsIVFSpermDefectTest; }
            set { _clsIVFSpermDefectTest = value; }
        }

        private List<clsIVFSpermDefectTestVO> _objSpermDefectTestList;

        public List<clsIVFSpermDefectTestVO> ObjSpermDefectTestList
        {
            get { return _objSpermDefectTestList; }
            set { _objSpermDefectTestList = value; }
        }


        public string LinkServer { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
    }

}
