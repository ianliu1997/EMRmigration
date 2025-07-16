using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetExpiryItemForDashBoardBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetExpiryItemForDashBoardBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public DateTime? Date { get; set; }
        public long Day { get; set; }
        public long? UnitId { get; set; }
        public Boolean? IsOrderBy { get; set; }
        public Boolean? IsPaging { get; set; }

        public long? StartIndex { get; set; }
        public long? NoOfRecordShow { get; set; }
        public long? TotalRow { get; set; }
        public long StoreID { get; set; }
        public long UserID { get; set; }

        public List<clsExpiredItemReturnDetailVO> ExpiredItemList { get; set; }

        //Added by Sayali 21th Aug 2018
        public DateTime? ExpiryFromDate { get; set; }
        public DateTime? ExpiryToDate { get; set; }
        //

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

        private bool _IsSelected = false;
        public bool IsSelected 
        { 
            get { return _IsSelected; } 
            set { _IsSelected = value; 
                OnChanged("IsSelected"); } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

    }
}
