using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsAddUpdateIPDPatientVitalsMasterBIzActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsAddUpdateIPDPatientVitalsMasterBIzAction";

        }

        #endregion
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        private clsIPDPatientVitalsMasterVO objDetails = null;
        public clsIPDPatientVitalsMasterVO PatientVitalDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsIPDPatientVitalsMasterVO> PatientVitalList = new List<clsIPDPatientVitalsMasterVO>();
        public List<clsIPDPatientVitalsMasterVO> PatientVitalDetailList
        {
            get { return PatientVitalList; }
            set { PatientVitalList = value; }
        }

        #endregion


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
    }

    public class clsGetIPDPatientVitalsMasterBIzActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsGetIPDPatientVitalsMasterBIzAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }


        //private clsGetIPDPatientVitalsMasterBIzActionVO objDetails = null;
        //public clsGetIPDPatientVitalsMasterBIzActionVO VitalDetails
        //{
        //    get { return objDetails; }
        //    set { objDetails = value; }
        //}

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }


        private List<clsIPDPatientVitalsMasterVO> myList = new List<clsIPDPatientVitalsMasterVO>();
        public List<clsIPDPatientVitalsMasterVO> VitalDetailsList
        {
            get { return myList; }
            set { myList = value; }
        }

    }

    public class clsUpdateStatusIPDPatientVitalsMasterBIzActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsUpdateStatusIPDPatientVitalsMasterBIzAction";

        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsIPDPatientVitalsMasterVO objDetails = null;
        public clsIPDPatientVitalsMasterVO VitalDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsIPDPatientVitalsMasterVO> UpdateList = new List<clsIPDPatientVitalsMasterVO>();
        public List<clsIPDPatientVitalsMasterVO> UpdateDetailList
        {
            get { return UpdateList; }
            set { UpdateList = value; }
        }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
    }
}
