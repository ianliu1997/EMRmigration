using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
   public class clsGetPathTestDetailsBizActionVO:IBizActionValueObject
    {
        private List<clsPathOrderBookingVO> objTemplateList = null;
        public List<clsPathOrderBookingVO> TestList
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }

        public string Description { get; set; }
        public long Category { get; set; }
        public long ServiceID { get; set; }

        public long pobID { get; set; }
        public long pobUnitID { get; set; }

       //Added By Bhushanp 18012017
        public long pChargeID { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathTestDetailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}
