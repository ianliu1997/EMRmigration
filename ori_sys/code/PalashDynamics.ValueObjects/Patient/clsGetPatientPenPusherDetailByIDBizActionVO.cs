using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientPenPusherDetailByIDBizActionVO : IBizActionValueObject
    {
        private clsPatientPerscriptionInfoVO objDetails = null;
        public clsPatientPerscriptionInfoVO PatientPrescriptionDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        private List<clsPatientPerscriptionInfoVO> _PatientPrescriptionDetailsList;
        public List<clsPatientPerscriptionInfoVO> PatientPrescriptionDetailsList
        {
            get { return _PatientPrescriptionDetailsList; }
            set { _PatientPrescriptionDetailsList = value; }
        }
        private long _ID;
        public long PatientID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }

        private long _Status;
        public long Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

      


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsGetPatientPenPusherDetailByIDBizAction";
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
