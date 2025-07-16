using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class AddUpdateStoreLocationLockBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.AddUpdateStoreLocationLockBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

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

        private StoreLocationLockVO _ObjStoreLocationLockDetails;
        public StoreLocationLockVO ObjStoreLocationLockDetails
        {
            get { return _ObjStoreLocationLockDetails; }
            set { _ObjStoreLocationLockDetails = value; }
        }

        private StoreLocationLockVO _ObjStoreLocationLock;
        public StoreLocationLockVO ObjStoreLocationLock
        {
            get { return _ObjStoreLocationLock; }
            set { _ObjStoreLocationLock = value; }
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

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public bool IsModify { get; set; } 
    }
}
