using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsUpdateDoctorShareServiceBizActionVO : IBizActionValueObject
    {

        private clsDoctorShareServicesDetailsVO _objServiceDetail = null;
        public clsDoctorShareServicesDetailsVO objServiceDetail
        {
            get { return _objServiceDetail; }
            set { _objServiceDetail = value; }
        }

        private List<clsDoctorShareServicesDetailsVO> _objServiceList = null;
        public List<clsDoctorShareServicesDetailsVO> objServiceList
        {
            get { return _objServiceList; }
            set { _objServiceList = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Master.clsUpdateDoctorShareServiceBizAction";
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
