using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetIndenListBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetIndenListBizAction";
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

        public string IndentNO { get; set; }

        public bool CheckStatusType { get; set; }
        public int IndentStatus { get; set; }

        public bool isFrowrded { get; set; }
        public bool isCancelled { get; set; }
        public bool isApproved { get; set; }
        public bool IsFreezed { get; set; }
        //By Anjali.......................................
        //public bool isIndent { get; set; }
        public int isIndent { get; set; }
        //......................................................
        public long TotalRow { get; set; }
        public long NoOfRecords { get; set; }
        public long UserID { get; set; }


        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public Boolean IsPagingEnabled { get; set; }
       

        public List<clsIndentMasterVO> IndentList { get; set; }

        //***//
        public string MRNo { get; set; }
        public string PatientName { get; set; }


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
        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;
            }
        }




    }
}
