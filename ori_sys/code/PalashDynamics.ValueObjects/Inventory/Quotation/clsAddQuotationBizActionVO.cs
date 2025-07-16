using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.Quotation
{
    public class clsAddQuotationBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsAddQuotationBizAction";
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

        public clsQuotaionVO Quotation { get; set; }
        public List<clsQuotaionVO> QuotaionList { get; set; }

        public List<clsItemsEnquiryTermConditionVO> TermsAndConditions { get; set; }

    }

    public class clsAddQuotationAttachmentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsAddQuotationAttachmentBizAction";
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

        public clsQuotaionVO Quotation { get; set; }
        public List<clsQuotationAttachmentsVO> AttachmentList { get; set; }

       // public List<clsItemsEnquiryTermConditionVO> TermsAndConditions { get; set; }
    }

    public class clsGetQuotationAttachmentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsGetQuotationAttachmentBizAction";
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

        public clsQuotaionVO Quotation { get; set; }
        public List<clsQuotationAttachmentsVO> AttachmentList { get; set; }
        public long QuotationID { get; set; }
        public Boolean IsPagingEnabled { get; set; }
        public int StartIndex { get; set; }
        public int MaxRows { get; set; }
        public int TotalRowCount { get; set; }
    }

    public class clsDeleteQuotationAttachmentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.Quotation.clsDeleteQuotationAttachmentBizAction";
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

        //public clsQuotaionVO Quotation { get; set; }
        //public List<clsQuotationAttachmentsVO> AttachmentList { get; set; }
        public long QuotationID { get; set; }
        public long ID { get; set; }
        // public List<clsItemsEnquiryTermConditionVO> TermsAndConditions { get; set; }
    }
}
