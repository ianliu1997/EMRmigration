using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
     public class clsAddUpadateCashCounterBizActionVO : IBizActionValueObject
     {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpadateCashCounterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCashCounterVO> objListCashCounterDetails = new List<clsCashCounterVO>();
        public List<clsCashCounterVO> ListCashCounterDetails
        {
            get
            {
                return objListCashCounterDetails;
            }
            set
            {
                objListCashCounterDetails = value;
            }
        }

        private clsCashCounterVO _objCashCounter;
        public clsCashCounterVO ObjCashCounter
        {
            get { return _objCashCounter;  }
            set { _objCashCounter = value; }
        }
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }

     public class clsCashCounterVO
     {
         public long Id { get; set; }
         public long UnitId { get; set; }
         public string Code { get; set; }
         public string Description { get; set; }
         public long ClinicId { get; set; }
         public string ClinicName { get; set; }
         public Boolean Status { get; set; }

         public long? CreatedUnitID { get; set; }
         public long? UpdatedUnitID { get; set; }
         public long? AddedBy { get; set; }
         public string AddedOn { get; set; }
         public DateTime? AddedDateTime { get; set; }
         public long? UpdatedBy { get; set; }
         public string UpdatedOn { get; set; }
         public DateTime? UpdatedDateTime { get; set; }
         public string AddedWindowsLoginName { get; set; }
         public string UpdateWindowsLoginName { get; set; }
     }

     public class clsGetCashCounterDetailsBizActionVO : IBizActionValueObject
     {
         #region  IBizActionValueObject
         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.Administration.clsGetCashCounterDetailsBizAction";
         }

         public string ToXml()
         {
             return this.ToXml();
         }
         #endregion


         public long StartRowIndex { get; set; }
         public long MaximumRows { get; set; }
         public long TotalRows { get; set; }
         public bool PagingEnabled { get; set; }
         public string SearchExpression { get; set; }

         public long StateId { get; set; }
         public long UnitId { get; set; }
         public string Code { get; set; }
         public string Description { get; set; }
         public long ClinicId { get; set; }

         /// <summary>
         /// This property contains Item master details.
         /// </summary>
         /// <summary>
         /// This property contains Item master details.
         /// </summary>
         private List<clsCashCounterVO> objListCashCounterDetails = new List<clsCashCounterVO>();
         public List<clsCashCounterVO> ListCashCounterDetails
         {
             get
             {
                 return objListCashCounterDetails;
             }
             set
             {
                 objListCashCounterDetails = value;

             }
         }

         private clsCashCounterVO _objCashCounter;
         public clsCashCounterVO ObjCashCounter
         {
             get { return _objCashCounter; }
             set
             {
                 _objCashCounter = value;
             }
         }

         /// <summary>
         ///  Output Property.
         /// This property states the outcome of BizAction Process.
         /// </summary>
         private int _SuccessStatus;
         public int SuccessStatus
         {
             get
             {
                 return _SuccessStatus;
             }
             set
             {
                 _SuccessStatus = value;
             }
         }
     }


     public class clsGetCashCounterDetailsByClinicIDBizActionVO : IBizActionValueObject
     {
         #region  IBizActionValueObject
         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.Administration.clsGetCashCounterDetailsByClinicIDBizAction";
         }
         public string ToXml()
         {
             return this.ToXml();
         }
         #endregion
         //public long StartRowIndex { get; set; }
         //public long MaximumRows { get; set; }
         //public long TotalRows { get; set; }
         //public bool PagingEnabled { get; set; }
         //public string SearchExpression { get; set; }
         public long ClinicID { get; set; }
         /// <summary>
         /// This property contains Item master details.
         /// </summary>
         /// <summary>
         /// This property contains Item master details.
         /// </summary>
         private List<clsCashCounterVO> objListCashCODetails = new List<clsCashCounterVO>();
         public List<clsCashCounterVO> ListCashCODetails
         {
             get
             {
                 return objListCashCODetails;
             }
             set
             {
                 objListCashCODetails = value;

             }
         }

         private clsCashCounterVO _objCashCo;
         public clsCashCounterVO ObjCashCo
         {
             get { return _objCashCo; }
             set
             {
                 _objCashCo = value;
             }
         }
         /// <summary>
         ///  Output Property.
         /// This property states the outcome of BizAction Process.
         /// </summary>
         private int _SuccessStatus;
         public int SuccessStatus
         {
             get
             {
                 return _SuccessStatus;
             }
             set
             {
                 _SuccessStatus = value;
             }
         }
     }
}
