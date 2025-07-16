using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetCoupleDetailsBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetCoupleDetailsBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool IsAllCouple { get; set; }

        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        private List<clsCoupleVO> _AllCoupleDetails = new List<clsCoupleVO>();
        public List<clsCoupleVO> AllCoupleDetails
        {
            get
            {
                return _AllCoupleDetails;
            }
            set
            {
                _AllCoupleDetails = value;
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

    public class clsCoupleVO
    {
        #region Properties
       

        public string CoupleRegNo { get; set; }
        public long CoupleId { get; set; }
        public long CoupleUnitId { get; set; }
        public DateTime? CoupleRegDate { get; set; }

        #endregion

        private clsPatientGeneralVO _MalePatient = new clsPatientGeneralVO();
        public clsPatientGeneralVO MalePatient
        {
            get
            {
                return _MalePatient;
            }
            set
            {
                _MalePatient = value;
            }
        }
        private clsPatientGeneralVO _FemalePatient = new clsPatientGeneralVO();
        public clsPatientGeneralVO FemalePatient
        {
            get
            {
                return _FemalePatient;
            }
            set
            {
                _FemalePatient = value;
            }
        }
    }

    public class clsGetGetCoupleHeightAndWeightBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetGetCoupleHeightAndWeightBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long FemalePatientID { get; set; }
        public long FemalePatientUnitID { get; set; }
        public long MalePatientID { get; set; }
        public long MalePatientUnitID { get; set; }
        public long PatientUnitID { get; set; }


        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
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
