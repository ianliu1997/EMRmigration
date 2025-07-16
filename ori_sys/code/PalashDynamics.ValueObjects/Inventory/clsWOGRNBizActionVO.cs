using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddWOGRNBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsAddWOGRNBizAction";
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

        private clsWOGRNVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsWOGRNVO Details
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

        private clsWOGRNDetailsVO _GRNItemDetails = null;
        public clsWOGRNDetailsVO GRNItemDetails
        {
            get { return _GRNItemDetails; }
            set { _GRNItemDetails = value; }
        }

        public bool IsGRNBarcodePrint;
    }

    public class clsGetWOGRNListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetWOGRNListBizAction";
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

        private List<clsWOGRNVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }



    }
    public class clsGetWOGRNDetailsListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetWOGRNDetailsListBizAction";
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

        private List<clsWOGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private List<clsWOGRNDetailsVO> objWOGRNDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNDetailsVO> POGRNList
        {
            get { return objWOGRNDetails; }
            set { objWOGRNDetails = value; }
        }

    }
    public class clsGetWOGRNDetailsListByIDBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetWOGRNDetailsListByIDBizAction";
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

        private List<clsWOGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }


        private List<clsWOGRNDetailsVO> objWOGRNDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNDetailsVO> WOGRNList
        {
            get { return objWOGRNDetails; }
            set { objWOGRNDetails = value; }
        }

    }
    public class clsGetWOGRNDetailsForGRNReturnListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.lsGetWOGRNDetailsForGRNReturnListBizAction";
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

        private List<clsWOGRNDetailsVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsWOGRNDetailsVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }




    }
}
