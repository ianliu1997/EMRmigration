using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
   public class clsGetPatientFamilyDetailsBizActionVO:IBizActionValueObject
    {
        private List<clsPatientFamilyDetailsVO> _Details;
        public List<clsPatientFamilyDetailsVO> FamilyDetails
        {
            get { return _Details; }
            set { _Details = value; }

        }

        public long PatientID { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientFamilyDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

   public class clsCheckPatientMemberRegisteredBizActionVO : IBizActionValueObject
   {
       private clsPatientFamilyDetailsVO _Details;
       public clsPatientFamilyDetailsVO FamilyDetails
       {
           get { return _Details; }
           set { _Details = value; }

       }

       public long PatientID { get; set; }
       public long RelationID { get; set; }
       public bool SuccessStatus { get; set; }

       
      

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsCheckPatientMemberRegisteredBizAction";
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
