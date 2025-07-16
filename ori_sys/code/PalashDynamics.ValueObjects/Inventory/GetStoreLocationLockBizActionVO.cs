using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class GetStoreLocationLockBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetStoreLocationLockBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<StoreLocationLockVO> objStoreLocationLockList = new List<StoreLocationLockVO>();
        public List<StoreLocationLockVO> StoreLocationLock
        {
            get
            {
                return objStoreLocationLockList;
            }
            set
            {
                objStoreLocationLockList = value;

            }
        }

        private List<StoreLocationLockVO> objStoreLocationLockDetailsList = new List<StoreLocationLockVO>();
        public List<StoreLocationLockVO> StoreLocationLockDetails
        {
            get
            {
                return objStoreLocationLockDetailsList;
            }
            set
            {
                objStoreLocationLockDetailsList = value;

            }
        }

        private List<ItemStoreLocationDetailsVO> objStoreLocationLockDetailsList1 = new List<ItemStoreLocationDetailsVO>();
        public List<ItemStoreLocationDetailsVO> StoreLocationLockDetailsList
        {
            get
            {
                return objStoreLocationLockDetailsList1;
            }
            set
            {
                objStoreLocationLockDetailsList1 = value;

            }
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

        private StoreLocationLockVO _ObjStoreLocationLock;
        public StoreLocationLockVO ObjStoreLocationLock
        {
            get { return _ObjStoreLocationLock; }
            set { _ObjStoreLocationLock = value; }
        }

        private StoreLocationLockVO _ObjStoreLocationLockDetails;
        public StoreLocationLockVO ObjStoreLocationLockDetails
        {
            get { return _ObjStoreLocationLockDetails; }
            set { _ObjStoreLocationLockDetails = value; }
        }


        private List<clsStoreVO> objStoreVO = new List<clsStoreVO>();
        public List<clsStoreVO> StoreVO
        {
            get
            {
                return objStoreVO;
            }
            set
            {
                objStoreVO = value;

            }
        }


        public int Flag { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
        public Boolean IsViewClick { get; set; }
        public Boolean IsForValidation = false;

    }

    public class GetItemStoreLocationLockBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetItemStoreLocationLockBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private List<StoreLocationLockVO> objStoreLocationLockList = new List<StoreLocationLockVO>();
        public List<StoreLocationLockVO> StoreLocationLockList
        {
            get
            {
                return objStoreLocationLockList;
            }
            set
            {
                objStoreLocationLockList = value;

            }
        }

        private List<ItemStoreLocationDetailsVO> objStoreLocationLockDetailsList = new List<ItemStoreLocationDetailsVO>();
        public List<ItemStoreLocationDetailsVO> StoreLocationLockDetailsList 
        {
            get
            {
                return objStoreLocationLockDetailsList;
            }
            set
            {
                objStoreLocationLockDetailsList = value;

            }
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

        private StoreLocationLockVO _ObjStoreLocationLock;
        public StoreLocationLockVO ObjStoreLocationLockVO
        {
            get { return _ObjStoreLocationLock; }
            set { _ObjStoreLocationLock = value; }
        }

        private StoreLocationLockVO _ObjStoreLocationLockDetails;
        public StoreLocationLockVO ObjStoreLocationLockDetailsVO
        {
            get { return _ObjStoreLocationLockDetails; }
            set { _ObjStoreLocationLockDetails = value; }
        }


        private List<clsStoreVO> objStoreVO = new List<clsStoreVO>();
        public List<clsStoreVO> StoreVO
        {
            get
            {
                return objStoreVO;
            }
            set
            {
                objStoreVO = value;

            }
        }


        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
        public Boolean IsForUnBlockRecord { get; set; }
        public bool IsForMainRecord { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
    }

    public class GetRackMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetRackMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
        public Boolean IsForUnBlockRecord { get; set; }
        public bool IsForMainRecord { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
    }

    public class GetShelfMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetShelfMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
        public Boolean IsForUnBlockRecord { get; set; }
        public bool IsForMainRecord { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StoreID { get; set; }
        public long RackID { get; set; }
    }

    public class GetBinMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetBinMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        /// <summary>
        /// Gets or Sets MasterList
        /// </summary>
        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
        public Boolean IsForUnBlockRecord { get; set; }
        public bool IsForMainRecord { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StoreID { get; set; }
        public long RackID { get; set; }
        public long ShelfID { get; set; }
    }
}
