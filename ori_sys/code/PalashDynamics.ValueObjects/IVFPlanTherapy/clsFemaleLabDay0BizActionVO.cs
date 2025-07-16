using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateFemaleLabDay0BizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateFemaleLabDay0BizAction";
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
        private clsFemaleLabDay0VO _LabDay0 = new clsFemaleLabDay0VO();
        public clsFemaleLabDay0VO LabDay0
        {
            get
            {
                return _LabDay0;
            }
            set
            {
                _LabDay0 = value;
            }
        }

        private clsFemaleLabDay0VO objFemaleLabDay0 = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains FemaleLabDay0 Details Which is Added.
        /// </summary>
        public clsFemaleLabDay0VO FemaleLabDay0
        {
            get { return objFemaleLabDay0; }
            set { objFemaleLabDay0 = value; }
        }
    }

    public class clsGetFemaleLabDay0BizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetFemaleLabDay0BizAction";
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

        public long OocyteID { get; set; }
        public long UnitID { get; set; }
        public long CoupleID { get; set; }
        public long CoupleUnitID { get; set; }

        private clsFemaleLabDay0VO objFemaleLabDay0 = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains FemaleLabDay0 Details Which is Added.
        /// </summary>
        public clsFemaleLabDay0VO FemaleLabDay0
        {
            get { return objFemaleLabDay0; }
            set { objFemaleLabDay0 = value; }
        }
    }

    
}
