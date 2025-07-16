using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsAddAreaBizActionVO : IBizActionValueObject
    {
        private List<clsAreaVO> objArea = null;
        public List<clsAreaVO> AreaList
        {
            get { return objArea; }
            set { objArea = value; }
        }
        private clsAreaVO _objAreaDetail = null;
        public clsAreaVO objAreaDetail
        {
            get { return _objAreaDetail; }
            set { _objAreaDetail = value; }
        }

        private List<clsCityVO> objAreaCityInfo = null;
        public List<clsCityVO> AreaCityList
        {
            get { return objAreaCityInfo; }
            set { objAreaCityInfo = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.Location.clsAddAreaBizAction";
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
