using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.DepartmentScheduleMaster
{
    public class clsAddDepartmentScheduleMasterBizActionVO : IBizActionValueObject
    {
        public clsAddDepartmentScheduleMasterBizActionVO()
        {
        }

        private clsDepartmentScheduleVO objDepartmentSchedule = null;
        public clsDepartmentScheduleVO DepartmentScheduleDetails
        {
            get { return objDepartmentSchedule; }
            set { objDepartmentSchedule = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddDepartmentScheduleMasterBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetDepartmentScheduleMasterBizActionVO : IBizActionValueObject
    {
        private clsDepartmentScheduleVO _Details;
        public clsDepartmentScheduleVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        public string DayID { get; set; }
        public string Schedule1_StartTime { get; set; }
        public string Schedule1_EndTime { get; set; }
        public string Schedule2_EndTime { get; set; }
        public string Schedule2_StartTime { get; set; }

        public long DepartmentID { get; set; }
        public long UnitID { get; set; }

        public bool SuccessStatus { get; set; }
        public long Status { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentScheduleMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

    public class clsGetDepartmentScheduleMasterListBizActionVO : IBizActionValueObject
    {
        private List<clsDepartmentScheduleVO> myVar = new List<clsDepartmentScheduleVO>();

        public List<clsDepartmentScheduleVO> DepartmentScheduleList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private string _SearchExpression = "";
        public string InputSearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }
        public long? UnitID { get; set; }

        public long? DepartmentID { get; set; }
        public long? DoctorModalityID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        private bool _IsDepartmentSchedule;
        public bool IsDepartmentSchedule
        {
            get { return _IsDepartmentSchedule; }
            set
            {
                if(_IsDepartmentSchedule != value)
                    _IsDepartmentSchedule = value;
            }
        }

        private bool _IsDepartment;
        public bool IsDepartment
        {
            get { return _IsDepartment; }
            set
            {
                if (_IsDepartment != value)
                    _IsDepartment = value;
            }
        }

        private bool _IsDoctor;
        public bool IsDoctor
        {
            get { return _IsDoctor; }
            set
            {
                if (_IsDoctor != value)
                    _IsDoctor = value;
            }
        }

        private bool _IsModality;
        public bool IsModality
        {
            get { return _IsModality; }
            set
            {
                if (_IsModality != value)
                    _IsModality = value;
            }
        }

        private bool _IsNursing;
        public bool IsNursing
        {
            get { return _IsNursing; }
            set
            {
                if(_IsNursing != value)
                    _IsNursing = value;
            }
        }

        private bool _IsOTSchedule;
        public bool IsOTSchedule
        {
            get { return _IsOTSchedule; }
            set
            {
                if(_IsOTSchedule != value)
                    _IsOTSchedule = value;
            }
        }

        public string sortExpression { get; set; }

        public string LinkServer { get; set; }

        private bool _checkDepartmentSchedule = false;
        public bool checkDepartmentSchedule
        {
            get { return _checkDepartmentSchedule; }
            set { _checkDepartmentSchedule = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentScheduleMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsCheckTimeForScheduleExistanceDepartmentBizActionVO : IBizActionValueObject
    {
        private List<clsDepartmentScheduleDetailsVO> _Details;
        public List<clsDepartmentScheduleDetailsVO> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        //public string DayID { get; set; }
        public long DayID { get; set; }
        public string Schedule1_StartTime { get; set; }
        public string Schedule1_EndTime { get; set; }

        public string Schedule2_EndTime { get; set; }
        public string Schedule2_StartTime { get; set; }
        public long DepartmentID { get; set; }
        public long UnitID { get; set; }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set
            {
                if(value != _StartTime)
                {
                    _StartTime = value;
                }
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set
            {
                if(value != _EndTime)
                {
                    _EndTime = value;
                }
            }
        }

        public bool? IsSchedulePresent { get; set; }

        public bool SuccessStatus { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsCheckTimeForScheduleExistanceDepartmentBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDepartmentScheduleTimeVO : IBizActionValueObject
    {

        private List<clsDepartmentScheduleDetailsVO> _DepartmentScheduleDetails;
        public List<clsDepartmentScheduleDetailsVO> DepartmentScheduleDetailsList
        {
            get { return _DepartmentScheduleDetails; }
            set { _DepartmentScheduleDetails = value; }
        }

        public long UnitId { get; set; }
        public long DepartmentId { get; set; }

        private string _SearchExpression;
        public string InputSearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }


        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public string DayID { get; set; }

        public string LinkServer { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentScheduleTimeBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDepartmentScheduleListBizActionVO : IBizActionValueObject
    {
        private List<clsDepartmentScheduleDetailsVO> myVar = new List<clsDepartmentScheduleDetailsVO>();

        public List<clsDepartmentScheduleDetailsVO> DepartmentScheduleList
        {
            get { return myVar; }
            set { myVar = value; }
        }
        public long DepartmentScheduleID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentScheduleListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDepartmentDepartmentDetailsBizActionVO : IBizActionValueObject
    {
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitId { get; set; }
        public long DepartmentId { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentDepartmentDetailsBizAction";
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
