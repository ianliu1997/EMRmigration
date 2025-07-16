using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetWardMasterBizActionVO: IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDWardMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDWardMasterVO> objWardMaster = new List<clsIPDWardMasterVO>();
        public List<clsIPDWardMasterVO> objWardMasterDetails
        {
            get
            {
                return objWardMaster;
            }
            set
            {
                objWardMaster = value;
            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
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

    public class clsIPDAddUpdateWardMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsAddUpdateIPDWardMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDWardMasterVO> objWardMaster = new List<clsIPDWardMasterVO>();
        public List<clsIPDWardMasterVO> objWardMatserDetails
        {
            get
            {
                return objWardMaster;
            }
            set
            {
                objWardMaster = value;
            }
        }

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

    public class clsIPDUpdateWardMasterStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDUpdateWardMasterStatusBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

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

        //   public long ID { get; set; }
        private clsIPDWardMasterVO objWardStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>

        public clsIPDWardMasterVO WardStatus
        {
            get { return objWardStatus; }
            set { objWardStatus = value; }
        }   
    }


}
