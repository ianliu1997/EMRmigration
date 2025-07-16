using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddClinicalSummaryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddClinicalSummaryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsClinicalSummaryVO _Details = new clsClinicalSummaryVO();
        public clsClinicalSummaryVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }

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
        public bool IsUpdate { get; set; }
    }

    public class clsGetClinicalSummaryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetClinicalSummaryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        private clsClinicalSummaryVO _Details = new clsClinicalSummaryVO();
        public clsClinicalSummaryVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }

        private List<clsClinicalSummaryVO> _DetailsList = new List<clsClinicalSummaryVO>();
        public List<clsClinicalSummaryVO> DetailsList
        {
            get
            {
                return _DetailsList;
            }
            set
            {
                _DetailsList = value;
            }
        }

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
        public bool IsUpdate { get; set; }
    }
}
