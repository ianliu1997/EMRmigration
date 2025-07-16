using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
   public class clsGetCompoundDrugBizActionVO:IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsGetCompoundDrugBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public clsCompoundDrugMasterVO CompoundDrug { get; set; }

        public List<clsCompoundDrugMasterVO> CompoundDrugList { get; set; }

        public string SearchByCode { get; set; }
        public string SearchByDescription { get; set; }
        public List<clsCompoundDrugDetailVO> CompoundDrugDetailList { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }
        public Int32 TotalRowCount { get; set; }
        public Int32 StartRowIndex { get; set; }
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

   public class clsCheckCompoundDrugBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.CompoundDrug.clsCheckCompoundDrugBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }
       #endregion

       public clsCompoundDrugMasterVO CompoundDrug { get; set; }

      
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
