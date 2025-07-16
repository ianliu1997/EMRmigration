using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetIndenListByStoreIdBizActionVO : IBizActionValueObject
    {


        public clsGetIndenListByStoreIdBizActionVO()
        {

        }

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetIndenListByStoreIdBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public List<clsIndentMasterVO> IndentList { get; set; }

        public long? FromIndentStoreId { get; set; }
        public long? ToIndentStoreId { get; set; }
        public long? LoginUserUnitId { get; set; }
        public long UnitID { get; set; }
        public int StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool Freezed { get; set; }
        public bool PagingEnabled { get; set; }
        public Int32 TotalRowCount { get; set; }
        public string IndentNumber { get; set; }

     
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

        public bool FromPO = false;
        //By Anjali...............................
        //public bool IsIndent = false;
        public int IsIndent = 0;
        //.....................................
    }
}
