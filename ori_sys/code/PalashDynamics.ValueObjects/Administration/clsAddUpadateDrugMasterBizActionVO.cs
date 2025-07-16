//Created Date:04/Octomber/2012
//Created By: Nilesh Raut
//Specification: Biz Action for Drug

//Review By:
//Review Date:
//Modified By:
//Modified Date: 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddUpadateDrugMasterBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpadateDrugMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsDrugsVO> objListDrugDetails;
        public List<clsDrugsVO> ListDrugDetails
        {
            get
            {
                return objListDrugDetails;
            }
            set
            {
                objListDrugDetails = value;

            }
        }

        private clsDrugsVO _objDrug;
        public clsDrugsVO ObjDrug
        {
            get { return _objDrug; }
            set
            {
                _objDrug = value;
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

    public class clsDrugsVO
    {
        public long Id { get; set; }
        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long MoleculeID { get; set; }
        public string MoleculeName { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Boolean Status { get; set; }
        
    }

    public class clsGetDrugMasterDetailsBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDrugMasterDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion


        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

        public long Id { get; set; }
        public long UnitId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long CategoryID { get; set; }

        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        /// <summary>
        /// This property contains Item master details.
        /// </summary>
        private List<clsDrugsVO> objListDrugDetails;
        public List<clsDrugsVO> ListDrugDetails
        {
            get
            {
                return objListDrugDetails;
            }
            set
            {
                objListDrugDetails = value;

            }
        }

        private clsDrugsVO _objDrug;
        public clsDrugsVO ObjDrug
        {
            get { return _objDrug; }
            set
            {
                _objDrug = value;
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
