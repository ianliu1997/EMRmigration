using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsAddUpdateMISConfigurationBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsAddUpdateMISConfigurationBizAction";
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

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }
        public long ID { get; set; }
        public bool IsUpdateStatus { get; set; }

        private clsMISConfigurationVO objMISConfig = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>
        /// 
        public clsMISConfigurationVO AddMISConfig
        {
            get { return objMISConfig; }

            set { objMISConfig = value; }
        }

        private clsGetMISReportTypeVO objConfigReport = null;

        public clsGetMISReportTypeVO GetConfigReport
        {
            get { return objConfigReport; }
            set { objConfigReport=value; }
        }

        private clsGetMISStaffVO objConfigStaff = null;

        public clsGetMISStaffVO GetConfigStaff
        {
            get { return objConfigStaff; }
            set { objConfigStaff=value; }
        }
    }
}
