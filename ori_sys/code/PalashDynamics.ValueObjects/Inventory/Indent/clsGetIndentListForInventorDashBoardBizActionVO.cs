using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetIndentListForInventorDashBoardBizActionVO: IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsGetIndentListForInventorDashBoardBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public DateTime? FromDate { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ToDate { get; set; }

        public long FromStoreID { get; set; }
        public long ToStoreID { get; set; }
        public long UnitID { get; set; }
        public long UserID { get; set; }
        public string IndentNO { get; set; }
        public bool CheckStatusType { get; set; }
        public int IndentStatus { get; set; }

        public Boolean? IsOrderBy { get; set; }

        public long TotalRow { get; set; }
        public long StartRowIndex { get; set; }
        public long NoOfRecords { get; set; }
        public Boolean IsPagingEnabled { get; set; }
        
        //By Anjali.....................
        public bool IsIndent { get; set; }
        //...............................

        public List<clsIndentMasterVO> IndentList { get; set; }




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
