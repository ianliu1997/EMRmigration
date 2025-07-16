using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsGetDistListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.Location.clsGetDistListBizAction";
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


        private List<clsDistVO> objDist = null;
        public List<clsDistVO> DistList
        {
            get { return objDist; }
            set { objDist = value; }
        }

        private List<clsCityVO> objCity = null;
        public List<clsCityVO> CityList
        {
            get { return objCity; }
            set { objCity = value; }
        }

        public long DistID { get; set; }
        public string Code { get; set; }
        public string DistName { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }
    }
}
