using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsGetCompoundDrugDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsGetCompoundDrugDetailsBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

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
        public bool IsPagingEnabled { get; set; }

        public Int32 StartIndex { get; set; }

        public Int32 MinRows { get; set; }

        public long UnitID { get; set; }

        public clsCompoundDrugMasterVO CompoundDrug { get; set; }

        public List<clsCompoundDrugDetailVO> CompoundDrugDetailList { get; set; }
        public string ItemID { get; set; }
        public string CompoundDrugValue { get; set; }
        public int TotalRowCount { get; set; }
    }
}
