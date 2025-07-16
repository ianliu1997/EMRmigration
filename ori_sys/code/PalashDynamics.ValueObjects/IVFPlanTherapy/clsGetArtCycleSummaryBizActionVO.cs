using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetArtCycleSummaryBizActionVO : IBizActionValueObject
    {
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


        public long CoupleUnitID { get; set; }
        public long CoupleID { get; set; }

        private List<clsArtCycleSummaryVO> objArtCycleSummary = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains LabDaysSummary Details Which is Added.
        /// </summary>
        public List<clsArtCycleSummaryVO> ArtCycleSummary
        {
            get { return objArtCycleSummary; }
            set { objArtCycleSummary = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetArtCycleSummaryBizAction";
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
