using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetFloorMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetFloorMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDFloorMasterVO> objFloorMaster = new List<clsIPDFloorMasterVO>();
        public List<clsIPDFloorMasterVO> objFloorMasterDetails
        {
            get
            {
                return objFloorMaster;
            }
            set
            {
                objFloorMaster = value;
            }
        }

        private clsIPDFloorMasterVO _objFloorDetails = null;
        public clsIPDFloorMasterVO FlooMasterDetails
        {
            get { return _objFloorDetails; }
            set { _objFloorDetails = value; }

        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        public long SearchCategory { get; set; }
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

    public class clsIPDAddUpdateFloorMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddUpdateFloorMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private clsIPDFloorMasterVO _objFloorDetails = null;
        public clsIPDFloorMasterVO FlooMasterDetails
        {
            get { return _objFloorDetails; }
            set { _objFloorDetails = value; }

        }

        public bool IsStatus { get; set; }

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
