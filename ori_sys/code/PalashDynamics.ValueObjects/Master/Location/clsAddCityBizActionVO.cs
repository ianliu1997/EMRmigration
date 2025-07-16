using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsAddCityBizActionVO : IBizActionValueObject
    {
        private List<clsCityVO> objCity = null;
        public List<clsCityVO> CityList
        {
            get { return objCity; }
            set { objCity = value; }
        }
        private clsCityVO _objCityDetail = null;
        public clsCityVO objCityDetail
        {
            get { return _objCityDetail; }
            set { _objCityDetail = value; }
        }

        private List<clsAreaVO> objCityAreaInfo = null;
        public List<clsAreaVO> CityAreaList
        {
            get { return objCityAreaInfo; }
            set { objCityAreaInfo = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.Location.clsAddCityBizAction";
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
    }
}
