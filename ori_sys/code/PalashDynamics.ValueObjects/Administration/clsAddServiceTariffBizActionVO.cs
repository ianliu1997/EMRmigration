using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsAddServiceTariffBizActionVO:IBizActionValueObject
    {
       public clsAddServiceTariffBizActionVO()
        {

        }

        private clsServiceMasterVO _objServiceMasterDetails = null;
        public clsServiceMasterVO ServiceMasterDetails
        {
            get { return _objServiceMasterDetails; }
            set { _objServiceMasterDetails = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddServiceTariffBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

       
        public bool CheckForTariffExistanceInServiceTariffMaster { get; set; }
        public bool DeletetariffServiceAndServiceTariffMaster { get; set; }
        public bool DeleteTariffServiceClassRateDetail { get; set; }
        public string Query { get; set; }
        #endregion
    }


   //public class clsAddTariffServiceClassBizActionVO : IBizActionValueObject
   //{


   //    public string GetBizAction()
   //    {
   //        return "PalashDynamics.BusinessLayer.Administration.clsAddTariffServiceClassBizAction";
   //    }

   //    public string ToXml()
   //    {
   //        return this.ToString();
   //    }


   //    //public long TariffID { get; set; }
   //    //public long ServiceID { get; set; }
   //    //public long ClassID { get; set; }
   //    //public bool Status { get; set; }

   //    private int _SuccessStatus;
   //    /// <summary>
   //    /// Output Property.
   //    /// This property states the outcome of BizAction Process.
   //    /// </summary>
   //    public int SuccessStatus
   //    {
   //        get { return _SuccessStatus; }
   //        set { _SuccessStatus = value; }
   //    }

   //    private List<clsServiceTarrifClassRateDetailsVO> _ClassList = null;
   //    /// <summary>
   //    /// Output Property.
   //    /// This Property Contains OPDPatient Details Which is Added.
   //    /// </summary>
   //    public List<clsServiceTarrifClassRateDetailsVO> ClassList
   //    {
   //        get { return _ClassList; }
   //        set { _ClassList = value; }
   //    }

   //}


   public class clsGetTariffServiceClassBizActionVO : IBizActionValueObject
   {


       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.clsGetTariffServiceClassBizAction";
       }

       public string ToXml()
       {
           return this.ToString();
       }

       public long TariffID { get; set; }
       public long ServiceID { get; set; }
      // public long ClassID { get; set; }

       private List<clsServiceTarrifClassRateDetailsVO> _ClassList = null;
       /// <summary>
       /// Output Property.
       /// This Property Contains OPDPatient Details Which is Added.
       /// </summary>
       public List<clsServiceTarrifClassRateDetailsVO> ClassList
       {
           get { return _ClassList; }
           set { _ClassList = value; }
       }
   }

}
