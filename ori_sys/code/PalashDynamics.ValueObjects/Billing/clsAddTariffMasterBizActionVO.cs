using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Billing
{
   public class clsAddTariffMasterBizActionVO:IBizActionValueObject
    {
        private clsTariffMasterBizActionVO _Details;
        public clsTariffMasterBizActionVO TariffDetails
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Billing.clsAddTariffMasterBizAction";
        }
        #endregion

        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
    }

   public class clsGetServiceByTariffIDBizActionVO : IBizActionValueObject
   {

       private clsTariffMasterBizActionVO objDetails = null;
       public clsTariffMasterBizActionVO TariffDetails
       {
           get { return objDetails; }
           set { objDetails = value; }
       }

       private long _TariffID;
       public long TariffID
       {
           get { return _TariffID; }
           set { _TariffID = value; }
       }

       public bool IsPagingEnabled { get; set; }

       public int StartRowIndex { get; set; }

       public int MaximumRows { get; set; }

       public string SearchExpression { get; set; }

       public int TotalRows { get; set; }

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsGetServiceByTariffIDBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
        
   }


   public class clsGetServiceForIssueBizActionVO : IBizActionValueObject
   {
       private List<clsPatientServiceDetails> objDetails = null;
       public List<clsPatientServiceDetails> ServiceList
       {
           get { return objDetails; }
           set { objDetails = value; }
       }

       private long _TariffID;
       public long TariffID
       {
           get { return _TariffID; }
           set { _TariffID = value; }
       }
       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsGetServiceForIssueBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
        
   }


   public class clsGetTariffListBizActionVO : IBizActionValueObject
   {

       private List<clsTariffMasterBizActionVO> objDetails = null;
       public List<clsTariffMasterBizActionVO> TariffList
       {
           get { return objDetails; }
           set { objDetails = value; }
       }

       private long _TariffID;
       public long TariffID
       {
           get { return _TariffID; }
           set { _TariffID = value; }
       }

       public long StartRowIndex { get; set; }
       public long MaximumRows { get; set; }
       public long TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public string SearchExpression { get; set; }

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsGetTariffListBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion

   }

   public class clsGetSpecializationsByTariffIdBizActionVO : IBizActionValueObject
   {

       private List<clsTariffMasterBizActionVO> objDetails = null;
       public List<clsTariffMasterBizActionVO> TariffList
       {
           get { return objDetails; }
           set { objDetails = value; }
       }

       private List<clsSubSpecializationVO> _SpecializationList = null;
       public List<clsSubSpecializationVO> SpecializationList
       {
           get { return _SpecializationList; }
           set { _SpecializationList = value; }
       }

       private long _TariffID;
       public long TariffID
       {
           get { return _TariffID; }
           set { _TariffID = value; }
       }

       private bool _IsFromTariffCopyUtility;
       public bool IsFromTariffCopyUtility
       {
           get { return _IsFromTariffCopyUtility; }
           set { _IsFromTariffCopyUtility = value; }
       }

       public long StartRowIndex { get; set; }
       public long MaximumRows { get; set; }
       public long TotalRows { get; set; }
       public bool PagingEnabled { get; set; }
       public string SearchExpression { get; set; }

       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Billing.clsGetSpecializationsByTariffIdBizAction";
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
