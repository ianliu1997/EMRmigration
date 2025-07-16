using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsAddCountryBizActionVO : IBizActionValueObject
    {

        private List<clsCountryVO> objCountry = null;
        public List<clsCountryVO> CountryList
        {
            get { return objCountry; }
            set { objCountry = value; }
        }


        private List<clsStateVO> objCountryStateInfo = null;
        public List<clsStateVO> CountryStateList
        {
            get { return objCountryStateInfo; }
            set { objCountryStateInfo = value; }
        }
        private clsCountryVO _objCountryDetail = null;
        public clsCountryVO objCountryDetail
        {
            get { return _objCountryDetail; }
            set { _objCountryDetail = value; }
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

            return "PalashDynamics.BusinessLayer.Master.Location.clsAddCountryBizAction";
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
