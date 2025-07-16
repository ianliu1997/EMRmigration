using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.ValueObjects.Inventory
{

    public class clsAddGRNBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsAddGRNBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public Boolean IsEditMode { get; set; }

        private clsGRNVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsGRNVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        //added by Ashish Thombre
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (value != _FileName)
                {
                    _FileName = value;
                }
            }
        }
        public string ItemName = string.Empty;
        private byte[] _File;
        public byte[] File
        {
            get { return _File; }
            set
            {
                if (value != _File)
                {
                    _File = value;
                }
            }
        }

        private bool? _IsFileAttached;
        public bool? IsFileAttached
        {
            get { return _IsFileAttached; }
            set { _IsFileAttached = value; }
        }

        private bool _IsDraft;
        public bool IsDraft
        {
            get { return _IsDraft; }
            set { _IsDraft = value; }
        }

        private clsGRNDetailsVO _GRNItemDetails = null;
        public clsGRNDetailsVO GRNItemDetails
        {
            get { return _GRNItemDetails; }
            set { _GRNItemDetails = value; }
        }

        public bool IsGRNBarcodePrint;

        // For the Activity Log List by rohinee dated 29/9/16
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }
    }

    public class clsGetGRNListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetGRNListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long SupplierID { get; set; }
        public long StoreID { get; set; }
        public bool GrnReturnSearch { get; set; }
        public string GRNNo { get; set; }
        public bool Freezed { get; set; }
        public bool IsForViewInvoice { get; set; }

        private List<clsGRNVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        public clsGRNVO GRNVO { get; set; }

    }


    public class clsGetGRNDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetGRNDetailsListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ID { get; set; }
       
        public long GRNID { get; set; }
        public long UnitId { get; set; }

        public bool Freezed { get; set; }

        private List<clsGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsGRNDetailsFreeVO> objFreeItemsDetails = null;
        public List<clsGRNDetailsFreeVO> FreeItemsList
        {
            get { return objFreeItemsDetails; }
            set { objFreeItemsDetails = value; }
        }


        private List<clsGRNDetailsVO> objPOGRNDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNDetailsVO> POGRNList
        {
            get { return objPOGRNDetails; }
            set { objPOGRNDetails = value; }
        }

    }

    public class clsGetGRNDetailsListByIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetGRNDetailsListByIDBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ID { get; set; }

        public long GRNID { get; set; }
        public long UnitId { get; set; }

        public bool Freezed { get; set; }

        private List<clsGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private List<clsGRNDetailsVO> objPOGRNDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNDetailsVO> POGRNList
        {
            get { return objPOGRNDetails; }
            set { objPOGRNDetails = value; }
        }

        #region For Free Items

        private List<clsGRNDetailsFreeVO> objDetailsFree = null;
        public List<clsGRNDetailsFreeVO> ListFree
        {
            get { return objDetailsFree; }
            set { objDetailsFree = value; }
        }

        #endregion

    }

     public class clsGetGRNDetailsForGRNReturnListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.lsGetGRNDetailsForGRNReturnListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public long ID { get; set; }
       
        public long GRNID { get; set; }
        public long UnitId { get; set; }

        public bool Freezed { get; set; }

        public bool IsForQS = false;

        public bool IsFromGRNReturnQS = false;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long StoreId { get; set; }
        public long SupplierId { get; set; }
        public string GRNNo { get; set; }
        public string ItemName { get; set; }

        private List<clsGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    
    //Added By CDS 28/12/2015 

     public class clsUpdateGRNForApprovalVO : IBizActionValueObject
     {
         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.Inventory.clsUpdateGRNForApproval";
             //return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsUpdatePurchaseOrderForApprovalBizAction";
         }

         public string ToXml()
         {
             throw new NotImplementedException();
         }

         public Boolean IsEditMode { get; set; }
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

         public clsGRNVO Details { get; set; }
         public List<clsGRNDetailsVO> GRNItemDetailsList { get; set; }
     }


     public class clsUpdateGRNForRejectionVO : IBizActionValueObject
     {
         public string GetBizAction()
         {
             return "PalashDynamics.BusinessLayer.Inventory.clsUpdateGRNForRejection";
             //return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsUpdatePurchaseOrderForApprovalBizAction";
         }

         public string ToXml()
         {
             throw new NotImplementedException();
         }

         public Boolean IsEditMode { get; set; }
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

         public clsGRNVO Details { get; set; }
         public List<clsGRNDetailsVO> GRNItemDetailsList { get; set; }
     }
}

