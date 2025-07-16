using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
   public  class clsGetPatientSourceDetailsByIDBizActionVO:IBizActionValueObject
    {
       public clsPatientSourceVO _Details;
       public clsPatientSourceVO Details
       {
           get { return _Details; }
           set { _Details = value; }
       }


       public long ID { get; set; }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetPatientSourceDetailsByIDBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }


   public class clsGetRegistrationChargesDetailsByIDBizActionVO : IBizActionValueObject
   {
       public clsRegistrationChargesVO _Details;
       public clsRegistrationChargesVO Details
       {
           get { return _Details; }
           set { _Details = value; }
       }


       public long ID { get; set; }

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetRegistrationChargesDetailsByIDBizAction";
       }

       public string ToXml()
       {
           return this.ToString();
       }
   }

   // Added By CDS 
   public class clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO : IBizActionValueObject
   {
       public clsRegistrationChargesVO _Details;
       public clsRegistrationChargesVO Details
       {
           get { return _Details; }
           set { _Details = value; }
       }

       public long ID { get; set; }
       public long PatientTypeID { get; set; }

       List<clsRegistrationChargesVO> _List = new List<clsRegistrationChargesVO>();

       /// <summary>
       /// Output Property.
       /// Get Property To Access And Modify RoleList
       /// </summary> 
       public List<clsRegistrationChargesVO> List
       {
           get { return _List; }
           set { _List = value; }

       }
            

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsGetRegistrationChargesDetailsByPatientTypeIDBizAction";
       }

       public string ToXml()
       {
           return this.ToString();
       }
   }

}
