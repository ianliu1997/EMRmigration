using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
    public class clsGetCampFreeAndConServiceListBizActionVO : IBizActionValueObject
    {
        private List<CampserviceDetailsVO> _CampFreeServiceList;
        public List<CampserviceDetailsVO> CampFreeServiceList
        {
            get { return _CampFreeServiceList; }
            set { _CampFreeServiceList = value; }
        }

        private List<CampserviceDetailsVO> _CampConServiceList;
        public List<CampserviceDetailsVO> CampConServiceList
        {
            get { return _CampConServiceList; }
            set { _CampConServiceList = value; }
        }

        public long CampID { get; set; }


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetCampFreeAndConServiceListBizAction";
        }

        #endregion
    }
}

