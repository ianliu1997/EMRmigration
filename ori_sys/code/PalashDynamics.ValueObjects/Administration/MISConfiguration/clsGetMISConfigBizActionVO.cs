using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsGetMISConfigBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsGetMISConfigBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>


        public long ID { get; set; }


        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }

        private clsMISConfigurationVO objMISConfig = null;

        public clsMISConfigurationVO GetMISConfig
        {
            get { return objMISConfig; }
            set {  objMISConfig=value ; }
        }

        private List<clsGetMISReportTypeVO> objConfigReport = null;

        public List<clsGetMISReportTypeVO> GetConfigReport
        {
            get { return objConfigReport; }
            set {objConfigReport =value  ; }
        }

        private List<clsGetMISStaffVO> objConfigStaff = null;

        public List<clsGetMISStaffVO> GetConfigStaff
        {
            get { return objConfigStaff; }
            set { objConfigStaff=value  ; }
        }        
    }
}
