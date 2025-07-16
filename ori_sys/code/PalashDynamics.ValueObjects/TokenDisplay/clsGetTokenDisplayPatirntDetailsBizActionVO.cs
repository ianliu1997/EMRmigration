using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.TokenDisplay
{
    public class clsGetTokenDisplayPatirntDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.TokenDisplay.clsGetTokenDisplayPatirntDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        private List<clsTokenDisplayVO> objTokenDisplay = new List<clsTokenDisplayVO>();
        public List<clsTokenDisplayVO> ListTokenDisplay
        {
            get
            {
                return objTokenDisplay;
            }
            set
            {
                objTokenDisplay = value;

            }
        }


        public long UnitId { get; set; }
        public long PatientId { get; set; }
        public long VisitId { get; set; }


        public DateTime VisitDate { get; set; }
        public bool IsDisplay { get; set; }

        #endregion

        private long _SuccessStatus;
        public long SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        private long _ResultStatus;
        public long ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

    }
}
