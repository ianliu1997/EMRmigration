using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddTariffItemBizActionVO : IBizActionValueObject
    {
        public clsAddTariffItemBizActionVO()
        {

        }

        private clsItemMasterVO _objItemMasterDetails = null;
        public clsItemMasterVO ItemMasterDetails
        {
            get { return _objItemMasterDetails; }
            set { _objItemMasterDetails = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }
        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set { _TariffID = value; }
        }
        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }

        private long _TariffItemID;
        public long TariffItemID
        {
            get { return _TariffItemID; }
            set { _TariffItemID = value; }
        }

        private bool _TariffItemForm;
        public bool TariffItemForm
        {
            get { return _TariffItemForm; }
            set { _TariffItemForm = value; }
        }

        private bool _IsForTariffMaster;
        public bool IsForTariffMaster
        {
            get { return _IsForTariffMaster; }
            set { _IsForTariffMaster = value; }
        }
        private bool _IsAllItemsSelected;
        public bool IsAllItemsSelected
        {
            get { return _IsAllItemsSelected; }
            set { _IsAllItemsSelected = value; }
        }

        private decimal _DiscountPer;
        public decimal DiscountPer
        {
            get { return _DiscountPer; }
            set { _DiscountPer = value; }
        }

        private decimal _DeductablePer;
        public decimal DeductablePer
        {
            get { return _DeductablePer; }
            set { _DeductablePer = value; }
        }

        private string _ItemIDList;
        public string ItemIDList
        {
            get { return _ItemIDList; }
            set { _ItemIDList = value; }
        }
        private string _ItemDisList;
        public string ItemDisList
        {
            get { return _ItemDisList; }
            set { _ItemDisList = value; }
        }

        private string _ItemDedList;
        public string ItemDedList
        {
            get { return _ItemDedList; }
            set { _ItemDedList = value; }
        }

        private string _RemoveItemIDList;
        public string RemoveItemIDList
        {
            get { return _RemoveItemIDList; }
            set { _RemoveItemIDList = value; }
        }
        private string _RemoveItemRateList;
        public string RemoveItemRateList
        {
            get { return _RemoveItemRateList; }
            set { _RemoveItemRateList = value; }
        }

        private long _OperationType;
        public long OperationType
        {
            get { return _OperationType; }
            set { _OperationType = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddTariffItemBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        public bool UpdateTariffItemMaster { get; set; }

        public List<long> TariffList { get; set; }

        public List<MasterListItem> Tariffs { get; set; }

        public string Query { get; set; }
       

        #endregion
    }
}
