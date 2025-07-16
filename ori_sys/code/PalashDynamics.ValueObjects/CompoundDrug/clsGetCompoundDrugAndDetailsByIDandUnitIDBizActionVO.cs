using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsGetCompoundDrugAndDetailsByIDandUnitIDBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsGetCompoundDrugAndDetailsByIDandUnitIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public clsCompoundDrugMasterVO CompoundDrugMaster { get; set; }

        public List<clsCompoundDrugDetailVO> CompoundDrugDetailList { get; set; }

      
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

        private long _CompoundDrugID;
        private long _CompoundDrugUnitID;
        private long _PrescriptionID;
        public long CompoundDrugID
        {
            get { return _CompoundDrugID; }
            set { if (_CompoundDrugID != value) { _CompoundDrugID = value; } }
        }
        public long CompoundDrugUnitID
        {
            get { return _CompoundDrugUnitID; }
            set { if (_CompoundDrugUnitID != value) { _CompoundDrugUnitID = value; } }
        }

        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set { if (_PrescriptionID != value) { _PrescriptionID = value; } }
        }
    }
}
