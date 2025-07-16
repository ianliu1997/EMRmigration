using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
   public  class clsgetTherapyDrugDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsgetTherapyDrugDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long TherapyExeID { get; set; }
        public string DayNo { get; set; }
        public long UnitID { get; set; }
        private clsTherapyDrug _TherapyDrugDetails = new clsTherapyDrug();
        public clsTherapyDrug TherapyDrugDetails
        {
            get
            {
                return _TherapyDrugDetails;
            }
            set
            {
                _TherapyDrugDetails = value;
            }
        }

        private List<clsTherapyDrug> _TherapyDetails = new List<clsTherapyDrug>();
        public List<clsTherapyDrug> TherapyDetails
        {
            get
            {
                return _TherapyDetails;
            }
            set
            {
                _TherapyDetails = value;
            }
        }
        public bool IsFromDelete { get; set; }
        public long PlanTherapyId { get; set; }
        public long TherapyTypeId { get; set; }

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
    public class clsTherapyDrug
    {
        public long ID { get; set; }
        public DateTime? DrugDate { get; set; }
        public string ForDays { get; set; }
        public string Dosage { get; set; }
        public string DrugNotes { get; set; }
        public bool IsSurrogate { get; set; }
        public long PlanTherapyExecutionId { get; set; }
        public long TherapyTypedetailId { get; set; }
        public string Drug { get; set; }
    }
}
