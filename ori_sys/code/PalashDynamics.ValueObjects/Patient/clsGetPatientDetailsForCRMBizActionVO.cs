using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsGetPatientDetailsForCRMBizActionVO:IBizActionValueObject
    {

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

     
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string MRNo { get; set; }

        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string OPDNo { get; set; }

        public string ContactNo { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public long GenderID { get; set; }
        public long MaritalStatusID { get; set; }

        public string CivilID { get; set; }

        public long UnitID { get; set; }
        public long DepartmentID { get; set; }
        public long DoctorID { get; set; }

        public int Age { get; set; }
        public string AgeFilter { get; set; }

        public long LoyaltyCardID { get; set; }
        public long ComplaintID { get; set; }



        private List<clsPatientVO> _PatientDetails;
        public List<clsPatientVO> PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }

        }

        public long ProtocolID { get; set; }
        public long TreatmentID { get; set; }


        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientDetailsForCRMBizAction";
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
