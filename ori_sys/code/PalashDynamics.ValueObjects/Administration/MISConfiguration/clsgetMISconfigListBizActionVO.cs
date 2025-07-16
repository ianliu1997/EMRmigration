using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsgetMISconfigListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsgetMISconfigListBizAction";
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

        public bool PagingEnabled { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public string FilterbyClinic { get; set; }
        public string FilterbyMISType { get; set; }

        

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }

        private List<clsMISConfigurationVO> objMISConfig =new List<clsMISConfigurationVO>();

        public List<clsMISConfigurationVO> GetMISConfig
        {
            get { return objMISConfig; }
            set { objMISConfig = value; }
        }

      
    }
}
