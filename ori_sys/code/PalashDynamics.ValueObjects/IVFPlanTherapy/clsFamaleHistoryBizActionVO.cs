using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddFamaleHistoryBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddFamaleHistoryBizAction";
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

        private clsFemaleHistoryVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsFemaleHistoryVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

    }


    public class clsGetFemaleHistoryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetFemaleHistoryBizAction";
        }
    
        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
       
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;           
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        private List<clsFemaleHistoryVO> _Details = new List<clsFemaleHistoryVO>();
        public List<clsFemaleHistoryVO> Details
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
        
        public long UnitID { get; set; }
        public long PatientID { get; set; }
    }

    public class clsUpdateFemaleHistoryBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsUpdateFemaleHistoryBizAction";
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

        private clsFemaleHistoryVO _Details = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsFemaleHistoryVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
              
    }
}
