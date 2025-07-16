using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.MISConfiguration
{
    public class clsGetMISReportTypeBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.MISConfiguration.clsGetReportTypeBizAction";
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

        public long MISTypeID { get; set; }
        private List<clsGetMISReportTypeVO> objMISReport = new List<clsGetMISReportTypeVO>();

        public List<clsGetMISReportTypeVO> GetMIsReport
        {
            get { return objMISReport; }
            set { objMISReport = value; }
        }

    }
    public class clsGetMISReportTypeVO
    {
        public long Id { get; set; }
        public long MISTypeID { get; set; }
        public string Description { get; set; }
        public string rptFileName { get; set; }
        public bool Status { get; set; }
    }
}
