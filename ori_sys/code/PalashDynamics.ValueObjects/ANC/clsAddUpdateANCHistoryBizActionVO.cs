using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.ANC
{
    public class clsAddUpdateANCHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsAddUpdateANCHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCHistoryVO _ANCHistory = new clsANCHistoryVO();
        public clsANCHistoryVO ANCHistory
        {
            get
            {
                return _ANCHistory;
            }
            set
            {
                _ANCHistory = value;
            }
        }
        private List<clsANCObstetricHistoryVO> _ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
        public List<clsANCObstetricHistoryVO> ANCObsetricHistoryList
        {
            get
            {
                return _ANCObsetricHistoryList;
            }
            set
            {
                _ANCObsetricHistoryList = value;
            }
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

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }
    }

    public class clsGetANCHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetANCHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCHistoryVO _ANCHistory = new clsANCHistoryVO();
        public clsANCHistoryVO ANCHistory
        {
            get
            {
                return _ANCHistory;
            }
            set
            {
                _ANCHistory = value;
            }
        }
        private List<clsANCObstetricHistoryVO> _ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
        public List<clsANCObstetricHistoryVO> ANCObsetricHistoryList
        {
            get
            {
                return _ANCObsetricHistoryList;
            }
            set
            {
                _ANCObsetricHistoryList = value;
            }
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

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }
    }

    public class clsAddUpdateObestricHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsAddUpdateObestricHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private clsANCObstetricHistoryVO _ANCObstetricHistory = new clsANCObstetricHistoryVO();
        public clsANCObstetricHistoryVO ANCObstetricHistory
        {
            get
            {
                return _ANCObstetricHistory;
            }
            set
            {
                _ANCObstetricHistory = value;
            }
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

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }
    }

    public class clsGetObstricHistoryListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.ANC.clsGetObstricHistoryListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        private List<clsANCObstetricHistoryVO> _ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
        public List<clsANCObstetricHistoryVO> ANCObsetricHistoryList
        {
            get
            {
                return _ANCObsetricHistoryList;
            }
            set
            {
                _ANCObsetricHistoryList = value;
            }
        }
        private clsANCObstetricHistoryVO _ANCObstetricHistory = new clsANCObstetricHistoryVO();
        public clsANCObstetricHistoryVO ANCObstetricHistory
        {
            get
            {
                return _ANCObstetricHistory;
            }
            set
            {
                _ANCObstetricHistory = value;
            }
        }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
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

        public long TabID { get; set; }

        public bool IsUpdate { get; set; }
    }
}