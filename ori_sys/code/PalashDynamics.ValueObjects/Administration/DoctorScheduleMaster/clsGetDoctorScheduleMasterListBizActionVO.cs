using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetDoctorScheduleMasterListBizActionVO : IBizActionValueObject
    {



        private List<clsDoctorScheduleVO> myVar = new List<clsDoctorScheduleVO>();

        public List<clsDoctorScheduleVO> DoctorScheduleList
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

        public long? DoctorID { get; set; }

        public bool IsNewSchedule { get; set; }     // added on 13032018 fro New Doctor Schedule

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        public string LinkServer { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDoctorScheduleMasterListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsGetDoctorScheduleListBizActionVO : IBizActionValueObject
    {


        private List<clsDoctorScheduleDetailsVO> myVar = new List<clsDoctorScheduleDetailsVO>();

        public List<clsDoctorScheduleDetailsVO> DoctorScheduleList
        {
            get { return myVar; }
            set { myVar = value; }
        }
        public long DoctorScheduleID { get; set; }

        public bool IsNewSchedule { get; set; }     // added on 13032018 for New Doctor Schedule

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDoctorScheduleListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    //rohinee

    public class clsGetDoctorScheduleListByIDBizActionVO : IBizActionValueObject
    {


        private List<clsDoctorScheduleDetailsVO> myVar = new List<clsDoctorScheduleDetailsVO>();

        public List<clsDoctorScheduleDetailsVO> DoctorScheduleListForDoctorID
        {
            get { return myVar; }
            set { myVar = value; }
        }
        public long DoctorScheduleID { get; set; }
        public long ID { get; set; }
        public long DayID { get; set; }
        public long ScheduleID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDoctorScheduleListByIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetDoctorDepartmentUnitListBizActionVO : IBizActionValueObject
    {



        private List<clsDoctorDepartmentUnitListVO> myVar = new List<clsDoctorDepartmentUnitListVO>();

        public List<clsDoctorDepartmentUnitListVO> DoctorDepartmentUnitList
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

        public long? DoctorID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }

        public string LinkServer { get; set; }



        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDoctorDepartmentUnitListBizAction";
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
