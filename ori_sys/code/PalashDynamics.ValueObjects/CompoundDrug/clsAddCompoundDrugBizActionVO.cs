using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
   public class clsAddCompoundDrugBizActionVO :IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsAddCompoundDrugBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public clsCompoundDrugMasterVO CompoundDrug { get; set; }

        public List<clsCompoundDrugDetailVO> CompoundDrugDetailList { get; set; }

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
