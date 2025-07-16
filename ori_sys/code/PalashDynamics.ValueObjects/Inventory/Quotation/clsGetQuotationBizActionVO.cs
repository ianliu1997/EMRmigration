using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.Quotation
{
  public  class clsGetQuotationBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsGetQuotationBizAction";
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

        public DateTime? SearchFromDate  { get; set; }
        public DateTime? SearchToDate { get; set; }
        public long SearchStoreID { get; set; }
        public long UserID { get; set; }
        public long SearchSupplierID { get; set; }
        public clsQuotaionVO Quotation { get; set; }
        public List<clsQuotaionVO> QuotaionList { get; set; }
        public string SupplierIDs { get; set; }
        public string ItemIDs { get; set; }
        public List<clsItemsEnquiryTermConditionVO> TermsAndConditions { get; set; }

        public Boolean IsPagingEnabled { get; set; }

        public int StartIndex { get; set; }

        public int MaxRows { get; set; }
        public int TotalRowCount { get; set; }
    }
}
