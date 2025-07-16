using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.Location
{
    public class clsUpdateAddressLocation6BizActionVO : IBizActionValueObject
    {
        //private List<clsAddressLocation6VO> objArea = null;
        //public List<clsAddressLocation6VO> AreaList
        //{
        //    get { return objArea; }
        //    set { objArea = value; }
        //}



        private clsAddressLocation6VO _objAddressLocation6Detail = null;
        public clsAddressLocation6VO objAddressLocation6Detail
        {
            get { return _objAddressLocation6Detail; }
            set { _objAddressLocation6Detail = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.Location.clsUpdateAddressLocation6BizAction";
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
