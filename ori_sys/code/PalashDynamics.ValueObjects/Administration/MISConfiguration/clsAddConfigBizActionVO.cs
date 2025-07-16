using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
   public class clsAddConfigBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsAddMISConfigBizAction";
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

        private clsMISConfigurationVO objMISConfig = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>
        /// 

        public long ID { get; set; }
        public clsMISConfigurationVO AddMISConfig
        {
            get { return objMISConfig; }
            set { value = objMISConfig; }
        }

        private clsConfig_MISReportDetailsVO objConfigReport = null;

        public clsConfig_MISReportDetailsVO GetConfigReport
        {
            get { return objConfigReport; }
            set { value = objConfigReport; }
        }

        private clsConfig_MISStaffVO objConfigStaff = null;

        public clsConfig_MISStaffVO GetConfigStaff
        {
            get { return objConfigStaff; }
            set { value = objConfigStaff; }
        }  
    }
}
