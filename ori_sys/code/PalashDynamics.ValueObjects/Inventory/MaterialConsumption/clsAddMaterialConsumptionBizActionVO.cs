using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.ValueObjects.Inventory.MaterialConsumption
{
    public class clsAddMaterialConsumptionBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddMaterialConsumptionBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        #endregion

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


        public clsMaterialConsumptionVO ConsumptionDetails { get; set; }
        public clsAddBillBizActionVO ObjBillDetails { get; set; }
    }

    public class clsGetMatarialConsumptionListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetMatarialConsumptionListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

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

        public long StoreId { get; set; }


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long UnitID { get; set; }

        public List<clsMaterialConsumptionVO> ConsumptionList { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        //Added by AJ Date 19/1/2017
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long AdmID { get; set; }
        public long AdmissionUnitID { get; set; }
        public bool IsPatientAgainstMaterialConsumption { get; set; }


    }

    public class clsGetMatarialConsumptionItemListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetMatarialConsumptionItemListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

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

        public long ConsumptionID { get; set; }
        public long UnitID { get; set; }

        public List<clsMaterialConsumptionItemDetailsVO> ItemList { get; set; }


     

        #region For PatientIndentReceiveStock
        public clsMaterialConsumptionVO objConsumptionVO { get; set; }
        public bool IsForPatientIndentReceiveStock { get; set; }    
        #endregion

    }

}
