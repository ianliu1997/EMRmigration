using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsGetAreaListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.Location.clsGetAreaListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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
        public long DistID { get; set; }
        public string AreaName { get; set; }

        private List<clsAreaVO> objArea = null;
        public List<clsAreaVO> AreaList
        {
            get { return objArea; }
            set { objArea = value; }
        }

      


        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


    }
}
