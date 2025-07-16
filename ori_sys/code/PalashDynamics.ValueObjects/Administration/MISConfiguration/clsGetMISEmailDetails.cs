using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public  class clsGetMISEmailDetailsBizActionVO:IBizActionValueObject
    {


        private List<clsMISConfigurationVO> objMISConfig = null;
        public List<clsMISConfigurationVO> MISConfigDetails
        {
            get { return objMISConfig; }
            set { objMISConfig = value; }
        }

        private List<clsGetMISReportTypeVO> objConfigReport = null;
        public List<clsGetMISReportTypeVO> ConfigReportDetails
        {
            get { return objConfigReport; }
            set { objConfigReport = value; }
        }

        private List<clsGetMISStaffVO> objConfigStaff = null;
        public List<clsGetMISStaffVO> ConfigStaffDetails
        {
            get { return objConfigStaff; }
            set { objConfigStaff = value; }
        }


        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsGetMISEmailDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

    }
}
