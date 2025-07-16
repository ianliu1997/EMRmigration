using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.CompoundMaster
{
    public class clsAddUpdateCompoundMasterBizActionVO : IBizActionValueObject
    {
        private clsCompoundMasterVO _CompoundMaster;
        public clsCompoundMasterVO CompoundMaster
        {
            get { return _CompoundMaster; }
            set { _CompoundMaster = value; }
        }

        private List<clsCompoundMasterDetailsVO> _CompoundMasterDetails;
        public List<clsCompoundMasterDetailsVO> CompoundMasterDetailsList
        {
            get { return _CompoundMasterDetails; }
            set { _CompoundMasterDetails = value; }
        }
        
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUpdateCompoundMasterBizAction";

        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetCompoundMasterBizActionVO : IBizActionValueObject
    {
        private long _CompoundId;
        public long CompoundId
        {
            get { return _CompoundId; }
            set
            {
                if(value != _CompoundId)
                {
                    _CompoundId = value;                   
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if(value != _ItemID)
                {
                    _ItemID = value;
                }
            }
        }

        private string _strItemID;
        public string strItemID
        {
            get { return _strItemID; }
            set
            {
                if (value != _strItemID)
                {
                    _strItemID = value;
                }
            }
        }

        private clsCompoundMasterVO _CompoundMaster;
        public clsCompoundMasterVO CompoundMaster
        {
            get { return _CompoundMaster; }
            set { _CompoundMaster = value; }
        }

        private List<clsCompoundMasterVO> _CompoundMasterList;
        public List<clsCompoundMasterVO> CompoundMasterList
        {
            get { return _CompoundMasterList; }
            set { _CompoundMasterList = value; }
        }

        private clsCompoundMasterDetailsVO _CompoundDetail;
        public clsCompoundMasterDetailsVO CompoundDetail
        {
            get { return _CompoundDetail; }
            set { _CompoundDetail = value; }
        }

        private List<clsCompoundMasterDetailsVO> _CompoundMasterDetails;
        public List<clsCompoundMasterDetailsVO> CompoundMasterDetailsList
        {
            get { return _CompoundMasterDetails; }
            set { _CompoundMasterDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private bool _IsCompoundDetaillist=false;
        public bool IsCompoundDetailList
        {
            get { return _IsCompoundDetaillist; }
            set { _IsCompoundDetaillist = value; }
        }

        private bool _IsMeasureUnit = false;
        public bool IsMeasureUnit
        {
            get { return _IsMeasureUnit; }
            set { _IsMeasureUnit = value; }
        }

        public bool IsCompoundDrug { get; set; }

        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public int TotalRowCount { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetCompoundMasterBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsCheckCompoundDrugBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsCheckCompoundDrugBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public clsCompoundMasterVO CompoundDrug { get; set; }

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
