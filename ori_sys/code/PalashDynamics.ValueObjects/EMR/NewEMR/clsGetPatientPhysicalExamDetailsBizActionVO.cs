
//Created Date:21/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Add and Update the Patient EMR Physical Exam

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientPhysicalExamDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientPhysicalExamDetailsBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {

                _ID = value;


            }
        }

        public bool IsOPDIPD { get; set; }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {

                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {

                _PatientUnitID = value;
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {

                _TemplateID = value;


            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {

                _VisitID = value;


            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {

                _UnitID = value;


            }
        }

        public bool IsUpdated { get; set; }
        private string _TemplateByNurse;
        public string TemplateByNurse
        {
            get { return _TemplateByNurse; }
            set
            {

                _TemplateByNurse = value;


            }
        }

        //private clsPatientEMRDataVO PatientEMRData;
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public clsPatientEMRDataVO objPatientEMRData
        //{
        //    get { return PatientEMRData; }
        //    set { PatientEMRData = value; }
        //}

    }
}
