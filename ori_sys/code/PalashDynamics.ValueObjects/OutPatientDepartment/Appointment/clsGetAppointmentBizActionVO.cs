using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
    public class clsGetAppointmentBizActionVO : IBizActionValueObject
    {
        public clsGetAppointmentBizActionVO(bool PagingEnabled)
        {
            this._PagingEnabled = PagingEnabled;

        }

        public clsGetAppointmentBizActionVO()
        {

        }

        private const string _BizActionName = "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsGetAppointmentBizAction";

        public string BizAction
        {
            get { return _BizActionName; }
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

        private string _SortExpression = "AppointmentId Desc";

        private string _SearchExpression = "";

        private string _FilterExpression;

        public string InputSearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }


        private bool? _Status = null;
        /// <summary>
        /// For Filtering Result List By Record Status
        /// </summary>
        public bool? Status { get { return _Status; } set { _Status = value; } }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public long? UnitId { get; set; }

        public bool? VisitMark { get; set; }

        public long? DepartmentId { get; set; }

        public long? DoctorId { get; set; }

        public long? PatientId { get; set; }

        public int AppintmentStatusID { get; set; }

        public long? AppoinTypeID { get; set; }

        public long AppType { get; set; }

        private bool _UnRegisterd;
        public bool UnRegistered
        {
            get { return _UnRegisterd; }
            set { _UnRegisterd = value; }
        }

        /// <summary>
        /// This is Input parameter.This Specifies Sort Expression For AppointmentDetail List
        /// </summary>
        public string InputSortExpression
        {
            get { return _SortExpression; }
            set { _SortExpression = value; }
        }

        public string InputFilterExpression
        {
            get { return _FilterExpression; }
            set { _FilterExpression = value; }
        }
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        private long _SpecialRegistrtaionId;
        public long SpecialRegistrationId
        {
            get { return _SpecialRegistrtaionId; }
            set { _SpecialRegistrtaionId = value; }

        }
        private string _ContactNo;
        public string ContactNo
        {
            get { return _ContactNo; }
            set { _ContactNo = value; }
        }

        private string _MrNo;
        public string MrNo
        {
            get { return _MrNo; }
            set { _MrNo = value; }
        }

        private bool _IsDocAttached;
        public bool IsDocAttached
        {
            get { return _IsDocAttached; }
            set
            {
                if (value != _IsDocAttached)
                {
                    _IsDocAttached = value;
                 
                }
            }

        }


        private string _IFilterWhereClause;
        public string InputFilterWhereClause
        {
            get { return _IFilterWhereClause; }
            set { _IFilterWhereClause = value; }
        }

        private List<clsAppointmentVO> myVar = new List<clsAppointmentVO>();

        public List<clsAppointmentVO> AppointmentDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private int _TotalRows = 0;

        public int OutputTotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; }
        }

        public string LinkServer { get; set; }
        public string SortExpression { get; set; }
       
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region IBizAction Members

        public string GetBizAction()
        {
            return BizAction;
        }
        #endregion


    }

    }

    
