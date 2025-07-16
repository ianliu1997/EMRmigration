using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsAddIPDAutoChargesServiceListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsAddIPDAutoChargesServiceListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDAutoChargesVO> objChargesMaster = new List<clsIPDAutoChargesVO>();
        public List<clsIPDAutoChargesVO> ChargesMasterList
        {
            get
            {
                return objChargesMaster;
            }
            set
            {
                objChargesMaster = value;

            }
        }


        public long Id { get; set; }
        public long UnitId { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
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

    public class clsGetIPDAutoChargesServiceListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IPD.clsGetIPDAutoChargesServiceListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDAutoChargesVO> objChargesMaster = new List<clsIPDAutoChargesVO>();
        public List<clsIPDAutoChargesVO> GetChargesMasterList
        {
            get
            {
                return objChargesMaster;
            }
            set
            {
                objChargesMaster = value;

            }
        }

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
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

    public class clsDeleteServiceBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsDeleteServiceBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// 


        private List<clsIPDAutoChargesVO> _DeleteDetails;
        public List<clsIPDAutoChargesVO> DeleteDetails
        {
            get { return _DeleteDetails; }
            set { _DeleteDetails = value; }
        }

        private List<clsIPDAutoChargesVO> objChargesMaster = new List<clsIPDAutoChargesVO>();
        public List<clsIPDAutoChargesVO> DeleteServiceList
        {
            get
            {
                return objChargesMaster;
            }
            set
            {
                objChargesMaster = value;

            }
        }

        public long Id { get; set; }
        public long UnitId { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
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
