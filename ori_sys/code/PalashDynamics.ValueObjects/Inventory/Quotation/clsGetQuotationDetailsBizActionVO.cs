using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.Quotation
{
   public class clsGetQuotationDetailsBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsGetQuotationDetailsBizAction";
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

        public long SearchQuotationID { get; set; }
        public long UnitID { get; set; }
        public clsQuotaionVO Quotation { get; set; }
        public List<clsQuotationDetailsVO> QuotaionList { get; set; }

     

        public bool IsPagingEnabled { get; set; }

        public Int32 StartIndex { get; set; }

        public Int32 MinRows { get; set; }
    }
}
