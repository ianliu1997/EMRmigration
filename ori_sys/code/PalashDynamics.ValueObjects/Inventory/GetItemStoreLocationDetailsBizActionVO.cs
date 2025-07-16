using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class GetItemStoreLocationDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetItemStoreLocationDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<ItemStoreLocationDetailsVO> objItemStoreLocationDetailslist = new List<ItemStoreLocationDetailsVO>();
        public List<ItemStoreLocationDetailsVO> ItemStoreLocationDetailslist
        {
            get
            {
                return objItemStoreLocationDetailslist;
            }
            set
            {
                objItemStoreLocationDetailslist = value;

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

        private ItemStoreLocationDetailsVO _ObjItemStoreLocationDetails;
        public ItemStoreLocationDetailsVO ObjItemStoreLocationDetails
        {
            get { return _ObjItemStoreLocationDetails; }
            set { _ObjItemStoreLocationDetails = value; }
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long ApplicableGender { get; set; }
    }
}
