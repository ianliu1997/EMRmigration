using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public  class clsGetEMRDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetEMRDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long TemplateID { get; set; }

        //Patient EMR Details Against Patient Id VisitId and (Tempalte ID Optional)
        private List<clsPatientEMRDetailsVO> _EMRDetailsList = new List<clsPatientEMRDetailsVO>();
        public List<clsPatientEMRDetailsVO> EMRDetailsList
        {
            get
            {
                return _EMRDetailsList;
            }
            set
            {
                _EMRDetailsList = value;
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
    }

}
