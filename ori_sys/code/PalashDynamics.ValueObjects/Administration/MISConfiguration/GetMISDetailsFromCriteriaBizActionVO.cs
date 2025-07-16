using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class GetMISDetailsFromCriteriaBizActionVO : IBizActionValueObject
    {
        public DateTime? StartDate { get; set; }
        public DateTime? ScheduleTime { get; set; }
        
        private List<clsMISConfigurationVO> objMISConfig = null;
        public List<clsMISConfigurationVO> MISConfigDetails
        {
            get { return objMISConfig; }
            set { objMISConfig = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.GetMISDetailsFromCriteriaBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
