using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsAddExpensesBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsAddExpensesBizAction";
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
        public bool myTransaction { get; set; }

        private clsExpensesVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsExpensesVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsGetExpensesListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetExpensesListBizAction";
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

        //    private clsExpensesDetailsVO objDetails = null;
        //    /// <summary>
        //    /// Output Property.
        //    /// This Property Contains OPDPatient Details Which is Added.
        //    /// </summary>
        //    public clsExpensesDetailsVO ExpensesDetails
        //    {
        //        get { return objDetails; }
        //        set { objDetails = value; }
        //    }
        //}
        public long ID { get; set; }

        public long UnitID { get; set; }

        public long Expense { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string Voucherno { get; set; }
        public string Vouchercreatedby { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool IsFreezed { get; set; }
        public string sortExpression { get; set; }
        public bool VoStatus { get; set; }

        private List<clsExpensesVO> objDetails = new List<clsExpensesVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>


        public List<clsExpensesVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }





    }
}
