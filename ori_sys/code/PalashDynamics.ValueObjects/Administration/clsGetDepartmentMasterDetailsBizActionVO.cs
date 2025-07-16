using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public  class clsGetDepartmentMasterDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDepartmentMasterDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        public long Id { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }
        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsDepartmentVO> objItemMaster = new List<clsDepartmentVO>();
        public List<clsDepartmentVO> ItemMatserDetails
        {
            get
            {
                return objItemMaster;
            }
            set
            {
                objItemMaster = value;

            }
        }

        private clsDepartmentVO _DeptSpecializationDetails = new clsDepartmentVO();
        public clsDepartmentVO DeptSpecializationDetails
        {
            get
            {
                return _DeptSpecializationDetails;
            }
            set
            {
                _DeptSpecializationDetails = value;

            }
        }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
}
