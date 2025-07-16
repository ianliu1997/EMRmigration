using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
   public  class clsGetPatientForLoyaltyCardIssueBizActionVO:IBizActionValueObject
    {
       private List<clsPatientGeneralVO> _PatientDetails;
       public List<clsPatientGeneralVO> PatientDetails
       {
           get { return _PatientDetails; }
           set { _PatientDetails = value; }
          
       }
       
       public string MrNo { get; set; }
       public string OPDNo { get; set; }
       public string LoyaltyCardNo { get; set; }
       public DateTime? FromDate { get; set; }
       public DateTime? ToDate { get; set; }
       public long UnitID { get; set;}
       public string FirstName { get; set;}
       public string MiddleName { get; set;}
       public string LastName { get; set;}
       public bool IsLoyaltymember { get; set; }
       public long LoyaltyProgramID { get; set; }
       public bool IssuDate { get; set; }



       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }

       public string sortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return  "PalashDynamics.BusinessLayer.clsGetPatientForLoyaltyCardIssueBizAction";
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
