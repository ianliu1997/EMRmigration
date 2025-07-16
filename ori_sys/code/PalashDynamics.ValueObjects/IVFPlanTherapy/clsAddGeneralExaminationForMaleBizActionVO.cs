using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddGeneralExaminationForMaleBizActionVO: IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddGeneralExaminationForMaleBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsGeneralExaminationVO _GeneralDetails = new clsGeneralExaminationVO();
        public clsGeneralExaminationVO GeneralDetails
        {
            get
            {
                return _GeneralDetails;
            }
            set
            {
                _GeneralDetails = value;
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

    public class clsAddMaleHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddMaleHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        private clsMaleHistoryVO _HistoryDetails = new clsMaleHistoryVO();
        public clsMaleHistoryVO HistoryDetails
        {
            get
            {
                return _HistoryDetails;
            }
            set
            {
                _HistoryDetails = value;
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

    public class clsGetGeneralExaminationForMaleBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetGeneralExaminationForMaleBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long PatientID { get; set; }

        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        private List<clsGeneralExaminationVO> _GeneralDetails = new List<clsGeneralExaminationVO>();
        public List<clsGeneralExaminationVO> GeneralDetails
        {
            get
            {
                return _GeneralDetails;
            }
            set
            {
                _GeneralDetails = value;
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

    public class clsGetMaleHistoryBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetMaleHistoryBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
               
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        private List<clsMaleHistoryVO> _Details = new List<clsMaleHistoryVO>();
        public List<clsMaleHistoryVO> Details
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
    
}
