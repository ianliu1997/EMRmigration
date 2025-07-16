using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsGetCompanyAssociateDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetCompanyAssociateDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }



        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsCompanyAssociateVO> objItemMaster = new List<clsCompanyAssociateVO>();
        public List<clsCompanyAssociateVO> ItemMatserDetails
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
        public long CompantAssociateId { get; set; }
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
}
