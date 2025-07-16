using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsUpdateDistMasterDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsDistVO> objDist = null;
        public List<clsDistVO> AreaList
        {
            get { return objDist; }
            set { objDist = value; }
        }


        private List<clsCityVO> objDistCityInfo = null;
        public List<clsCityVO> DistCityList
        {
            get { return objDistCityInfo; }
            set { objDistCityInfo = value; }
        }
        private clsDistVO _objDistDetail = null;
        public clsDistVO objDistDetail
        {
            get { return _objDistDetail; }
            set { _objDistDetail = value; }
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

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.Location.clsUpdateDistMasterDetailsBizAction";
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
