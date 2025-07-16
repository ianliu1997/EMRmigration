using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{
   public class clsIPDGetBlockMasterBizActionVO :IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetBlockMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsIPDBlockMasterVO> objBlockMaster = new List<clsIPDBlockMasterVO>();
        public List<clsIPDBlockMasterVO> objBlockMasterDetails
        {
            get
            {
                return objBlockMaster;
            }
            set
            {
                objBlockMaster = value;
            }
        }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
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

   public class clsAddUpdateIPDBlockMasterBizActionVO : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.IPD.clsAddUpdateBlockMasterBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }

       #endregion

       /// <summary>
       /// This property contains Item master details.
       /// </summary>
       private List<clsIPDBlockMasterVO> objBlockMaster = new List<clsIPDBlockMasterVO>();
       public List<clsIPDBlockMasterVO> objBlockMatserDetails
       {
           get
           {
               return objBlockMaster;
           }
           set
           {
               objBlockMaster = value;
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
   }

   public class clsIPDUpdateBlockMasterStatusBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDUpdateBlockMasterStatus";
       }

       public string ToXml()
       {
           return this.ToXml();
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

       //   public long ID { get; set; }
       private clsIPDBlockMasterVO objBlockStatus = null;

       /// <summary>
       /// Output Property.
       /// This Property Contains Status of the User Which is Added.
       /// </summary>
       public clsIPDBlockMasterVO BlockStatus
       {
           get { return objBlockStatus; }
           set { objBlockStatus = value; }
       } 
   }



}
