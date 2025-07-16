using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateLabDaysSummaryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateLabDaysSummaryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        public bool IsUpdate { get; set; }

        private clsLabDaysSummaryVO objLabDaysSummary = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains FemaleLabDay0 Details Which is Added.
        /// </summary>
        public clsLabDaysSummaryVO LabDaysSummary
        {
            get { return objLabDaysSummary; }
            set { objLabDaysSummary = value; }
        }
    }

    public class clsGetLabDaysSummaryBizActionVO : IBizActionValueObject
    {

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetLabDaysSummaryBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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


        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        private List<clsLabDaysSummaryVO> objLabDaysSummary = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains LabDaysSummary Details Which is Added.
        /// </summary>
        public List<clsLabDaysSummaryVO> LabDaysSummary
        {
            get { return objLabDaysSummary; }
            set { objLabDaysSummary = value; }
        }
    }
}
