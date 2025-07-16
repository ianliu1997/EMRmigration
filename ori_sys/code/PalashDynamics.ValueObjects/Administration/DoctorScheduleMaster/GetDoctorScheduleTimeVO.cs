using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.ValueObjects.Administration
{
    public class GetDoctorScheduleTimeVO : IBizActionValueObject
    {

        private List<clsDoctorScheduleDetailsVO> _DoctorScheduleDetails;
        public List<clsDoctorScheduleDetailsVO> DoctorScheduleDetailsList
        {
            get { return _DoctorScheduleDetails; }
            set { _DoctorScheduleDetails = value; }
        }
        public DateTime? Date { get; set; }
        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public long DoctorId { get; set; }

        public long AppointmentType { get; set; }
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
            return "PalashDynamics.BusinessLayer.Administration.GetDoctorScheduleTime";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class GetDoctorScheduleWiseVO : IBizActionValueObject
    {

        private List<clsDoctorScheduleDetailsVO> _DoctorScheduleDetails;
        public List<clsDoctorScheduleDetailsVO> DoctorScheduleDetailsList
        {
            get { return _DoctorScheduleDetails; }
            set { _DoctorScheduleDetails = value; }
        }

        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public long Day { get; set; }

        //For New Schedule Changes modified on 29052018
        public DateTime? Date { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.GetDoctorScheduleWise";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsGetMasterListForVisitBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //return "PalashDynamics.BusinessLayer.Master.clsGetMasterListForVisitBizAction";
            return "PalashDynamics.BusinessLayer.Administration.GetVisitTypeDetails";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private List<MasterListItem> objGenderDetails = new List<MasterListItem>();
        public List<MasterListItem> GenderDetails
        {
            get { return objGenderDetails; }
            set { objGenderDetails = value; }
        }

        public clsGetMasterListForVisitBizActionVO()
        {

        }
        private MasterTableNameList _MasterTable = MasterTableNameList.None;
        public MasterTableNameList MasterTable
        {
            get
            {
                return _MasterTable;
            }
            set
            {
                _MasterTable = value;
            }
        }

        public KeyValue Category { get; set; }

        private string _Error = "";
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public KeyValue Parent { get; set; }

        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets active record from list
        /// </summary>
        public bool? IsActive { get; set; }

        public bool _IsFromPOGRN = false;
        public bool IsFromPOGRN
        {
            get { return _IsFromPOGRN; }
            set { _IsFromPOGRN = value; }
        }


        /// <summary>
        /// Datatype: Boolean
        /// Gets or Sets IsDate 
        /// </summary>
        public bool? IsDate { get; set; }

        public bool IsParameterSearch = false;
        public string parametername = string.Empty;
    }
}
