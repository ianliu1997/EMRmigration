using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetCurrencyMasterListDetailsBizActionVO : IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetCurrencyMasterListDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long Id { get; set; }
        public String MasterTableName { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExperssion { get; set; }
       

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCurrencyMasterListVO> objItemMaster = new List<clsCurrencyMasterListVO>();
        public List<clsCurrencyMasterListVO> ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }
    }
   
}

