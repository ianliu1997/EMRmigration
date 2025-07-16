using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetTariffDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetTariffDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }



        #endregion
        public long TariffId { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
       
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        
        private List<clsTariffVO> objItemMaster = new List<clsTariffVO>();
        public List<clsTariffVO> ItemMatserDetails
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
