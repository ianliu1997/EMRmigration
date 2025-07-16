using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
    public class clsIPDGetClassMasterBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetClassMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDClassMasterVO> objClassMaster = new List<clsIPDClassMasterVO>();
        public List<clsIPDClassMasterVO> objClassMasterDetails
        {
            get
            {
                return objClassMaster;
            }
            set
            {
                objClassMaster = value;
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

    public class clsIPDAddUpdateClassMasterBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDAddUpdateClassMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDClassMasterVO> objClassMaster = new List<clsIPDClassMasterVO>();
        public List<clsIPDClassMasterVO> objClassMatserDetails
        {
            get
            {
                return objClassMaster;
            }
            set
            {
                objClassMaster = value;
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

    public class clsIPDUpdateClassMasterStatusBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDUpdateClassMasterStatusBizAction";
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
        private clsIPDClassMasterVO objClassStatus = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Status of the User Which is Added.
        /// </summary>
        public clsIPDClassMasterVO ClassStatus
        {
            get { return objClassStatus; }
            set { objClassStatus = value; }
        } 
    }

}
