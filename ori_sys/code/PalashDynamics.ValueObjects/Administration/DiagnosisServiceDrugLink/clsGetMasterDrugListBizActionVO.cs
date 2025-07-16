using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink
{
    public class clsGetMasterDrugListBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DiagnosisServiceDrugLink.clsGetDrugListBizAction";
        }
        #endregion


        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        private List<clsItemMasterVO> _DrugList = new List<clsItemMasterVO>();
        public List<clsItemMasterVO> DrugList
        {
            get
            {
                return _DrugList;
            }
            set
            {
                _DrugList = value;
            }
        }

        private clsItemMasterVO _Drugdetails;
        public clsItemMasterVO Drugdetails
        {
            get
            {
                return _Drugdetails;
            }
            set
            {
                _Drugdetails = value;
            }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
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
    }
}
