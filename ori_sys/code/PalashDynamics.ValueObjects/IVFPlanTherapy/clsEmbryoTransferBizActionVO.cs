using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateEmbryoTransferBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateEmbryoTransferBizAction";
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

        public bool IsUpdate { get; set; }

        private clsEmbryoTransferVO objEmbryoTransfer = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains FemaleLabDay0 Details Which is Added.
        /// </summary>
        public clsEmbryoTransferVO EmbryoTransfer
        {
            get { return objEmbryoTransfer; }
            set { objEmbryoTransfer = value; }
        }
    }

    public class clsGetEmbryoTransferBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetEmbryoTransferBizAction";
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
        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }


        private clsEmbryoTransferVO objEmbryoTransfer = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains FemaleLabDay0 Details Which is Added.
        /// </summary>
        public clsEmbryoTransferVO EmbryoTransfer
        {
            get { return objEmbryoTransfer; }
            set { objEmbryoTransfer = value; }
        }
    }

    public class clsGetForwardedEmbryoTransferBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetForwardedEmbryoTransferBizAction";
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

        public bool IsLatest { get; set; }

        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        private List<clsEmbryoTransferDetailsVO> objEmbryoTransfer = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains EmbryoTransfer Details Which is Added.
        /// </summary>
        public List<clsEmbryoTransferDetailsVO> EmbryoTransfer
        {
            get { return objEmbryoTransfer; }
            set { objEmbryoTransfer = value; }
        }
    }

    public class clsAddForwardedEmbryoTransferBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddForwardedEmbryoTransferBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

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



        private List<clsEmbryoTransferDetailsVO> objForwardedEmbryos = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains ForwardedEmbryos Details Which is Added.
        /// </summary>
        public List<clsEmbryoTransferDetailsVO> ForwardedEmbryos
        {
            get { return objForwardedEmbryos; }
            set { objForwardedEmbryos = value; }
        }
    }
}
